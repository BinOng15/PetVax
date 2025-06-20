﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PetVax.BusinessObjects.DTO;
using PetVax.BusinessObjects.DTO.DiseaseDTO;
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
    public class DiseaseService : IDiseaseService
    {
        private readonly IDiseaseRepository _diseaseRepository;
        private readonly IVaccineDiseaseRepository _vaccineDiseaseRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<DiseaseService> _logger;

        public DiseaseService(IDiseaseRepository diseaseRepository, IVaccineDiseaseRepository vaccineDiseaseRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor, ILogger<DiseaseService> logger)
        {
            _diseaseRepository = diseaseRepository;
            _vaccineDiseaseRepository = vaccineDiseaseRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }
        public async Task<BaseResponse<DiseaseResponseDTO>> CreateDiseaseAsync(CreateDiseaseDTO createDiseaseDTO, CancellationToken cancellationToken)
        {
            if (createDiseaseDTO == null)
            {
                _logger.LogError("CreateDiseaseAsync: createDiseaseDTO is null");
                return new BaseResponse<DiseaseResponseDTO>
                {
                    Code = 400,
                    Message = "Invalid request data",
                    Data = null
                };
            }
            try
            {
                var disease = _mapper.Map<Disease>(createDiseaseDTO);
                disease.Status = "Active";
                disease.CreatedAt = DateTime.UtcNow;
                disease.CreatedBy = GetCurrentUserName();

                var diseaseId = await _diseaseRepository.CreateDiseaseAsync(disease, cancellationToken);
                if (diseaseId <= 0)
                {
                    _logger.LogError("CreateDiseaseAsync: Failed to create disease");
                    return new BaseResponse<DiseaseResponseDTO>
                    {
                        Code = 500,
                        Message = "Failed to create disease",
                        Data = null
                    };
                }
                var diseaseResponse = _mapper.Map<DiseaseResponseDTO>(disease);
                diseaseResponse.DiseaseId = diseaseId;
                _logger.LogInformation($"CreateDiseaseAsync: Disease created successfully with ID {diseaseId} by {GetCurrentUserName()}");
                return new BaseResponse<DiseaseResponseDTO>
                {
                    Code = 201,
                    Message = "Disease created successfully",
                    Data = diseaseResponse
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CreateDiseaseAsync: An error occurred while creating disease");
                return new BaseResponse<DiseaseResponseDTO>
                {
                    Code = 500,
                    Message = "An error occurred while creating disease",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<bool>> DeleteDiseaseAsync(int diseaseId, CancellationToken cancellationToken)
        {
            try
            {
                var existingDisease = await _diseaseRepository.GetDiseaseByIdAsync(diseaseId, cancellationToken);
                if (existingDisease == null)
                {
                    _logger.LogError($"DeleteDiseaseAsync: Disease with ID {diseaseId} not found");
                    return new BaseResponse<bool>
                    {
                        Code = 404,
                        Message = "Disease not found",
                        Success = false
                    };
                }
                var result = await _diseaseRepository.DeleteDiseaseAsync(diseaseId, cancellationToken);
                if (!result)
                {
                    _logger.LogError($"DeleteDiseaseAsync: Failed to delete disease with ID {diseaseId}");
                    return new BaseResponse<bool>
                    {
                        Code = 500,
                        Message = "Failed to delete disease",
                        Success = false
                    };
                }
                _logger.LogInformation($"DeleteDiseaseAsync: Disease with ID {diseaseId} deleted successfully by {GetCurrentUserName()}");
                return new BaseResponse<bool>
                {
                    Code = 200,
                    Message = "Disease deleted successfully",
                    Success = true
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "DeleteDiseaseAsync: An error occurred while deleting disease");
                return new BaseResponse<bool>
                {
                    Code = 500,
                    Message = "An error occurred while deleting disease",
                    Success = false
                };
            }
        }

        public async Task<DynamicResponse<DiseaseResponseDTO>> GetAllDiseaseAsync(GetAllItemsDTO getAllItemsDTO, CancellationToken cancellationToken)
        {
            try
            {
                var diseases = await _diseaseRepository.GetAllDiseaseAsync(cancellationToken);
                if (!string.IsNullOrEmpty(getAllItemsDTO.KeyWord))
                {
                    var keyword = getAllItemsDTO.KeyWord.Trim().ToLower();
                    diseases = diseases.Where(d => d.Name.ToLower().Contains(keyword) || d.Description.ToLower().Contains(keyword)).ToList();
                }

                int pageNumber = getAllItemsDTO?.PageNumber > 0 ? getAllItemsDTO.PageNumber : 1;
                int pageSize = getAllItemsDTO?.PageSize > 0 ? getAllItemsDTO.PageSize : 10;
                int skip = (pageNumber - 1) * pageSize;
                int totalItem = diseases.Count;
                int totalPage = (int)Math.Ceiling((double)totalItem / pageSize);

                var pagedVaccines = diseases
                    .Skip(skip)
                    .Take(pageSize)
                    .ToList();

                var responseData = new MegaData<DiseaseResponseDTO>
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
                    PageData = _mapper.Map<List<DiseaseResponseDTO>>(pagedVaccines)
                };
                if (!pagedVaccines.Any())
                {
                    _logger.LogInformation("GetAllDiseaseAsync: No diseases found");
                    return new DynamicResponse<DiseaseResponseDTO>
                    {
                        Code = 404,
                        Success = false,
                        Message = "No diseases found",
                        Data = responseData
                    };
                }
                _logger.LogInformation($"GetAllDiseaseAsync: Retrieved {pagedVaccines.Count} diseases successfully");
                return new DynamicResponse<DiseaseResponseDTO>
                {
                    Code = 200,
                    Success = true,
                    Message = "Diseases retrieved successfully",
                    Data = responseData
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetAllDiseaseAsync: An error occurred while retrieving diseases");
                return new DynamicResponse<DiseaseResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "An error occurred while retrieving diseases",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<DiseaseResponseDTO>> GetDiseaseByIdAsync(int diseaseId, CancellationToken cancellationToken)
        {
            try
            {
                var disease = await _diseaseRepository.GetDiseaseByIdAsync(diseaseId, cancellationToken);
                if (disease == null)
                {
                    _logger.LogError($"GetDiseaseByIdAsync: Disease with ID {diseaseId} not found");
                    return new BaseResponse<DiseaseResponseDTO>
                    {
                        Code = 404,
                        Message = "Disease not found",
                        Data = null
                    };
                }
                var diseaseResponse = _mapper.Map<DiseaseResponseDTO>(disease);
                return new BaseResponse<DiseaseResponseDTO>
                {
                    Code = 200,
                    Message = "Disease retrieved successfully",
                    Data = diseaseResponse
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetDiseaseByIdAsync: An error occurred while retrieving disease");
                return new BaseResponse<DiseaseResponseDTO>
                {
                    Code = 500,
                    Message = "An error occurred while retrieving disease",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<DiseaseResponseDTO>> GetDiseaseByNameAsync(string name, CancellationToken cancellationToken)
        {
            try
            {
                var disease = await _diseaseRepository.GetDiseaseByName(name, cancellationToken);
                if (disease == null)
                {
                    _logger.LogError($"GetDiseaseByNameAsync: Disease with name {name} not found");
                    return new BaseResponse<DiseaseResponseDTO>
                    {
                        Code = 404,
                        Message = "Disease not found",
                        Data = null
                    };
                }
                var diseaseResponse = _mapper.Map<DiseaseResponseDTO>(disease);
                return new BaseResponse<DiseaseResponseDTO>
                {
                    Code = 200,
                    Message = "Disease retrieved successfully",
                    Data = diseaseResponse
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetDiseaseByNameAsync: An error occurred while retrieving disease by name");
                return new BaseResponse<DiseaseResponseDTO>
                {
                    Code = 500,
                    Message = "An error occurred while retrieving disease by name",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<DiseaseResponseDTO>> GetDiseaseByVaccineIdAsync(int vaccineId, CancellationToken cancellationToken)
        {
            if (vaccineId <= 0)
            {
                _logger.LogError("GetDiseaseByVaccineIdAsync: Invalid vaccine ID");
                return new BaseResponse<DiseaseResponseDTO>
                {
                    Code = 400,
                    Message = "Invalid vaccine ID",
                    Data = null
                };
            }
            try
            {
                var vaccineDisease = await _vaccineDiseaseRepository.GetVaccineDiseaseByVaccineIdAsync(vaccineId, cancellationToken);
                if (vaccineDisease == null)
                {
                    _logger.LogError($"GetDiseaseByVaccineIdAsync: No disease found for vaccine ID {vaccineId}");
                    return new BaseResponse<DiseaseResponseDTO>
                    {
                        Code = 404,
                        Message = "No disease found for the specified vaccine ID",
                        Data = null
                    };
                }

                var disease = await _diseaseRepository.GetDiseaseByIdAsync(vaccineDisease.DiseaseId, cancellationToken);
                if (disease == null)
                {
                    _logger.LogError($"GetDiseaseByVaccineIdAsync: Disease with ID {vaccineDisease.DiseaseId} not found");
                    return new BaseResponse<DiseaseResponseDTO>
                    {
                        Code = 404,
                        Message = "Disease not found",
                        Data = null
                    };
                }
                var diseaseResponse = _mapper.Map<DiseaseResponseDTO>(disease);
                return new BaseResponse<DiseaseResponseDTO>
                {
                    Code = 200,
                    Message = "Disease retrieved successfully",
                    Data = diseaseResponse
                };
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "GetDiseaseByVaccineIdAsync: An error occurred while retrieving disease by vaccine ID");
                return new BaseResponse<DiseaseResponseDTO>
                {
                    Code = 500,
                    Message = "An error occurred while retrieving disease by vaccine ID",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<DiseaseResponseDTO>> UpdateDiseaseAsync(int diseaseId, UpdateDiseaseDTO updateDiseaseDTO, CancellationToken cancellationToken)
        {
            if (updateDiseaseDTO == null)
            {
                _logger.LogError("UpdateDiseaseAsync: updateDiseaseDTO is null");
                return new BaseResponse<DiseaseResponseDTO>
                {
                    Code = 400,
                    Message = "Invalid request data",
                    Success = false
                };
            }
            try
            {
                var existingDisease = await _diseaseRepository.GetDiseaseByIdAsync(diseaseId, cancellationToken);
                if (existingDisease == null)
                {
                    _logger.LogError($"UpdateDiseaseAsync: Disease with ID {diseaseId} not found");
                    return new BaseResponse<DiseaseResponseDTO>
                    {
                        Code = 404,
                        Message = "Disease not found",
                        Success = false
                    };
                }
                if (!string.IsNullOrWhiteSpace(updateDiseaseDTO.Name))
                    existingDisease.Name = updateDiseaseDTO.Name;
                if (!string.IsNullOrWhiteSpace(updateDiseaseDTO.Description))
                    existingDisease.Description = updateDiseaseDTO.Description;
                if (!string.IsNullOrWhiteSpace(updateDiseaseDTO.Treatment))
                    existingDisease.Treatment = updateDiseaseDTO.Treatment;
                if (!string.IsNullOrWhiteSpace(updateDiseaseDTO.Symptoms))
                    existingDisease.Symptoms = updateDiseaseDTO.Symptoms;
                if (!string.IsNullOrWhiteSpace(updateDiseaseDTO.Species))
                    existingDisease.Species = updateDiseaseDTO.Species;
                existingDisease.ModifiedAt = DateTime.UtcNow;
                existingDisease.ModifiedBy = GetCurrentUserName();
                var result = await _diseaseRepository.UpdateDiseaseAsync(existingDisease, cancellationToken);
                if (result <= 0)
                {
                    _logger.LogError($"UpdateDiseaseAsync: Failed to update disease with ID {diseaseId}");
                    return new BaseResponse<DiseaseResponseDTO>
                    {
                        Code = 500,
                        Message = "Failed to update disease",
                        Success = false
                    };
                }
                var updatedDisease = await _diseaseRepository.GetDiseaseByIdAsync(diseaseId, cancellationToken);
                var diseaseResponse = _mapper.Map<DiseaseResponseDTO>(updatedDisease);
                _logger.LogInformation($"UpdateDiseaseAsync: Disease with ID {diseaseId} updated successfully by {GetCurrentUserName()}");
                return new BaseResponse<DiseaseResponseDTO>
                {
                    Code = 200,
                    Message = "Disease updated successfully",
                    Data = diseaseResponse,
                    Success = true
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "UpdateDiseaseAsync: An error occurred while updating disease");
                return new BaseResponse<DiseaseResponseDTO>
                {
                    Code = 500,
                    Message = "An error occurred while updating disease",
                    Success = false
                };
            }
        }
        private string GetCurrentUserName()
        {
            return _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value ?? "System";
        }
    }
}
