using AutoMapper;
using Microsoft.AspNetCore.Http;
using PetVax.BusinessObjects.DTO;
using PetVax.BusinessObjects.DTO.MicrochipDTO;
using PetVax.BusinessObjects.DTO.MicrochipItemDTO;
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
        private readonly IPetRepository _petRepository;

        public MicrochipService(
            IMicrochipRepository microchipRepository,
            IMicrochipItemRepository microchipItemRepository,
            IHttpContextAccessor httpContextAccessor,
            IMapper mapper,
            IPetRepository petRepository)
        {
            _microchipRepository = microchipRepository;
            _microchipItemRepository = microchipItemRepository;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _petRepository = petRepository;
        }

        //public async Task<BaseResponse<MicrochipResponseDTO>> CreateMicrochipAsync(MicrochipRequestDTO microchipRequestDTO, CancellationToken cancellationToken)
        //{
        //    try
        //    {
        //        Microchip microchip = new Microchip();
        //        microchip.MicrochipCode = microchipRequestDTO.MicrochipCode;
        //        microchip.Name = microchipRequestDTO.Name;
        //        microchip.Description = microchipRequestDTO.Description;
        //        microchip.Price = microchipRequestDTO.Price;
        //        microchip.Notes = microchipRequestDTO.Notes;
        //        microchip.Status = "Active";
        //        microchip.CreatedAt = DateTime.UtcNow;
        //        if (microchip.Price <= 0)
        //        {
        //            return new BaseResponse<MicrochipResponseDTO>
        //            {
        //                Code = 200,
        //                Message = "Giá microchip phải lớn hơn 0",
        //                Data = null
        //            };
        //        }
        //        var created = await _microchipRepository.CreateMicrochipAsync(microchip, cancellationToken);
        //        if (created <= 0)
        //        {
        //            return new BaseResponse<MicrochipResponseDTO>
        //            {
        //                Code = 200,
        //                Message = "Không thể tạo microchip",
        //                Data = null
        //            };
        //        }

        //        var microchipResponse = _mapper.Map<MicrochipResponseDTO>(microchip);                
        //        return new BaseResponse<MicrochipResponseDTO>
        //        {
        //            Code = 201,
        //            Message = "Tạo microchip thành công",
        //            Data = microchipResponse
        //        };

        //    }
        //    catch (Exception ex)
        //    {
        //        // Log ex.InnerException?.Message để biết lỗi chi tiết
        //        return new BaseResponse<MicrochipResponseDTO>
        //        {
        //            Code = 500,
        //            Message = $"Lỗi khi tạo microchip: {ex.Message} - {ex.InnerException}",
        //            Data = null
        //        };
        //    }
        //}

        public async Task<BaseResponse<BaseMicrochipItemResponse>> CreateFullMicrochipAsync(MicrochipRequestDTO request, CancellationToken cancellationToken)
        {
            try
            {
                // Validate Microchip Price
                if (request.Price <= 0)
                {
                    return new BaseResponse<BaseMicrochipItemResponse>
                    {
                        Code = 200,
                        Message = "Giá microchip phải lớn hơn 0",
                        Data = null
                    };
                }

                // Create Microchip
                var microchip = new Microchip
                {
                    MicrochipCode = request.MicrochipCode,
                    Name = request.Name,
                    Description = request.Description,
                    Price = request.Price,
                    Notes = request.Notes,
                    Status = "Active",
                    CreatedAt = DateTime.UtcNow
                };

                var created = await _microchipRepository.CreateMicrochipAsync(microchip, cancellationToken);
                if (created <= 0)
                {
                    return new BaseResponse<BaseMicrochipItemResponse>
                    {
                        Code = 200,
                        Message = "Không thể tạo microchip",
                        Data = null
                    };
                }           
              
                // Kiểm tra Pet đã gắn Microchip chưa
                var existingChip = await _microchipItemRepository.GetMicrochipItemByPetIdAsync(request.createMicrochipItemRequest.PetId, cancellationToken);
                if (existingChip != null)
                {
                    return new BaseResponse<BaseMicrochipItemResponse>
                    {
                        Code = 200,
                        Message = "Thú cưng này đã được cấy microchip",
                        Data = null
                    };
                }


                // Tạo MicrochipItem
                var microchipItem = new MicrochipItem();

                microchipItem.MicrochipId = microchip.MicrochipId;
                if (request.createMicrochipItemRequest.PetId == null || request.createMicrochipItemRequest.PetId <= 0)
                {
                    microchipItem.PetId = null;
                }
                else {
                    var pet = await _petRepository.GetPetAndAppointmentByIdAsync(request.createMicrochipItemRequest.PetId, cancellationToken);
                    if (pet == null)
                    {
                        return new BaseResponse<BaseMicrochipItemResponse>
                        {
                            Code = 200,
                            Message = "Thú cưng không tồn tại!",
                            Data = null
                        };
                    }
                }
                microchipItem.IsUsed = true;
                microchipItem.Name = request.createMicrochipItemRequest.Name;
                microchipItem.Description = request.createMicrochipItemRequest.Description;
                microchipItem.InstallationDate = request.createMicrochipItemRequest.InstallationDate;
                microchipItem.CreatedAt = DateTime.UtcNow;
                microchipItem.CreatedBy = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "system";
                microchipItem.Status = "Active";
              
                var itemResult = await _microchipItemRepository.CreateMicrochipItemAsync(microchipItem, cancellationToken);
                var microchipItemCheck = await _microchipItemRepository.GetMicrochipItemByIdAsync(microchipItem.MicrochipItemId, cancellationToken);

                return new BaseResponse<BaseMicrochipItemResponse>
                {
                    Code = 201,
                    Success = true,
                    Message = "Tạo Microchip thành công!",
                    Data = _mapper.Map<BaseMicrochipItemResponse>(microchipItemCheck)
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<BaseMicrochipItemResponse>
                {
                    Code = 500,
                    Message = "Lỗi khi tạo microchip: " + ex.Message,
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
                        status = getAllItemsDTO?.Status
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
                existingMicrochip.isDeleted = true;
                var result = await _microchipRepository.UpdateMicrochipAsync(existingMicrochip, cancellationToken);
                if (result <= 0)
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
