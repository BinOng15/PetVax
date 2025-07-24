using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PetVax.BusinessObjects.DTO;
using PetVax.BusinessObjects.DTO.VaccinationSchedule;
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
    public class VaccinationScheduleService : IVaccinationScheduleService
    {
        private readonly IVaccinationScheduleRepository _vaccinationScheduleRepository;
        private readonly ILogger<VaccinationScheduleService> _logger;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public VaccinationScheduleService(
            IVaccinationScheduleRepository vaccinationScheduleRepository,
            ILogger<VaccinationScheduleService> logger,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            _vaccinationScheduleRepository = vaccinationScheduleRepository;
            _logger = logger;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<BaseResponse<VaccinationScheduleResponseDTO>> CreateVaccinationScheduleAsync(CreateVaccinationScheduleDTO createVaccinationScheduleDTO, CancellationToken cancellationToken)
        {
            if (createVaccinationScheduleDTO == null)
            {
                return new BaseResponse<VaccinationScheduleResponseDTO>
                {
                    Code = 400,
                    Success = false,
                    Message = "Dữ liệu để tạo lịch tiêm mặc định không hợp lệ, vui lòng thử lại.",
                    Data = null
                };
            }
            try
            {
                var newVaccinationSchedule = _mapper.Map<VaccinationSchedule>(createVaccinationScheduleDTO);
                newVaccinationSchedule.DiseaseId = createVaccinationScheduleDTO.DiseaseId;
                newVaccinationSchedule.Species = createVaccinationScheduleDTO.Species;
                newVaccinationSchedule.DoseNumber = createVaccinationScheduleDTO.DoseNumber;
                newVaccinationSchedule.AgeInterval = createVaccinationScheduleDTO.AgeInterval;
                newVaccinationSchedule.CreatedBy = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "System";
                newVaccinationSchedule.CreatedAt = DateTimeHelper.Now();
                var createdId = await _vaccinationScheduleRepository.CreateVaccinationScheduleAsync(newVaccinationSchedule, cancellationToken);
                if (createdId > 0)
                {
                    var responseDTO = _mapper.Map<VaccinationScheduleResponseDTO>(newVaccinationSchedule);
                    return new BaseResponse<VaccinationScheduleResponseDTO>
                    {
                        Code = 201,
                        Success = true,
                        Message = "Lịch tiêm đã được tạo thành công.",
                        Data = responseDTO
                    };
                }
                else
                {
                    return new BaseResponse<VaccinationScheduleResponseDTO>
                    {
                        Code = 500,
                        Success = false,
                        Message = "Không thể tạo lịch tiêm, vui lòng thử lại sau.",
                        Data = null
                    };
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating vaccination schedule");
                return new BaseResponse<VaccinationScheduleResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "An error occurred while creating the vaccination schedule.",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<bool>> DeleteVaccinationScheduleAsync(int vaccinationScheduleId, CancellationToken cancellationToken)
        {
            if (vaccinationScheduleId <= 0)
            {
                return new BaseResponse<bool>
                {
                    Code = 400,
                    Success = false,
                    Message = "ID lịch tiêm không hợp lệ.",
                    Data = false
                };
            }
            try
            {
                var vaccinationSchedule = await _vaccinationScheduleRepository.GetVaccinationScheduleByIdAsync(vaccinationScheduleId, cancellationToken);
                if (vaccinationSchedule == null || (vaccinationSchedule.isDeleted.HasValue && vaccinationSchedule.isDeleted.Value))
                {
                    return new BaseResponse<bool>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Không tìm thấy lịch tiêm để xóa.",
                        Data = false
                    };
                }

                vaccinationSchedule.isDeleted = true;
                vaccinationSchedule.ModifiedAt = DateTimeHelper.Now();
                vaccinationSchedule.ModifiedBy = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "Unknown";

                var result = await _vaccinationScheduleRepository.UpdateVaccinationScheduleAsync(vaccinationSchedule, cancellationToken);
                if (result > 0)
                {
                    return new BaseResponse<bool>
                    {
                        Code = 200,
                        Success = true,
                        Message = "Lịch tiêm đã được xóa thành công.",
                        Data = true
                    };
                }
                else
                {
                    return new BaseResponse<bool>
                    {
                        Code = 500,
                        Success = false,
                        Message = "Không thể xóa lịch tiêm, vui lòng thử lại sau.",
                        Data = false
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting vaccination schedule");
                return new BaseResponse<bool>
                {
                    Code = 500,
                    Success = false,
                    Message = "Lỗi khi xóa mềm lịch tiêm",
                    Data = false
                };
            }
        }

        public async Task<DynamicResponse<VaccinationScheduleResponseDTO>> GetAllVaccinationScheduleAsync(GetAllItemsDTO getAllItemsDTO, CancellationToken cancellationToken)
        {
            try
            {
                var vaccinationSchedules = await _vaccinationScheduleRepository.GetAllVaccinationSchedulesAsync(cancellationToken);
                if(!string.IsNullOrEmpty(getAllItemsDTO?.KeyWord))
                {
                    var keyword = getAllItemsDTO.KeyWord.ToLower();
                    vaccinationSchedules = vaccinationSchedules
                        .Where(vs => vs.Disease?.Name.ToLower().Contains(keyword) == true ||
                                     vs.Species.ToLower().Contains(keyword))
                        .ToList();
                }
                int pageNumber = getAllItemsDTO?.PageNumber > 0 ? getAllItemsDTO.PageNumber : 1;
                int pageSize = getAllItemsDTO?.PageSize > 0 ? getAllItemsDTO.PageSize : 10;
                int skip = (pageNumber - 1) * pageSize;
                int totalItem = vaccinationSchedules.Count;
                int totalPage = (int)Math.Ceiling((double)totalItem / pageSize);

                var pagedVaccinationSchedules = vaccinationSchedules
                    .Skip(skip)
                    .Take(pageSize)
                    .Select(vs => _mapper.Map<VaccinationScheduleResponseDTO>(vs))
                    .ToList();

                var respnse = new MegaData<VaccinationScheduleResponseDTO>
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
                        status = getAllItemsDTO?.Status,
                    },
                    PageData = _mapper.Map<List<VaccinationScheduleResponseDTO>>(pagedVaccinationSchedules)
                };
                if (!pagedVaccinationSchedules.Any())
                {
                    return new DynamicResponse<VaccinationScheduleResponseDTO>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Không tìm thấy lịch tiêm nào.",
                        Data = respnse
                    };
                }
                return new DynamicResponse<VaccinationScheduleResponseDTO>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy tất cả lịch tiêm thành công.",
                    Data = respnse
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all vaccination schedules");
                return new DynamicResponse<VaccinationScheduleResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Lỗi khi lấy tất cả lịch tiêm.",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<VaccinationScheduleByDiseaseResponseDTO>> GetVaccinationScheduleByDiseaseIdAsync(int diseaseId, CancellationToken cancellationToken)
        {
            if (diseaseId <= 0)
            {
                return new BaseResponse<VaccinationScheduleByDiseaseResponseDTO>
                {
                    Code = 400,
                    Success = false,
                    Message = "ID bệnh không hợp lệ.",
                    Data = null
                };
            }
            try
            {
                var schedules = await _vaccinationScheduleRepository.GetVaccinationScheduleByDiseaseIdAsync(diseaseId, cancellationToken);
                if (schedules == null || !schedules.Any())
                {
                    return new BaseResponse<VaccinationScheduleByDiseaseResponseDTO>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Không tìm thấy lịch tiêm cho bệnh này.",
                        Data = null
                    };
                }

                var response = new VaccinationScheduleByDiseaseResponseDTO
                {
                    DiseaseId = diseaseId,
                    DiseaseName = schedules.First().Disease?.Name ?? "",
                    Schedules = schedules
                        .OrderBy(x => x.DoseNumber)
                        .Select(x => _mapper.Map<VaccinationScheduleResponseDTO>(x))
                        .ToList()
                };

                return new BaseResponse<VaccinationScheduleByDiseaseResponseDTO>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy lịch tiêm theo bệnh thành công.",
                    Data = response
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving vaccination schedule by disease ID");
                return new BaseResponse<VaccinationScheduleByDiseaseResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Lỗi khi lấy lịch tiêm theo bệnh.",
                    Data = null
                };
            }
        }


        public async Task<BaseResponse<VaccinationScheduleResponseDTO>> GetVaccinationScheduleByIdAsync(int vaccinationScheduleId, CancellationToken cancellationToken)
        {
            if (vaccinationScheduleId <= 0)
            {
                return new BaseResponse<VaccinationScheduleResponseDTO>
                {
                    Code = 400,
                    Success = false,
                    Message = "ID lịch tiêm không hợp lệ.",
                    Data = null
                };
            }
            try
            {
                var vaccinationSchedule = await _vaccinationScheduleRepository.GetVaccinationScheduleByIdAsync(vaccinationScheduleId, cancellationToken);
                if (vaccinationSchedule == null || (vaccinationSchedule.isDeleted.HasValue && vaccinationSchedule.isDeleted.Value))
                {
                    return new BaseResponse<VaccinationScheduleResponseDTO>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Không tìm thấy lịch tiêm.",
                        Data = null
                    };
                }
                var responseDTO = _mapper.Map<VaccinationScheduleResponseDTO>(vaccinationSchedule);
                return new BaseResponse<VaccinationScheduleResponseDTO>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy lịch tiêm theo ID thành công.",
                    Data = responseDTO
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving vaccination schedule by ID");
                return new BaseResponse<VaccinationScheduleResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Lỗi khi lấy lịch tiêm theo ID.",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<List<VaccinationScheduleBySpeciesResponseDTO>>> GetVaccinationScheduleBySpecies(string species, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(species))
            {
                return new BaseResponse<List<VaccinationScheduleBySpeciesResponseDTO>>
                {
                    Code = 400,
                    Success = false,
                    Message = "Loài không được bỏ trống.",
                    Data = null
                };
            }
            try
            {
                var schedules = await _vaccinationScheduleRepository.GetVaccinationScheduleBySpeciesAsync(species, cancellationToken);
                if (schedules == null || !schedules.Any())
                {
                    return new BaseResponse<List<VaccinationScheduleBySpeciesResponseDTO>>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Không tìm thấy lịch tiêm cho loài này.",
                        Data = null
                    };
                }
                var response = schedules
                    .GroupBy(x => x.Species)
                    .Select(g => new VaccinationScheduleBySpeciesResponseDTO
                    {
                        Species = g.Key,
                        Schedules = g.GroupBy(x => x.DiseaseId)
                            .Select(dg => new VaccinationScheduleByDiseaseResponseDTO
                            {
                                DiseaseId = dg.Key,
                                DiseaseName = dg.First().Disease?.Name ?? "",
                                Schedules = dg.OrderBy(x => x.DoseNumber)
                                    .Select(x => _mapper.Map<VaccinationScheduleResponseDTO>(x))
                                    .ToList()
                            })
                            .ToList()
                    })
                    .ToList();
                return new BaseResponse<List<VaccinationScheduleBySpeciesResponseDTO>>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy lịch tiêm theo loài thành công.",
                    Data = response
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving vaccination schedule by species");
                return new BaseResponse<List<VaccinationScheduleBySpeciesResponseDTO>>
                {
                    Code = 500,
                    Success = false,
                    Message = "Lỗi khi lấy lịch tiêm theo loài.",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<VaccinationScheduleResponseDTO>> UpdateVaccinationScheduleAsync(int vaccinationScheduleId, UpdateVaccinationScheduleDTO updateVaccinationScheduleDTO, CancellationToken cancellationToken)
        {
            if (vaccinationScheduleId <= 0 || updateVaccinationScheduleDTO == null)
            {
                return new BaseResponse<VaccinationScheduleResponseDTO>
                {
                    Code = 400,
                    Success = false,
                    Message = "ID lịch tiêm hoặc dữ liệu cập nhật không hợp lệ.",
                    Data = null
                };
            }
            try
            {
                var vaccinationSchedule = await _vaccinationScheduleRepository.GetVaccinationScheduleByIdAsync(vaccinationScheduleId, cancellationToken);
                if (vaccinationSchedule == null || (vaccinationSchedule.isDeleted.HasValue && vaccinationSchedule.isDeleted.Value))
                {
                    return new BaseResponse<VaccinationScheduleResponseDTO>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Không tìm thấy lịch tiêm để cập nhật.",
                        Data = null
                    };
                }
                // Update properties
                vaccinationSchedule.DiseaseId = updateVaccinationScheduleDTO.DiseaseId ?? vaccinationSchedule.DiseaseId;
                vaccinationSchedule.Species = updateVaccinationScheduleDTO.Species ?? vaccinationSchedule.Species;
                vaccinationSchedule.DoseNumber = updateVaccinationScheduleDTO.DoseNumber ?? vaccinationSchedule.DoseNumber;
                vaccinationSchedule.AgeInterval = updateVaccinationScheduleDTO.AgeInterval ?? vaccinationSchedule.AgeInterval;
                vaccinationSchedule.ModifiedAt = DateTimeHelper.Now();
                vaccinationSchedule.ModifiedBy = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "System";
                var result = await _vaccinationScheduleRepository.UpdateVaccinationScheduleAsync(vaccinationSchedule, cancellationToken);
                if (result > 0)
                {
                    var responseDTO = _mapper.Map<VaccinationScheduleResponseDTO>(vaccinationSchedule);
                    return new BaseResponse<VaccinationScheduleResponseDTO>
                    {
                        Code = 200,
                        Success = true,
                        Message = "Lịch tiêm đã được cập nhật thành công.",
                        Data = responseDTO
                    };
                }
                else
                {
                    return new BaseResponse<VaccinationScheduleResponseDTO>
                    {
                        Code = 500,
                        Success = false,
                        Message = "Không thể cập nhật lịch tiêm, vui lòng thử lại sau.",
                        Data = null
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating vaccination schedule");
                return new BaseResponse<VaccinationScheduleResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Lỗi khi cập nhật lịch tiêm.",
                    Data = null
                };
            }
        }
    }
}
