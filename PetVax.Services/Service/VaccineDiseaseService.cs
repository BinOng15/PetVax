using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PetVax.BusinessObjects.DTO;
using PetVax.BusinessObjects.DTO.VaccineDiseaseDTO;
using PetVax.BusinessObjects.DTO.VaccineDTO;
using PetVax.BusinessObjects.Models;
using PetVax.Repositories.IRepository;
using PetVax.Services.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static PetVax.BusinessObjects.DTO.ResponseModel;

namespace PetVax.Services.Service
{
    public class VaccineDiseaseService : IVaccineDiseaseService
    {
        private readonly IVaccineDiseaseRepository _vaccineDiseaseRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<VaccineDiseaseService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public VaccineDiseaseService(IVaccineDiseaseRepository vaccineDiseaseRepository, IMapper mapper, ILogger<VaccineDiseaseService> logger, IHttpContextAccessor httpContextAccessor)
        {
            _vaccineDiseaseRepository = vaccineDiseaseRepository;
            _mapper = mapper;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<BaseResponse<VaccineDiseaseResponseDTO>> CreateVaccineDiseaseAsync(CreateVaccineDiseaseDTO createVaccineDiseaseDTO, CancellationToken cancellationToken)
        {
            if (createVaccineDiseaseDTO == null)
            {
                return new BaseResponse<VaccineDiseaseResponseDTO>
                {
                    Code = 400,
                    Success = false,
                    Message = "CreateVaccineDiseaseDTO cannot be null"
                };
            }
            try
            {
                var vaccineDisease = _mapper.Map<VaccineDisease>(createVaccineDiseaseDTO);

                vaccineDisease.VaccineId = createVaccineDiseaseDTO.VaccineId;
                vaccineDisease.DiseaseId = createVaccineDiseaseDTO.DiseaseId;
                vaccineDisease.CreatedAt = DateTime.UtcNow;
                vaccineDisease.CreatedBy = GetCurrentUserName();

                var vaccineDiseaseId = await _vaccineDiseaseRepository.CreateVaccineDiseaseAsync(vaccineDisease, cancellationToken);
                if (vaccineDiseaseId <= 0)
                {
                    return new BaseResponse<VaccineDiseaseResponseDTO>
                    {
                        Code = 500,
                        Success = false,
                        Message = "Failed to create vaccine disease"
                    };
                }

                var responseDTO = _mapper.Map<VaccineDiseaseResponseDTO>(vaccineDisease);
                return new BaseResponse<VaccineDiseaseResponseDTO>
                {
                    Code = 201,
                    Success = true,
                    Message = "Vaccine disease created successfully",
                    Data = responseDTO
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating vaccine disease");
                return new BaseResponse<VaccineDiseaseResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "An error occurred while creating the vaccine disease"
                };
            }
        }

        public async Task<BaseResponse<bool>> DeleteVaccineDiseaseAsync(int vaccineDiseaseId, CancellationToken cancellationToken)
        {
            if (vaccineDiseaseId <= 0)
            {
                return new BaseResponse<bool>
                {
                    Code = 400,
                    Success = false,
                    Message = "Invalid vaccine disease ID"
                };
            }
            try
            {
                var isDeleted = await _vaccineDiseaseRepository.DeleteVaccineDiseaseAsync(vaccineDiseaseId, cancellationToken);
                if (!isDeleted)
                {
                    return new BaseResponse<bool>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Vaccine disease not found"
                    };
                }
                return new BaseResponse<bool>
                {
                    Code = 200,
                    Success = true,
                    Message = "Vaccine disease deleted successfully",
                    Data = true
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting vaccine disease");
                return new BaseResponse<bool>
                {
                    Code = 500,
                    Success = false,
                    Message = "An error occurred while deleting the vaccine disease"
                };
            }
        }

        public async Task<DynamicResponse<VaccineDiseaseResponseDTO>> GetAllVaccineDiseaseAsync(GetAllItemsDTO getAllItemsDTO, CancellationToken cancellationToken)
        {
            try
            {
                var vaccineDiseases = await _vaccineDiseaseRepository.GetAllVaccineDiseaseAsync(cancellationToken);

                int pageNumber = getAllItemsDTO?.PageNumber > 0 ? getAllItemsDTO.PageNumber : 1;
                int pageSize = getAllItemsDTO?.PageSize > 0 ? getAllItemsDTO.PageSize : 10;
                int skip = (pageNumber - 1) * pageSize;
                int totalItem = vaccineDiseases.Count;
                int totalPage = (int)Math.Ceiling((double)totalItem / pageSize);

                var paginatedDiseases = vaccineDiseases.Skip(skip).Take(pageSize).ToList();

                var responseData = new MegaData<VaccineDiseaseResponseDTO>
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
                    PageData = _mapper.Map<List<VaccineDiseaseResponseDTO>>(paginatedDiseases)
                };
                if (!paginatedDiseases.Any())
                {
                    return new DynamicResponse<VaccineDiseaseResponseDTO>
                    {
                        Code = 404,
                        Success = false,
                        Message = "No vaccine diseases found",
                        Data = responseData
                    };
                }
                return new DynamicResponse<VaccineDiseaseResponseDTO>
                {
                    Code = 200,
                    Success = true,
                    Message = "Vaccine diseases retrieved successfully",
                    Data = responseData
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all vaccine diseases");
                return new DynamicResponse<VaccineDiseaseResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "An error occurred while retrieving vaccine diseases"
                };
            }
        }

