using AutoMapper;
using Microsoft.Extensions.Logging;
using PetVax.BusinessObjects.DTO.AppointmentDetailDTO;
using PetVax.BusinessObjects.DTO.DiseaseDTO;
using PetVax.BusinessObjects.DTO.VaccineDTO;
using PetVax.BusinessObjects.DTO.VaccineProfileDTO;
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
    public class VaccineProfileService : IVaccineProfileService
    {
        private readonly IVaccineProfileRepository _vaccineProfileRepository;
        private readonly ILogger<VaccineProfileService> _logger;

        private readonly IMapper _mapper;
        private readonly IPetRepository _petRepository;

        private readonly IDiseaseRepository _diseaseRepo;

        public VaccineProfileService(IVaccineProfileRepository vaccineProfileRepository, ILogger<VaccineProfileService> logger, IMapper mapper, IPetRepository petRepository,
            IDiseaseRepository diseaseRepository)
        {
            _vaccineProfileRepository = vaccineProfileRepository;
            _logger = logger;
            _mapper = mapper;
            _petRepository = petRepository;
            _diseaseRepo = diseaseRepository;
        }



        public async Task<List<BaseResponse<VaccineProfileResponseDTO>>> GetAllVaccineProfilesAsync(CancellationToken cancellationToken)
        {
            try
            {
                var vaccineProfiles = await _vaccineProfileRepository.GetAllVaccineProfilesAsync(cancellationToken);

                if (vaccineProfiles == null || !vaccineProfiles.Any())
                {
                    return new List<BaseResponse<VaccineProfileResponseDTO>>();
                }
                return vaccineProfiles.Select(vp => new BaseResponse<VaccineProfileResponseDTO>
                {
                    Code = 200,
                    Data = new VaccineProfileResponseDTO
                    {
                        VaccineProfileId = vp.VaccineProfileId,
                        PetId = vp.PetId,
                        PreferedDate = vp.PreferedDate,
                        VaccinationDate = vp.VaccinationDate,
                        Dose = vp.Dose,
                        Reaction = vp.Reaction,
                        NextVaccinationInfo = vp.NextVaccinationInfo,
                        IsActive = vp.IsActive,
                        IsCompleted = vp.IsCompleted,
                        CreatedAt = vp.CreatedAt,
                    },
                    Success = true,
                    Message = "Vaccine profiles retrieved successfully."
                }).ToList();
            }
            catch (Exception ex)
            {
                return new List<BaseResponse<VaccineProfileResponseDTO>>
                {
                    new BaseResponse<VaccineProfileResponseDTO>
                    {
                        Code = 500,
                        Success = false,
                        Message = $"An error occurred while retrieving vaccine profiles: {ex.Message}"
                    }
                };
            }
        }

        public async Task<BaseResponse<VaccineProfileResponseDTO>> GetVaccineProfileByIdAsync(int vaccineProfileId, CancellationToken cancellationToken)
        {
            try
            {
                var vaccineProfile = await _vaccineProfileRepository.GetVaccineProfileByIdAsync(vaccineProfileId, cancellationToken);
                if (vaccineProfile == null)
                {
                    return new BaseResponse<VaccineProfileResponseDTO>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Vaccine profile not found."
                    };
                }
                return new BaseResponse<VaccineProfileResponseDTO>
                {
                    Code = 200,
                    Data = new VaccineProfileResponseDTO
                    {
                        VaccineProfileId = vaccineProfile.VaccineProfileId,
                        PetId = vaccineProfile.PetId,
                        PreferedDate = vaccineProfile.PreferedDate,
                        VaccinationDate = vaccineProfile.VaccinationDate,
                        Dose = vaccineProfile.Dose,
                        Reaction = vaccineProfile.Reaction,
                        NextVaccinationInfo = vaccineProfile.NextVaccinationInfo,
                        IsActive = vaccineProfile.IsActive,
                        IsCompleted = vaccineProfile.IsCompleted,
                        CreatedAt = vaccineProfile.CreatedAt,

                    },
                    Success = true,
                    Message = "Vaccine profile retrieved successfully."
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<VaccineProfileResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = $"An error occurred while retrieving the vaccine profile: {ex.Message}"
                };
            }
        }

        public async Task<BaseResponse<VaccineProfileResponseDTO>> GetVaccineProfileByPetIdAsync(int petId, CancellationToken cancellationToken)
        {
            try
            {
                var vaccineProfile = await _vaccineProfileRepository.GetVaccineProfileByPetIdAsync(petId, cancellationToken);

                if (vaccineProfile == null)
                {
                    return new BaseResponse<VaccineProfileResponseDTO>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Vaccine profile not found"
                    };
                }

                var result = _mapper.Map<VaccineProfileResponseDTO>(vaccineProfile);

                return new BaseResponse<VaccineProfileResponseDTO>
                {
                    Code = 200,
                    Success = true,
                    Data = result,
                    Message = "Vaccine profile retrieved successfully"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving vaccine profile for pet {PetId}", petId);
                return new BaseResponse<VaccineProfileResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = $"Error: {ex.Message}"
                };
            }
        }

        public async Task<BaseResponse<VaccineProfileResponseDTO>> CreateVaccineProfileAsync(VaccineProfileRequestDTO vaccineProfileRequest, CancellationToken cancellationToken)
        {
            try
            {

                VaccineProfile vaccineProfile = new VaccineProfile();

                vaccineProfile.PetId = vaccineProfileRequest.PetId;
                vaccineProfile.AppointmentDetailId = vaccineProfileRequest.AppointmentDetailId;
                vaccineProfile.PreferedDate = vaccineProfileRequest.PreferedDate;
                vaccineProfile.VaccinationDate = vaccineProfileRequest.VaccinationDate;
                vaccineProfile.NextVaccinationInfo = vaccineProfileRequest.NextVaccinationInfo;
                vaccineProfile.Dose = vaccineProfileRequest.Dose;
                vaccineProfile.Reaction = vaccineProfileRequest.Reaction;
                vaccineProfile.CreatedAt = DateTimeHelper.Now();
                vaccineProfile.IsActive = true;

                var pet = await _petRepository.GetPetByIdAsync(vaccineProfileRequest.PetId, cancellationToken);

                var disease = await _diseaseRepo.GetDiseaseByIdAsync(vaccineProfileRequest.DiseaseId, cancellationToken);

                if (pet == null)
                {
                    return new BaseResponse<VaccineProfileResponseDTO>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Pet not found."
                    };
                }

                if (disease == null)
                {
                    return new BaseResponse<VaccineProfileResponseDTO>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Disease not found."
                    };
                }

                var createVaccineProfile = await _vaccineProfileRepository.CreateVaccineProfileAsync(vaccineProfile, cancellationToken);

                if (createVaccineProfile <= 0)
                {
                    return new BaseResponse<VaccineProfileResponseDTO>
                    {
                        Code = 400,
                        Success = false,
                        Message = "Failed to create vaccine profile."
                    };
                }

                return new BaseResponse<VaccineProfileResponseDTO>
                {
                    Code = 201,
                    Data = new VaccineProfileResponseDTO
                    {
                        VaccineProfileId = vaccineProfile.VaccineProfileId,
                        PetId = vaccineProfile.PetId,
                        PreferedDate = vaccineProfile.PreferedDate,
                        VaccinationDate = vaccineProfile.VaccinationDate,
                        Dose = vaccineProfile.Dose,
                        Reaction = vaccineProfile.Reaction,
                        NextVaccinationInfo = vaccineProfile.NextVaccinationInfo,
                        IsActive = vaccineProfile.IsActive,
                        IsCompleted = vaccineProfile.IsCompleted,
                        CreatedAt = DateTimeHelper.Now(), // Assuming CreatedAt is set to current time
                    },
                    Success = true,
                    Message = "Vaccine profile created successfully."
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<VaccineProfileResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = $"An error occurred while creating the vaccine profile: {ex.Message}"
                };
            }
        }

        public async Task<BaseResponse<VaccineProfileResponseDTO>> UpdateVaccineProfileAsync(int vaccineProfileId, VaccineProfileRequestDTO vaccineProfileRequest, CancellationToken cancellationToken)
        {
            try
            {
                var existingVaccineProfile = await _vaccineProfileRepository.GetVaccineProfileByIdAsync(vaccineProfileId, cancellationToken);

                var pet = await _petRepository.GetPetByIdAsync(vaccineProfileRequest.PetId, cancellationToken);
                if (pet == null)
                {
                    return new BaseResponse<VaccineProfileResponseDTO>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Pet not found."
                    };
                }
                var disease = await _diseaseRepo.GetDiseaseByIdAsync(vaccineProfileRequest.DiseaseId, cancellationToken);
                if (disease == null)
                {
                    return new BaseResponse<VaccineProfileResponseDTO>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Disease not found."
                    };
                }
                if (existingVaccineProfile == null)
                {
                    return new BaseResponse<VaccineProfileResponseDTO>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Vaccine profile not found."
                    };
                }
                existingVaccineProfile.PetId = vaccineProfileRequest.PetId;
                existingVaccineProfile.PreferedDate = vaccineProfileRequest.PreferedDate;
                existingVaccineProfile.VaccinationDate = vaccineProfileRequest.VaccinationDate;
                existingVaccineProfile.Dose = vaccineProfileRequest.Dose;
                existingVaccineProfile.Reaction = vaccineProfileRequest.Reaction;
                existingVaccineProfile.NextVaccinationInfo = vaccineProfileRequest.NextVaccinationInfo;
                existingVaccineProfile.ModifiedAt = DateTimeHelper.Now();
                var updateResult = await _vaccineProfileRepository.UpdateVaccineProfileAsync(existingVaccineProfile, cancellationToken);
                if (updateResult <= 0)
                {
                    return new BaseResponse<VaccineProfileResponseDTO>
                    {
                        Code = 400,
                        Success = false,
                        Message = "Failed to update vaccine profile."
                    };
                }
                return new BaseResponse<VaccineProfileResponseDTO>
                {
                    Code = 200,
                    Data = new VaccineProfileResponseDTO
                    {
                        VaccineProfileId = existingVaccineProfile.VaccineProfileId,
                        PetId = existingVaccineProfile.PetId,
                        PreferedDate = existingVaccineProfile.PreferedDate,
                        VaccinationDate = existingVaccineProfile.VaccinationDate,
                        Dose = existingVaccineProfile.Dose,
                        Reaction = existingVaccineProfile.Reaction,
                        NextVaccinationInfo = existingVaccineProfile.NextVaccinationInfo,
                        IsActive = existingVaccineProfile.IsActive,
                        IsCompleted = existingVaccineProfile.IsCompleted,
                        CreatedAt = existingVaccineProfile.CreatedAt, // Assuming CreatedAt is not changed during update
                    },
                    Success = true,
                    Message = "Vaccine profile updated successfully."
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<VaccineProfileResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = $"An error occurred while updating the vaccine profile: {ex.Message}"
                };
            }
        }

        public async Task<BaseResponse<VaccineProfileResponseDTO>> DeleteVaccineProfileAsync(int vaccineProfileId, CancellationToken cancellationToken)
        {
            try
            {
                var existingVaccineProfile = await _vaccineProfileRepository.GetVaccineProfileByIdAsync(vaccineProfileId, cancellationToken);
                if (existingVaccineProfile == null)
                {
                    return new BaseResponse<VaccineProfileResponseDTO>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Vaccine profile not found."
                    };
                }
                var deleteResult = await _vaccineProfileRepository.DeleteVaccineProfileAsync(vaccineProfileId, cancellationToken);
                if (!deleteResult)
                {
                    return new BaseResponse<VaccineProfileResponseDTO>
                    {
                        Code = 400,
                        Success = false,
                        Message = "Failed to delete vaccine profile."
                    };
                }
                return new BaseResponse<VaccineProfileResponseDTO>
                {
                    Code = 200,
                    Success = true,
                    Message = "Vaccine profile deleted successfully."
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<VaccineProfileResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = $"An error occurred while deleting the vaccine profile: {ex.Message}"
                };
            }
        }

        public async Task<BaseResponse<List<VaccineProfileGroupByDiseaseResponseDTO>>> GetGroupedVaccineProfilesByPetIdAsync(int petId, CancellationToken cancellationToken)
        {
            try
            {
                var vaccineProfiles = await _vaccineProfileRepository.GetListVaccineProfileByPetIdAsync(petId, cancellationToken);

                // Map sang DTO như cũ
                var data = vaccineProfiles?.Select(vp => new VaccineProfileResponseDTO
                {
                    VaccineProfileId = vp.VaccineProfileId,
                    PetId = vp.PetId,
                    DiseaseId = vp.DiseaseId,
                    AppointmentDetailId = vp.AppointmentDetailId,
                    VaccinationScheduleId = vp.VaccinationScheduleId,
                    PreferedDate = vp.PreferedDate,
                    VaccinationDate = vp.VaccinationDate,
                    Dose = vp.Dose,
                    Reaction = vp.Reaction,
                    NextVaccinationInfo = vp.NextVaccinationInfo,
                    IsActive = vp.IsActive,
                    IsCompleted = vp.IsCompleted,
                    CreatedAt = vp.CreatedAt,
                    AppointmentDetail = vp.AppointmentDetail != null ? new AppointmentVaccinationForProfileResponseDTO
                    {
                        AppointmentDetailCode = vp.AppointmentDetail.AppointmentDetailCode,
                        VetId = vp.AppointmentDetail.VetId ?? 0,
                        ServiceType = vp.AppointmentDetail.ServiceType,
                        VaccineBatchId = vp.AppointmentDetail.VaccineBatchId,
                        Temperature = vp.AppointmentDetail.Temperature,
                        HeartRate = vp.AppointmentDetail.HeartRate,
                        GeneralCondition = vp.AppointmentDetail.GeneralCondition,
                        Others = vp.AppointmentDetail.Others,
                        Notes = vp.AppointmentDetail.Notes,
                        AppointmentDate = vp.AppointmentDetail.AppointmentDate,
                        AppointmentStatus = vp.AppointmentDetail.AppointmentStatus,
                        Vet = vp.AppointmentDetail.Vet != null ? new VetVaccineProfileResponseDTO
                        {
                            VetCode = vp.AppointmentDetail.Vet.VetCode,
                            Name = vp.AppointmentDetail.Vet.Name,
                            Specialization = vp.AppointmentDetail.Vet.Specialization,
                        } : null,
                        VaccineBatch = vp.AppointmentDetail.VaccineBatch != null ? new VaccineBatchVaccineProfileResponseDTO
                        {
                            BatchNumber = vp.AppointmentDetail.VaccineBatch.BatchNumber,
                            ManufactureDate = vp.AppointmentDetail.VaccineBatch.ManufactureDate,
                            ExpiryDate = vp.AppointmentDetail.VaccineBatch.ExpiryDate,
                            Quantity = vp.AppointmentDetail.VaccineBatch.Quantity,
                            VaccineId = vp.AppointmentDetail.VaccineBatch.VaccineId,
                            Vaccine = vp.AppointmentDetail.VaccineBatch.Vaccine != null ? new VaccineForBatchVaccineProfileResponseDTO
                            {
                                VaccineCode = vp.AppointmentDetail.VaccineBatch.Vaccine.VaccineCode,
                                Name = vp.AppointmentDetail.VaccineBatch.Vaccine.Name,
                                Description = vp.AppointmentDetail.VaccineBatch.Vaccine.Description,
                                Price = vp.AppointmentDetail.VaccineBatch.Vaccine.Price,
                                Image = vp.AppointmentDetail.VaccineBatch.Vaccine.Image,
                                Notes = vp.AppointmentDetail.VaccineBatch.Vaccine.Notes,
                                // ... các trường khác của Vaccine
                            } : null,
                            // ... các trường khác của VaccineBatch
                        } : null
                    } : null,
                    Disease = vp.Disease != null ? new DiseaseVaccineProfileResponseDTO
                    {
                        DiseaseName = vp.Disease.Name,
                    } : null
                }).ToList() ?? new List<VaccineProfileResponseDTO>();

                // Group by DiseaseId
                var grouped = data
                    .GroupBy(x => new { x.DiseaseId, x.Disease?.DiseaseName })
                    .Select(g => new VaccineProfileGroupByDiseaseResponseDTO
                    {
                        DiseaseId = g.Key.DiseaseId,
                        DiseaseName = g.Key.DiseaseName,
                        Doses = g.OrderBy(x => x.Dose).ToList()
                    })
                    .ToList();

                return new BaseResponse<List<VaccineProfileGroupByDiseaseResponseDTO>>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy danh sách hồ sơ tiêm chủng đã nhóm theo bệnh thành công.",
                    Data = grouped
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<VaccineProfileGroupByDiseaseResponseDTO>>
                {
                    Code = 500,
                    Success = false,
                    Message = $"Có lỗi khi lấy danh sách hồ sơ tiêm chủng: {ex.Message}",
                    Data = new List<VaccineProfileGroupByDiseaseResponseDTO>()
                };
            }
        }

    }
}
                