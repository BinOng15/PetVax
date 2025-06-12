using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PetVax.BusinessObjects.DTO;
using PetVax.BusinessObjects.DTO.CustomerDTO;
using PetVax.BusinessObjects.DTO.VaccineDTO;
using PetVax.BusinessObjects.Models;
using PetVax.Repositories.IRepository;
using PetVax.Services.ExternalService;
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
    public class VaccineService : IVaccineService
    {
        private readonly IVaccineRepository _vaccineRepository;
        private readonly IVaccineDiseaseRepository _vaccineDiseaseRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<VaccineService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICloudinariService _cloudinariService;

        public VaccineService(IVaccineRepository vaccineRepository, IVaccineDiseaseRepository vaccineDiseaseRepository, IMapper mapper, ILogger<VaccineService> logger, IHttpContextAccessor httpContextAccessor, ICloudinariService cloudinariService)
        {
            _vaccineRepository = vaccineRepository;
            _vaccineDiseaseRepository = vaccineDiseaseRepository;
            _mapper = mapper;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _cloudinariService = cloudinariService;
        }

        public async Task<BaseResponse<VaccineResponseDTO>> CreateVaccineAsync(CreateVaccineDTO createVaccineDTO, CancellationToken cancellationToken)
        {
            if (createVaccineDTO == null)
            {
                return new BaseResponse<VaccineResponseDTO>
                {
                    Code = 400,
                    Success = false,
                    Message = "Invalid vaccine data provided.",
                };
            }
            try
            {
                var vaccine = _mapper.Map<Vaccine>(createVaccineDTO);
                if (createVaccineDTO.Image != null)
                {
                    vaccine.Image = await _cloudinariService.UploadImage(createVaccineDTO.Image);
                }

                vaccine.Name = createVaccineDTO.Name;
                // Generate VaccineCode: "VX" + 6 random digits
                var random = new Random();
                vaccine.VaccineCode = "VX" + random.Next(0, 1000000).ToString("D6");
                vaccine.Image = await _cloudinariService.UploadImage(createVaccineDTO.Image);
                vaccine.Status = "Active";
                vaccine.CreatedAt = DateTime.UtcNow;
                vaccine.CreatedBy = GetCurrentUserName(); // Or get from context if available

                var createdVaccineId = await _vaccineRepository.CreateVaccineAsync(vaccine, cancellationToken);
                if (createdVaccineId <= 0)
                {
                    return new BaseResponse<VaccineResponseDTO>
                    {
                        Code = 500,
                        Success = false,
                        Message = "Failed to create vaccine."
                    };
                }

                // Get the created vaccine from DB to ensure all fields (like VaccineId) are set
                var createdVaccine = await _vaccineRepository.GetVaccineByIdAsync(vaccine.VaccineId != 0 ? vaccine.VaccineId : createdVaccineId, cancellationToken);
                var responseDTO = _mapper.Map<VaccineResponseDTO>(createdVaccine ?? vaccine);

                return new BaseResponse<VaccineResponseDTO>
                {
                    Code = 201,
                    Success = true,
                    Message = "Vaccine created successfully.",
                    Data = responseDTO
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating vaccine");
                return new BaseResponse<VaccineResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "An error occurred while creating the vaccine."
                };
            }
        }

        public async Task<BaseResponse<bool>> DeleteVaccineAsync(int vaccineId, CancellationToken cancellationToken)
        {
            if (vaccineId <= 0)
            {
                return new BaseResponse<bool>
                {
                    Code = 400,
                    Success = false,
                    Message = "Invalid vaccine ID provided."
                };
            }
            try
            {
                var isDeleted = await _vaccineRepository.DeleteVaccineAsync(vaccineId, cancellationToken);
                if (!isDeleted)
                {
                    return new BaseResponse<bool>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Vaccine not found."
                    };
                }
                return new BaseResponse<bool>
                {
                    Code = 200,
                    Success = true,
                    Message = "Vaccine deleted successfully.",
                    Data = true
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting vaccine");
                return new BaseResponse<bool>
                {
                    Code = 500,
                    Success = false,
                    Message = "An error occurred while deleting the vaccine."
                };
            }
        }

        public async Task<DynamicResponse<VaccineResponseDTO>> GetAllVaccineAsync(GetAllItemsDTO getAllItemsDTO, CancellationToken cancellationToken)
        {
            try
            {
                var vaccines = await _vaccineRepository.GetAllVaccineAsync(cancellationToken);
                if (!string.IsNullOrWhiteSpace(getAllItemsDTO?.KeyWord))
                {
                    var keyword = getAllItemsDTO.KeyWord.Trim().ToLower();
                    vaccines = vaccines.Where(v => v.Name.ToLower().Contains(keyword) || v.VaccineCode.ToLower().Contains(keyword)).ToList();
                }

                int pageNumber = getAllItemsDTO?.PageNumber > 0 ? getAllItemsDTO.PageNumber : 1;
                int pageSize = getAllItemsDTO?.PageSize > 0 ? getAllItemsDTO.PageSize : 10;
                int skip = (pageNumber - 1) * pageSize;
                int totalItem = vaccines.Count;
                int totalPage = (int)Math.Ceiling((double)totalItem / pageSize);

                var pagedVaccines = vaccines
                    .Skip(skip)
                    .Take(pageSize)
                    .ToList();

                var responseData = new MegaData<VaccineResponseDTO>
                {
                    PageInfo = new PagingMetaData
                    {
                        Page = pageNumber,
                        Size = pageSize,
                        TotalItem = totalItem,
                        TotalPage = totalPage,
                        Sort = null,
                        Order = null
                    },
                    SearchInfo = new SearchCondition
                    {
                        keyWord = getAllItemsDTO?.KeyWord,
                    },
                    PageData = _mapper.Map<List<VaccineResponseDTO>>(pagedVaccines)
                };
                if (!pagedVaccines.Any())
                {
                    return new DynamicResponse<VaccineResponseDTO>
                    {
                        Code = 404,
                        Success = false,
                        Message = "No vaccines found.",
                        Data = responseData
                    };
                }
                return new DynamicResponse<VaccineResponseDTO>
                {
                    Code = 200,
                    Success = true,
                    Message = "Vaccines retrieved successfully.",
                    Data = responseData
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving vaccines");
                return new DynamicResponse<VaccineResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "An error occurred while retrieving vaccines."
                };
            }
        }

        public async Task<BaseResponse<VaccineResponseDTO>> GetVaccineByDiseaseIdAsync(int diseaseId, CancellationToken cancellationToken)
        {
            if (diseaseId <= 0)
            {
                return new BaseResponse<VaccineResponseDTO>
                {
                    Code = 400,
                    Success = false,
                    Message = "Invalid disease ID provided."
                };
            }
            try
            {
                // Get VaccineDisease by diseaseId
                var vaccineDisease = await _vaccineDiseaseRepository.GetVaccineDiseaseByDiseaseIdAsync(diseaseId, cancellationToken);
                if (vaccineDisease == null)
                {
                    return new BaseResponse<VaccineResponseDTO>
                    {
                        Code = 404,
                        Success = false,
                        Message = "No vaccine found for the specified disease."
                    };
                }

                // Get Vaccine by VaccineId from VaccineDisease
                var vaccine = await _vaccineRepository.GetVaccineByIdAsync(vaccineDisease.VaccineId, cancellationToken);
                if (vaccine == null)
                {
                    return new BaseResponse<VaccineResponseDTO>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Vaccine not found for the specified disease."
                    };
                }
                var responseDTO = _mapper.Map<VaccineResponseDTO>(vaccine);
                return new BaseResponse<VaccineResponseDTO>
                {
                    Code = 200,
                    Success = true,
                    Message = "Vaccine retrieved successfully.",
                    Data = responseDTO
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving vaccine by disease ID");
                return new BaseResponse<VaccineResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "An error occurred while retrieving the vaccine."
                };
            }
        }

        public async Task<BaseResponse<VaccineResponseDTO>> GetVaccineByIdAsync(int vaccineId, CancellationToken cancellationToken)
        {
            if (vaccineId <= 0)
            {
                return new BaseResponse<VaccineResponseDTO>
                {
                    Code = 400,
                    Success = false,
                    Message = "Invalid vaccine ID provided."
                };
            }
            try
            {
                var vaccine = await _vaccineRepository.GetVaccineByIdAsync(vaccineId, cancellationToken);
                if (vaccine == null)
                {
                    return new BaseResponse<VaccineResponseDTO>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Vaccine not found."
                    };
                }
                var responseDTO = _mapper.Map<VaccineResponseDTO>(vaccine);
                return new BaseResponse<VaccineResponseDTO>
                {
                    Code = 200,
                    Success = true,
                    Message = "Vaccine retrieved successfully.",
                    Data = responseDTO
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving vaccine by ID");
                return new BaseResponse<VaccineResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "An error occurred while retrieving the vaccine."
                };
            }
        }

        public async Task<BaseResponse<VaccineResponseDTO>> GetVaccineByNameAsync(string Name, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                return new BaseResponse<VaccineResponseDTO>
                {
                    Code = 400,
                    Success = false,
                    Message = "Invalid vaccine name provided."
                };
            }
            try
            {
                var vaccine = await _vaccineRepository.GetVaccineByName(Name, cancellationToken);
                if (vaccine == null)
                {
                    return new BaseResponse<VaccineResponseDTO>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Vaccine not found."
                    };
                }
                var responseDTO = _mapper.Map<VaccineResponseDTO>(vaccine);
                return new BaseResponse<VaccineResponseDTO>
                {
                    Code = 200,
                    Success = true,
                    Message = "Vaccine retrieved successfully.",
                    Data = responseDTO
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving vaccine by name");
                return new BaseResponse<VaccineResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "An error occurred while retrieving the vaccine."
                };
            }
        }

        public async Task<BaseResponse<VaccineResponseDTO>> GetVaccineByVaccineCodeAsync(string vaccineCode, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(vaccineCode))
            {
                return new BaseResponse<VaccineResponseDTO>
                {
                    Code = 400,
                    Success = false,
                    Message = "Invalid vaccine code provided."
                };
            }
            try
            {
                var vaccine = await _vaccineRepository.GetVaccineByVaccineCodeAsync(vaccineCode, cancellationToken);
                if (vaccine == null)
                {
                    return new BaseResponse<VaccineResponseDTO>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Vaccine not found."
                    };
                }
                var responseDTO = _mapper.Map<VaccineResponseDTO>(vaccine);
                return new BaseResponse<VaccineResponseDTO>
                {
                    Code = 200,
                    Success = true,
                    Message = "Vaccine retrieved successfully.",
                    Data = responseDTO
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving vaccine by code");
                return new BaseResponse<VaccineResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "An error occurred while retrieving the vaccine."
                };
            }
        }

        public async Task<BaseResponse<VaccineResponseDTO>> UpdateVaccineAsync(int vaccineId, UpdateVaccineDTO updateVaccineDTO, CancellationToken cancellationToken)
        {
            if (updateVaccineDTO == null)
            {
                return new BaseResponse<VaccineResponseDTO>
                {
                    Code = 400,
                    Success = false,
                    Message = "Invalid vaccine data provided."
                };
            }
            try
            {
                var existingVaccine = await _vaccineRepository.GetVaccineByIdAsync(vaccineId, cancellationToken);
                if (existingVaccine == null)
                {
                    return new BaseResponse<VaccineResponseDTO>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Vaccine not found."
                    };
                }
                if (!string.IsNullOrWhiteSpace(updateVaccineDTO.Name))
                    existingVaccine.Name = updateVaccineDTO.Name;
                if (!string.IsNullOrWhiteSpace(updateVaccineDTO.Description))
                    existingVaccine.Description = updateVaccineDTO.Description;
                if (updateVaccineDTO.Price.HasValue)
                    existingVaccine.Price = updateVaccineDTO.Price.Value;
                if (!string.IsNullOrWhiteSpace(updateVaccineDTO.Notes))
                    existingVaccine.Notes = updateVaccineDTO.Notes;
                if (updateVaccineDTO.Image != null)
                    existingVaccine.Image = await _cloudinariService.UploadImage(updateVaccineDTO.Image);

                existingVaccine.ModifiedAt = DateTime.UtcNow;
                existingVaccine.ModifiedBy = GetCurrentUserName();

                var updatedVaccine = await _vaccineRepository.UpdateVaccineAsync(existingVaccine, cancellationToken);
                if (updatedVaccine <= 0)
                {
                    return new BaseResponse<VaccineResponseDTO>
                    {
                        Code = 500,
                        Success = false,
                        Message = "Failed to update vaccine."
                    };
                }
                // Get the updated vaccine from DB to ensure all fields (like VaccineId) are set
                var vaccine = await _vaccineRepository.GetVaccineByIdAsync(vaccineId, cancellationToken);
                var responseDTO = _mapper.Map<VaccineResponseDTO>(vaccine ?? existingVaccine);
                return new BaseResponse<VaccineResponseDTO>
                {
                    Code = 200,
                    Success = true,
                    Message = "Vaccine updated successfully.",
                    Data = responseDTO
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating vaccine");
                return new BaseResponse<VaccineResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "An error occurred while updating the vaccine."
                };
            }
        }
        private string GetCurrentUserName()
        {
            return _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value ?? "System";
        }
    }
}