        public async Task<BaseResponse<VaccineDiseaseResponseDTO>> GetVaccineDiseaseByDiseaseIdAsync(int diseaseId, CancellationToken cancellationToken)
        {
            if (diseaseId <= 0)
            {
                return new BaseResponse<VaccineDiseaseResponseDTO>
                {
                    Code = 400,
                    Success = false,
                    Message = "Invalid disease ID"
                };
            }
            try
            {
                var vaccineDisease = await _vaccineDiseaseRepository.GetVaccineDiseaseByDiseaseIdAsync(diseaseId, cancellationToken);
                if (vaccineDisease == null)
                {
                    return new BaseResponse<VaccineDiseaseResponseDTO>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Vaccine disease not found"
                    };
                }
                var responseDTO = _mapper.Map<VaccineDiseaseResponseDTO>(vaccineDisease);
                return new BaseResponse<VaccineDiseaseResponseDTO>
                {
                    Code = 200,
                    Success = true,
                    Message = "Vaccine disease retrieved successfully",
                    Data = responseDTO
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving vaccine disease by disease ID");
                return new BaseResponse<VaccineDiseaseResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "An error occurred while retrieving the vaccine disease"
                };
            }
        }

        public async Task<BaseResponse<VaccineDiseaseResponseDTO>> GetVaccineDiseaseByIdAsync(int vaccineDiseaseId, CancellationToken cancellationToken)
        {
            if (vaccineDiseaseId <= 0)
            {
                return new BaseResponse<VaccineDiseaseResponseDTO>
                {
                    Code = 400,
                    Success = false,
                    Message = "Invalid vaccine disease ID"
                };
            }
            try
            {
                var vaccineDisease = await _vaccineDiseaseRepository.GetVaccineDiseaseByIdAsync(vaccineDiseaseId, cancellationToken);
                if (vaccineDisease == null)
                {
                    return new BaseResponse<VaccineDiseaseResponseDTO>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Vaccine disease not found"
                    };
                }
                var responseDTO = _mapper.Map<VaccineDiseaseResponseDTO>(vaccineDisease);
                return new BaseResponse<VaccineDiseaseResponseDTO>
                {
                    Code = 200,
                    Success = true,
                    Message = "Vaccine disease retrieved successfully",
                    Data = responseDTO
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving vaccine disease by ID");
                return new BaseResponse<VaccineDiseaseResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "An error occurred while retrieving the vaccine disease"
                };
            }
        }

        public async Task<BaseResponse<VaccineDiseaseResponseDTO>> GetVaccineDiseaseByVaccineIdAsync(int vaccineId, CancellationToken cancellationToken)
        {
            if (vaccineId <= 0)
            {
                return new BaseResponse<VaccineDiseaseResponseDTO>
                {
                    Code = 400,
                    Success = false,
                    Message = "Invalid vaccine ID"
                };
            }
            try
            {
                var vaccineDisease = await _vaccineDiseaseRepository.GetVaccineDiseaseByVaccineIdAsync(vaccineId, cancellationToken);
                if (vaccineDisease == null)
                {
                    return new BaseResponse<VaccineDiseaseResponseDTO>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Vaccine disease not found"
                    };
                }
                var responseDTO = _mapper.Map<VaccineDiseaseResponseDTO>(vaccineDisease);
                return new BaseResponse<VaccineDiseaseResponseDTO>
                {
                    Code = 200,
                    Success = true,
                    Message = "Vaccine disease retrieved successfully",
                    Data = responseDTO
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving vaccine disease by vaccine ID");
                return new BaseResponse<VaccineDiseaseResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "An error occurred while retrieving the vaccine disease"
                };
            }
        }

        public async Task<BaseResponse<VaccineDiseaseResponseDTO>> UpdateVaccineDiseaseAsync(int vaccineDiseaseId, UpdateVaccineDiseaseDTO updateVaccineDiseaseDTO, CancellationToken cancellationToken)
        {
            if (vaccineDiseaseId <= 0 || updateVaccineDiseaseDTO == null)
            {
                return new BaseResponse<VaccineDiseaseResponseDTO>
                {
                    Code = 400,
                    Success = false,
                    Message = "Invalid vaccine disease ID or update data"
                };
            }
            try
            {
                var existingVaccineDisease = await _vaccineDiseaseRepository.GetVaccineDiseaseByIdAsync(vaccineDiseaseId, cancellationToken);
                if (existingVaccineDisease == null)
                {
                    return new BaseResponse<VaccineDiseaseResponseDTO>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Vaccine disease not found"
                    };
                }
                var updatedVaccineDisease = _mapper.Map(updateVaccineDiseaseDTO, existingVaccineDisease);
                updatedVaccineDisease.ModifiedAt = DateTime.UtcNow;
                updatedVaccineDisease.ModifiedBy = GetCurrentUserName();
                var result = await _vaccineDiseaseRepository.UpdateVaccineDiseaseAsync(updatedVaccineDisease, cancellationToken);
                if (result <= 0)
                {
                    return new BaseResponse<VaccineDiseaseResponseDTO>
                    {
                        Code = 500,
                        Success = false,
                        Message = "Failed to update vaccine disease"
                    };
                }
                var responseDTO = _mapper.Map<VaccineDiseaseResponseDTO>(updatedVaccineDisease);
                return new BaseResponse<VaccineDiseaseResponseDTO>
                {
                    Code = 200,
                    Success = true,
                    Message = "Vaccine disease updated successfully",
                    Data = responseDTO
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating vaccine disease");
                return new BaseResponse<VaccineDiseaseResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "An error occurred while updating the vaccine disease"
                };
            }
        }
        private string GetCurrentUserName()
        {
            return _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value ?? "System";
        }
    }
}
