using AutoMapper;
using Microsoft.Extensions.Logging;
using PetVax.BusinessObjects.DTO;
using PetVax.BusinessObjects.DTO.FAQItemDTO;
using PetVax.BusinessObjects.Helpers;
using PetVax.BusinessObjects.Models;
using PetVax.Repositories.IRepository;
using PetVax.Services.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PetVax.BusinessObjects.DTO.ResponseModel;

namespace PetVax.Services.Service
{
    public class FAQItemService : IFAQItemService
    {
        private readonly IFAQItemRepository _faqItemRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<FAQItemService> _logger;

        public FAQItemService(IFAQItemRepository faqItemRepository, IMapper mapper, ILogger<FAQItemService> logger)
        {
            _faqItemRepository = faqItemRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<BaseResponse<FAQResponseDTO>> CreateFAQAsync(CreateFAQDTO createFAQDTO, CancellationToken cancellationToken)
        {
            if (createFAQDTO == null)
            {
                _logger.LogError("CreateFAQAsync: createFAQDTO is null");
                return new BaseResponse<FAQResponseDTO>
                {
                    Code = 400,
                    Success = false,
                    Message = "Dữ liệu không hợp lệ",
                    Data = null
                };
            }
            try
            {
                var faqItem = _mapper.Map<FAQItem>(createFAQDTO);
                faqItem.Question = createFAQDTO.Question.Trim();
                faqItem.Answer = createFAQDTO.Answer.Trim();
                faqItem.CreatedAt = DateTimeHelper.Now();

                var createdFAQItemId = await _faqItemRepository.CreateFAQItemAsync(faqItem, cancellationToken);
                if (createdFAQItemId <= 0)
                {
                    _logger.LogError("CreateFAQAsync: Failed to create FAQ item");
                    return new BaseResponse<FAQResponseDTO>
                    {
                        Code = 500,
                        Success = false,
                        Message = "Không thể tạo câu hỏi thường gặp",
                        Data = null
                    };
                }
                var createdFAQItem = await _faqItemRepository.GetFAQItemByIdAsync(createdFAQItemId, cancellationToken);
                if (createdFAQItem == null)
                {
                    _logger.LogError("CreateFAQAsync: Created FAQ item not found");
                    return new BaseResponse<FAQResponseDTO>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Câu hỏi thường gặp không tồn tại",
                        Data = null
                    };
                }
                var faqResponseDTO = _mapper.Map<FAQResponseDTO>(createdFAQItem);
                return new BaseResponse<FAQResponseDTO>
                {
                    Code = 201,
                    Success = true,
                    Message = "Câu hỏi thường gặp đã được tạo thành công",
                    Data = faqResponseDTO
                };
            } 
            catch (Exception ex)
            {
                _logger.LogError(ex, "CreateFAQAsync: An error occurred while creating FAQ");
                return new BaseResponse<FAQResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã xảy ra lỗi khi tạo câu hỏi thường gặp",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<FAQResponseDTO>> DeleteFAQAsync(int faqId, CancellationToken cancellationToken)
        {
            if (faqId <= 0)
            {
                _logger.LogError("DeleteFAQAsync: faqId is invalid");
                return new BaseResponse<FAQResponseDTO>
                {
                    Code = 400,
                    Success = false,
                    Message = "ID câu hỏi thường gặp không hợp lệ",
                    Data = null
                };
            }
            try
            {
                var faqItem = await _faqItemRepository.GetFAQItemByIdAsync(faqId, cancellationToken);
                if (faqItem == null)
                {
                    _logger.LogError("DeleteFAQAsync: FAQ item not found");
                    return new BaseResponse<FAQResponseDTO>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Câu hỏi thường gặp không tồn tại",
                        Data = null
                    };
                }
                var isDeleted = await _faqItemRepository.DeleteFAQItemAsync(faqId, cancellationToken);
                if (!isDeleted)
                {
                    _logger.LogError("DeleteFAQAsync: Failed to delete FAQ item");
                    return new BaseResponse<FAQResponseDTO>
                    {
                        Code = 500,
                        Success = false,
                        Message = "Không thể xóa câu hỏi thường gặp",
                        Data = null
                    };
                }
                return new BaseResponse<FAQResponseDTO>
                {
                    Code = 200,
                    Success = true,
                    Message = "Câu hỏi thường gặp đã được xóa thành công",
                    Data = null
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "DeleteFAQAsync: An error occurred while deleting FAQ");
                return new BaseResponse<FAQResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã xảy ra lỗi khi xóa câu hỏi thường gặp",
                    Data = null
                };
            }
        }

