using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PetVax.BusinessObjects.DTO;
using PetVax.BusinessObjects.DTO.SupportCategoryDTO;
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
    public class SupportCategoryService : ISupportCategoryService
    {
        private readonly ISupportCategoryRepository _supportCategoryRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<SupportCategoryService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SupportCategoryService(ISupportCategoryRepository supportCategoryRepository, IMapper mapper, ILogger<SupportCategoryService> logger, IHttpContextAccessor httpContextAccessor)
        {
            _supportCategoryRepository = supportCategoryRepository;
            _mapper = mapper;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<BaseResponse<SupportCategoryResponseDTO>> CreateSupportCategoryAsync(CreateSupportCategoryDTO createSupportCategoryDTO, CancellationToken cancellationToken)
        {
            if (createSupportCategoryDTO == null)
            {
                _logger.LogError("CreateSupportCategoryAsync: createSupportCategoryDTO is null");
                return new BaseResponse<SupportCategoryResponseDTO>
                {
                    Code = 400,
                    Success = false,
                    Message = "Dữ liệu không hợp lệ.",
                    Data = null
                };
            }
            try
            {
                var supportCategory = _mapper.Map<SupportCategory>(createSupportCategoryDTO);
                supportCategory.Title = createSupportCategoryDTO.Title.Trim();
                supportCategory.Description = createSupportCategoryDTO.Description?.Trim();
                supportCategory.Content = createSupportCategoryDTO.Content?.Trim();
                supportCategory.CreatedAt = DateTimeHelper.Now();

                var createdSupportCategoryId = await _supportCategoryRepository.CreateSupportCategoryAsync(supportCategory, cancellationToken);
                if (createdSupportCategoryId <= 0)
                {
                    _logger.LogError("CreateSupportCategoryAsync: Failed to create support category");
                    return new BaseResponse<SupportCategoryResponseDTO>
                    {
                        Code = 500,
                        Success = false,
                        Message = "Không thể tạo danh mục hỗ trợ.",
                        Data = null
                    };
                }
                var createdSupportCategory = await _supportCategoryRepository.GetSupportCategoryByIdAsync(createdSupportCategoryId, cancellationToken);
                if (createdSupportCategory == null)
                {
                    _logger.LogError("CreateSupportCategoryAsync: Created support category not found");
                    return new BaseResponse<SupportCategoryResponseDTO>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Danh mục hỗ trợ vừa tạo không tồn tại.",
                        Data = null
                    };
                }
                var responseDTO = _mapper.Map<SupportCategoryResponseDTO>(createdSupportCategory);
                return new BaseResponse<SupportCategoryResponseDTO>
                {
                    Code = 201,
                    Success = true,
                    Message = "Tạo danh mục hỗ trợ thành công.",
                    Data = responseDTO
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"CreateSupportCategoryAsync: An error occurred while creating support category: {ex.Message}");
                return new BaseResponse<SupportCategoryResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã xảy ra lỗi khi tạo danh mục hỗ trợ.",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<SupportCategoryResponseDTO>> DeleteSupportCategoryAsync(int supportCategoryId, CancellationToken cancellationToken)
        {
            if (supportCategoryId <= 0)
            {
                _logger.LogError("DeleteSupportCategoryAsync: Invalid support category ID");
                return new BaseResponse<SupportCategoryResponseDTO>
                {
                    Code = 400,
                    Success = false,
                    Message = "ID danh mục hỗ trợ không hợp lệ.",
                    Data = null
                };
            }
            try
            {
                var isDeleted = await _supportCategoryRepository.DeleteSupportCategoryAsync(supportCategoryId, cancellationToken);
                if (!isDeleted)
                {
                    _logger.LogError("DeleteSupportCategoryAsync: Failed to delete support category");
                    return new BaseResponse<SupportCategoryResponseDTO>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Danh mục hỗ trợ không tồn tại hoặc đã bị xóa.",
                        Data = null
                    };
                }
                return new BaseResponse<SupportCategoryResponseDTO>
                {
                    Code = 200,
                    Success = true,
                    Message = "Xóa danh mục hỗ trợ thành công.",
                    Data = null
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"DeleteSupportCategoryAsync: An error occurred while deleting support category: {ex.Message}");
                return new BaseResponse<SupportCategoryResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã xảy ra lỗi khi xóa danh mục hỗ trợ.",
                    Data = null
                };
            }
        }

        public async Task<DynamicResponse<SupportCategoryResponseDTO>> GetAllSupportCategoriesAsync(GetAllItemsDTO getAllItemsDTO, CancellationToken cancellationToken)
        {
            try
            {
                var supportCategories = await _supportCategoryRepository.GetAllSupportCategoriesAsync(cancellationToken);

                if (!string.IsNullOrWhiteSpace(getAllItemsDTO.KeyWord))
                {
                    supportCategories = supportCategories
                        .Where(sc => sc.Title.Contains(getAllItemsDTO.KeyWord, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                }

                int pageNumber = getAllItemsDTO?.PageNumber > 0 ? getAllItemsDTO.PageNumber : 1;
                int pageSize = getAllItemsDTO?.PageSize > 0 ? getAllItemsDTO.PageSize : 10;
                int skip = (pageNumber - 1) * pageSize;
                int totalItem = supportCategories.Count;
                int totalPage = (int)Math.Ceiling((double)totalItem / pageSize);

                var pagedSupportCategories = supportCategories
                    .Skip(skip)
                    .Take(pageSize)
                    .ToList();

                var responseData = new MegaData<SupportCategoryResponseDTO>
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
                    PageData = _mapper.Map<List<SupportCategoryResponseDTO>>(pagedSupportCategories)
                };

                if (!pagedSupportCategories.Any())
                {
                    _logger.LogInformation("GetAllSupportCategoriesAsync: No support categories found");
                    return new DynamicResponse<SupportCategoryResponseDTO>
                    {
                        Code = 200,
                        Success = false,
                        Message = "Không tìm thấy danh mục hỗ trợ nào.",
                        Data = responseData
                    };
                }

                return new DynamicResponse<SupportCategoryResponseDTO>
                {
                    Code = 200,
                    Success = true,
                    Message = "Danh sách danh mục hỗ trợ đã được lấy thành công.",
                    Data = responseData
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetAllSupportCategoriesAsync: An error occurred while retrieving support categories");
                return new DynamicResponse<SupportCategoryResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã xảy ra lỗi khi lấy danh sách danh mục hỗ trợ.",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<SupportCategoryResponseDTO>> GetSupportCategoryByIdAsync(int supportCategoryId, CancellationToken cancellationToken)
        {
            if (supportCategoryId <= 0)
            {
                _logger.LogError("GetSupportCategoryByIdAsync: Invalid support category ID");
                return new BaseResponse<SupportCategoryResponseDTO>
                {
                    Code = 400,
                    Success = false,
                    Message = "ID danh mục hỗ trợ không hợp lệ.",
                    Data = null
                };
            }
            try
            {
                var supportCategory = await _supportCategoryRepository.GetSupportCategoryByIdAsync(supportCategoryId, cancellationToken);
                if (supportCategory == null)
                {
                    _logger.LogError("GetSupportCategoryByIdAsync: Support category not found");
                    return new BaseResponse<SupportCategoryResponseDTO>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Danh mục hỗ trợ không tồn tại.",
                        Data = null
                    };
                }
                var responseDTO = _mapper.Map<SupportCategoryResponseDTO>(supportCategory);
                return new BaseResponse<SupportCategoryResponseDTO>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy danh mục hỗ trợ thành công.",
                    Data = responseDTO
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"GetSupportCategoryByIdAsync: An error occurred while retrieving support category: {ex.Message}");
                return new BaseResponse<SupportCategoryResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã xảy ra lỗi khi lấy danh mục hỗ trợ.",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<SupportCategoryResponseDTO>> UpdateSupportCategoryAsync(int supportCategoryId, UpdateSupportCategoryDTO updateSupportCategoryDTO, CancellationToken cancellationToken)
        {
            if (supportCategoryId <= 0 || updateSupportCategoryDTO == null)
            {
                _logger.LogError("UpdateSupportCategoryAsync: Invalid support category ID or DTO");
                return new BaseResponse<SupportCategoryResponseDTO>
                {
                    Code = 400,
                    Success = false,
                    Message = "ID danh mục hỗ trợ hoặc dữ liệu không hợp lệ.",
                    Data = null
                };
            }
            try
            {
                var existingSupportCategory = await _supportCategoryRepository.GetSupportCategoryByIdAsync(supportCategoryId, cancellationToken);
                if (existingSupportCategory == null)
                {
                    _logger.LogError("UpdateSupportCategoryAsync: Support category not found");
                    return new BaseResponse<SupportCategoryResponseDTO>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Danh mục hỗ trợ không tồn tại.",
                        Data = null
                    };
                }
                existingSupportCategory.Title = updateSupportCategoryDTO.Title?.Trim() ?? existingSupportCategory.Title;
                existingSupportCategory.Description = updateSupportCategoryDTO.Description?.Trim() ?? existingSupportCategory.Description;
                existingSupportCategory.Content = updateSupportCategoryDTO.Content?.Trim() ?? existingSupportCategory.Content;
                existingSupportCategory.ModifiedAt = DateTimeHelper.Now();
                var updatedSupportCategoryId = await _supportCategoryRepository.UpdateSupportCategoryAsync(existingSupportCategory, cancellationToken);
                if (updatedSupportCategoryId <= 0)
                {
                    _logger.LogError("UpdateSupportCategoryAsync: Failed to update support category");
                    return new BaseResponse<SupportCategoryResponseDTO>
                    {
                        Code = 500,
                        Success = false,
                        Message = "Không thể cập nhật danh mục hỗ trợ.",
                        Data = null
                    };
                }
                var updatedSupportCategory = await _supportCategoryRepository.GetSupportCategoryByIdAsync(updatedSupportCategoryId, cancellationToken);
                var responseDTO = _mapper.Map<SupportCategoryResponseDTO>(updatedSupportCategory);
                return new BaseResponse<SupportCategoryResponseDTO>
                {
                    Code = 200,
                    Success = true,
                    Message = "Cập nhật danh mục hỗ trợ thành công.",
                    Data = responseDTO
                };
            }
            catch (Exception ex)
            {
                _logger.LogError($"UpdateSupportCategoryAsync: An error occurred while updating support category: {ex.Message}");
                return new BaseResponse<SupportCategoryResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã xảy ra lỗi khi cập nhật danh mục hỗ trợ.",
                    Data = null
                };
            }
        }
    }
}
