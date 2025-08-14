using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PetVax.BusinessObjects.DTO;
using PetVax.BusinessObjects.DTO.VaccineExportDetailDTO;
using PetVax.BusinessObjects.Helpers;
using PetVax.BusinessObjects.Models;
using PetVax.Repositories.IRepository;
using PetVax.Repositories.Repository;
using PetVax.Services.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PetVax.BusinessObjects.DTO.ResponseModel;

namespace PetVax.Services.Service
{
    public class VaccineExportDetailService : IVaccineExportDetailService
    {
        private readonly IVaccineExportDetailRepository _vaccineExportDetailRepository;
        private readonly IColdChainLogRepository _coldChainLogRepository;
        private readonly IAppointmentDetailRepository _appointmentDetailRepository;
        private readonly IVaccineExportRepository _vaccineExportRepository;
        private readonly IVaccineBatchRepository _vaccineBatchRepository;
        private readonly ILogger<VaccineExportDetailService> _logger;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public VaccineExportDetailService(
            IVaccineExportDetailRepository vaccineExportDetailRepository,
            IColdChainLogRepository coldChainLogRepository,
            IAppointmentDetailRepository appointmentDetailRepository,
            IVaccineExportRepository vaccineExportRepository,
            IVaccineBatchRepository vaccineBatchRepository,
            ILogger<VaccineExportDetailService> logger,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            _vaccineExportDetailRepository = vaccineExportDetailRepository;
            _coldChainLogRepository = coldChainLogRepository;
            _appointmentDetailRepository = appointmentDetailRepository;
            _vaccineExportRepository = vaccineExportRepository;
            _vaccineBatchRepository = vaccineBatchRepository;
            _logger = logger;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<BaseResponse<VaccineExportDetailResponseDTO>> CreateFullVaccineExportAsync(CreateFullVaccineExportDTO createFullVaccineExportDTO, CancellationToken cancellationToken)
        {
            if (createFullVaccineExportDTO == null)
            {
                return new BaseResponse<VaccineExportDetailResponseDTO>
                {
                    Code = 400,
                    Success = false,
                    Message = "Dữ liệu không hợp lệ.",
                    Data = null
                };
            }
            try
            {
                // Tạo VaccineExport
                var vaccineExport = new VaccineExport
                {
                    // Gán các thuộc tính cần thiết từ DTO, ví dụ:
                    ExportCode = "EXPORT" + new Random().Next(100000, 1000000).ToString(),
                    ExportDate = DateTimeHelper.Now(),
                    CreatedAt = DateTimeHelper.Now(),
                    CreatedBy = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "System",
                };
                var createdVaccineExportId = await _vaccineExportRepository.CreateVaccineExportAsync(vaccineExport, cancellationToken);

                // Tạo VaccineExportDetail
                var vaccineExportDetail = new VaccineExportDetail
                {
                    VaccineExportId = createdVaccineExportId,
                    VaccineBatchId = createFullVaccineExportDTO.VaccineBatchId,
                    Quantity = createFullVaccineExportDTO.Quantity,
                    Purpose = createFullVaccineExportDTO.Purpose,
                    Notes = createFullVaccineExportDTO.Notes,
                    CreatedAt = DateTimeHelper.Now(),
                    CreatedBy = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "System",
                    isDeleted = false
                };
                var createdVaccineExportDetailId = await _vaccineExportDetailRepository.CreateVaccineExportDetailAsync(vaccineExportDetail, cancellationToken);

                // Trừ số lượng từ VaccineBatch
                var vaccineBatch = await _vaccineBatchRepository.GetVaccineBatchByIdAsync(createFullVaccineExportDTO.VaccineBatchId, cancellationToken);
                if (vaccineBatch == null || vaccineBatch.isDeleted == true)
                {
                    return new BaseResponse<VaccineExportDetailResponseDTO>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Lô vắc xin không tồn tại hoặc đã bị xóa.",
                        Data = null
                    };
                }
                if (createFullVaccineExportDTO.Quantity > vaccineBatch.Quantity)
                {
                    return new BaseResponse<VaccineExportDetailResponseDTO>
                    {
                        Code = 400,
                        Success = false,
                        Message = "Số lượng xuất vượt quá số lượng hiện có của lô vắc xin.",
                        Data = null
                    };
                }
                vaccineBatch.Quantity -= createFullVaccineExportDTO.Quantity;
                await _vaccineBatchRepository.UpdateVaccineBatchAsync(vaccineBatch, cancellationToken);

                // Tạo ColdChainLog nếu có
                if (createFullVaccineExportDTO.ColdChainLog != null)
                {
                    var coldChainLogDTO = createFullVaccineExportDTO.ColdChainLog;
                    var coldChainLog = new ColdChainLog
                    {
                        VaccineBatchId = createFullVaccineExportDTO.VaccineBatchId,
                        LogTime = coldChainLogDTO?.LogTime ?? DateTimeHelper.Now(),
                        Temperature = coldChainLogDTO?.Temperature ?? 0,
                        Humidity = coldChainLogDTO?.Humidity ?? 0,
                        Event = coldChainLogDTO?.Event ?? "Xuất kho",
                        Notes = coldChainLogDTO?.Notes ?? $"Tạo log cho chi tiết xuất kho có id: {createdVaccineExportDetailId}",
                        RecordedAt = DateTimeHelper.Now(),
                        RecordedBy = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "System",
                        isDeleted = false
                    };
                    await _coldChainLogRepository.CreateColdChainLogAsync(coldChainLog, cancellationToken);
                }

                var createdDetail = await _vaccineExportDetailRepository.GetVaccineExportDetailByIdAsync(createdVaccineExportDetailId, cancellationToken);
                var responseDTO = _mapper.Map<VaccineExportDetailResponseDTO>(createdDetail);

                return new BaseResponse<VaccineExportDetailResponseDTO>
                {
                    Code = 201,
                    Success = true,
                    Message = "Tạo phiếu xuất và chi tiết phiếu xuất vắc xin thành công.",
                    Data = responseDTO
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi tạo phiếu xuất và chi tiết phiếu xuất vắc xin.");
                return new BaseResponse<VaccineExportDetailResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã xảy ra lỗi khi tạo phiếu xuất kho cho lô vắc xin, vui lòng thử lại sau!",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<VaccineExportDetailResponseDTO>> CreateVaccineExportDetailAsync(CreateVaccineExportDetailDTO createVaccineExportDetailDTO, CancellationToken cancellationToken)
        {
            if (createVaccineExportDetailDTO == null)
            {
                return new BaseResponse<VaccineExportDetailResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Dữ liệu không hợp lệ.",
                    Data = null
                };
            }
            try
            {
                // Check if VaccineExport exists
                var vaccineExport = await _vaccineExportRepository.GetVaccineExportByIdAsync(createVaccineExportDetailDTO.VaccineExportId, cancellationToken);
                if (vaccineExport == null || vaccineExport.isDeleted == true)
                {
                    return new BaseResponse<VaccineExportDetailResponseDTO>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Phiếu xuất vắc xin không tồn tại hoặc đã bị xóa.",
                        Data = null
                    };
                }
                // Check if VaccineBatch exists
                var vaccineBatch = await _vaccineBatchRepository.GetVaccineBatchByIdAsync(createVaccineExportDetailDTO.VaccineBatchId, cancellationToken);
                if (vaccineBatch == null || vaccineBatch.isDeleted == true)
                {
                    return new BaseResponse<VaccineExportDetailResponseDTO>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Lô vắc xin không tồn tại hoặc đã bị xóa.",
                        Data = null
                    };
                }
                // Check quantity
                if (createVaccineExportDetailDTO.Quantity > vaccineBatch.Quantity)
                {
                    return new BaseResponse<VaccineExportDetailResponseDTO>
                    {
                        Code = 400,
                        Success = false,
                        Message = "Số lượng xuất vượt quá số lượng hiện có của lô vắc xin.",
                        Data = null
                    };
                }
                //Create VaccineExportDetail
                var vaccineExportDetail = new VaccineExportDetail
                {
                    VaccineExportId = createVaccineExportDetailDTO.VaccineExportId,
                    VaccineBatchId = createVaccineExportDetailDTO.VaccineBatchId,
                    Quantity = createVaccineExportDetailDTO.Quantity,
                    Purpose = createVaccineExportDetailDTO.Purpose,
                    Notes = createVaccineExportDetailDTO.Notes,
                    CreatedAt = DateTimeHelper.Now(),
                    CreatedBy = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "System",
                    isDeleted = false
                };
                var createdVaccineExportDetailId = await _vaccineExportDetailRepository.CreateVaccineExportDetailAsync(vaccineExportDetail, cancellationToken);

                // Subtract quantity from VaccineBatch
                vaccineBatch.Quantity -= createVaccineExportDetailDTO.Quantity;
                await _vaccineBatchRepository.UpdateVaccineBatchAsync(vaccineBatch, cancellationToken);

                //Create ColdChainLog
                var coldChainLogDTO = createVaccineExportDetailDTO.ColdChainLog;
                var coldChainLog = new ColdChainLog
                {
                    VaccineBatchId = createVaccineExportDetailDTO.VaccineBatchId,
                    LogTime = coldChainLogDTO?.LogTime ?? DateTimeHelper.Now(),
                    Temperature = coldChainLogDTO?.Temperature ?? 0,
                    Humidity = coldChainLogDTO?.Humidity ?? 0,
                    Event = coldChainLogDTO?.Event ?? "Xuất kho",
                    Notes = coldChainLogDTO?.Notes ?? $"Tạo log cho chi tiết xuất kho có id: {createdVaccineExportDetailId}",
                    RecordedAt = DateTimeHelper.Now(),
                    RecordedBy = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "System",
                    isDeleted = false
                };
                await _coldChainLogRepository.CreateColdChainLogAsync(coldChainLog, cancellationToken);

                var createdDetail = await _vaccineExportDetailRepository.GetVaccineExportDetailByIdAsync(createdVaccineExportDetailId, cancellationToken);
                var responseDTO = _mapper.Map<VaccineExportDetailResponseDTO>(createdDetail);

                return new BaseResponse<VaccineExportDetailResponseDTO>
                {
                    Code = 201,
                    Success = true,
                    Message = "Tạo chi tiết phiếu xuất vắc xin thành công.",
                    Data = responseDTO
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi tạo chi tiết phiếu xuất vắc xin.");
                return new BaseResponse<VaccineExportDetailResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã xảy ra lỗi khi tạo phiếu xuất kho cho lô vắc xin, vui lòng thử lại sau!",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<VaccineExportDetailResponseForVaccinationDTO>> CreateVaccineExportDetailForVaccinationAsync(CreateVaccineExportDetailForVaccinationDTO createVaccineExportDetailForVaccinationDTO, CancellationToken cancellationToken)
        {
            if (createVaccineExportDetailForVaccinationDTO == null)
            {
                return new BaseResponse<VaccineExportDetailResponseForVaccinationDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Dữ liệu không hợp lệ.",
                    Data = null
                };
            }
            try
            {
                // Check if VaccineExport exists
                var vaccineExport = await _vaccineExportRepository.GetVaccineExportByIdAsync(createVaccineExportDetailForVaccinationDTO.VaccineExportId, cancellationToken);
                if (vaccineExport == null || vaccineExport.isDeleted == true)
                {
                    return new BaseResponse<VaccineExportDetailResponseForVaccinationDTO>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Phiếu xuất vắc xin không tồn tại hoặc đã bị xóa.",
                        Data = null
                    };
                }
                // Check if VaccineBatch exists
                var vaccineBatch = await _vaccineBatchRepository.GetVaccineBatchByIdAsync(createVaccineExportDetailForVaccinationDTO.VaccineBatchId, cancellationToken);
                if (vaccineBatch == null || vaccineBatch.isDeleted == true)
                {
                    return new BaseResponse<VaccineExportDetailResponseForVaccinationDTO>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Lô vắc xin không tồn tại hoặc đã bị xóa.",
                        Data = null
                    };
                }
                //Check if AppointmentDetail exists
                var appointmentDetail = await _appointmentDetailRepository.GetAppointmentDetailByIdAsync(createVaccineExportDetailForVaccinationDTO.AppointmentDetailId, cancellationToken);
                if (appointmentDetail == null || appointmentDetail.isDeleted == true)
                {
                    return new BaseResponse<VaccineExportDetailResponseForVaccinationDTO>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Chi tiết cuộc hẹn không tồn tại hoặc đã bị xóa.",
                        Data = null
                    };
                }
                //Check if AppointmentDetail is already exported
                var existingExportDetail = await _vaccineExportDetailRepository.GetVaccineExportDetailByAppointmentDetailIdAsync(createVaccineExportDetailForVaccinationDTO.AppointmentDetailId, cancellationToken);
                _logger.LogInformation("AppointmentDetail", existingExportDetail);
                if (existingExportDetail != null)
                {
                    return new BaseResponse<VaccineExportDetailResponseForVaccinationDTO>
                    {
                        Code = 400,
                        Success = false,
                        Message = "Chi tiết cuộc hẹn đã được xuất kho trước đó.",
                        Data = null
                    };
                }
                // Check quantity
                if (createVaccineExportDetailForVaccinationDTO.Quantity > vaccineBatch.Quantity)
                {
                    return new BaseResponse<VaccineExportDetailResponseForVaccinationDTO>
                    {
                        Code = 400,
                        Success = false,
                        Message = "Số lượng xuất vượt quá số lượng hiện có của lô vắc xin.",
                        Data = null
                    };
                }
                //Create VaccineExportDetail
                var vaccineExportDetail = new VaccineExportDetail
                {
                    VaccineExportId = createVaccineExportDetailForVaccinationDTO.VaccineExportId,
                    VaccineBatchId = createVaccineExportDetailForVaccinationDTO.VaccineBatchId,
                    AppointmentDetailId = createVaccineExportDetailForVaccinationDTO.AppointmentDetailId,
                    Quantity = createVaccineExportDetailForVaccinationDTO.Quantity,
                    Purpose = createVaccineExportDetailForVaccinationDTO.Purpose,
                    Notes = createVaccineExportDetailForVaccinationDTO.Notes,
                    CreatedAt = DateTimeHelper.Now(),
                    CreatedBy = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "System",
                    isDeleted = false
                };
                var createdVaccineExportDetailId = await _vaccineExportDetailRepository.CreateVaccineExportDetailAsync(vaccineExportDetail, cancellationToken);

                // Subtract quantity from VaccineBatch
                vaccineBatch.Quantity -= createVaccineExportDetailForVaccinationDTO.Quantity;
                await _vaccineBatchRepository.UpdateVaccineBatchAsync(vaccineBatch, cancellationToken);

                //Create ColdChainLog
                var coldChainLogDTO = createVaccineExportDetailForVaccinationDTO.ColdChainLog;
                var coldChainLog = new ColdChainLog
                {
                    VaccineBatchId = createVaccineExportDetailForVaccinationDTO.VaccineBatchId,
                    LogTime = coldChainLogDTO?.LogTime ?? DateTimeHelper.Now(),
                    Temperature = coldChainLogDTO?.Temperature ?? 0,
                    Humidity = coldChainLogDTO?.Humidity ?? 0,
                    Event = coldChainLogDTO?.Event ?? "Xuất kho",
                    Notes = coldChainLogDTO?.Notes ?? $"Tạo log cho chi tiết xuất kho có id: {createdVaccineExportDetailId}",
                    RecordedAt = DateTimeHelper.Now(),
                    RecordedBy = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "System",
                    isDeleted = false
                };
                await _coldChainLogRepository.CreateColdChainLogAsync(coldChainLog, cancellationToken);

                var createdDetail = await _vaccineExportDetailRepository.GetVaccineExportDetailByIdAsync(createdVaccineExportDetailId, cancellationToken);
                var responseDTO = _mapper.Map<VaccineExportDetailResponseForVaccinationDTO>(createdDetail);

                return new BaseResponse<VaccineExportDetailResponseForVaccinationDTO>
                {
                    Code = 201,
                    Success = true,
                    Message = "Tạo chi tiết phiếu xuất vắc xin thành công.",
                    Data = responseDTO
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi tạo chi tiết phiếu xuất vắc xin.");
                return new BaseResponse<VaccineExportDetailResponseForVaccinationDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã xảy ra lỗi khi tạo phiếu xuất kho cho lô vắc xin, vui lòng thử lại sau!",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<bool>> DeleteVaccineExportDetailAsync(int vaccineExportDetailId, CancellationToken cancellationToken)
        {
            if (vaccineExportDetailId <= 0)
            {
                return new BaseResponse<bool>
                {
                    Code = 400,
                    Success = false,
                    Message = "ID chi tiết xuất kho không hợp lệ.",
                    Data = false
                };
            }
            try
            {
                var vaccineExportDetail = await _vaccineExportDetailRepository.GetVaccineExportDetailByIdAsync(vaccineExportDetailId, cancellationToken);
                if (vaccineExportDetail == null || vaccineExportDetail.isDeleted == true)
                {
                    return new BaseResponse<bool>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Chi tiết phiếu xuất vắc xin không tồn tại hoặc đã bị xóa.",
                        Data = false
                    };
                }
                vaccineExportDetail.isDeleted = true;
                await _vaccineExportDetailRepository.UpdateVaccineExportDetailAsync(vaccineExportDetail, cancellationToken);
                return new BaseResponse<bool>
                {
                    Code = 200,
                    Success = true,
                    Message = "Xóa chi tiết phiếu xuất vắc xin thành công.",
                    Data = true
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi xóa chi tiết phiếu xuất vắc xin.");
                return new BaseResponse<bool>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã xảy ra lỗi khi xóa chi tiết phiếu xuất kho cho lô vắc xin, vui lòng thử lại sau!",
                    Data = false
                };
            }
        }

        public async Task<DynamicResponse<VaccineExportDetailResponseDTO>> GetAllVaccineExportDetailsAsync(GetAllItemsDTO getAllItemsDTO, CancellationToken cancellationToken)
        {
            try
            {
                var vaccineExportDetails = await _vaccineExportDetailRepository.GetAllVaccineExportDetailsAsync(cancellationToken);
                if (!string.IsNullOrWhiteSpace(getAllItemsDTO.KeyWord))
                {
                    vaccineExportDetails = vaccineExportDetails
                        .Where(ved => ved.VaccineBatch?.Vaccine?.Name.Contains(getAllItemsDTO.KeyWord, StringComparison.OrdinalIgnoreCase) == true ||
                                      ved.VaccineExport?.ExportCode.Contains(getAllItemsDTO.KeyWord, StringComparison.OrdinalIgnoreCase) == true ||
                                      ved.Purpose.Contains(getAllItemsDTO.KeyWord, StringComparison.OrdinalIgnoreCase) ||
                                      ved.Notes.Contains(getAllItemsDTO.KeyWord, StringComparison.OrdinalIgnoreCase) ||
                                      ved.Quantity.ToString().Contains(getAllItemsDTO.KeyWord, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                }

                int pageNumber = getAllItemsDTO?.PageNumber > 0 ? getAllItemsDTO.PageNumber : 1;
                int pageSize = getAllItemsDTO?.PageSize > 0 ? getAllItemsDTO.PageSize : 10;
                int skip = (pageNumber - 1) * pageSize;
                int totalItem = vaccineExportDetails.Count;
                int totalPage = (int)Math.Ceiling((double)totalItem / pageSize);

                var pagedDetails = vaccineExportDetails
                    .Skip(skip)
                    .Take(pageSize)
                    .Select(ved => _mapper.Map<VaccineExportDetailResponseDTO>(ved))
                    .ToList();

                var response = new MegaData<VaccineExportDetailResponseDTO>
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
                    PageData = _mapper.Map<List<VaccineExportDetailResponseDTO>>(pagedDetails)
                };
                if (pagedDetails.Any())
                {
                    return new DynamicResponse<VaccineExportDetailResponseDTO>
                    {
                        Code = 200,
                        Success = true,
                        Message = "Lấy danh sách chi tiết phiếu xuất vắc xin thành công.",
                        Data = response
                    };
                }
                else
                {
                    return new DynamicResponse<VaccineExportDetailResponseDTO>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Không tìm thấy chi tiết phiếu xuất vắc xin nào.",
                        Data = response
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy danh sách chi tiết phiếu xuất vắc xin.");
                return new DynamicResponse<VaccineExportDetailResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã xảy ra lỗi khi lấy danh sách chi tiết phiếu xuất kho cho lô vắc xin, vui lòng thử lại sau!",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<List<VaccineExportDetailResponseDTO>>> GetListVaccineExportDetailByVaccineBatchIdAsync(int vaccineBatchId, CancellationToken cancellationToken)
        {
            if (vaccineBatchId <= 0)
            {
                return new BaseResponse<List<VaccineExportDetailResponseDTO>>
                {
                    Code = 400,
                    Success = false,
                    Message = "ID lô vắc xin không hợp lệ.",
                    Data = null
                };
            }
            try
            {
                var vaccineExportDetails = await _vaccineExportDetailRepository.GetListVaccineExportDetailByVaccineBatchIdAsync(vaccineBatchId, cancellationToken);
                if (vaccineExportDetails == null || !vaccineExportDetails.Any())
                {
                    return new BaseResponse<List<VaccineExportDetailResponseDTO>>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Không tìm thấy chi tiết phiếu xuất vắc xin nào cho lô này.",
                        Data = null
                    };
                }
                var responseDTO = _mapper.Map<List<VaccineExportDetailResponseDTO>>(vaccineExportDetails);
                return new BaseResponse<List<VaccineExportDetailResponseDTO>>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy danh sách chi tiết phiếu xuất vắc xin theo lô thành công.",
                    Data = responseDTO
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy danh sách chi tiết phiếu xuất vắc xin theo lô.");
                return new BaseResponse<List<VaccineExportDetailResponseDTO>>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã xảy ra lỗi khi lấy danh sách chi tiết phiếu xuất kho cho lô vắc xin, vui lòng thử lại sau!",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<VaccineExportDetailResponseForVaccinationDTO>> GetVaccineExportDetailByAppointmentDetailIdAsync(int appointmentDetailId, CancellationToken cancellationToken)
        {
            if (appointmentDetailId <= 0)
            {
                return new BaseResponse<VaccineExportDetailResponseForVaccinationDTO>
                {
                    Code = 400,
                    Success = false,
                    Message = "ID chi tiết cuộc hẹn không hợp lệ.",
                    Data = null
                };
            }
            try
            {
                var vaccineExportDetail = await _vaccineExportDetailRepository.GetVaccineExportDetailByAppointmentDetailIdAsync(appointmentDetailId, cancellationToken);
                if (vaccineExportDetail == null || vaccineExportDetail.isDeleted == true)
                {
                    return new BaseResponse<VaccineExportDetailResponseForVaccinationDTO>
                    {
                        Code = 200,
                        Success = false,
                        Message = "Chi tiết phiếu xuất vắc xin không tồn tại hoặc đã bị xóa.",
                        Data = null
                    };
                }
                var responseDTO = _mapper.Map<VaccineExportDetailResponseForVaccinationDTO>(vaccineExportDetail);
                return new BaseResponse<VaccineExportDetailResponseForVaccinationDTO>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy chi tiết phiếu xuất vắc xin theo chi tiết cuộc hẹn thành công.",
                    Data = responseDTO
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy chi tiết phiếu xuất vắc xin theo chi tiết cuộc hẹn.");
                return new BaseResponse<VaccineExportDetailResponseForVaccinationDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã xảy ra lỗi khi lấy chi tiết phiếu xuất kho cho lô vắc xin, vui lòng thử lại sau!",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<VaccineExportDetailResponseDTO>> GetVaccineExportDetailByIdAsync(int vaccineExportDetailId, CancellationToken cancellationToken)
        {
            if (vaccineExportDetailId <= 0)
            {
                return new BaseResponse<VaccineExportDetailResponseDTO>
                {
                    Code = 400,
                    Success = false,
                    Message = "ID chi tiết xuất kho không hợp lệ.",
                    Data = null
                };
            }
            try
            {
                var vaccineExportDetail = await _vaccineExportDetailRepository.GetVaccineExportDetailByIdAsync(vaccineExportDetailId, cancellationToken);
                if (vaccineExportDetail == null || vaccineExportDetail.isDeleted == true)
                {
                    return new BaseResponse<VaccineExportDetailResponseDTO>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Chi tiết phiếu xuất vắc xin không tồn tại hoặc đã bị xóa.",
                        Data = null
                    };
                }
                var responseDTO = _mapper.Map<VaccineExportDetailResponseDTO>(vaccineExportDetail);
                return new BaseResponse<VaccineExportDetailResponseDTO>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy chi tiết phiếu xuất vắc xin thành công.",
                    Data = responseDTO
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy chi tiết phiếu xuất vắc xin.");
                return new BaseResponse<VaccineExportDetailResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã xảy ra lỗi khi lấy chi tiết phiếu xuất kho cho lô vắc xin, vui lòng thử lại sau!",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<List<VaccineExportDetailResponseDTO>>> GetVaccineExportDetailByVaccineBatchIdAsync(int vaccineBatchId, CancellationToken cancellationToken)
        {
            if (vaccineBatchId <= 0)
            {
                return new BaseResponse<List<VaccineExportDetailResponseDTO>>
                {
                    Code = 400,
                    Success = false,
                    Message = "ID lô vắc xin không hợp lệ.",
                    Data = null
                };
            }
            try
            {
                var vaccineExportDetail = await _vaccineExportDetailRepository.GetVaccineExportDetailByVaccineBatchIdAsync(vaccineBatchId, cancellationToken);
                if (vaccineExportDetail == null)
                {
                    return new BaseResponse<List<VaccineExportDetailResponseDTO>>
                    {
                        Code = 200,
                        Success = true,
                        Message = "Chi tiết phiếu xuất vắc xin không tồn tại hoặc đã bị xóa.",
                        Data = null
                    };
                }
                var responseDTO = _mapper.Map<List<VaccineExportDetailResponseDTO>>(vaccineExportDetail);
                return new BaseResponse<List<VaccineExportDetailResponseDTO>>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy chi tiết phiếu xuất vắc xin theo lô thành công.",
                    Data = responseDTO
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy chi tiết phiếu xuất vắc xin theo lô.");
                return new BaseResponse<List<VaccineExportDetailResponseDTO>>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã xảy ra lỗi khi lấy chi tiết phiếu xuất kho cho lô vắc xin, vui lòng thử lại sau!",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<List<VaccineExportDetailResponseDTO>>> GetVaccineExportDetailByVaccineExportIdAsync(int vaccineExportId, CancellationToken cancellationToken)
        {
            if (vaccineExportId <= 0)
            {
                return new BaseResponse<List<VaccineExportDetailResponseDTO>>
                {
                    Code = 400,
                    Success = false,
                    Message = "ID phiếu xuất vắc xin không hợp lệ.",
                    Data = null
                };
            }
            try
            {
                var vaccineExportDetails = await _vaccineExportDetailRepository.GetVaccineExportDetailsByVaccineExportIdAsync(vaccineExportId, cancellationToken);
                if (vaccineExportDetails == null)
                {
                    return new BaseResponse<List<VaccineExportDetailResponseDTO>>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Không tìm thấy chi tiết phiếu xuất vắc xin nào cho phiếu xuất này.",
                        Data = null
                    };
                }
                var responseDTOs = _mapper.Map<List<VaccineExportDetailResponseDTO>>(vaccineExportDetails);
                return new BaseResponse<List<VaccineExportDetailResponseDTO>>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy danh sách chi tiết phiếu xuất vắc xin thành công.",
                    Data = responseDTOs
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy chi tiết phiếu xuất vắc xin theo phiếu xuất.");
                return new BaseResponse<List<VaccineExportDetailResponseDTO>>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã xảy ra lỗi khi lấy chi tiết phiếu xuất kho cho lô vắc xin, vui lòng thử lại sau!",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<VaccineExportDetailResponseDTO>> UpdateVaccineExportDetailAsync(int vaccineExportDetailId, UpdateVaccineExportDetailDTO updateVaccineExportDetailDTO, CancellationToken cancellationToken)
        {
            if (vaccineExportDetailId <= 0 || updateVaccineExportDetailDTO == null)
            {
                return new BaseResponse<VaccineExportDetailResponseDTO>
                {
                    Code = 400,
                    Success = false,
                    Message = "ID chi tiết xuất kho hoặc dữ liệu cập nhật không hợp lệ.",
                    Data = null
                };
            }
            try
            {
                var vaccineExportDetail = await _vaccineExportDetailRepository.GetVaccineExportDetailByIdAsync(vaccineExportDetailId, cancellationToken);
                if (vaccineExportDetail == null || vaccineExportDetail.isDeleted == true)
                {
                    return new BaseResponse<VaccineExportDetailResponseDTO>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Chi tiết phiếu xuất vắc xin không tồn tại hoặc đã bị xóa.",
                        Data = null
                    };
                }

                // If quantity is provided, subtract it from VaccineBatch
                if (updateVaccineExportDetailDTO.Quantity.HasValue)
                {
                    var vaccineBatchId = updateVaccineExportDetailDTO.VaccineBatchId ?? vaccineExportDetail.VaccineBatchId;
                    var vaccineBatch = await _vaccineBatchRepository.GetVaccineBatchByIdAsync(vaccineBatchId, cancellationToken);
                    if (vaccineBatch != null)
                    {
                        if (updateVaccineExportDetailDTO.Quantity.Value > vaccineBatch.Quantity)
                        {
                            return new BaseResponse<VaccineExportDetailResponseDTO>
                            {
                                Code = 400,
                                Success = false,
                                Message = "Số lượng cập nhật vượt quá số lượng hiện có của lô vắc xin.",
                                Data = null
                            };
                        }
                        vaccineBatch.Quantity = vaccineBatch.Quantity + vaccineExportDetail.Quantity - updateVaccineExportDetailDTO.Quantity.Value;
                        await _vaccineBatchRepository.UpdateVaccineBatchAsync(vaccineBatch, cancellationToken);
                    }
                }

                // Update properties
                vaccineExportDetail.VaccineExportId = updateVaccineExportDetailDTO.VaccineExportId ?? vaccineExportDetail.VaccineExportId;
                vaccineExportDetail.VaccineBatchId = updateVaccineExportDetailDTO.VaccineBatchId ?? vaccineExportDetail.VaccineBatchId;
                vaccineExportDetail.Quantity = updateVaccineExportDetailDTO.Quantity ?? vaccineExportDetail.Quantity;
                vaccineExportDetail.Purpose = updateVaccineExportDetailDTO.Purpose ?? vaccineExportDetail.Purpose;
                vaccineExportDetail.Notes = updateVaccineExportDetailDTO.Notes ?? vaccineExportDetail.Notes;
                vaccineExportDetail.ModifiedAt = DateTimeHelper.Now();
                vaccineExportDetail.ModifiedBy = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "System";
                await _vaccineExportDetailRepository.UpdateVaccineExportDetailAsync(vaccineExportDetail, cancellationToken);

                if (updateVaccineExportDetailDTO.ColdChainLog != null)
                {
                    var coldChainLogs = await _coldChainLogRepository.GetColdChainLogsByVaccineBatchIdAsync(vaccineExportDetail.VaccineBatchId, cancellationToken);
                    var coldChainLog = coldChainLogs.FirstOrDefault();
                    if (coldChainLog != null)
                    {
                        coldChainLog.VaccineBatchId = updateVaccineExportDetailDTO.VaccineBatchId ?? coldChainLog.VaccineBatchId;
                        coldChainLog.LogTime = updateVaccineExportDetailDTO.ColdChainLog.LogTime ?? coldChainLog.LogTime;
                        coldChainLog.Temperature = updateVaccineExportDetailDTO.ColdChainLog.Temperature ?? coldChainLog.Temperature;
                        coldChainLog.Humidity = updateVaccineExportDetailDTO.ColdChainLog.Humidity ?? coldChainLog.Humidity;
                        coldChainLog.Event = updateVaccineExportDetailDTO.ColdChainLog.Event ?? coldChainLog.Event;
                        coldChainLog.Notes = updateVaccineExportDetailDTO.ColdChainLog.Notes ?? coldChainLog.Notes;
                        coldChainLog.ModifiedAt = DateTimeHelper.Now();
                        coldChainLog.ModifiedBy = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "System";
                        await _coldChainLogRepository.UpdateColdChainLogAsync(coldChainLog, cancellationToken);
                    }
                }

                var updatedDetail = await _vaccineExportDetailRepository.GetVaccineExportDetailByIdAsync(vaccineExportDetailId, cancellationToken);
                var responseDTO = _mapper.Map<VaccineExportDetailResponseDTO>(updatedDetail);
                return new BaseResponse<VaccineExportDetailResponseDTO>
                {
                    Code = 200,
                    Success = true,
                    Message = "Cập nhật chi tiết phiếu xuất vắc xin thành công.",
                    Data = responseDTO
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi cập nhật chi tiết phiếu xuất vắc xin.");
                return new BaseResponse<VaccineExportDetailResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã xảy ra lỗi khi cập nhật chi tiết phiếu xuất kho cho lô vắc xin, vui lòng thử lại sau!",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<VaccineExportDetailResponseForVaccinationDTO>> UpdateVaccineExportDetailForVaccinationAsync(int vaccineExportDetailId, UpdateVaccineExportDetailForVaccinationDTO updateVaccineExportDetailForVaccinationDTO, CancellationToken cancellationToken)
        {
            if (vaccineExportDetailId <= 0 || updateVaccineExportDetailForVaccinationDTO == null)
            {
                return new BaseResponse<VaccineExportDetailResponseForVaccinationDTO>
                {
                    Code = 400,
                    Success = false,
                    Message = "ID chi tiết xuất kho hoặc dữ liệu cập nhật không hợp lệ.",
                    Data = null
                };
            }
            try
            {
                var vaccineExportDetail = await _vaccineExportDetailRepository.GetVaccineExportDetailByIdAsync(vaccineExportDetailId, cancellationToken);
                if (vaccineExportDetail == null || vaccineExportDetail.isDeleted == true)
                {
                    return new BaseResponse<VaccineExportDetailResponseForVaccinationDTO>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Chi tiết phiếu xuất vắc xin không tồn tại hoặc đã bị xóa.",
                        Data = null
                    };
                }
                // If quantity is provided, subtract it from VaccineBatch
                if (updateVaccineExportDetailForVaccinationDTO.Quantity.HasValue)
                {
                    var vaccineBatchId = updateVaccineExportDetailForVaccinationDTO.VaccineBatchId ?? vaccineExportDetail.VaccineBatchId;
                    var vaccineBatch = await _vaccineBatchRepository.GetVaccineBatchByIdAsync(vaccineBatchId, cancellationToken);
                    if (vaccineBatch != null)
                    {
                        if (updateVaccineExportDetailForVaccinationDTO.Quantity.Value > vaccineBatch.Quantity)
                        {
                            return new BaseResponse<VaccineExportDetailResponseForVaccinationDTO>
                            {
                                Code = 400,
                                Success = false,
                                Message = "Số lượng cập nhật vượt quá số lượng hiện có của lô vắc xin.",
                                Data = null
                            };
                        }
                        vaccineBatch.Quantity = vaccineBatch.Quantity + vaccineExportDetail.Quantity - updateVaccineExportDetailForVaccinationDTO.Quantity.Value;
                        await _vaccineBatchRepository.UpdateVaccineBatchAsync(vaccineBatch, cancellationToken);
                    }
                }

                // Update properties
                vaccineExportDetail.VaccineExportId = updateVaccineExportDetailForVaccinationDTO.VaccineExportId ?? vaccineExportDetail.VaccineExportId;
                vaccineExportDetail.VaccineBatchId = updateVaccineExportDetailForVaccinationDTO.VaccineBatchId ?? vaccineExportDetail.VaccineBatchId;
                vaccineExportDetail.Quantity = updateVaccineExportDetailForVaccinationDTO.Quantity ?? vaccineExportDetail.Quantity;
                vaccineExportDetail.Purpose = updateVaccineExportDetailForVaccinationDTO.Purpose ?? vaccineExportDetail.Purpose;
                vaccineExportDetail.Notes = updateVaccineExportDetailForVaccinationDTO.Notes ?? vaccineExportDetail.Notes;
                vaccineExportDetail.ModifiedAt = DateTimeHelper.Now();
                vaccineExportDetail.ModifiedBy = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "System";
                await _vaccineExportDetailRepository.UpdateVaccineExportDetailAsync(vaccineExportDetail, cancellationToken);

                if (updateVaccineExportDetailForVaccinationDTO.ColdChainLog != null)
                {
                    var coldChainLogs = await _coldChainLogRepository.GetColdChainLogsByVaccineBatchIdAsync(vaccineExportDetail.VaccineBatchId, cancellationToken);
                    var coldChainLog = coldChainLogs.FirstOrDefault();
                    if (coldChainLog != null)
                    {
                        coldChainLog.VaccineBatchId = updateVaccineExportDetailForVaccinationDTO.VaccineBatchId ?? coldChainLog.VaccineBatchId;
                        coldChainLog.LogTime = updateVaccineExportDetailForVaccinationDTO.ColdChainLog.LogTime ?? coldChainLog.LogTime;
                        coldChainLog.Temperature = updateVaccineExportDetailForVaccinationDTO.ColdChainLog.Temperature ?? coldChainLog.Temperature;
                        coldChainLog.Humidity = updateVaccineExportDetailForVaccinationDTO.ColdChainLog.Humidity ?? coldChainLog.Humidity;
                        coldChainLog.Event = updateVaccineExportDetailForVaccinationDTO.ColdChainLog.Event ?? coldChainLog.Event;
                        coldChainLog.Notes = updateVaccineExportDetailForVaccinationDTO.ColdChainLog.Notes ?? coldChainLog.Notes;
                        coldChainLog.ModifiedAt = DateTimeHelper.Now();
                        coldChainLog.ModifiedBy = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "System";
                        await _coldChainLogRepository.UpdateColdChainLogAsync(coldChainLog, cancellationToken);
                    }
                }

                var updatedDetail = await _vaccineExportDetailRepository.GetVaccineExportDetailByIdAsync(vaccineExportDetailId, cancellationToken);
                var responseDTO = _mapper.Map<VaccineExportDetailResponseForVaccinationDTO>(updatedDetail);
                return new BaseResponse<VaccineExportDetailResponseForVaccinationDTO>
                {
                    Code = 200,
                    Success = true,
                    Message = "Cập nhật chi tiết phiếu xuất vắc xin thành công.",
                    Data = responseDTO
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi cập nhật chi tiết phiếu xuất vắc xin.");
                return new BaseResponse<VaccineExportDetailResponseForVaccinationDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã xảy ra lỗi khi cập nhật chi tiết phiếu xuất kho cho lô vắc xin, vui lòng thử lại sau!",
                    Data = null
                };
            }
        }
    }
}
