using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PetVax.BusinessObjects.DTO;
using PetVax.BusinessObjects.DTO.HandbookDTO;
using PetVax.BusinessObjects.Helpers;
using PetVax.BusinessObjects.Models;
using PetVax.Repositories.IRepository;
using PetVax.Services.ExternalService;
using PetVax.Services.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PetVax.BusinessObjects.DTO.ResponseModel;

namespace PetVax.Services.Service
{
    public class HandbookService : IHandbookService
    {
        private readonly IHandbookRepository _handbookRepository;
        private readonly ICloudinariService _cloudinariService;
        private readonly ILogger<HandbookService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public HandbookService(IHandbookRepository handbookRepository, ICloudinariService cloudinariService, ILogger<HandbookService> logger, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _handbookRepository = handbookRepository;
            _cloudinariService = cloudinariService;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        public async Task<BaseResponse<HandbookResponseDTO>> CreateHandbookAsync(CreateHandbookDTO createHandbookDTO, CancellationToken cancellationToken)
        {
            if (createHandbookDTO == null)
            {
                _logger.LogError("CreateHandbookAsync: createHandbookDTO is null");
                return new BaseResponse<HandbookResponseDTO>
                {
                    Code = 400,
                    Success = false,
                    Message = "Dữ liệu tạo cẩm nang không hợp lệ.",
                    Data = null
                };
            }
            try
            {
                var handbook = _mapper.Map<Handbook>(createHandbookDTO);

                if (handbook.ImageUrl != null)
                {
                    handbook.ImageUrl = await _cloudinariService.UploadImage(createHandbookDTO.ImageUrl);
                }

                handbook.Title = createHandbookDTO.Title.Trim();
                handbook.Introduction = createHandbookDTO.Introduction.Trim();
                handbook.Highlight = createHandbookDTO.Highlight?.Trim();
                handbook.Content = createHandbookDTO.Content.Trim();
                handbook.ImportantNote = createHandbookDTO.ImportantNote?.Trim();
                handbook.CreatedAt = DateTimeHelper.Now();
                handbook.CreatedBy = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "System";

                var createdHandbookId = await _handbookRepository.CreateHandbookAsync(handbook, cancellationToken);
                if (createdHandbookId == null)
                {
                    _logger.LogError("CreateHandbookAsync: Failed to create handbook");
                    return new BaseResponse<HandbookResponseDTO>
                    {
                        Code = 500,
                        Success = false,
                        Message = "Không thể tạo cẩm nang.",
                        Data = null
                    };
                }
                var createHandbook = await _handbookRepository.GetHandbookByIdAsync(createdHandbookId, cancellationToken);
                var responseDTO = _mapper.Map<HandbookResponseDTO>(createHandbook);
                return new BaseResponse<HandbookResponseDTO>
                {
                    Code = 201,
                    Success = true,
                    Message = "Cẩm nang đã được tạo thành công.",
                    Data = responseDTO
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CreateHandbookAsync: An error occurred while creating handbook");
                return new BaseResponse<HandbookResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã xảy ra lỗi khi tạo cẩm nang.",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<HandbookResponseDTO>> DeleteHandbookAsync(int handbookId, CancellationToken cancellationToken)
        {
            if (handbookId <= 0)
            {
                _logger.LogError("DeleteHandbookAsync: Invalid handbookId");
                return new BaseResponse<HandbookResponseDTO>
                {
                    Code = 400,
                    Success = false,
                    Message = "ID cẩm nang không hợp lệ.",
                    Data = null
                };
            }
            try
            {
                var handbook = await _handbookRepository.GetHandbookByIdAsync(handbookId, cancellationToken);
                if (handbook == null)
                {
                    _logger.LogError($"DeleteHandbookAsync: Handbook with ID {handbookId} not found");
                    return new BaseResponse<HandbookResponseDTO>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Cẩm nang không tồn tại.",
                        Data = null
                    };
                }
                var isDeleted = await _handbookRepository.DeleteHandbookAsync(handbookId, cancellationToken);
                if (!isDeleted)
                {
                    _logger.LogError($"DeleteHandbookAsync: Failed to delete handbook with ID {handbookId}");
                    return new BaseResponse<HandbookResponseDTO>
                    {
                        Code = 500,
                        Success = false,
                        Message = "Không thể xóa cẩm nang.",
                        Data = null
                    };
                }
                return new BaseResponse<HandbookResponseDTO>
                {
                    Code = 200,
                    Success = true,
                    Message = "Cẩm nang đã được xóa thành công.",
                    Data = null
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"DeleteHandbookAsync: An error occurred while deleting handbook with ID {handbookId}");
                return new BaseResponse<HandbookResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã xảy ra lỗi khi xóa cẩm nang.",
                    Data = null
                };
            }
        }

        public async Task<DynamicResponse<HandbookResponseDTO>> GetAllHandbooksAsync(GetAllItemsDTO getAllItemsDTO, CancellationToken cancellationToken)
        {
            try
            {
                var handbooks = await _handbookRepository.GetAllHandbookAsync(cancellationToken);
                if (!string.IsNullOrWhiteSpace(getAllItemsDTO.KeyWord))
                {
                    handbooks = handbooks.Where(h => h.Title.Contains(getAllItemsDTO.KeyWord, StringComparison.OrdinalIgnoreCase)).ToList();
                }

                int pageNumber = getAllItemsDTO?.PageNumber > 0 ? getAllItemsDTO.PageNumber : 1;
                int pageSize = getAllItemsDTO?.PageSize > 0 ? getAllItemsDTO.PageSize : 10;
                int skip = (pageNumber - 1) * pageSize;
                int totalItem = handbooks.Count;
                int totalPage = (int)Math.Ceiling((double)totalItem / pageSize);

                var pagedHandbooks = handbooks
                    .Skip(skip)
                    .Take(pageSize)
                    .ToList();

                var responseData = new MegaData<HandbookResponseDTO>
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
                    PageData = _mapper.Map<List<HandbookResponseDTO>>(pagedHandbooks)
                };
                if (!pagedHandbooks.Any())
                {
                    _logger.LogInformation("GetAllHandbooksAsync: No handbooks found");
                    return new DynamicResponse<HandbookResponseDTO>
                    {
                        Code = 200,
                        Success = false,
                        Message = "Không tìm thấy cẩm nang nào.",
                        Data = responseData
                    };
                }
                return new DynamicResponse<HandbookResponseDTO>
                {
                    Code = 200,
                    Success = true,
                    Message = "Danh sách cẩm nang đã được lấy thành công.",
                    Data = responseData
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetAllHandbooksAsync: An error occurred while retrieving handbooks");
                return new DynamicResponse<HandbookResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã xảy ra lỗi khi lấy danh sách cẩm nang.",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<HandbookResponseDTO>> GetHandbookByIdAsync(int handbookId, CancellationToken cancellationToken)
        {
            if (handbookId <= 0)
            {
                _logger.LogError("GetHandbookByIdAsync: Invalid handbookId");
                return new BaseResponse<HandbookResponseDTO>
                {
                    Code = 400,
                    Success = false,
                    Message = "ID cẩm nang không hợp lệ.",
                    Data = null
                };
            }
            try
            {
                var handbook = await _handbookRepository.GetHandbookByIdAsync(handbookId, cancellationToken);
                if (handbook == null)
                {
                    _logger.LogError($"GetHandbookByIdAsync: Handbook with ID {handbookId} not found");
                    return new BaseResponse<HandbookResponseDTO>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Cẩm nang không tồn tại.",
                        Data = null
                    };
                }
                var responseDTO = _mapper.Map<HandbookResponseDTO>(handbook);
                return new BaseResponse<HandbookResponseDTO>
                {
                    Code = 200,
                    Success = true,
                    Message = "Cẩm nang đã được lấy thành công.",
                    Data = responseDTO
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"GetHandbookByIdAsync: An error occurred while retrieving handbook with ID {handbookId}");
                return new BaseResponse<HandbookResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã xảy ra lỗi khi lấy cẩm nang.",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<HandbookResponseDTO>> UpdateHandbookAsync(int handbookId, UpdateHandbookDTO updateHandbookDTO, CancellationToken cancellationToken)
        {
            if (handbookId <= 0 || updateHandbookDTO == null)
            {
                _logger.LogError("UpdateHandbookAsync: Invalid handbookId or updateHandbookDTO is null");
                return new BaseResponse<HandbookResponseDTO>
                {
                    Code = 400,
                    Success = false,
                    Message = "ID cẩm nang hoặc dữ liệu cập nhật không hợp lệ.",
                    Data = null
                };
            }
            try
            {
                var handbook = await _handbookRepository.GetHandbookByIdAsync(handbookId, cancellationToken);
                if (handbook == null)
                {
                    _logger.LogError($"UpdateHandbookAsync: Handbook with ID {handbookId} not found");
                    return new BaseResponse<HandbookResponseDTO>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Cẩm nang không tồn tại.",
                        Data = null
                    };
                }
                handbook.Title = updateHandbookDTO.Title ?? handbook.Title;
                handbook.Introduction = updateHandbookDTO.Introduction ?? handbook.Introduction;
                handbook.Highlight = updateHandbookDTO.Highlight ?? handbook.Highlight;
                handbook.Content = updateHandbookDTO.Content ?? handbook.Content;
                handbook.ImportantNote = updateHandbookDTO.ImportantNote ?? handbook.ImportantNote;
                if (updateHandbookDTO.ImageUrl != null)
                {
                    handbook.ImageUrl = await _cloudinariService.UploadImage(updateHandbookDTO.ImageUrl);
                }
                handbook.ModifiedAt = DateTimeHelper.Now();
                handbook.ModifiedBy = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "System";
                var updatedHandbookId = await _handbookRepository.UpdateHandbookAsync(handbook, cancellationToken);
                if (updatedHandbookId == null)
                {
                    _logger.LogError($"UpdateHandbookAsync: Failed to update handbook with ID {handbookId}");
                    return new BaseResponse<HandbookResponseDTO>
                    {
                        Code = 500,
                        Success = false,
                        Message = "Không thể cập nhật cẩm nang.",
                        Data = null
                    };
                }

                var responseDTO = _mapper.Map<HandbookResponseDTO>(handbook);
                return new BaseResponse<HandbookResponseDTO>
                {
                    Code = 200,
                    Success = true,
                    Message = "Cẩm nang đã được cập nhật thành công.",
                    Data = responseDTO
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"UpdateHandbookAsync: An error occurred while updating handbook with ID {handbookId}");
                return new BaseResponse<HandbookResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã xảy ra lỗi khi cập nhật cẩm nang.",
                    Data = null
                };
            }
        }
    }
}
