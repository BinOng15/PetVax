using Microsoft.AspNetCore.Http;
using PetVax.BusinessObjects.DTO;
using PetVax.BusinessObjects.DTO.VetDTO;
using PetVax.BusinessObjects.DTO.VetScheduleDTO;
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
    public class VetScheduleService : IVetScheduleService
    {
        private readonly IVetScheduleRepository _vetScheduleRepository;
        private readonly IVetRepository _vetRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public VetScheduleService(IVetScheduleRepository vetScheduleRepository, IVetRepository vetRepository, IHttpContextAccessor httpContextAccessor)
        {
            _vetScheduleRepository = vetScheduleRepository;
            _vetRepository = vetRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<DynamicResponse<VetScheduleDTO>> GetAllVetSchedulesAsync(GetAllItemsDTO getAllItemsDTO, CancellationToken cancellationToken)
        {
            try
            {
                var vetSchedules = await _vetScheduleRepository.GetAllVetSchedulesAsync(cancellationToken);

                if (!string.IsNullOrWhiteSpace(getAllItemsDTO.KeyWord))
                {
                    vetSchedules = vetSchedules
                        .Where(vs => vs.VetId.ToString().Contains(getAllItemsDTO.KeyWord, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                }

                int pageNumber = getAllItemsDTO?.PageNumber > 0 ? getAllItemsDTO.PageNumber : 1;
                int pageSize = getAllItemsDTO?.PageSize > 0 ? getAllItemsDTO.PageSize : 10;
                int skip = (pageNumber - 1) * pageSize;
                int totalItem = vetSchedules.Count;
                int totalPage = (int)Math.Ceiling((double)totalItem / pageSize);

                var pagedSchedules = vetSchedules
                    .Skip(skip)
                    .Take(pageSize)
                    .ToList();

                var responseData = new MegaData<VetScheduleDTO>
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
                    PageData = pagedSchedules.Select(vs => new VetScheduleDTO
                    {
                        VetScheduleId = vs.VetScheduleId,
                        VetId = vs.VetId,
                        ScheduleDate = vs.ScheduleDate,
                        SlotNumber = vs.SlotNumber,
                        Status = vs.Status,
                        CreatedAt = vs.CreatedAt,
                        CreatedBy = vs.CreatedBy
                    }).ToList()
                };

                if (!pagedSchedules.Any())
                {
                    return new DynamicResponse<VetScheduleDTO>
                    {
                        Code = 200,
                        Success = false,
                        Message = "No vet schedules found.",
                        Data = responseData
                    };
                }
                return new DynamicResponse<VetScheduleDTO>
                {
                    Code = 200,
                    Success = true,
                    Message = "Vet schedules retrieved successfully.",
                    Data = responseData
                };
            }
            catch (Exception ex)
            {
                return new DynamicResponse<VetScheduleDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = $"An error occurred while retrieving vet schedules: {ex.Message}",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<VetScheduleDTO>> GetVetScheduleByIdAsync(int vetScheduleId, CancellationToken cancellationToken)
        {
            try
            {
                var vetSchedule = await _vetScheduleRepository.GetVetScheduleByIdAsync(vetScheduleId, cancellationToken);
                if (vetSchedule == null)
                {
                    return new BaseResponse<VetScheduleDTO>
                    {
                        Success = false,
                        Message = "Vet schedule not found",
                        Code = 200,
                        Data = null
                    };
                }
                return new BaseResponse<VetScheduleDTO>
                {
                    Success = true,
                    Message = "Vet schedule retrieved successfully",
                    Code = 200,
                    Data = new VetScheduleDTO
                    {
                        VetScheduleId = vetSchedule.VetScheduleId,
                        VetId = vetSchedule.VetId,
                        ScheduleDate = vetSchedule.ScheduleDate,
                        SlotNumber = vetSchedule.SlotNumber,
                        Status = vetSchedule.Status,
                        CreatedAt = vetSchedule.CreatedAt,
                        CreatedBy = vetSchedule.CreatedBy
                    }
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<VetScheduleDTO>
                {
                    Success = false,
                    Message = $"An error occurred while retrieving the vet schedule: {ex.Message}",
                    Code = 500,
                    Data = null
                };
            }
        }
        public async Task<BaseResponse<List<VetScheduleDTO>>> CreateVetScheduleAsync(CreateVetScheduleRequestDTO request, CancellationToken cancellationToken)
        {
            try
            {
                var vet = await _vetRepository.GetVetByIdAsync(request.VetId, cancellationToken);
                if (vet == null)
                {
                    return new BaseResponse<List<VetScheduleDTO>>
                    {
                        Success = false,
                        Message = "Vet not found",
                        Code = 200,
                        Data = null
                    };
                }

                var createdSchedules = new List<VetScheduleDTO>();

                foreach (var schedule in request.Schedules)
                {
                    var date = schedule.ScheduleDate.Date;

                    if (date < DateTime.UtcNow.Date)
                    {
                        return new BaseResponse<List<VetScheduleDTO>>
                        {
                            Success = false,
                            Message = $"Không thể tạo lịch cho ngày trong quá khứ: {date}",
                            Code = 200,
                            Data = null
                        };
                    }
                        foreach (var slot in schedule.SlotNumbers)
                    {
                        var vetSchedule = new VetSchedule
                        {
                            VetId = request.VetId,
                            ScheduleDate = date,
                            SlotNumber = slot,
                            Status = request.Status,
                            CreatedAt = DateTime.UtcNow,
                            CreatedBy = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "system"
                        };

                        var createdId = await _vetScheduleRepository.CreateVetScheduleAsync(vetSchedule, cancellationToken);
                        if (createdId > 0)
                        {
                            createdSchedules.Add(new VetScheduleDTO
                            {
                                VetScheduleId = vetSchedule.VetScheduleId,
                                VetId = vetSchedule.VetId,
                                ScheduleDate = vetSchedule.ScheduleDate,
                                SlotNumber = vetSchedule.SlotNumber,
                                Status = vetSchedule.Status,
                                CreatedAt = vetSchedule.CreatedAt,
                                CreatedBy = vetSchedule.CreatedBy
                            });
                        }
                    }
                }

                if (!createdSchedules.Any())
                {
                    return new BaseResponse<List<VetScheduleDTO>>
                    {
                        Success = false,
                        Message = "Không tạo được lịch hẹn nào.",
                        Code = 200,
                        Data = null
                    };
                }

                return new BaseResponse<List<VetScheduleDTO>>
                {
                    Success = true,
                    Message = "Tạo lịch hẹn thành công.",
                    Code = 201,
                    Data = createdSchedules
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<VetScheduleDTO>>
                {
                    Success = false,
                    Message = $"Lỗi khi tạo lịch hẹn: {ex.Message} - {ex.InnerException}",
                    Code = 500,
                    Data = null
                };
            }
        }


        public async Task<BaseResponse<VetScheduleDTO>> UpdateVetScheduleAsync(UpdateVetScheduleRequestDTO request, CancellationToken cancellationToken)
        {
            try
            {
                var vetSchedule = await _vetScheduleRepository.GetVetScheduleByIdAsync(request.vetScheduleId, cancellationToken);
                if (vetSchedule == null)
                {
                    return new BaseResponse<VetScheduleDTO>
                    {
                        Success = false,
                        Message = "Vet schedule not found",
                        Code = 200,
                        Data = null
                    };
                }

                vetSchedule.VetId = request.VetId;
                vetSchedule.ScheduleDate = request.ScheduleDate;
                vetSchedule.SlotNumber = request.SlotNumber;
                vetSchedule.Status = request.Status;
                vetSchedule.ModifiedAt = DateTime.UtcNow;
                var updatedVetScheduleId = await _vetScheduleRepository.UpdateVetScheduleAsync(vetSchedule, cancellationToken);
                if (updatedVetScheduleId <= 0)
                {
                    return new BaseResponse<VetScheduleDTO>
                    {
                        Success = false,
                        Message = "Failed to update vet schedule",
                        Code = 200,
                        Data = null
                    };
                }
                return new BaseResponse<VetScheduleDTO>
                {
                    Success = true,
                    Message = "Vet schedule updated successfully",
                    Code = 200,
                    Data = new VetScheduleDTO
                    {
                        VetScheduleId = updatedVetScheduleId,
                        VetId = vetSchedule.VetId,
                        ScheduleDate = vetSchedule.ScheduleDate,
                        SlotNumber = vetSchedule.SlotNumber,
                        Status = vetSchedule.Status,
                        CreatedAt = vetSchedule.CreatedAt,
                        CreatedBy = vetSchedule.CreatedBy
                    }
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<VetScheduleDTO>
                {
                    Success = false,
                    Message = $"An error occurred while updating the vet schedule: {ex.Message}",
                    Code = 500,
                    Data = null
                };
            }
        }

        public async Task<List<BaseResponse<VetScheduleDTO>>> GetAllVetSchedulesByVetIdAsync(int vetId, CancellationToken cancellationToken)
        {
            try
            {
                var vetSchedules = await _vetScheduleRepository.GetVetSchedulesByVetIdAsync(vetId, cancellationToken);
                if (vetSchedules == null || !vetSchedules.Any())
                {
                    return new List<BaseResponse<VetScheduleDTO>>()
                    {
                        new BaseResponse<VetScheduleDTO>
                        {
                            Success = false,
                            Message = "No vet schedules found",
                            Code = 200,
                            Data = null
                        }
                    };
                }
                return vetSchedules.Select(vs => new BaseResponse<VetScheduleDTO>
                {
                    Success = true,
                    Message = "Vet schedules retrieved successfully",
                    Code = 200,
                    Data = new VetScheduleDTO
                    {
                        VetScheduleId = vs.VetScheduleId,
                        VetId = vs.VetId,
                        ScheduleDate = vs.ScheduleDate,
                        SlotNumber = vs.SlotNumber,
                        Status = vs.Status,
                        CreatedAt = vs.CreatedAt,
                        CreatedBy = vs.CreatedBy
                    }
                }).ToList();
            }
            catch (Exception ex)
            {
                return new List<BaseResponse<VetScheduleDTO>>()
                {
                    new BaseResponse<VetScheduleDTO>
                    {
                        Success = false,
                        Message = $"An error occurred while retrieving vet schedules: {ex.Message}",
                        Code = 500,
                        Data = null
                    }
                };
            }
        }

        public async Task<BaseResponse<VetScheduleDTO>> DeleteVetScheduleAsync(int vetScheduleId, CancellationToken cancellationToken)
        {
            try
            {
                var vetSchedule = await _vetScheduleRepository.GetVetScheduleByIdAsync(vetScheduleId, cancellationToken);
                if (vetSchedule == null)
                {
                    return new BaseResponse<VetScheduleDTO>
                    {
                        Success = false,
                        Message = "Failed to delete vet schedule",
                        Code = 200,
                        Data = null
                    };
                }
                
                var isDeleted = await _vetScheduleRepository.DeleteVetScheduleAsync(vetScheduleId, cancellationToken);
                if (!isDeleted)
                {
                    return new BaseResponse<VetScheduleDTO>
                    {
                        Success = false,
                        Message = "Failed to delete vet schedule",
                        Code = 200,
                        Data = null
                    };
                }
                return new BaseResponse<VetScheduleDTO>
                {
                    Success = true,
                    Message = "Vet schedule deleted successfully",
                    Code = 200,
                    Data = new VetScheduleDTO
                    {
                        VetScheduleId = vetSchedule.VetScheduleId,
                        VetId = vetSchedule.VetId,
                        ScheduleDate = vetSchedule.ScheduleDate,
                        SlotNumber = vetSchedule.SlotNumber,
                        Status = vetSchedule.Status,
                        CreatedAt = vetSchedule.CreatedAt,
                        CreatedBy = vetSchedule.CreatedBy
                    }
                };
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while deleting the vet schedule: {ex.Message}", ex);
            }
        }
    }
}
