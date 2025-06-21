using AutoMapper;
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
using System.Reflection.PortableExecutable;
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
                    Message = "Dữ liệu không hợp lệ",
                    Data = null
                };
            }
            try
            {
                var existingDisease = await _diseaseRepository.GetDiseaseByName(createDiseaseDTO.Name, cancellationToken);
                if (existingDisease != null)
                {
                    _logger.LogError($"CreateDiseaseAsync: Disease with name {createDiseaseDTO.Name} already exists");
                    return new BaseResponse<DiseaseResponseDTO>
                    {
                        Code = 409,
                        Message = $"Bệnh với tên '{createDiseaseDTO.Name}' đã tồn tại trong hệ thống",
                        Data = null
                    };
                }

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
                        Message = "Lỗi khi tạo bệnh mới",
                        Data = null
                    };
                }

                var createdDisease = await _diseaseRepository.GetDiseaseByIdAsync(disease.DiseaseId != 0 ? disease.DiseaseId : diseaseId, cancellationToken);
                var diseaseResponse = _mapper.Map<DiseaseResponseDTO>(createdDisease ?? disease);
                _logger.LogInformation($"CreateDiseaseAsync: Disease created successfully with ID {diseaseId} by {GetCurrentUserName()}");
                return new BaseResponse<DiseaseResponseDTO>
                {
                    Code = 201,
                    Message = "Bệnh đã được tạo thành công",
                    Data = diseaseResponse
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CreateDiseaseAsync: An error occurred while creating disease");
                return new BaseResponse<DiseaseResponseDTO>
                {
                    Code = 500,
                    Message = "Lỗi khi tạo bệnh mới",
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
                        Message = "Bệnh không tồn tại",
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
                        Message = "Lỗi khi xóa bệnh",
                        Success = false
                    };
                }
                _logger.LogInformation($"DeleteDiseaseAsync: Disease with ID {diseaseId} deleted successfully by {GetCurrentUserName()}");
                return new BaseResponse<bool>
                {
                    Code = 200,
                    Message = "Bệnh đã được xóa thành công",
                    Success = true
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "DeleteDiseaseAsync: An error occurred while deleting disease");
                return new BaseResponse<bool>
                {
                    Code = 500,
                    Message = "Lỗi khi xóa bệnh",
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
                        Code = 200,
                        Success = false,
                        Message = "Không tìm thấy bệnh nào",
                        Data = responseData
                    };
                }
                _logger.LogInformation($"GetAllDiseaseAsync: Retrieved {pagedVaccines.Count} diseases successfully");
                return new DynamicResponse<DiseaseResponseDTO>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy danh sách bệnh thành công",
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
                    Message = "Lỗi khi lấy danh sách bệnh",
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
                        Code = 200,
                        Message = "Bệnh không tồn tại",
                        Data = null
                    };
                }
                var diseaseResponse = _mapper.Map<DiseaseResponseDTO>(disease);
                return new BaseResponse<DiseaseResponseDTO>
                {
                    Code = 200,
                    Message = "Bệnh đã được lấy thành công",
                    Data = diseaseResponse
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetDiseaseByIdAsync: An error occurred while retrieving disease");
                return new BaseResponse<DiseaseResponseDTO>
                {
                    Code = 500,
                    Message = "Lỗi khi lấy bệnh",
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
                        Code = 200,
                        Message = "Bệnh không tồn tại",
                        Data = null
                    };
                }
                var diseaseResponse = _mapper.Map<DiseaseResponseDTO>(disease);
                return new BaseResponse<DiseaseResponseDTO>
                {
                    Code = 200,
                    Message = "Bệnh đã được lấy thành công",
                    Data = diseaseResponse
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetDiseaseByNameAsync: An error occurred while retrieving disease by name");
                return new BaseResponse<DiseaseResponseDTO>
                {
                    Code = 500,
                    Message = "Lỗi khi lấy bệnh theo tên",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<List<DiseaseResponseDTO>>> GetDiseaseBySpeciesAsync(string species, CancellationToken cancellationToken)
        {
            try
            {
                if (species.ToLower() != "dog" && species.ToLower() != "cat")
                {
                    _logger.LogError($"GetDiseaseBySpeciesAsync: Invalid species {species}");
                    return new BaseResponse<List<DiseaseResponseDTO>>
                    {
                        Code = 400,
                        Message = "Loài không hợp lệ, vui lòng thử lại với loài 'Chó' hoặc 'Mèo'",
                        Data = null
                    };
                }
                var disease = await _diseaseRepository.GetDiseaseBySpecies(species, cancellationToken);
                if (disease == null)
                {
                    _logger.LogError($"GetDiseaseBySpeciesAsync: Disease with species {species} not found");
                    return new BaseResponse<List<DiseaseResponseDTO>>
                    {
                        Code = 200,
                        Message = "Bệnh không tồn tại",
                        Data = null
                    };
                }
                var diseaseResponse = _mapper.Map<List<DiseaseResponseDTO>>(disease);
                return new BaseResponse<List<DiseaseResponseDTO>>
                {
                    Code = 200,
                    Message = "Bệnh đã được lấy thành công",
                    Data = diseaseResponse
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetDiseaseBySpeciesAsync: An error occurred while retrieving disease by species");
                return new BaseResponse<List<DiseaseResponseDTO>>
                {
                    Code = 500,
                    Message = "Lỗi khi lấy bệnh theo loài",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<List<DiseaseResponseDTO>>> GetDiseaseByVaccineIdAsync(int vaccineId, CancellationToken cancellationToken)
        {
            if (vaccineId <= 0)
            {
                _logger.LogError("GetDiseaseByVaccineIdAsync: Invalid vaccine ID");
                return new BaseResponse<List<DiseaseResponseDTO>>
                {
                    Code = 400,
                    Message = "Vắc xin ID không hợp lệ",
                    Data = null
                };
            }
            try
            {
                var disease = await _diseaseRepository.GetDiseaseByVaccineId(vaccineId, cancellationToken);
                if (disease == null)
                {
                    return new BaseResponse<List<DiseaseResponseDTO>>
                    {
                        Code = 200,
                        Success = false,
                        Message = "Bệnh không tồn tại với vaccineId này",
                        Data = null
                    };
                }
                var diseaseResponse = _mapper.Map<List<DiseaseResponseDTO>>(disease);
                
                return new BaseResponse<List<DiseaseResponseDTO>>
                {
                    Code = 200,
                    Message = "Bệnh đã được lấy thành công theo vắc xin ID",
                    Data = diseaseResponse
                };
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "GetDiseaseByVaccineIdAsync: An error occurred while retrieving disease by vaccine ID");
                return new BaseResponse<List<DiseaseResponseDTO>>
                {
                    Code = 500,
                    Message = "Lỗi khi lấy bệnh theo vắc xin ID",
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
                    Message = "Dữ liệu không hợp lệ",
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
                        Message = "Bệnh không tồn tại",
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
                        Message = "Lỗi khi cập nhật bệnh",
                        Success = false
                    };
                }
                var updatedDisease = await _diseaseRepository.GetDiseaseByIdAsync(diseaseId, cancellationToken);
                var diseaseResponse = _mapper.Map<DiseaseResponseDTO>(updatedDisease);
                _logger.LogInformation($"UpdateDiseaseAsync: Disease with ID {diseaseId} updated successfully by {GetCurrentUserName()}");
                return new BaseResponse<DiseaseResponseDTO>
                {
                    Code = 200,
                    Message = "Bệnh đã được cập nhật thành công",
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
                    Message = "Lỗi khi cập nhật bệnh",
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