        public async Task<DynamicResponse<FAQResponseDTO>> GetAllFAQsAsync(GetAllItemsDTO getAllItemsDTO, CancellationToken cancellationToken)
        {
            try
            {
                var faqItems = await _faqItemRepository.GetAllFAQItemsAsync(cancellationToken);

                if (!string.IsNullOrWhiteSpace(getAllItemsDTO.KeyWord))
                {
                    faqItems = faqItems
                        .Where(f => f.Question.Contains(getAllItemsDTO.KeyWord, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                }

                int pageNumber = getAllItemsDTO?.PageNumber > 0 ? getAllItemsDTO.PageNumber : 1;
                int pageSize = getAllItemsDTO?.PageSize > 0 ? getAllItemsDTO.PageSize : 10;
                int skip = (pageNumber - 1) * pageSize;
                int totalItem = faqItems.Count;
                int totalPage = (int)Math.Ceiling((double)totalItem / pageSize);

                var pagedFAQs = faqItems
                    .Skip(skip)
                    .Take(pageSize)
                    .ToList();

                var responseData = new MegaData<FAQResponseDTO>
                {
                    PageInfo = new PagingMetaData
                    {
                        Page = pageNumber,
                        Size = pageSize,
                        TotalItem = totalItem,
                        TotalPage = totalPage,
                    },
                    SearchInfo = new SearchCondition
                    {
                        keyWord = getAllItemsDTO?.KeyWord,
                        status = getAllItemsDTO?.Status
                    },
                    PageData = _mapper.Map<List<FAQResponseDTO>>(pagedFAQs)
                };

                if (!pagedFAQs.Any())
                {
                    _logger.LogInformation("GetAllFAQsAsync: No FAQs found");
                    return new DynamicResponse<FAQResponseDTO>
                    {
                        Code = 200,
                        Success = true,
                        Message = "Không tìm thấy câu hỏi thường gặp nào.",
                        Data = responseData
                    };
                }

                return new DynamicResponse<FAQResponseDTO>
                {
                    Code = 200,
                    Success = true,
                    Message = "Danh sách câu hỏi thường gặp đã được lấy thành công.",
                    Data = responseData
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetAllFAQsAsync: An error occurred while retrieving FAQs");
                return new DynamicResponse<FAQResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã xảy ra lỗi khi lấy danh sách câu hỏi thường gặp.",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<FAQResponseDTO>> GetFAQByIdAsync(int faqId, CancellationToken cancellationToken)
        {
            if (faqId <= 0)
            {
                _logger.LogError("GetFAQByIdAsync: faqId is invalid");
                return new BaseResponse<FAQResponseDTO>
                {
                    Code = 400,
                    Success = false,
                    Message = "ID câu hỏi thường gặp không hợp lệ",
                    Data = null
                };
            }
            try
            {
                var faqItem = await _faqItemRepository.GetFAQItemByIdAsync(faqId, cancellationToken);
                if (faqItem == null)
                {
                    _logger.LogError("GetFAQByIdAsync: FAQ item not found");
                    return new BaseResponse<FAQResponseDTO>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Câu hỏi thường gặp không tồn tại",
                        Data = null
                    };
                }
                var faqResponseDTO = _mapper.Map<FAQResponseDTO>(faqItem);
                return new BaseResponse<FAQResponseDTO>
                {
                    Code = 200,
                    Success = true,
                    Message = "Câu hỏi thường gặp đã được lấy thành công",
                    Data = faqResponseDTO
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetFAQByIdAsync: An error occurred while retrieving FAQ");
                return new BaseResponse<FAQResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã xảy ra lỗi khi lấy câu hỏi thường gặp",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<FAQResponseDTO>> UpdateFAQAsync(int faqId, UpdateFAQDTO updateFAQDTO, CancellationToken cancellationToken)
        {
            if (faqId <= 0 || updateFAQDTO == null)
            {
                _logger.LogError("UpdateFAQAsync: faqId or updateFAQDTO is invalid");
                return new BaseResponse<FAQResponseDTO>
                {
                    Code = 400,
                    Success = false,
                    Message = "ID câu hỏi thường gặp hoặc dữ liệu cập nhật không hợp lệ",
                    Data = null
                };
            }
            try
            {
                var faqItem = await _faqItemRepository.GetFAQItemByIdAsync(faqId, cancellationToken);
                if (faqItem == null)
                {
                    _logger.LogError("UpdateFAQAsync: FAQ item not found");
                    return new BaseResponse<FAQResponseDTO>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Câu hỏi thường gặp không tồn tại",
                        Data = null
                    };
                }
                faqItem.Question = updateFAQDTO.Question.Trim() ?? faqItem.Question;
                faqItem.Answer = updateFAQDTO.Answer.Trim() ?? faqItem.Answer;
                faqItem.ModifiedAt = DateTimeHelper.Now();
                var updatedFAQItemId = await _faqItemRepository.UpdateFAQItemAsync(faqItem, cancellationToken);
                if (updatedFAQItemId <= 0)
                {
                    _logger.LogError("UpdateFAQAsync: Failed to update FAQ item");
                    return new BaseResponse<FAQResponseDTO>
                    {
                        Code = 500,
                        Success = false,
                        Message = "Không thể cập nhật câu hỏi thường gặp",
                        Data = null
                    };
                }
                var updatedFAQItem = await _faqItemRepository.GetFAQItemByIdAsync(updatedFAQItemId, cancellationToken);
                var faqResponseDTO = _mapper.Map<FAQResponseDTO>(updatedFAQItem);
                return new BaseResponse<FAQResponseDTO>
                {
                    Code = 200,
                    Success = true,
                    Message = "Câu hỏi thường gặp đã được cập nhật thành công",
                    Data = faqResponseDTO
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "UpdateFAQAsync: An error occurred while updating FAQ");
                return new BaseResponse<FAQResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã xảy ra lỗi khi cập nhật câu hỏi thường gặp",
                    Data = null
                };
            }
        }
    }
}
