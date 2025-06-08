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
        public VetScheduleService(IVetScheduleRepository vetScheduleRepository, IVetRepository vetRepository)
        {
            _vetScheduleRepository = vetScheduleRepository;
            _vetRepository = vetRepository;
        }

        public async Task<List<BaseResponse<VetScheduleDTO>>> GetAllVetSchedulesAsync(CancellationToken cancellationToken)
        {
            try
            {
                var vetSchedules = await _vetScheduleRepository.GetAllVetSchedulesAsync(cancellationToken);
                if (vetSchedules == null || !vetSchedules.Any())
                {
                    return new List<BaseResponse<VetScheduleDTO>>()
                    {
                        new BaseResponse<VetScheduleDTO>
                        {
                            Success = false,
                            Message = "No vet schedules found",
                            Code = 404,
                            Data = default! // Use default! to explicitly indicate a nullable value
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
                        Code = 404,
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
        public async Task<BaseResponse<VetScheduleDTO>> CreateVetScheduleAsync(CreateVetScheduleRequestDTO request, CancellationToken cancellationToken)
        {
            try
            {
                var vet = await _vetRepository.GetVetByIdAsync(request.VetId, cancellationToken);
                if (vet == null)
                {
                    return new BaseResponse<VetScheduleDTO>
                    {
                        Success = false,
                        Message = "Vet not found",
                        Code = 404,
                        Data = null
                    };
                }
                var vetSchedule = new VetSchedule
                {
                    VetId = request.VetId,
                    ScheduleDate = request.ScheduleDate,
                    SlotNumber = request.SlotNumber,
                    Status = request.Status,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = "admin"
                };
                var createdVetScheduleId = await _vetScheduleRepository.CreateVetScheduleAsync(vetSchedule, cancellationToken);
                if (createdVetScheduleId <= 0)
                {
                    return new BaseResponse<VetScheduleDTO>
                    {
                        Success = false,
                        Message = "Failed to create vet schedule",
                        Code = 500,
                        Data = null
                    };
                }
                return new BaseResponse<VetScheduleDTO>
                {
                    Success = true,
                    Message = "Vet schedule created successfully",
                    Code = 201,
                    Data = new VetScheduleDTO
                    {
                        VetScheduleId = createdVetScheduleId,
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
                    Message = $"An error occurred while creating the vet schedule: {ex.Message}",
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
                        Code = 404,
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
                        Code = 500,
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
                            Code = 404,
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
                        Code = 500,
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
                        Code = 500,
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
