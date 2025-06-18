using AutoMapper;
using Microsoft.AspNetCore.Http;
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
                        Code = 400,
                        Message = "Giá microchip phải lớn hơn 0",
                        Data = null
                    };
                }
                var created = await _microchipRepository.CreateMicrochipAsync(microchip, cancellationToken);
                if (created <= 0)
                {
                    return new BaseResponse<MicrochipResponseDTO>
                    {
                        Code = 500,
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
                        Code = 404,
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

        public async Task<BaseResponse<List<MicrochipResponseDTO>>> GetAllMicrochipsAsync(CancellationToken cancellationToken)
        {
            try
            {
                var microchips = await _microchipRepository.GetAllMicrochipsAsync(cancellationToken);
                if (microchips == null || !microchips.Any())
                {
                    return new BaseResponse<List<MicrochipResponseDTO>>
                    {
                        Code = 404,
                        Message = "Không có microchip nào",
                        Data = null
                    };
                }
                return new BaseResponse<List<MicrochipResponseDTO>>
                {
                    Code = 200,
                    Message = "Lấy danh sách microchip thành công",
                    Data = _mapper.Map<List<MicrochipResponseDTO>>(microchips)
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<MicrochipResponseDTO>>
                {
                    Code = 500,
                    Message = $"Lỗi khi lấy danh sách microchip: {ex.Message}",
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
                        Code = 404,
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
                        Code = 500,
                        Message = "Không thể cập nhật microchip",
                        Data = null
                    };
                }
                var microchipResponse = _mapper.Map<MicrochipResponseDTO>(updatedMicrochip);
                return new BaseResponse<MicrochipResponseDTO>
                {
                    Code = 200,
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
                        Code = 404,
                        Message = "Microchip không tồn tại",
                        Data = false
                    };
                }
                var result = await _microchipRepository.DeleteMicrochipAsync(microchipId, cancellationToken);
                if (!result)
                {
                    return new BaseResponse<bool>
                    {
                        Code = 500,
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
