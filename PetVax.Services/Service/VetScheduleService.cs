using AutoMapper;
using Microsoft.AspNetCore.Http;
using PetVax.BusinessObjects.DTO;
using PetVax.BusinessObjects.DTO.VetDTO;
using PetVax.BusinessObjects.DTO.VetScheduleDTO;
using PetVax.BusinessObjects.Enum;
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
    public class VetScheduleService : IVetScheduleService
    {
        private readonly IVetScheduleRepository _vetScheduleRepository;
        private readonly IVetRepository _vetRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        private readonly IMapper _mapper;
        public VetScheduleService(IVetScheduleRepository vetScheduleRepository, IVetRepository vetRepository, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            _vetScheduleRepository = vetScheduleRepository;
            _vetRepository = vetRepository;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
        }

        public async Task<DynamicResponse<VetScheduleDTO>> GetAllVetSchedulesAsync(GetAllItemsDTO getAllItemsDTO, CancellationToken cancellationToken)
        {
            try
            {
                var vetSchedules = await _vetScheduleRepository.GetAllVetSchedulesAsync(getAllItemsDTO.PageNumber, getAllItemsDTO.PageSize, getAllItemsDTO.KeyWord, cancellationToken);

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
                        status = getAllItemsDTO?.Status
                    },
                    PageData = pagedSchedules.Select(vs => _mapper.Map<VetScheduleDTO>(vs)).ToList()
                };

                if (!pagedSchedules.Any())
                {
                    return new DynamicResponse<VetScheduleDTO>
                    {
                        Code = 200,
                        Success = true,
                        Message = "Không tìm thấy lịch làm việc của vet.",
                        Data = responseData
                    };
                }
                return new DynamicResponse<VetScheduleDTO>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy lịch làm việc của vet thành công.",
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

                var vetScheduleDto = _mapper.Map<VetScheduleDTO>(vetSchedule);

                return new BaseResponse<VetScheduleDTO>
                {
                    Success = true,
                    Message = "Vet schedule retrieved successfully",
                    Code = 200,
                    Data = vetScheduleDto
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
                var duplicateSlots = new List<int>();

                foreach (var schedule in request.Schedules)
                {
                    var date = schedule.ScheduleDate.Date;

                    if (date < DateTimeHelper.Now().Date)
                    {
                        return new BaseResponse<List<VetScheduleDTO>>
                        {
                            Success = false,
                            Message = $"Không thể tạo lịch cho ngày trong quá khứ: {date}",
                            Code = 200,
                            Data = null
                        };
                    }

                    // Get all existing schedules for this vet and date
                    var existingSchedules = await _vetScheduleRepository.GetVetSchedulesByVetIdAsync(request.VetId, cancellationToken);
                    var existingSlotsToday = existingSchedules
                        .Where(x => x.ScheduleDate.Date == date)
                        .Select(x => x.SlotNumber)
                        .ToHashSet();

                    foreach (var slot in schedule.SlotNumbers)
                    {
                        if (existingSlotsToday.Contains(slot))
                        {
                            duplicateSlots.Add(slot);
                            continue;
                        }

                        var vetSchedule = new VetSchedule
                        {
                            VetId = request.VetId,
                            ScheduleDate = date,
                            SlotNumber = slot,
                            Status = EnumList.VetScheduleStatus.Available,
                            CreatedAt = DateTimeHelper.Now(),
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
                                Status = EnumList.VetScheduleStatus.Available,
                                CreatedAt = vetSchedule.CreatedAt,
                                CreatedBy = vetSchedule.CreatedBy
                            });
                        }
                    }
                }

                if (!createdSchedules.Any())
                {
                    string message = "Không tạo được lịch hẹn nào.";
                    if (duplicateSlots.Any())
                    {
                        message += $" Các slot đã tồn tại trong ngày: {string.Join(", ", duplicateSlots.Distinct())}.";
                    }
                    return new BaseResponse<List<VetScheduleDTO>>
                    {
                        Success = false,
                        Message = message,
                        Code = 200,
                        Data = null
                    };
                }

                string successMessage = "Tạo lịch hẹn thành công.";
                if (duplicateSlots.Any())
                {
                    successMessage += $" Các slot đã tồn tại trong ngày và không được tạo: {string.Join(", ", duplicateSlots.Distinct())}.";
                }

                return new BaseResponse<List<VetScheduleDTO>>
                {
                    Success = true,
                    Message = successMessage,
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
                        Message = "Không tìm thấy lịch làm việc của vet",
                        Code = 200,
                        Data = null
                    };
                }

                // Check for duplicate slot in the same day for the same vet, excluding the current schedule
                var existingSchedules = await _vetScheduleRepository.GetVetSchedulesByVetIdAsync(request.VetId, cancellationToken);
                bool isDuplicate = existingSchedules.Any(x =>
                    x.ScheduleDate.Date == request.ScheduleDate.Date &&
                    x.SlotNumber == request.SlotNumber &&
                    x.VetScheduleId != request.vetScheduleId);

                if (isDuplicate)
                {
                    return new BaseResponse<VetScheduleDTO>
                    {
                        Success = false,
                        Message = $"Slot {request.SlotNumber} vày ngày {request.ScheduleDate:dd-MM-yyyy} đã được lên lịch cho vet.",
                        Code = 200,
                        Data = null
                    };
                }

                vetSchedule.VetId = request.VetId;
                vetSchedule.ScheduleDate = request.ScheduleDate;
                vetSchedule.SlotNumber = request.SlotNumber;
                vetSchedule.Status = EnumList.VetScheduleStatus.Available;
                vetSchedule.ModifiedAt = DateTimeHelper.Now();
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
                    Data = _mapper.Map<VetScheduleDTO>(vs)
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

        public async Task<List<BaseResponse<VetScheduleDTO>>> GetAllVetSchedulesByDateAndSlotAsync(DateTime? date, int? slot, CancellationToken cancellationToken)
        {
            try
            {
                var vetSchedules = await _vetScheduleRepository.GetVetSchedulesByDateAndSlotAsync(date, slot, cancellationToken);

                if (date != null && slot <= 0)
                {
                    vetSchedules = await _vetScheduleRepository.GetVetSchedulesByDateAsync(date, cancellationToken);
                    if (vetSchedules == null || !vetSchedules.Any())
                    {
                        return new List<BaseResponse<VetScheduleDTO>>()
                        {
                            new BaseResponse<VetScheduleDTO>
                            {
                                Success = false,
                                Message = "Không có lịch làm theo ngày đã chọn",
                                Code = 200,
                                Data = null
                            }
                        };
                    }
                }
                else if (slot > 0 && date == null)
                {
                    vetSchedules = await _vetScheduleRepository.GetVetSchedulesBySlotAsync(slot, cancellationToken);
                    if (vetSchedules == null || !vetSchedules.Any())
                    {
                        return new List<BaseResponse<VetScheduleDTO>>()
                        {
                            new BaseResponse<VetScheduleDTO>
                            {
                                Success = false,
                                Message = "Không có lịch làm theo slot đã chọn",
                                Code = 200,
                                Data = null
                            }
                        };
                    }
                }

                return vetSchedules.Select(vs => new BaseResponse<VetScheduleDTO>
                {
                    Success = true,
                    Message = "Vet schedules retrieved successfully",
                    Code = 200,
                    Data = _mapper.Map<VetScheduleDTO>(vs)
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

        public async Task<List<BaseResponse<VetScheduleDTO>>> GetVetScheduleFromDateToDate(DateTime? fromDate, DateTime? toDate, CancellationToken cancellationToken)
        {
            try
            {
                var vetSchedules = await _vetScheduleRepository.GetVetSchedulesFromDateToDateAsync(fromDate, toDate, cancellationToken);
                if (vetSchedules == null || !vetSchedules.Any())
                {
                    return new List<BaseResponse<VetScheduleDTO>>()
                    {
                        new BaseResponse<VetScheduleDTO>
                        {
                            Success = false,
                            Message = "Không tìm thấy lịch làm việc trong khoảng thời gian này",
                            Code = 200,
                            Data = null
                        }
                    };
                }
                return vetSchedules.Select(vs => new BaseResponse<VetScheduleDTO>
                {
                    Success = true,
                    Message = "Lấy lịch làm việc thành công",
                    Code = 200,
                    Data = _mapper.Map<VetScheduleDTO>(vs)
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
    }
}
