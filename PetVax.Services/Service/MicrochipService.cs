using AutoMapper;
using Microsoft.AspNetCore.Http;
using PetVax.BusinessObjects.DTO;
using PetVax.BusinessObjects.DTO.MicrochipDTO;
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
    public class MicrochipService : IMicrochipService
    {
        private readonly IMicrochipRepository _microchipRepository;
        private readonly IMicrochipItemRepository _microchipItemRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMapper _mapper;

        public MicrochipService(IMicrochipRepository microchipRepository, IMicrochipItemRepository microchipItemRepository, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _microchipRepository = microchipRepository;
            _microchipItemRepository = microchipItemRepository;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        public async Task<BaseResponse<MicrochipResponseDTO>> CreateMicrochipAsync(MicrochipRequestDTO microchipRequestDTO, CancellationToken cancellationToken)
        {
            try
            {
                Microchip microchip = new Microchip();
                microchip.MicrochipCode = microchipRequestDTO.MicrochipCode;
                microchip.Name = microchipRequestDTO.Name;
                microchip.Description = microchipRequestDTO.Description;
                microchip.Price = microchipRequestDTO.Price;
                microchip.Notes = microchipRequestDTO.Notes;
                microchip.Status = "Active";
                microchip.CreatedAt = DateTime.UtcNow;
                if (microchip.Price <= 0)
                {
                    return new BaseResponse<MicrochipResponseDTO>
                    {
                        Code = 200,
                        Message = "Giá microchip phải lớn hơn 0",
                        Data = null
                    };
                }
                var created = await _microchipRepository.CreateMicrochipAsync(microchip, cancellationToken);
                if (created <= 0)
                {
                    return new BaseResponse<MicrochipResponseDTO>
                    {
                        Code = 200,
                        Message = "Không thể tạo microchip",
                        Data = null
                    };
                }

                var microchipResponse = _mapper.Map<MicrochipResponseDTO>(microchip);                
                return new BaseResponse<MicrochipResponseDTO>
                {
                    Code = 201,
                    Message = "Tạo microchip thành công",
                    Data = microchipResponse
                };

            }
            catch (Exception ex)
            {
                // Log ex.InnerException?.Message để biết lỗi chi tiết
                return new BaseResponse<MicrochipResponseDTO>
                {
                    Code = 500,
                    Message = $"Lỗi khi tạo microchip: {ex.Message} - {ex.InnerException}",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<MicrochipResponseDTO>> GetMicrochipByIdAsync(int microchipId, CancellationToken cancellationToken)
        {
            try
            {
                var microchip = await _microchipRepository.GetMicrochipByIdAsync(microchipId, cancellationToken);
                if (microchip == null)
                {
                    return new BaseResponse<MicrochipResponseDTO>
                    {
                        Code = 200,
                        Message = "Microchip không tồn tại",
                        Data = null
                    };
                }
                var microchipResponse = _mapper.Map<MicrochipResponseDTO>(microchip);
                return new BaseResponse<MicrochipResponseDTO>
                {
                    Code = 200,
                    Message = "Lấy thông tin microchip thành công",
                    Data = microchipResponse
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<MicrochipResponseDTO>
                {
                    Code = 500,
                    Message = $"Lỗi khi lấy thông tin microchip: {ex.Message}",
                    Data = null
                };
            }
        }

        public async Task<DynamicResponse<MicrochipResponseDTO>> GetAllMicrochipsDynamicAsync(GetAllItemsDTO getAllItemsDTO, CancellationToken cancellationToken)
        {
            try
            {
                var microchips = await _microchipRepository.GetAllMicrochipsAsync(cancellationToken);
                if (!string.IsNullOrWhiteSpace(getAllItemsDTO.KeyWord))
                {
                    microchips = microchips
                        .Where(m => m.MicrochipCode.Contains(getAllItemsDTO.KeyWord, StringComparison.OrdinalIgnoreCase) ||
                                    m.Name.Contains(getAllItemsDTO.KeyWord, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                }

                int pageNumber = getAllItemsDTO?.PageNumber > 0 ? getAllItemsDTO.PageNumber : 1;
                int pageSize = getAllItemsDTO?.PageSize > 0 ? getAllItemsDTO.PageSize : 10;
                int skip = (pageNumber - 1) * pageSize;
                int totalItem = microchips.Count;
                int totalPage = (int)Math.Ceiling((double)totalItem / pageSize);

                var pagedMicrochips = microchips
                    .Skip(skip)
                    .Take(pageSize)
                    .ToList();

                var responseData = new MegaData<MicrochipResponseDTO>
                {
                    PageInfo = new PagingMetaData
                    {
                        Page = pageNumber,
                        Size = pageSize,
                        TotalItem = totalItem,
                        TotalPage = totalPage
                    },
                    SearchInfo = new SearchCondition
                    {
                        keyWord = getAllItemsDTO?.KeyWord,
                    },
                    PageData = _mapper.Map<List<MicrochipResponseDTO>>(pagedMicrochips)
                };

                if (!pagedMicrochips.Any())
                {
                    return new DynamicResponse<MicrochipResponseDTO>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Không tìm thấy microchip nào.",
                        Data = responseData
                    };
                }
                return new DynamicResponse<MicrochipResponseDTO>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy tất cả microchip thành công.",
                    Data = responseData
                };
            }
            catch (Exception ex)
            {
                // _logger.LogError(ex, "Error occurred while getting all microchips.");
                return new DynamicResponse<MicrochipResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã xảy ra lỗi khi lấy tất cả microchip.",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<MicrochipResponseDTO>> UpdateMicrochipAsync(int microchipId, MicrochipRequestDTO microchipRequestDTO, CancellationToken cancellationToken)
        {
            try
            {
                var existingMicrochip = await _microchipRepository.GetMicrochipByIdAsync(microchipId, cancellationToken);
                if (existingMicrochip == null)
                {
                    return new BaseResponse<MicrochipResponseDTO>
                    {
                        Code = 200,
                        Message = "Microchip không tồn tại",
                        Data = null
                    };
                }
                var updatedMicrochip = _mapper.Map(microchipRequestDTO, existingMicrochip);
                updatedMicrochip.MicrochipId = microchipId;
                var result = await _microchipRepository.UpdateMicrochipAsync(updatedMicrochip, cancellationToken);
                if (result <= 0)
                {
                    return new BaseResponse<MicrochipResponseDTO>
                    {
                        Code = 200,
                        Message = "Không thể cập nhật microchip",
                        Data = null
                    };
                }
                var microchipResponse = _mapper.Map<MicrochipResponseDTO>(updatedMicrochip);
                return new BaseResponse<MicrochipResponseDTO>
                {
                    Code = 200,
                    Success = true,
                    Message = "Cập nhật microchip thành công",
                    Data = microchipResponse
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<MicrochipResponseDTO>
                {
                    Code = 500,
                    Message = $"Lỗi khi cập nhật microchip: {ex.Message}",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<bool>> DeleteMicrochipAsync(int microchipId, CancellationToken cancellationToken)
        {
            try
            {
                var existingMicrochip = await _microchipRepository.GetMicrochipByIdAsync(microchipId, cancellationToken);
                if (existingMicrochip == null)
                {
                    return new BaseResponse<bool>
                    {
                        Code = 200,
                        Message = "Microchip không tồn tại",
                        Data = false
                    };
                }
                var result = await _microchipRepository.DeleteMicrochipAsync(microchipId, cancellationToken);
                if (!result)
                {
                    return new BaseResponse<bool>
                    {
                        Code = 200,
                        Message = "Không thể xóa microchip",
                        Data = false
                    };
                }
                return new BaseResponse<bool>
                {
                    Code = 200,
                    Message = "Xóa microchip thành công",
                    Data = true
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<bool>
                {
                    Code = 500,
                    Message = $"Lỗi khi xóa microchip: {ex.Message}",
                    Data = false
                };
            }
        }

    }
}
