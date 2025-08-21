using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PetVax.BusinessObjects.DTO;
using PetVax.BusinessObjects.DTO.VaccineDiseaseDTO;
using PetVax.BusinessObjects.DTO.VaccineDTO;
using PetVax.BusinessObjects.Helpers;
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
                    Message = "Không có dữ liệu để tạo vaccine disease"
                };
            }
            try
            {
                var vaccineDisease = _mapper.Map<VaccineDisease>(createVaccineDiseaseDTO);

                vaccineDisease.VaccineId = createVaccineDiseaseDTO.VaccineId;
                vaccineDisease.DiseaseId = createVaccineDiseaseDTO.DiseaseId;
                vaccineDisease.CreatedAt = DateTimeHelper.Now();
                vaccineDisease.CreatedBy = GetCurrentUserName();

                var vaccineDiseaseId = await _vaccineDiseaseRepository.CreateVaccineDiseaseAsync(vaccineDisease, cancellationToken);
                if (vaccineDiseaseId <= 0)
                {
                    return new BaseResponse<VaccineDiseaseResponseDTO>
                    {
                        Code = 500,
                        Success = false,
                        Message = "Lỗi khi tạo vaccine disease"
                    };
                }

                var responseDTO = _mapper.Map<VaccineDiseaseResponseDTO>(vaccineDisease);
                return new BaseResponse<VaccineDiseaseResponseDTO>
                {
                    Code = 201,
                    Success = true,
                    Message = "Tạo vaccine disease thành công",
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
                    Message = "Lỗi khi tạo vaccine disease"
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
                    Message = "Lỗi ID vaccine disease không hợp lệ"
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
                        Message = "Vaccine disease không tồn tại hoặc đã bị xóa trước đó"
                    };
                }
                return new BaseResponse<bool>
                {
                    Code = 200,
                    Success = true,
                    Message = "Xóa vaccine disease thành công",
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
                    Message = "Lỗi khi xóa vaccine disease"
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
                        status = getAllItemsDTO?.Status
                    },
                    PageData = _mapper.Map<List<VaccineDiseaseResponseDTO>>(paginatedDiseases)
                };
                if (!paginatedDiseases.Any())
                {
                    return new DynamicResponse<VaccineDiseaseResponseDTO>
                    {
                        Code = 200,
                        Success = true,
                        Message = "Không có vaccine diseases nào được tìm thấy",
                        Data = responseData
                    };
                }
                return new DynamicResponse<VaccineDiseaseResponseDTO>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy danh sách vaccine diseases thành công",
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
                    Message = "Lỗi khi lấy danh sách vaccine diseases",
                };
            }
        }

        public async Task<BaseResponse<List<VaccineDiseaseResponseDTO>>> GetVaccineDiseaseByDiseaseIdAsync(int diseaseId, CancellationToken cancellationToken)
        {
            if (diseaseId <= 0)
            {
                return new BaseResponse<List<VaccineDiseaseResponseDTO>>
                {
                    Code = 400,
                    Success = false,
                    Message = "Không có ID bệnh hợp lệ"
                };
            }
            try
            {
                var vaccineDisease = await _vaccineDiseaseRepository.GetVaccineDiseaseByDiseaseIdAsync(diseaseId, cancellationToken);
                if (vaccineDisease == null)
                {
                    return new BaseResponse<List<VaccineDiseaseResponseDTO>>
                    {
                        Code = 200,
                        Success = false,
                        Message = "Vaccine disease không tồn tại cho bệnh này"
                    };
                }
                var responseDTO = _mapper.Map<List<VaccineDiseaseResponseDTO>>(vaccineDisease);
                return new BaseResponse<List<VaccineDiseaseResponseDTO>>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy vaccine disease thành công",
                    Data = responseDTO
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving vaccine disease by disease ID");
                return new BaseResponse<List<VaccineDiseaseResponseDTO>>
                {
                    Code = 500,
                    Success = false,
                    Message = "Lỗi khi lấy vaccine disease cho bệnh này"
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
                    Message = "Không có ID vaccine disease hợp lệ"
                };
            }
            try
            {
                var vaccineDisease = await _vaccineDiseaseRepository.GetVaccineDiseaseByIdAsync(vaccineDiseaseId, cancellationToken);
                if (vaccineDisease == null)
                {
                    return new BaseResponse<VaccineDiseaseResponseDTO>
                    {
                        Code = 200,
                        Success = false,
                        Message = "Vaccine disease không tồn tại"
                    };
                }
                var responseDTO = _mapper.Map<VaccineDiseaseResponseDTO>(vaccineDisease);
                return new BaseResponse<VaccineDiseaseResponseDTO>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy vaccine disease thành công",
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
                    Message = "Lỗi khi lấy vaccine disease"
                };
            }
        }

        public async Task<BaseResponse<List<VaccineDiseaseResponseDTO>>> GetVaccineDiseaseByVaccineIdAsync(int vaccineId, CancellationToken cancellationToken)
        {
            if (vaccineId <= 0)
            {
                return new BaseResponse<List<VaccineDiseaseResponseDTO>>
                {
                    Code = 400,
                    Success = false,
                    Message = "Không có ID vaccine hợp lệ"
                };
            }
            try
            {
                var vaccineDisease = await _vaccineDiseaseRepository.GetVaccineDiseaseByVaccineIdAsync(vaccineId, cancellationToken);
                if (vaccineDisease == null)
                {
                    return new BaseResponse<List<VaccineDiseaseResponseDTO>>
                    {
                        Code = 200,
                        Success = false,
                        Message = "Vaccine disease không tồn tại cho vaccine này"
                    };
                }
                var responseDTO = _mapper.Map<List<VaccineDiseaseResponseDTO>>(vaccineDisease);
                return new BaseResponse<List<VaccineDiseaseResponseDTO>>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy vaccine disease thành công",
                    Data = responseDTO
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving vaccine disease by vaccine ID");
                return new BaseResponse<List<VaccineDiseaseResponseDTO>>
                {
                    Code = 500,
                    Success = false,
                    Message = "Lỗi khi lấy vaccine disease cho vaccine này"
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
                    Message = "Không có ID vaccine disease hợp lệ hoặc dữ liệu cập nhật không hợp lệ"
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
                        Message = "Vaccine disease không tồn tại"
                    };
                }
                var updatedVaccineDisease = _mapper.Map(updateVaccineDiseaseDTO, existingVaccineDisease);
                updatedVaccineDisease.ModifiedAt = DateTimeHelper.Now();
                updatedVaccineDisease.ModifiedBy = GetCurrentUserName();
                var result = await _vaccineDiseaseRepository.UpdateVaccineDiseaseAsync(updatedVaccineDisease, cancellationToken);
                if (result <= 0)
                {
                    return new BaseResponse<VaccineDiseaseResponseDTO>
                    {
                        Code = 500,
                        Success = false,
                        Message = "Lỗi khi cập nhật vaccine disease"
                    };
                }
                var responseDTO = _mapper.Map<VaccineDiseaseResponseDTO>(updatedVaccineDisease);
                return new BaseResponse<VaccineDiseaseResponseDTO>
                {
                    Code = 200,
                    Success = true,
                    Message = "Cập nhật vaccine disease thành công",
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
                    Message = "Lỗi khi cập nhật vaccine disease"
                };
            }
        }
        private string GetCurrentUserName()
        {
            return _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value ?? "System";
        }
    }
}
