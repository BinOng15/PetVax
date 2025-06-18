using AutoMapper;
using Microsoft.Extensions.Logging;
using PetVax.BusinessObjects.DTO.DiseaseDTO;
using PetVax.BusinessObjects.DTO.VaccineProfileDTO;
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

                        Disease = new DiseaseResponseDTO
                        {
                            DiseaseId = vp.Disease.DiseaseId,
                            Name = vp.Disease.Name,
                            Description = vp.Disease.Description,
                            Species = vp.Disease.Species,
                            Symptoms = vp.Disease.Symptoms,
                            Treatment = vp.Disease.Treatment
                        }
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
                        Disease = new DiseaseResponseDTO
                        {
                            DiseaseId = vaccineProfile.Disease.DiseaseId,
                            Name = vaccineProfile.Disease.Name,
                            Description = vaccineProfile.Disease.Description,
                            Species = vaccineProfile.Disease.Species,
                            Symptoms = vaccineProfile.Disease.Symptoms,
                            Treatment = vaccineProfile.Disease.Treatment
                        }
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
                var pet = await _petRepository.GetPetByIdAsync(petId, cancellationToken);
                if (pet == null)
                {
                    return new BaseResponse<VaccineProfileResponseDTO>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Pet not found."
                    };
                }
                var vaccineProfile = await _vaccineProfileRepository.GetVaccineProfileByPetIdAsync(petId, cancellationToken);
                if (vaccineProfile == null)
                {
                    return new BaseResponse<VaccineProfileResponseDTO>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Vaccine profile for the specified pet not found."
                    };
                }
                return new BaseResponse<VaccineProfileResponseDTO>
                {
                    Code = 200,
                    Data = _mapper.Map<VaccineProfileResponseDTO>(vaccineProfile),
                    Success = true,
                    Message = "Vaccine profile for the specified pet retrieved successfully."
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<VaccineProfileResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = $"An error occurred while retrieving the vaccine profile for the specified pet: {ex.Message}"
                };
            }
        }

        public async Task<BaseResponse<VaccineProfileResponseDTO>> CreateVaccineProfileAsync(VaccineProfileRequestDTO vaccineProfileRequest, CancellationToken cancellationToken)
        {
            try
            {

                VaccineProfile vaccineProfile = new VaccineProfile();

                vaccineProfile.PetId = vaccineProfileRequest.PetId;
                vaccineProfile.DiseaseId = vaccineProfileRequest.DiseaseId;
                vaccineProfile.PreferedDate = vaccineProfileRequest.PreferedDate;
                vaccineProfile.VaccinationDate = vaccineProfileRequest.VaccinationDate;
                vaccineProfile.NextVaccinationInfo = vaccineProfileRequest.NextVaccinationInfo;
                vaccineProfile.Dose = vaccineProfileRequest.Dose;
                vaccineProfile.Reaction = vaccineProfileRequest.Reaction;
                vaccineProfile.CreatedAt = DateTime.UtcNow;
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
                        CreatedAt = DateTime.UtcNow, // Assuming CreatedAt is set to current time
                        Disease = new DiseaseResponseDTO
                        {
                            DiseaseId = vaccineProfile.Disease?.DiseaseId ?? 0, // Assuming Disease is optional
                            Name = vaccineProfile.Disease?.Name ?? string.Empty,
                            Description = vaccineProfile.Disease?.Description ?? string.Empty,
                            Species = vaccineProfile.Disease?.Species ?? string.Empty,
                            Symptoms = vaccineProfile.Disease?.Symptoms ?? string.Empty,
                            Treatment = vaccineProfile.Disease?.Treatment ?? string.Empty
                        }
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
                existingVaccineProfile.DiseaseId = vaccineProfileRequest.DiseaseId;
                existingVaccineProfile.PreferedDate = vaccineProfileRequest.PreferedDate;
                existingVaccineProfile.VaccinationDate = vaccineProfileRequest.VaccinationDate;
                existingVaccineProfile.Dose = vaccineProfileRequest.Dose;
                existingVaccineProfile.Reaction = vaccineProfileRequest.Reaction;
                existingVaccineProfile.NextVaccinationInfo = vaccineProfileRequest.NextVaccinationInfo;
                existingVaccineProfile.ModifiedAt = DateTime.UtcNow;
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
                        Disease = new DiseaseResponseDTO
                        {
                            DiseaseId = existingVaccineProfile.Disease?.DiseaseId ?? 0, // Assuming Disease is optional
                            Name = existingVaccineProfile.Disease?.Name ?? string.Empty,
                            Description = existingVaccineProfile.Disease?.Description ?? string.Empty,
                            Species = existingVaccineProfile.Disease?.Species ?? string.Empty,
                            Symptoms = existingVaccineProfile.Disease?.Symptoms ?? string.Empty,
                            Treatment = existingVaccineProfile.Disease?.Treatment ?? string.Empty
                        }
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
    }
}
                