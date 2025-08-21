using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PetVax.BusinessObjects.DTO;
using PetVax.BusinessObjects.DTO.AppointmentDetailDTO;
using PetVax.BusinessObjects.DTO.AppointmentDTO;
using PetVax.BusinessObjects.DTO.CustomerDTO;
using PetVax.BusinessObjects.DTO.HealthConditionDTO;
using PetVax.BusinessObjects.DTO.VaccineProfileDTO;
using PetVax.BusinessObjects.Enum;
using PetVax.BusinessObjects.Helpers;
using PetVax.BusinessObjects.Models;
using PetVax.Repositories.IRepository;
using PetVax.Repositories.Repository;
using PetVax.Services.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static PetVax.BusinessObjects.DTO.ResponseModel;
using static PetVax.BusinessObjects.Enum.EnumList;

namespace PetVax.Services.Service
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IPetRepository _petRepository;
        private readonly IDiseaseRepository _diseaseRepository;
        private readonly IVaccineDiseaseRepository _vaccineDiseaseRepository;
        private readonly IAppointmentDetailRepository _appointmentDetailRepository;
        private readonly IVaccineBatchRepository _vaccineBatchRepository;
        private readonly IVaccineProfileRepository _vaccineProfileRepository;
        private readonly IVaccinationScheduleRepository _vaccinationScheduleRepository;
        private readonly IVetScheduleRepository _vetScheduleRepository;
        private readonly IVaccinationCertificateRepository _vaccinationCertificateRepository;
        private readonly IVaccineExportRepository _vaccineExportRepository;
        private readonly IVaccineExportDetailRepository _vaccineExportDetailRepository;
        private readonly IColdChainLogRepository _coldChainLogRepository;
        private readonly IAddressRepository _addressRepository;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AppointmentService> _logger;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMicrochipItemRepository _microchipItemRepository;
        private readonly IHealthConditionRepository _healthConditionRepository;

        public AppointmentService(
            IAppointmentRepository appointmentRepository,
            IPetRepository petRepository,
            IDiseaseRepository diseaseRepository,
            IVaccineDiseaseRepository vaccineDiseaseRepository,
            IAppointmentDetailRepository appointmentDetailRepository,
            IVaccineBatchRepository vaccineBatchRepository,
            IVaccineProfileRepository vaccineProfileRepository,
            IVaccinationScheduleRepository vaccinationScheduleRepository,
            IVetScheduleRepository vetScheduleRepository,
            IVaccinationCertificateRepository vaccinationCertificateRepository,
            IVaccineExportRepository vaccineExportRepository,
            IVaccineExportDetailRepository vaccineExportDetailRepository,
            IColdChainLogRepository coldChainLogRepository,
            IAddressRepository addressRepository,
            IConfiguration configuration,
            ILogger<AppointmentService> logger,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            IMicrochipItemRepository microchipItemRepository,
            IHealthConditionRepository healthConditionRepository)
        {
            _appointmentRepository = appointmentRepository;
            _petRepository = petRepository;
            _diseaseRepository = diseaseRepository;
            _vaccineDiseaseRepository = vaccineDiseaseRepository;
            _appointmentDetailRepository = appointmentDetailRepository;
            _vaccineBatchRepository = vaccineBatchRepository;
            _vaccineProfileRepository = vaccineProfileRepository;
            _vaccinationScheduleRepository = vaccinationScheduleRepository;
            _vetScheduleRepository = vetScheduleRepository;
            _vaccinationCertificateRepository = vaccinationCertificateRepository;
            _vaccineExportRepository = vaccineExportRepository;
            _vaccineExportDetailRepository = vaccineExportDetailRepository;
            _coldChainLogRepository = coldChainLogRepository;
            _addressRepository = addressRepository;
            _configuration = configuration;
            _logger = logger;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _microchipItemRepository = microchipItemRepository;
            _healthConditionRepository = healthConditionRepository;
        }
        private int GetSlotNumberFromAppointmentDate(DateTime appointmenDate)
        {
            var hour = appointmenDate.Hour;

            return hour switch
            {
                8 => (int)Slot.Slot_8h,
                9 => (int)Slot.Slot_9h,
                10 => (int)Slot.Slot_10h,
                11 => (int)Slot.Slot_11h,
                13 => (int)Slot.Slot_13h,
                14 => (int)Slot.Slot_14h,
                15 => (int)Slot.Slot_15h,
                16 => (int)Slot.Slot_16h,
                _ => throw new ArgumentException("Khung giờ hẹn không hợp lệ.")
            };
        }

        #region Full Appointment Service
        public async Task<BaseResponse<bool>> DeleteAppointmentAsync(int appointmentId, CancellationToken cancellationToken)
        {
            try
            {
                var appointment = await _appointmentRepository.GetAppointmentByIdAsync(appointmentId, cancellationToken);
                if (appointment == null)
                {
                    return new BaseResponse<bool>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Cuộc hẹn không tồn tại.",
                        Data = false
                    };
                }

                // Soft delete: set isDeleted = true
                appointment.isDeleted = true;
                appointment.ModifiedAt = DateTimeHelper.Now();
                appointment.ModifiedBy = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "System";

                var updatedAppointment = await _appointmentRepository.UpdateAppointmentAsync(appointment, cancellationToken);
                if (updatedAppointment != null && updatedAppointment.isDeleted == true)
                {
                    return new BaseResponse<bool>
                    {
                        Code = 200,
                        Success = true,
                        Message = "Xóa cuộc hẹn thành công (đã chuyển sang trạng thái đã xóa).",
                        Data = true
                    };
                }
                return new BaseResponse<bool>
                {
                    Code = 400,
                    Success = false,
                    Message = "Không thể xóa cuộc hẹn.",
                    Data = false
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Đã xảy ra lỗi khi xóa cuộc hẹn.");
                return new BaseResponse<bool>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã xảy ra lỗi khi xóa cuộc hẹn.",
                    Data = false
                };
            }
        }
        public async Task<DynamicResponse<AppointmentResponseDTO>> GetAllAppointmentAsync(GetAllItemsDTO getAllItemsDTO, CancellationToken cancellationToken)
        {
            try
            {
                var appointments = await _appointmentRepository.GetAllAppointmentsAsync(cancellationToken);
                if (!string.IsNullOrWhiteSpace(getAllItemsDTO.KeyWord))
                {
                    var keyword = getAllItemsDTO.KeyWord.ToLower();
                    appointments = appointments
                        .Where(a =>
                            // Search by AppointmentCode (case-insensitive)
                            (a.AppointmentCode != null && a.AppointmentCode.ToLower().Contains(keyword)) ||

                            // Search by Pet Name (case-insensitive, check null)
                            (a.Pet != null &&
                             a.Pet.Name != null &&
                             a.Pet.Name.ToLower().Contains(keyword)) ||

                            // Search by Customer FullName (case-insensitive, check null)
                            (a.Customer != null &&
                             a.Customer.FullName != null &&
                             a.Customer.FullName.ToLower().Contains(keyword)) ||

                            // Search by Location (convert enum to string)
                            (a.Location.ToString().ToLower().Contains(keyword)) ||

                            // Search by ServiceType (convert enum to string)
                            (a.ServiceType.ToString().ToLower().Contains(keyword)) ||

                            // Search by Address (case-insensitive, check null)
                            (a.Address != null && a.Address.ToLower().Contains(keyword))
                        )
                        .ToList();
                }
                // Filter by status if provided
                if (getAllItemsDTO?.Status.HasValue == true)
                {
                    appointments = appointments
                        .Where(a => a.isDeleted == getAllItemsDTO.Status.Value)
                        .ToList();
                }

                int pageNumber = getAllItemsDTO?.PageNumber > 0 ? getAllItemsDTO.PageNumber : 1;
                int pageSize = getAllItemsDTO?.PageSize > 0 ? getAllItemsDTO.PageSize : 10;
                int skip = (pageNumber - 1) * pageSize;
                int totalItem = appointments.Count;
                int totalPage = (int)Math.Ceiling((double)totalItem / pageSize);

                var pagedAppointments = appointments
                    .Skip(skip)
                    .Take(pageSize)
                    .ToList();

                var responseData = new MegaData<AppointmentResponseDTO>
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
                    PageData = _mapper.Map<List<AppointmentResponseDTO>>(pagedAppointments)
                };

                if (!pagedAppointments.Any())
                {
                    return new DynamicResponse<AppointmentResponseDTO>
                    {
                        Code = 200,
                        Success = false,
                        Message = "Không tìm thấy cuộc hẹn nào.",
                        Data = null
                    };
                }
                return new DynamicResponse<AppointmentResponseDTO>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy tất cả cuộc hẹn thành công.",
                    Data = responseData
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Đã xảy ra lỗi khi lấy tất cả cuộc hẹn.");
                return new DynamicResponse<AppointmentResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã xảy ra lỗi khi lấy tất cả cuộc hẹn.",
                    Data = null
                };
            }
        }
        public async Task<BaseResponse<AppointmentResponseDTO>> GetAppointmentByIdAsync(int appointmentId, CancellationToken cancellationToken)
        {
            try
            {
                var appointment = await _appointmentRepository.GetAppointmentByIdAsync(appointmentId, cancellationToken);
                if (appointment == null)
                {
                    return new BaseResponse<AppointmentResponseDTO>
                    {
                        Code = 200,
                        Success = false,
                        Message = "Cuộc hẹn không tồn tại.",
                        Data = null
                    };
                }
                var appointmentResponse = _mapper.Map<AppointmentResponseDTO>(appointment);
                return new BaseResponse<AppointmentResponseDTO>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy cuộc hẹn thành công.",
                    Data = appointmentResponse
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Đã xảy ra lỗi khi lấy cuộc hẹn theo ID.");
                return new BaseResponse<AppointmentResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã xảy ra lỗi khi lấy cuộc hẹn theo ID.",
                    Data = null
                };
            }
        }
        public async Task<BaseResponse<List<AppointmentResponseDTO>>> GetAppointmentByPetIdAsync(int petId, CancellationToken cancellationToken)
        {
            try
            {
                var appointments = await _appointmentRepository.GetAppointmentsByPetIdAsync(petId, cancellationToken);
                if (appointments == null)
                {
                    return new BaseResponse<List<AppointmentResponseDTO>>
                    {
                        Code = 202,
                        Success = false,
                        Message = "Không tìm thấy cuộc hẹn cho thú cưng này.",
                        Data = null
                    };
                }
                var appointmentResponses = _mapper.Map<List<AppointmentResponseDTO>>(appointments);
                return new BaseResponse<List<AppointmentResponseDTO>>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy cuộc hẹn theo thú cưng thành công.",
                    Data = appointmentResponses
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Đã xảy ra lỗi khi lấy cuộc hẹn theo ID thú cưng.");
                return new BaseResponse<List<AppointmentResponseDTO>>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã xảy ra lỗi khi lấy cuộc hẹn theo ID thú cưng.",
                    Data = null
                };
            }
        }
        public async Task<BaseResponse<List<AppointmentResponseDTO>>> GetAppointmentByPetAndStatusAsync(int petId, AppointmentStatus status, CancellationToken cancellationToken)
        {
            try
            {
                var appointment = await _appointmentRepository.GetAppointmentByPetIdAndStatusAsync(petId, status, cancellationToken);
                if (appointment == null)
                {
                    return new BaseResponse<List<AppointmentResponseDTO>>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Không tìm thấy cuộc hẹn cho thú cưng này với trạng thái đã chỉ định.",
                        Data = null
                    };
                }
                var appointmentResponse = _mapper.Map<List<AppointmentResponseDTO>>(appointment);
                return new BaseResponse<List<AppointmentResponseDTO>>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy cuộc hẹn theo thú cưng và trạng thái thành công.",
                    Data = appointmentResponse
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Đã xảy ra lỗi khi lấy cuộc hẹn theo ID thú cưng và trạng thái.");
                return new BaseResponse<List<AppointmentResponseDTO>>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã xảy ra lỗi khi lấy cuộc hẹn theo ID thú cưng và trạng thái.",
                    Data = null
                };
            }
        }
        public async Task<BaseResponse<List<AppointmentResponseDTO>>> GetAppointmentByCustomerIdAsync(int customerId, CancellationToken cancellationToken)
        {
            try
            {
                var appointments = await _appointmentRepository.GetAppointmentsByCustomerIdAsync(customerId, cancellationToken);
                if (appointments == null || !appointments.Any())
                {
                    return new BaseResponse<List<AppointmentResponseDTO>>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Không tìm thấy cuộc hẹn nào cho khách hàng này.",
                        Data = null
                    };
                }
                var appointmentResponses = _mapper.Map<List<AppointmentResponseDTO>>(appointments);
                return new BaseResponse<List<AppointmentResponseDTO>>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy tất cả cuộc hẹn theo ID khách hàng thành công.",
                    Data = appointmentResponses
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Đã xảy ra lỗi khi lấy tất cả cuộc hẹn theo ID khách hàng.");
                return new BaseResponse<List<AppointmentResponseDTO>>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã xảy ra lỗi khi lấy tất cả cuộc hẹn theo ID khách hàng.",
                    Data = null
                };
            }
        }
        public async Task<BaseResponse<List<AppointmentResponseDTO>>> GetAppointmentStatusAsync(AppointmentStatus status, CancellationToken cancellationToken)
        {
            try
            {
                var appointment = await _appointmentRepository.GetAppointmentsByStatusAsync(status, cancellationToken);
                if (appointment == null || !appointment.Any())
                {
                    return new BaseResponse<List<AppointmentResponseDTO>>
                    {
                        Code = 200,
                        Success = false,
                        Message = "Không tìm thấy cuộc hẹn với trạng thái đã chỉ định.",
                        Data = null
                    };
                }
                var appointmentResponse = _mapper.Map<List<AppointmentResponseDTO>>(appointment);
                return new BaseResponse<List<AppointmentResponseDTO>>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy cuộc hẹn theo trạng thái thành công.",
                    Data = appointmentResponse
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Đã xảy ra lỗi khi lấy cuộc hẹn theo ID thú cưng và trạng thái.");
                return new BaseResponse<List<AppointmentResponseDTO>>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã xảy ra lỗi khi lấy cuộc hẹn theo ID thú cưng và trạng thái.",
                    Data = null
                };
            }
        }
        public async Task<BaseResponse<List<AppointmentResponseDTO>>> GetAppointmentByCustomerIdAndStatusAsync(int customerId, AppointmentStatus status, CancellationToken cancellationToken)
        {
            try
            {
                var appointment = await _appointmentRepository.GetAppointmentsByCustomerIdAndStatusAsync(customerId, status, cancellationToken);
                if (appointment == null)
                {
                    return new BaseResponse<List<AppointmentResponseDTO>>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Không tìm thấy cuộc hẹn cho khách hàng này với trạng thái đã chỉ định.",
                        Data = null
                    };
                }
                var appointmentResponse = _mapper.Map<List<AppointmentResponseDTO>>(appointment);
                return new BaseResponse<List<AppointmentResponseDTO>>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy cuộc hẹn theo khách hàng và trạng thái thành công.",
                    Data = appointmentResponse
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Đã xảy ra lỗi khi lấy cuộc hẹn theo ID thú cưng và trạng thái.");
                return new BaseResponse<List<AppointmentResponseDTO>>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã xảy ra lỗi khi lấy cuộc hẹn theo ID thú cưng và trạng thái.",
                    Data = null
                };
            }
        }
        public async Task<DynamicResponse<AppointmentResponseDTO>> GetPastAppointmentsByCustomerIdAsync(int customerId, GetAllItemsDTO getAllItemsDTO, CancellationToken cancellationToken)
        {
            try
            {
                var now = DateTime.UtcNow;
                var appointments = await _appointmentRepository.GetPastAppointmentsByCustomerIdAsync(now, customerId, cancellationToken);
                if (!string.IsNullOrWhiteSpace(getAllItemsDTO.KeyWord))
                {
                    var keyword = getAllItemsDTO.KeyWord.ToLower();
                    appointments = appointments
                        .Where(a =>
                            // Search by AppointmentCode (case-insensitive)
                            (a.AppointmentCode != null && a.AppointmentCode.ToLower().Contains(keyword)) ||

                            // Search by Pet Name (case-insensitive, check null)
                            (a.Pet != null &&
                             a.Pet.Name != null &&
                             a.Pet.Name.ToLower().Contains(keyword)) ||

                            // Search by Customer FullName (case-insensitive, check null)
                            (a.Customer != null &&
                             a.Customer.FullName != null &&
                             a.Customer.FullName.ToLower().Contains(keyword)) ||

                            // Search by Location (convert enum to string)
                            (a.Location.ToString().ToLower().Contains(keyword)) ||

                            // Search by ServiceType (convert enum to string)
                            (a.ServiceType.ToString().ToLower().Contains(keyword)) ||

                            // Search by Address (case-insensitive, check null)
                            (a.Address != null && a.Address.ToLower().Contains(keyword))
                        )
                        .ToList();
                }

                int pageNumber = getAllItemsDTO?.PageNumber > 0 ? getAllItemsDTO.PageNumber : 1;
                int pageSize = getAllItemsDTO?.PageSize > 0 ? getAllItemsDTO.PageSize : 10;
                int skip = (pageNumber - 1) * pageSize;
                int totalItem = appointments.Count;
                int totalPage = (int)Math.Ceiling((double)totalItem / pageSize);

                var pagedAppointments = appointments
                    .Skip(skip)
                    .Take(pageSize)
                    .ToList();

                var responseData = new MegaData<AppointmentResponseDTO>
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
                    PageData = _mapper.Map<List<AppointmentResponseDTO>>(pagedAppointments)
                };

                if (!pagedAppointments.Any())
                {
                    return new DynamicResponse<AppointmentResponseDTO>
                    {
                        Code = 200,
                        Success = false,
                        Message = "Không tìm thấy cuộc hẹn nào.",
                        Data = null
                    };
                }
                return new DynamicResponse<AppointmentResponseDTO>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy tất cả cuộc hẹn thành công.",
                    Data = responseData
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Đã xảy ra lỗi khi lấy tất cả cuộc hẹn trong quá khứ.");
                return new DynamicResponse<AppointmentResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã xảy ra lỗi khi lấy tất cả cuộc hẹn trong quá khứ.",
                    Data = null
                };
            }
        }
        public async Task<DynamicResponse<AppointmentResponseDTO>> GetTodayAppointmentsByCustomerIdAsync(int customerId, GetAllItemsDTO getAllItemsDTO, CancellationToken cancellationToken)
        {
            try
            {
                var today = DateTime.UtcNow.Date;
                var appointments = await _appointmentRepository.GetTodayAppointmentsByCustomerIdAsync(today, customerId, cancellationToken);
                if (!string.IsNullOrWhiteSpace(getAllItemsDTO.KeyWord))
                {
                    var keyword = getAllItemsDTO.KeyWord.ToLower();
                    appointments = appointments
                        .Where(a =>
                            // Search by AppointmentCode (case-insensitive)
                            (a.AppointmentCode != null && a.AppointmentCode.ToLower().Contains(keyword)) ||
                            // Search by Pet Name (case-insensitive, check null)
                            (a.Pet != null &&
                             a.Pet.Name != null &&
                             a.Pet.Name.ToLower().Contains(keyword)) ||
                            // Search by Customer FullName (case-insensitive, check null)
                            (a.Customer != null &&
                             a.Customer.FullName != null &&
                             a.Customer.FullName.ToLower().Contains(keyword)) ||
                            // Search by Location (convert enum to string)
                            (a.Location.ToString().ToLower().Contains(keyword)) ||
                            // Search by ServiceType (convert enum to string)
                            (a.ServiceType.ToString().ToLower().Contains(keyword)) ||
                            // Search by Address (case-insensitive, check null)
                            (a.Address != null && a.Address.ToLower().Contains(keyword))
                        )
                        .ToList();
                }
                int pageNumber = getAllItemsDTO?.PageNumber > 0 ? getAllItemsDTO.PageNumber : 1;
                int pageSize = getAllItemsDTO?.PageSize > 0 ? getAllItemsDTO.PageSize : 10;
                int skip = (pageNumber - 1) * pageSize;
                int totalItem = appointments.Count;
                int totalPage = (int)Math.Ceiling((double)totalItem / pageSize);
                var pagedAppointments = appointments
                    .Skip(skip)
                    .Take(pageSize)
                    .ToList();
                var responseData = new MegaData<AppointmentResponseDTO>
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
                    PageData = _mapper.Map<List<AppointmentResponseDTO>>(pagedAppointments)
                };
                if (!pagedAppointments.Any())
                {
                    return new DynamicResponse<AppointmentResponseDTO>
                    {
                        Code = 200,
                        Success = false,
                        Message = "Không tìm thấy cuộc hẹn nào trong ngày hôm nay.",
                        Data = null
                    };
                }
                return new DynamicResponse<AppointmentResponseDTO>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy tất cả cuộc hẹn trong ngày hôm nay thành công.",
                    Data = responseData
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Đã xảy ra lỗi khi lấy tất cả cuộc hẹn trong ngày hôm nay.");
                return new DynamicResponse<AppointmentResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã xảy ra lỗi khi lấy tất cả cuộc hẹn trong ngày hôm nay.",
                    Data = null
                };
            }
        }
        public async Task<DynamicResponse<AppointmentResponseDTO>> GetFutureAppointmentsByCustomerIdAsync(int customerId, GetAllItemsDTO getAllItemsDTO, CancellationToken cancellationToken)
        {
            try
            {
                var now = DateTime.UtcNow;
                var appointments = await _appointmentRepository.GetFutureAppointmentsByCustomerIdAsync(now, customerId, cancellationToken);
                if (!string.IsNullOrWhiteSpace(getAllItemsDTO.KeyWord))
                {
                    var keyword = getAllItemsDTO.KeyWord.ToLower();
                    appointments = appointments
                        .Where(a =>
                            // Search by AppointmentCode (case-insensitive)
                            (a.AppointmentCode != null && a.AppointmentCode.ToLower().Contains(keyword)) ||
                            // Search by Pet Name (case-insensitive, check null)
                            (a.Pet != null &&
                             a.Pet.Name != null &&
                             a.Pet.Name.ToLower().Contains(keyword)) ||
                            // Search by Customer FullName (case-insensitive, check null)
                            (a.Customer != null &&
                             a.Customer.FullName != null &&
                             a.Customer.FullName.ToLower().Contains(keyword)) ||
                            // Search by Location (convert enum to string)
                            (a.Location.ToString().ToLower().Contains(keyword)) ||
                            // Search by ServiceType (convert enum to string)
                            (a.ServiceType.ToString().ToLower().Contains(keyword)) ||
                            // Search by Address (case-insensitive, check null)
                            (a.Address != null && a.Address.ToLower().Contains(keyword))
                        )
                        .ToList();
                }
                int pageNumber = getAllItemsDTO?.PageNumber > 0 ? getAllItemsDTO.PageNumber : 1;
                int pageSize = getAllItemsDTO?.PageSize > 0 ? getAllItemsDTO.PageSize : 10;
                int skip = (pageNumber - 1) * pageSize;
                int totalItem = appointments.Count;
                int totalPage = (int)Math.Ceiling((double)totalItem / pageSize);
                var pagedAppointments = appointments
                    .Skip(skip)
                    .Take(pageSize)
                    .ToList();
                var responseData = new MegaData<AppointmentResponseDTO>
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
                    PageData = _mapper.Map<List<AppointmentResponseDTO>>(pagedAppointments)
                };
                if (!pagedAppointments.Any())
                {
                    return new DynamicResponse<AppointmentResponseDTO>
                    {
                        Code = 200,
                        Success = false,
                        Message = "Không tìm thấy cuộc hẹn nào trong tương lai.",
                        Data = null
                    };
                }
                return new DynamicResponse<AppointmentResponseDTO>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy tất cả cuộc hẹn trong tương lai thành công.",
                    Data = responseData
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Đã xảy ra lỗi khi lấy tất cả cuộc hẹn trong tương lai.");
                return new DynamicResponse<AppointmentResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã xảy ra lỗi khi lấy tất cả cuộc hẹn trong tương lai.",
                    Data = null
                };
            }
        }
        public async Task<DynamicResponse<AppointmentForVaccinationResponseDTO>> GetAllAppointmentVaccinationAsync(GetAllItemsDTO getAllItemsDTO, int? vetId, CancellationToken cancellationToken)
        {
            try
            {
                var appointments = await _appointmentDetailRepository.GetAllAppointmentDetailsForVaccinationAsync(cancellationToken);
                // Only show items where isDeleted is false or null (not deleted)
                appointments = appointments
                    .Where(d => d.isDeleted == false || d.isDeleted == null)
                    .ToList();

                // Filter by vetId if provided
                if (vetId.HasValue && vetId > 0)
                {
                    appointments = appointments
                        .Where(d => d.VetId == vetId.Value)
                        .ToList();
                }

                if (!string.IsNullOrWhiteSpace(getAllItemsDTO.KeyWord))
                {
                    var keyword = getAllItemsDTO.KeyWord.ToLower();
                    appointments = appointments
                        .Where(d =>
                            // Search by AppointmentDetailCode
                            (d.AppointmentDetailCode != null && d.AppointmentDetailCode.ToLower().Contains(keyword)) ||

                            // Search by Vet name
                            (d.Vet != null && d.Vet.Name != null && d.Vet.Name.ToLower().Contains(keyword)) ||

                            // Search by Vaccine batch code
                            (d.VaccineBatch != null && d.VaccineBatch.Vaccine.Name != null && d.VaccineBatch.Vaccine.Name.ToLower().Contains(keyword)) ||

                            // Search by Disease name
                            (d.Disease != null && d.Disease.Name != null && d.Disease.Name.ToLower().Contains(keyword)) ||

                            // Search by Appointment status (enum)
                            (d.AppointmentStatus.ToString().ToLower().Contains(keyword))
                        )
                        .ToList();
                }
                int pageNumber = getAllItemsDTO?.PageNumber > 0 ? getAllItemsDTO.PageNumber : 1;
                int pageSize = getAllItemsDTO?.PageSize > 0 ? getAllItemsDTO.PageSize : 10;
                int skip = (pageNumber - 1) * pageSize;
                int totalItem = appointments.Count;
                int totalPage = (int)Math.Ceiling((double)totalItem / pageSize);
                var pagedAppointments = appointments
                    .Skip(skip)
                    .Take(pageSize)
                    .ToList();
                var responseData = new MegaData<AppointmentForVaccinationResponseDTO>
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
                        is_Delete = getAllItemsDTO?.Status
                    },
                    PageData = pagedAppointments.Select(detail =>
                        new AppointmentForVaccinationResponseDTO
                        {
                            Appointment = _mapper.Map<AppointmentResponseDTO>(detail.Appointment),
                            AppointmentHasDiseaseResponseDTO = _mapper.Map<AppointmentHasDiseaseResponseDTO>(detail)
                        }
                    ).ToList()
                };
                if (!pagedAppointments.Any())
                {
                    return new DynamicResponse<AppointmentForVaccinationResponseDTO>
                    {
                        Code = 200,
                        Success = true,
                        Message = "Không tìm thấy cuộc hẹn tiêm phòng nào.",
                        Data = null
                    };
                }
                return new DynamicResponse<AppointmentForVaccinationResponseDTO>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy tất cả cuộc hẹn tiêm phòng thành công.",
                    Data = responseData
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Đã xảy ra lỗi khi lấy tất cả cuộc hẹn tiêm phòng.");
                return new DynamicResponse<AppointmentForVaccinationResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã xảy ra lỗi khi lấy tất cả cuộc hẹn tiêm phòng.",
                    Data = null
                };
            }
        }
        public async Task<BaseResponse<AppointmentWithVaccinationResponseDTO>> CreateAppointmentVaccinationAsync(CreateAppointmentVaccinationDTO createAppointmentVaccinationDTO, CancellationToken cancellationToken)
        {
            if (createAppointmentVaccinationDTO == null)
            {
                return new BaseResponse<AppointmentWithVaccinationResponseDTO>
                {
                    Code = 400,
                    Success = false,
                    Message = "Dữ liệu tạo cuộc hẹn tiêm phòng không hợp lệ.",
                    Data = null
                };
            }
            if (createAppointmentVaccinationDTO.Appointment.Location == EnumList.Location.HomeVisit)
            {
                if (string.IsNullOrWhiteSpace(createAppointmentVaccinationDTO.Appointment.Address))
                {
                    return new BaseResponse<AppointmentWithVaccinationResponseDTO>
                    {
                        Code = 400,
                        Success = false,
                        Message = "Vui lòng nhập địa chỉ khi chọn dịch vụ tại nhà.",
                        Data = null
                    };
                }
                // Check if address is in Ho Chi Minh City
                bool isInHCM = await IsAddressInHoChiMinhCity(createAppointmentVaccinationDTO.Appointment.Address);
                if (!isInHCM)
                {
                    return new BaseResponse<AppointmentWithVaccinationResponseDTO>
                    {
                        Code = 400,
                        Success = false,
                        Message = "Địa chỉ bạn nhập không thuộc khu vực Thành phố Hồ Chí Minh. Vui lòng nhập địa chỉ hợp lệ trong khu vực này.",
                        Data = null
                    };
                }
            }
            if (createAppointmentVaccinationDTO.Appointment.Location == EnumList.Location.Clinic)
            {
                var addresses = await _addressRepository.GetAllAddressesAsync(CancellationToken.None);
                var defaultAddress = addresses.FirstOrDefault()?.Location;
                createAppointmentVaccinationDTO.Appointment.Address = defaultAddress;
            }
            var appointmentDate = createAppointmentVaccinationDTO.Appointment.AppointmentDate;
            var now = DateTimeHelper.Now();
            if (appointmentDate.Date == now.Date)
            {
                return new BaseResponse<AppointmentWithVaccinationResponseDTO>
                {
                    Code = 400,
                    Success = false,
                    Message = "Không cho phép đặt lịch trong ngày. Vui lòng chọn ngày khác.",
                    Data = null
                };
            }
            var hour = appointmentDate.Hour;
            if (!((hour >= 8 && hour <= 11) || (hour >= 13 && hour <= 16)))
            {
                return new BaseResponse<AppointmentWithVaccinationResponseDTO>
                {
                    Code = 400,
                    Success = false,
                    Message = "Chỉ cho phép đặt lịch trong khung giờ từ 8h-11h và 13h-16h.",
                    Data = null
                };
            }
            var pet = await _petRepository.GetPetByIdAsync(createAppointmentVaccinationDTO.Appointment.PetId, cancellationToken);
            if (pet == null || pet.CustomerId != createAppointmentVaccinationDTO.Appointment.CustomerId)
            {
                return new BaseResponse<AppointmentWithVaccinationResponseDTO>
                {
                    Code = 404,
                    Success = false,
                    Message = "Thú cưng này không thuộc quyền sở hữu của chủ nuôi này",
                    Data = null
                };
            }
            if (createAppointmentVaccinationDTO.AppointmentDetailVaccination.DiseaseId == null || createAppointmentVaccinationDTO.AppointmentDetailVaccination.DiseaseId <= 0)
            {
                return new BaseResponse<AppointmentWithVaccinationResponseDTO>
                {
                    Code = 400,
                    Success = false,
                    Message = "Vui lòng cung cấp DiseaseId cho dịch vụ tiêm phòng.",
                    Data = null
                };
            }
            var diseaseId = createAppointmentVaccinationDTO.AppointmentDetailVaccination.DiseaseId;
            var disease = await _diseaseRepository.GetDiseaseByIdAsync(diseaseId, cancellationToken);
            if (disease == null)
            {
                return new BaseResponse<AppointmentWithVaccinationResponseDTO>
                {
                    Code = 404,
                    Success = false,
                    Message = "Không tìm thấy bệnh với ID đã cung cấp.",
                    Data = null
                };
            }

            // 1. Kiểm tra lịch tiêm cùng bệnh trong cùng ngày
            var appointmentDateOnly = appointmentDate.Date;
            var appointmentsForPet = await _appointmentDetailRepository.GetAppointmentVaccinationDetailByPetId(pet.PetId, cancellationToken);
            var sameDaySameDisease = appointmentsForPet
                .Where(a => a.DiseaseId == diseaseId && a.AppointmentDate.Date == appointmentDateOnly)
                .ToList();
            if (sameDaySameDisease.Any())
            {
                return new BaseResponse<AppointmentWithVaccinationResponseDTO>
                {
                    Code = 400,
                    Success = false,
                    Message = "Đã có lịch tiêm cho bệnh này trong ngày. Vui lòng chọn ngày khác.",
                    Data = null
                };
            }

            // 2. Kiểm tra trạng thái lịch cũ cho cùng bệnh (chưa hoàn thành hoặc đang chờ xử lý)
            var pendingStatuses = new[]
            {
                EnumList.AppointmentStatus.Processing,
                EnumList.AppointmentStatus.Confirmed,
                EnumList.AppointmentStatus.CheckedIn,
                EnumList.AppointmentStatus.Processed,
                EnumList.AppointmentStatus.Paid
            };
            var unfinishedForDisease = appointmentsForPet
                .Where(a => a.DiseaseId == diseaseId && pendingStatuses.Contains(a.AppointmentStatus))
                .ToList();
            if (unfinishedForDisease.Any())
            {
                return new BaseResponse<AppointmentWithVaccinationResponseDTO>
                {
                    Code = 400,
                    Success = false,
                    Message = "Đã có lịch tiêm cho bệnh này chưa hoàn thành hoặc đang chờ xử lý. Vui lòng hoàn thành lịch cũ trước khi tạo mới.",
                    Data = null
                };
            }

            // 3. Kiểm tra khoảng cách giữa các mũi tiêm theo vaccinationSchedule
            var vaccinationSchedules = await _vaccinationScheduleRepository.GetAllVaccinationSchedulesAsync(cancellationToken);
            var schedulesForDisease = vaccinationSchedules
                .Where(s => s.DiseaseId == diseaseId)
                .OrderBy(s => s.DoseNumber)
                .ToList();
            int totalDose = schedulesForDisease.Sum(s => s.DoseNumber);

            var vaccineProfiles = await _vaccineProfileRepository.GetListVaccineProfileByPetIdAsync(pet.PetId, cancellationToken);
            var profilesForDisease = vaccineProfiles?.Where(p => p.DiseaseId == diseaseId).OrderBy(p => p.Dose ?? 0).ToList() ?? new List<VaccineProfile>();
            int completedDoses = profilesForDisease.Count(p => p.IsCompleted == true);

            if (totalDose > 0 && completedDoses >= totalDose)
            {
                return new BaseResponse<AppointmentWithVaccinationResponseDTO>
                {
                    Code = 400,
                    Success = false,
                    Message = "Bệnh này đã được tiêm đủ liều lượng theo lịch tiêm chủng cho thú cưng. Không thể tạo thêm lịch tiêm cho bệnh này.",
                    Data = null
                };
            }

            try
            {
                var random = new Random();
                var appointment = _mapper.Map<Appointment>(createAppointmentVaccinationDTO.Appointment);

                appointment.AppointmentCode = "AP" + random.Next(0, 1000000).ToString("D6");
                appointment.AppointmentDate = createAppointmentVaccinationDTO.Appointment.AppointmentDate;
                appointment.ServiceType = createAppointmentVaccinationDTO.Appointment.ServiceType;
                appointment.Location = createAppointmentVaccinationDTO.Appointment.Location;
                appointment.Address = createAppointmentVaccinationDTO.Appointment.Address;
                appointment.AppointmentStatus = EnumList.AppointmentStatus.Processing;
                appointment.CreatedAt = DateTime.UtcNow;
                appointment.CreatedBy = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "System";

                var createdAppointmentId = await _appointmentRepository.CreateAppointmentAsync(appointment, cancellationToken);
                if (createdAppointmentId == null)
                {
                    return new BaseResponse<AppointmentWithVaccinationResponseDTO>
                    {
                        Code = 500,
                        Success = false,
                        Message = "Không thể tạo cuộc hẹn tiêm phòng.",
                        Data = null
                    };
                }

                var createdAppointment = await _appointmentRepository.GetAppointmentByIdAsync(createdAppointmentId.AppointmentId, cancellationToken);

                var appointmentDetail = new AppointmentDetail
                {
                    AppointmentId = createdAppointment.AppointmentId,
                    AppointmentDate = createdAppointment.AppointmentDate,
                    ServiceType = createdAppointment.ServiceType,
                    AppointmentStatus = createdAppointment.AppointmentStatus,
                    AppointmentDetailCode = "AD" + random.Next(0, 1000000).ToString("D6"),
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "System",
                    DiseaseId = diseaseId,
                };
                var createdAppointmentDetailId = await _appointmentDetailRepository.AddAppointmentDetailAsync(appointmentDetail, cancellationToken);
                if (createdAppointmentDetailId == null)
                {
                    await _appointmentRepository.DeleteAppointmentAsync(createdAppointmentId.AppointmentId, cancellationToken);
                    return new BaseResponse<AppointmentWithVaccinationResponseDTO>
                    {
                        Code = 500,
                        Success = false,
                        Message = "Không thể tạo chi tiết cuộc hẹn tiêm phòng.",
                        Data = null
                    };
                }
                var fullDetail = await _appointmentDetailRepository.GetAppointmentDetailByIdAsync(createdAppointmentDetailId.AppointmentDetailId, cancellationToken);
                if (createdAppointment == null || fullDetail == null)
                {
                    return new BaseResponse<AppointmentWithVaccinationResponseDTO>
                    {
                        Code = 500,
                        Success = false,
                        Message = "Lỗi khi lấy thông tin cuộc hẹn đã tạo.",
                        Data = null
                    };
                }
                return new BaseResponse<AppointmentWithVaccinationResponseDTO>
                {
                    Code = 201,
                    Success = true,
                    Message = "Tạo cuộc hẹn tiêm phòng thành công.",
                    Data = new AppointmentWithVaccinationResponseDTO
                    {
                        Appointment = _mapper.Map<AppointmentResponseDTO>(createdAppointment),
                        Vaccinations = _mapper.Map<AppointmentVaccinationDetailResponseDTO>(fullDetail)
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Đã xảy ra lỗi khi tạo cuộc hẹn tiêm phòng.");
                return new BaseResponse<AppointmentWithVaccinationResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã xảy ra lỗi khi tạo cuộc hẹn tiêm phòng.",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<AppointmentVaccinationDetailResponseDTO>> UpdateAppointmentVaccination(int appointmentId, UpdateAppointmentVaccinationDTO updateAppointmentVaccinationDTO, CancellationToken cancellationToken)
        {
            if (updateAppointmentVaccinationDTO == null)
            {
                return new BaseResponse<AppointmentVaccinationDetailResponseDTO>
                {
                    Code = 400,
                    Success = false,
                    Message = "Dữ liệu cập nhật tiêm phòng không hợp lệ.",
                    Data = null
                };
            }
            try
            {
                var appointment = await _appointmentRepository.GetAppointmentByIdAsync(appointmentId, cancellationToken);
                if (appointment == null)
                {
                    return new BaseResponse<AppointmentVaccinationDetailResponseDTO>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Cuộc hẹn không tồn tại.",
                        Data = null
                    };
                }

                var currentStatus = appointment.AppointmentStatus;
                var newStatus = updateAppointmentVaccinationDTO.AppointmentStatus ?? currentStatus;
                bool isValidTransition = false;
                switch (currentStatus)
                {
                    case EnumList.AppointmentStatus.Processing:
                        isValidTransition = newStatus == EnumList.AppointmentStatus.Confirmed || newStatus == EnumList.AppointmentStatus.Cancelled;
                        break;
                    case EnumList.AppointmentStatus.Confirmed:
                        isValidTransition = newStatus == EnumList.AppointmentStatus.CheckedIn || newStatus == EnumList.AppointmentStatus.Rejected;
                        break;
                    case EnumList.AppointmentStatus.CheckedIn:
                        isValidTransition = newStatus == EnumList.AppointmentStatus.Processed || newStatus == EnumList.AppointmentStatus.Cancelled;
                        break;
                    case EnumList.AppointmentStatus.Processed:
                        isValidTransition = newStatus == EnumList.AppointmentStatus.Completed;
                        break;
                    case EnumList.AppointmentStatus.Paid:
                        isValidTransition = newStatus == EnumList.AppointmentStatus.Completed;
                        break;
                    default:
                        isValidTransition = false;
                        break;
                }
                if (newStatus != currentStatus && !isValidTransition)
                {
                    return new BaseResponse<AppointmentVaccinationDetailResponseDTO>
                    {
                        Code = 400,
                        Success = false,
                        Message = $"Không thể chuyển trạng thái từ {currentStatus} sang {newStatus}.",
                        Data = null
                    };
                }

                if (appointment.ServiceType != EnumList.ServiceType.Vaccination)
                {
                    return new BaseResponse<AppointmentVaccinationDetailResponseDTO>
                    {
                        Code = 400,
                        Success = false,
                        Message = "Cuộc hẹn này không phải là dịch vụ tiêm phòng.",
                        Data = null
                    };
                }

                var appointmentDetail = await _appointmentDetailRepository.GetAppointmentDetailsByAppointmentIdAsync(appointmentId, cancellationToken);
                if (appointmentDetail == null)
                {
                    return new BaseResponse<AppointmentVaccinationDetailResponseDTO>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Chi tiết cuộc hẹn không tồn tại.",
                        Data = null
                    };
                }

                // --- CHECK VACCINEBATCH VÀ DISEASEID ---
                if (updateAppointmentVaccinationDTO.VaccineBatchId.HasValue && updateAppointmentVaccinationDTO.DiseaseId.HasValue)
                {
                    var vaccineBatch = await _vaccineBatchRepository.GetVaccineBatchByIdAsync(updateAppointmentVaccinationDTO.VaccineBatchId.Value, cancellationToken);
                    if (vaccineBatch == null)
                    {
                        return new BaseResponse<AppointmentVaccinationDetailResponseDTO>
                        {
                            Code = 400,
                            Success = false,
                            Message = "VaccineBatchId không hợp lệ.",
                            Data = null
                        };
                    }
                    var vaccineDiseases = await _vaccineDiseaseRepository.GetVaccineDiseaseByVaccineIdAsync(vaccineBatch.VaccineId, cancellationToken);
                    var diseaseIdsOfBatch = vaccineDiseases?.Select(vd => vd.DiseaseId).ToList() ?? new List<int>();
                    if (!diseaseIdsOfBatch.Contains(updateAppointmentVaccinationDTO.DiseaseId.Value))
                    {
                        return new BaseResponse<AppointmentVaccinationDetailResponseDTO>
                        {
                            Code = 400,
                            Success = false,
                            Message = "DiseaseId không hợp lệ hoặc không liên kết với VaccineId của VaccineBatch đã chọn.",
                            Data = null
                        };
                    }
                }
                // --- END CHECK VACCINEBATCH VÀ DISEASEID ---

                if (updateAppointmentVaccinationDTO.VetId.HasValue)
                {
                    var appointmentDate = appointmentDetail.AppointmentDate;
                    var slotNumber = GetSlotNumberFromAppointmentDate(appointmentDate);

                    var vetSchedules = await _vetScheduleRepository.GetVetSchedulesByVetIdAsync(updateAppointmentVaccinationDTO.VetId.Value, cancellationToken);

                    var isValidSchedule = vetSchedules.Any(s =>
                        s.ScheduleDate.Date == appointmentDate.Date &&
                        s.SlotNumber == slotNumber &&
                        s.Status == EnumList.VetScheduleStatus.Available ||
                        s.Status == EnumList.VetScheduleStatus.Scheduled);

                    if (!isValidSchedule)
                    {
                        return new BaseResponse<AppointmentVaccinationDetailResponseDTO>
                        {
                            Code = 400,
                            Success = false,
                            Message = "Bác sĩ không có lịch làm việc vào thời gian này.",
                            Data = null
                        };
                    }

                    // Không cho chọn nếu VetScheduleStatus là Scheduled
                    var vetScheduleToCheck = vetSchedules.FirstOrDefault(s =>
                        s.ScheduleDate.Date == appointmentDate.Date &&
                        s.SlotNumber == slotNumber);

                    if (vetScheduleToCheck != null && vetScheduleToCheck.Status == EnumList.VetScheduleStatus.Scheduled)
                    {
                        return new BaseResponse<AppointmentVaccinationDetailResponseDTO>
                        {
                            Code = 400,
                            Success = false,
                            Message = "Bác sĩ đã có lịch hẹn khác vào khung giờ này.",
                            Data = null
                        };
                    }

                    // Chuyển trạng thái VetSchedule sang Scheduled nếu vet được chọn cho slot này
                    var vetScheduleToUpdate = vetSchedules.FirstOrDefault(s =>
                        s.ScheduleDate.Date == appointmentDate.Date &&
                        s.SlotNumber == slotNumber &&
                        s.Status == EnumList.VetScheduleStatus.Available);

                    if (vetScheduleToUpdate != null)
                    {
                        vetScheduleToUpdate.Status = EnumList.VetScheduleStatus.Scheduled;
                        await _vetScheduleRepository.UpdateVetScheduleAsync(vetScheduleToUpdate, cancellationToken);
                    }
                }

                // Cập nhật thông tin tiêm phòng
                appointmentDetail.DiseaseId = updateAppointmentVaccinationDTO.DiseaseId ?? appointmentDetail.DiseaseId;
                appointmentDetail.VaccineBatchId = updateAppointmentVaccinationDTO.VaccineBatchId ?? appointmentDetail.VaccineBatchId;
                if (appointmentDetail.VaccineBatchId.HasValue)
                {
                    var vaccineBatch = await _vaccineBatchRepository.GetVaccineBatchByIdAsync(appointmentDetail.VaccineBatchId.Value, cancellationToken);
                    if (vaccineBatch == null)
                    {
                        return new BaseResponse<AppointmentVaccinationDetailResponseDTO>
                        {
                            Code = 400,
                            Success = false,
                            Message = "VaccineBatchId không hợp lệ.",
                            Data = null
                        };
                    }
                }
                appointmentDetail.VetId = updateAppointmentVaccinationDTO.VetId ?? appointmentDetail.VetId;
                appointmentDetail.Reaction = updateAppointmentVaccinationDTO.Reaction ?? appointmentDetail.Reaction;
                appointmentDetail.Temperature = updateAppointmentVaccinationDTO.Temperature ?? appointmentDetail.Temperature;
                appointmentDetail.HeartRate = updateAppointmentVaccinationDTO.HeartRate ?? appointmentDetail.HeartRate;
                appointmentDetail.Others = updateAppointmentVaccinationDTO.Others ?? appointmentDetail.Others;
                appointmentDetail.GeneralCondition = updateAppointmentVaccinationDTO.GeneralCondition ?? appointmentDetail.GeneralCondition;
                appointmentDetail.Notes = updateAppointmentVaccinationDTO.Notes ?? appointmentDetail.Notes;
                appointmentDetail.AppointmentStatus = newStatus;
                appointmentDetail.ModifiedAt = DateTimeHelper.Now();
                appointmentDetail.ModifiedBy = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "System";

                appointment.AppointmentStatus = newStatus;
                appointment.ModifiedAt = DateTimeHelper.Now();
                appointment.ModifiedBy = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "System";

                using (var transaction = await _appointmentRepository.BeginTransactionAsync())
                {
                    try
                    {
                        // Nếu appointmentStatus là Processed thì trừ quantity của vaccineBatch đi 1 và cập nhật vaccineProfile
                        if (newStatus == EnumList.AppointmentStatus.Processed && appointmentDetail.VaccineBatchId.HasValue)
                        {
                            var vaccineBatch = await _vaccineBatchRepository.GetVaccineBatchByIdAsync(appointmentDetail.VaccineBatchId.Value, cancellationToken);
                            if (vaccineBatch != null)
                            {
                                if (vaccineBatch.Quantity <= 0)
                                {
                                    await transaction.RollbackAsync();
                                    return new BaseResponse<AppointmentVaccinationDetailResponseDTO>
                                    {
                                        Code = 400,
                                        Success = false,
                                        Message = "VaccineBatchId không hợp lệ hoặc đã hết hàng.",
                                        Data = null
                                    };
                                }
                                vaccineBatch.Quantity -= 1;
                                await _vaccineBatchRepository.UpdateVaccineBatchAsync(vaccineBatch, cancellationToken);

                                // Tạo mới VaccineExport
                                var vaccineExport = new VaccineExport
                                {
                                    ExportCode = "EXPORT" + new Random().Next(100000, 1000000).ToString(),
                                    ExportDate = DateTimeHelper.Now(),
                                    CreatedAt = DateTimeHelper.Now(),
                                    CreatedBy = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "System",
                                };
                                var createdExport = await _vaccineExportRepository.CreateVaccineExportAsync(vaccineExport, cancellationToken);

                                // Tạo mới VaccineExportDetail
                                if (createdExport != null)
                                {
                                    var vaccineExportDetail = new VaccineExportDetail
                                    {
                                        VaccineExportId = createdExport,
                                        VaccineBatchId = appointmentDetail.VaccineBatchId.Value,
                                        AppointmentDetailId = appointmentDetail.AppointmentDetailId,
                                        Quantity = 1,
                                        Purpose = "Tiêm phòng",
                                        Notes = "Xuất kho vắc xin để tiêm phòng",
                                        CreatedAt = DateTimeHelper.Now(),
                                        CreatedBy = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "System"
                                    };
                                    await _vaccineExportDetailRepository.CreateVaccineExportDetailAsync(vaccineExportDetail, cancellationToken);

                                    // Tạo mới ColdChainLog
                                    var coldChainLog = new ColdChainLog
                                    {
                                        VaccineBatchId = appointmentDetail.VaccineBatchId.Value,
                                        LogTime = DateTimeHelper.Now(),
                                        Temperature = 8,
                                        Humidity = 30,
                                        Event = "Tiêm phòng",
                                        Notes = "Ghi nhận xuất kho vắc xin cho tiêm phòng",
                                        RecordedAt = DateTimeHelper.Now(),
                                        RecordedBy = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "System"
                                    };
                                    await _coldChainLogRepository.CreateColdChainLogAsync(coldChainLog, cancellationToken);
                                }
                            }
                            if (appointmentDetail.DiseaseId.HasValue && appointmentDetail.VaccineBatchId.HasValue)
                            {
                                // Lấy vaccineBatch và các bệnh liên quan đến vaccine này
                                if (appointmentDetail.VaccineBatchId.HasValue)
                                {
                                    var innerVaccineBatch = await _vaccineBatchRepository.GetVaccineBatchByIdAsync(appointmentDetail.VaccineBatchId.Value, cancellationToken);
                                    if (innerVaccineBatch == null)
                                    {
                                        return new BaseResponse<AppointmentVaccinationDetailResponseDTO>
                                        {
                                            Code = 400,
                                            Success = false,
                                            Message = "VaccineBatchId không hợp lệ.",
                                            Data = null
                                        };
                                    }
                                    if (innerVaccineBatch != null)
                                    {
                                        var vaccineDiseases = await _vaccineDiseaseRepository.GetVaccineDiseaseByVaccineIdAsync(innerVaccineBatch.VaccineId, cancellationToken);
                                        var diseaseIdsOfBatch = vaccineDiseases?.Select(vd => vd.DiseaseId).ToList() ?? new List<int>();

                                        // Lấy toàn bộ vaccineProfile của pet
                                        var profiles = await _vaccineProfileRepository.GetListVaccineProfileByPetIdAsync(appointment.PetId, cancellationToken);

                                        foreach (var diseaseId in diseaseIdsOfBatch)
                                        {
                                            // Lấy các profile của bệnh này, sắp xếp theo Dose
                                            var diseaseProfiles = profiles
                                                .Where(p => p.DiseaseId == diseaseId)
                                                .OrderBy(p => p.Dose ?? 0)
                                                .ToList();

                                            // Cập nhật các mũi tiêm chưa hoàn thành theo thứ tự (dose tăng dần)
                                            for (int i = 1; i <= 100; i++)
                                            {
                                                var doseProfile = diseaseProfiles.FirstOrDefault(p => (p.Dose ?? i) == i);
                                                if (doseProfile != null && doseProfile.IsCompleted != true &&
                                                    (i == 1 || (diseaseProfiles.FirstOrDefault(p2 => (p2.Dose ?? (i - 1)) == (i - 1))?.IsCompleted == true)))
                                                {
                                                    doseProfile.AppointmentDetailId = appointmentDetail.AppointmentDetailId;
                                                    doseProfile.VaccinationDate = appointmentDetail.AppointmentDate;
                                                    doseProfile.Reaction = appointmentDetail.Reaction;
                                                    doseProfile.IsActive = true;
                                                    doseProfile.IsCompleted = true;
                                                    doseProfile.ModifiedAt = DateTimeHelper.Now();
                                                    doseProfile.ModifiedBy = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "System";
                                                    // Tránh lỗi tracking disease
                                                    doseProfile.Disease = null;
                                                    doseProfile.AppointmentDetail = null;
                                                    await _vaccineProfileRepository.UpdateVaccineProfileAsync(doseProfile, cancellationToken);

                                                    // Cập nhật ngày tiêm dự kiến cho tất cả các mũi tiếp theo cho tới mũi cuối cùng
                                                    var schedulesForDisease = await _vaccinationScheduleRepository.GetVaccinationScheduleByDiseaseIdAsync(diseaseId, cancellationToken);
                                                    var lastVaccinationDate = appointmentDetail.AppointmentDate;
                                                    for (int nextDose = i + 1; nextDose <= diseaseProfiles.Max(p => p.Dose ?? 0) + schedulesForDisease.Count(); nextDose++)
                                                    {
                                                        var nextProfile = diseaseProfiles.FirstOrDefault(p => (p.Dose ?? nextDose) == nextDose && p.IsCompleted != true);
                                                        var nextSchedule = schedulesForDisease.FirstOrDefault(s => s.DoseNumber == nextDose);
                                                        if (nextProfile != null && nextSchedule != null)
                                                        {
                                                            // Nếu mũi hiện tại đã tiêm, dự kiến mũi tiếp theo = ngày tiêm hiện tại + ageInterval (tính theo tuần)
                                                            lastVaccinationDate = lastVaccinationDate.AddDays(nextSchedule.AgeInterval * 7);
                                                            nextProfile.PreferedDate = lastVaccinationDate;
                                                            nextProfile.Disease = null;
                                                            nextProfile.AppointmentDetail = null;
                                                            await _vaccineProfileRepository.UpdateVaccineProfileAsync(nextProfile, cancellationToken);
                                                        }
                                                    }
                                                    break;
                                                }
                                            }
                                            // BẮT ĐẦU: Tạo mũi tiêm nhắc lại sau 1 năm nếu đã tiêm đủ liều
                                            var vaccinationSchedules = await _vaccinationScheduleRepository.GetVaccinationScheduleByDiseaseIdAsync(diseaseId, cancellationToken);
                                            int totalDose = vaccinationSchedules?.Sum(s => s.DoseNumber) ?? 0;
                                            int completedDoses = diseaseProfiles.Count(p => p.IsCompleted == true);

                                            if (totalDose > 0 && completedDoses >= totalDose)
                                            {
                                                // Kiểm tra đã có mũi tiêm nhắc lại chưa
                                                var reminderProfile = diseaseProfiles.FirstOrDefault(p => p.Dose == totalDose + 1);
                                                if (reminderProfile == null)
                                                {
                                                    var lastCompleted = diseaseProfiles.Where(p => p.IsCompleted == true).OrderByDescending(p => p.Dose ?? 0).FirstOrDefault();
                                                    var preferedDate = lastCompleted?.VaccinationDate?.AddYears(1) ?? DateTimeHelper.Now().AddYears(1);

                                                    var newReminderProfile = new VaccineProfile
                                                    {
                                                        PetId = appointment.PetId,
                                                        DiseaseId = diseaseId,
                                                        Dose = totalDose + 1,
                                                        IsActive = true,
                                                        IsCompleted = false,
                                                        PreferedDate = preferedDate,
                                                        CreatedAt = DateTimeHelper.Now(),
                                                        CreatedBy = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "System",
                                                        VaccinationScheduleId = null
                                                    };
                                                    _logger.LogInformation("Creating new reminder VaccineProfile for PetId: {PetId}, DiseaseId: {DiseaseId}, Dose: {Dose}, PreferedDate: {PreferedDate}",
                                                        appointment.PetId, diseaseId, totalDose + 1, preferedDate);
                                                    await _vaccineProfileRepository.CreateVaccineProfileAsync(newReminderProfile, cancellationToken);
                                                }

                                                else
                                                {
                                                    _logger.LogInformation("Reminder VaccineProfile already exists for PetId: {PetId}, DiseaseId: {DiseaseId}, Dose: {Dose}",
                                                        appointment.PetId, diseaseId, totalDose + 1);
                                                }
                                            }
                                            // KẾT THÚC: Tạo mũi tiêm nhắc lại sau 1 năm
                                        }
                                    }
                                }
                            }
                            await UpdateNextVaccinationInfoForAllDiseases(appointmentDetail, appointment.PetId, cancellationToken);
                        }

                        int rowEffected = await _appointmentDetailRepository.UpdateAppointmentDetailAsync(appointmentDetail, cancellationToken);
                        var updatedAppointment = await _appointmentRepository.UpdateAppointmentAsync(appointment, cancellationToken);

                        if (rowEffected <= 0 || updatedAppointment == null)
                        {
                            await transaction.RollbackAsync();
                            return new BaseResponse<AppointmentVaccinationDetailResponseDTO>
                            {
                                Code = 500,
                                Success = false,
                                Message = "Không thể cập nhật thông tin tiêm phòng cho cuộc hẹn.",
                                Data = null
                            };
                        }

                        await NotifyCustomerIfCancelledOrRejectedAsync(appointment, cancellationToken);

                        await transaction.CommitAsync();
                        var appointmentVaccinationDetailResponse = _mapper.Map<AppointmentVaccinationDetailResponseDTO>(appointmentDetail);
                        return new BaseResponse<AppointmentVaccinationDetailResponseDTO>
                        {
                            Code = 200,
                            Success = true,
                            Message = "Cập nhật thông tin tiêm phòng thành công.",
                            Data = appointmentVaccinationDetailResponse
                        };
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        _logger.LogError(ex, "Đã xảy ra lỗi khi cập nhật thông tin tiêm phòng cho cuộc hẹn.");
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Đã xảy ra lỗi khi cập nhật thông tin tiêm phòng cho cuộc hẹn.");
                return new BaseResponse<AppointmentVaccinationDetailResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã xảy ra lỗi khi cập nhật thông tin tiêm phòng cho cuộc hẹn.",
                    Data = null
                };
            }
        }
        public async Task<BaseResponse<AppointmentForVaccinationResponseDTO>> UpdateAppointmentVaccinationAsync(int appointmentId, UpdateAppointmentForVaccinationDTO updateAppointmentForVaccinationDTO, CancellationToken cancellationToken)
        {
            if (updateAppointmentForVaccinationDTO == null || updateAppointmentForVaccinationDTO.Appointment == null || updateAppointmentForVaccinationDTO.UpdateDiseaseForAppointmentDTO == null)
            {
                return new BaseResponse<AppointmentForVaccinationResponseDTO>
                {
                    Code = 400,
                    Success = false,
                    Message = "Dữ liệu cập nhật cuộc hẹn tiêm phòng không hợp lệ.",
                    Data = null
                };
            }
            var appointment = await _appointmentRepository.GetAppointmentByIdAsync(appointmentId, cancellationToken);
            if (appointment == null)
            {
                return new BaseResponse<AppointmentForVaccinationResponseDTO>
                {
                    Code = 404,
                    Success = false,
                    Message = "Không tìm thấy cuộc hẹn với ID đã cung cấp.",
                    Data = null
                };
            }
            var appointmentDetail = await _appointmentDetailRepository.GetAppointmentDetailsByAppointmentIdAsync(appointmentId, cancellationToken);
            if (appointmentDetail == null)
            {
                return new BaseResponse<AppointmentForVaccinationResponseDTO>
                {
                    Code = 404,
                    Success = false,
                    Message = "Không tìm thấy chi tiết cuộc hẹn với ID đã cung cấp.",
                    Data = null
                };
            }
            try
            {
                // Update Appointment fields
                var updateApp = updateAppointmentForVaccinationDTO.Appointment;

                // Check address like CreateAppointmentVaccinationAsync
                if (updateApp.Location == EnumList.Location.HomeVisit)
                {
                    if (string.IsNullOrWhiteSpace(updateApp.Address))
                    {
                        return new BaseResponse<AppointmentForVaccinationResponseDTO>
                        {
                            Code = 400,
                            Success = false,
                            Message = "Vui lòng nhập địa chỉ khi chọn dịch vụ tại nhà.",
                            Data = null
                        };
                    }
                    bool isInHCM = await IsAddressInHoChiMinhCity(updateApp.Address);
                    if (!isInHCM)
                    {
                        return new BaseResponse<AppointmentForVaccinationResponseDTO>
                        {
                            Code = 400,
                            Success = false,
                            Message = "Địa chỉ bạn nhập không thuộc khu vực Thành phố Hồ Chí Minh. Vui lòng nhập địa chỉ hợp lệ trong khu vực này.",
                            Data = null
                        };
                    }
                }
                else
                {
                    // Nếu không phải HomeVisit, vẫn kiểm tra nếu có Address truyền vào thì phải thuộc HCM
                    if (!string.IsNullOrWhiteSpace(updateApp.Address))
                    {
                        bool isInHCM = await IsAddressInHoChiMinhCity(updateApp.Address);
                        if (!isInHCM)
                        {
                            return new BaseResponse<AppointmentForVaccinationResponseDTO>
                            {
                                Code = 400,
                                Success = false,
                                Message = "Địa chỉ bạn nhập không thuộc khu vực Thành phố Hồ Chí Minh. Vui lòng nhập địa chỉ hợp lệ trong khu vực này.",
                                Data = null
                            };
                        }
                    }
                }
                if (updateApp.Location == EnumList.Location.Clinic)
                {
                    var addresses = await _addressRepository.GetAllAddressesAsync(CancellationToken.None);
                    var defaultAddress = addresses.FirstOrDefault()?.Location;
                    updateApp.Address = defaultAddress;
                }

                if (updateApp.AppointmentDate.HasValue)
                    appointment.AppointmentDate = updateApp.AppointmentDate.Value;
                if (updateApp.Location.HasValue)
                    appointment.Location = updateApp.Location.Value;
                if (!string.IsNullOrWhiteSpace(updateApp.Address))
                    appointment.Address = updateApp.Address;
                appointment.ModifiedAt = DateTime.UtcNow;
                appointment.ModifiedBy = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "System";

                // Update AppointmentDetail fields
                var updateDetail = updateAppointmentForVaccinationDTO.UpdateDiseaseForAppointmentDTO;
                if (updateDetail.DiseaseId > 0)
                    appointmentDetail.DiseaseId = updateDetail.DiseaseId;
                appointmentDetail.AppointmentDate = updateApp.AppointmentDate ?? appointmentDetail.AppointmentDate;
                appointmentDetail.ModifiedAt = DateTimeHelper.Now();
                appointmentDetail.ModifiedBy = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "System";

                await _appointmentRepository.UpdateAppointmentAsync(appointment, cancellationToken);
                await _appointmentDetailRepository.UpdateAppointmentDetailAsync(appointmentDetail, cancellationToken);

                // Lấy lại thông tin đã cập nhật
                var updatedAppointment = await _appointmentRepository.GetAppointmentByIdAsync(appointment.AppointmentId, cancellationToken);
                var updatedAppointmentDetail = await _appointmentDetailRepository.GetAppointmentDetailByIdAsync(appointmentDetail.AppointmentDetailId, cancellationToken);

                return new BaseResponse<AppointmentForVaccinationResponseDTO>
                {
                    Code = 200,
                    Success = true,
                    Message = "Cập nhật thông tin cuộc hẹn tiêm phòng thành công.",
                    Data = new AppointmentForVaccinationResponseDTO
                    {
                        Appointment = _mapper.Map<AppointmentResponseDTO>(updatedAppointment),
                        AppointmentHasDiseaseResponseDTO = _mapper.Map<AppointmentHasDiseaseResponseDTO>(updatedAppointmentDetail)
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Đã xảy ra lỗi khi cập nhật cuộc hẹn tiêm phòng.");
                return new BaseResponse<AppointmentForVaccinationResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã xảy ra lỗi khi cập nhật cuộc hẹn tiêm phòng.",
                    Data = null
                };
            }
        }
        public async Task<BaseResponse<AppointmentForVaccinationResponseDTO>> GetAppointmentVaccinationByIdAsync(int appointmentId, CancellationToken cancellationToken)
        {
            try
            {
                var appointment = await _appointmentRepository.GetAppointmentByIdAsync(appointmentId, cancellationToken);
                if (appointment == null || appointment.ServiceType != EnumList.ServiceType.Vaccination)
                {
                    return new BaseResponse<AppointmentForVaccinationResponseDTO>
                    {
                        Code = 200,
                        Success = false,
                        Message = "Không tìm thấy cuộc hẹn tiêm chủng với ID đã cung cấp.",
                        Data = null
                    };
                }
                var appointmentDetail = await _appointmentDetailRepository.GetAppointmentDetailsByAppointmentIdAsync(appointmentId, cancellationToken);
                if (appointmentDetail == null || appointment.ServiceType != EnumList.ServiceType.Vaccination)
                {
                    return new BaseResponse<AppointmentForVaccinationResponseDTO>
                    {
                        Code = 200,
                        Success = false,
                        Message = "Không tìm thấy chi tiết cuộc hẹn tiêm chủng với ID đã cung cấp.",
                        Data = null
                    };
                }
                return new BaseResponse<AppointmentForVaccinationResponseDTO>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy thông tin cuộc hẹn tiêm phòng thành công.",
                    Data = new AppointmentForVaccinationResponseDTO
                    {
                        Appointment = _mapper.Map<AppointmentResponseDTO>(appointment),
                        AppointmentHasDiseaseResponseDTO = _mapper.Map<AppointmentHasDiseaseResponseDTO>(appointmentDetail)
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Đã xảy ra lỗi khi lấy thông tin cuộc hẹn tiêm phòng.");
                return new BaseResponse<AppointmentForVaccinationResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã xảy ra lỗi khi lấy thông tin cuộc hẹn tiêm phòng.",
                    Data = null
                };
            }
        }
        public async Task<BaseResponse<AppointmentWithMicorchipResponseDTO>> CreateAppointmentMicrochipAsync(CreateAppointmentMicrochipDTO createAppointmentMicrochipDTO, CancellationToken cancellationToken)
        {
            if (createAppointmentMicrochipDTO == null)
            {
                return new BaseResponse<AppointmentWithMicorchipResponseDTO>
                {
                    Code = 200,
                    Success = false,
                    Message = "Dữ liệu tạo cuộc hẹn tiêm phòng không hợp lệ.",
                    Data = null
                };
            }
            // Check ServiceType == 2 (Microchip)
            if (createAppointmentMicrochipDTO.Appointment.ServiceType != EnumList.ServiceType.Microchip)
            {
                return new BaseResponse<AppointmentWithMicorchipResponseDTO>
                {
                    Code = 400,
                    Success = false,
                    Message = "Loại dịch vụ không hợp lệ. Chỉ cho phép tạo cuộc hẹn với dịch vụ là 2 - Microchip.",
                    Data = null
                };
            }
            if (createAppointmentMicrochipDTO.Appointment.Location == EnumList.Location.HomeVisit)
            {
                if (string.IsNullOrWhiteSpace(createAppointmentMicrochipDTO.Appointment.Address))
                {
                    return new BaseResponse<AppointmentWithMicorchipResponseDTO>
                    {
                        Code = 200,
                        Success = false,
                        Message = "Vui lòng nhập địa chỉ khi chọn dịch vụ tại nhà.",
                        Data = null
                    };
                }
                // Check if address is in Ho Chi Minh City
                bool isInHCM = await IsAddressInHoChiMinhCity(createAppointmentMicrochipDTO.Appointment.Address);
                if (!isInHCM)
                {
                    return new BaseResponse<AppointmentWithMicorchipResponseDTO>
                    {
                        Code = 400,
                        Success = false,
                        Message = "Địa chỉ bạn nhập không thuộc khu vực Thành phố Hồ Chí Minh. Vui lòng nhập địa chỉ hợp lệ trong khu vực này.",
                        Data = null
                    };
                }
            }
            if (createAppointmentMicrochipDTO.Appointment.Location == EnumList.Location.Clinic)
            {
                var addresses = await _addressRepository.GetAllAddressesAsync(CancellationToken.None);
                var defaultAddress = addresses.FirstOrDefault()?.Location;
                createAppointmentMicrochipDTO.Appointment.Address = defaultAddress;
            }
            var appointmentDate = createAppointmentMicrochipDTO.Appointment.AppointmentDate;
            var now = DateTimeHelper.Now();
            if (appointmentDate.Date == now.Date)
            {
                return new BaseResponse<AppointmentWithMicorchipResponseDTO>
                {
                    Code = 400,
                    Success = false,
                    Message = "Không cho phép đặt lịch trong ngày. Vui lòng chọn ngày khác.",
                    Data = null
                };
            }
            var hour = createAppointmentMicrochipDTO.Appointment.AppointmentDate.Hour;
            if (!((hour >= 8 && hour <= 11) || (hour >= 13 && hour <= 16)))
            {
                return new BaseResponse<AppointmentWithMicorchipResponseDTO>
                {
                    Code = 400,
                    Success = false,
                    Message = "Chỉ cho phép đặt lịch trong khung giờ từ 8h-11h và 13h-16h.",
                    Data = null
                };
            }
            var pet = await _petRepository.GetPetByIdAsync(createAppointmentMicrochipDTO.Appointment.PetId, cancellationToken);
            if (pet == null || pet.CustomerId != createAppointmentMicrochipDTO.Appointment.CustomerId)
            {
                return new BaseResponse<AppointmentWithMicorchipResponseDTO>
                {
                    Code = 200,
                    Success = false,
                    Message = "Thú cưng này không thuộc quyền sở hữu của chủ nuôi này",
                    Data = null
                };
            }

            try
            {
                var random = new Random();
                var appointment = _mapper.Map<Appointment>(createAppointmentMicrochipDTO.Appointment);

                appointment.AppointmentCode = "AP" + random.Next(0, 1000000).ToString("D6");
                appointment.AppointmentDate = createAppointmentMicrochipDTO.Appointment.AppointmentDate;
                appointment.ServiceType = createAppointmentMicrochipDTO.Appointment.ServiceType;
                appointment.Location = createAppointmentMicrochipDTO.Appointment.Location;
                appointment.Address = createAppointmentMicrochipDTO.Appointment.Address;
                appointment.AppointmentStatus = EnumList.AppointmentStatus.Processing;
                appointment.CreatedAt = DateTime.UtcNow;
                appointment.CreatedBy = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "System";

                var createdAppointment = await _appointmentRepository.CreateAppointmentAsync(appointment, cancellationToken);
                if (createdAppointment == null)
                {
                    return new BaseResponse<AppointmentWithMicorchipResponseDTO>
                    {
                        Code = 500,
                        Success = false,
                        Message = "Không thể tạo cuộc hẹn cấy microchip.",
                        Data = null
                    };
                }

                var createdAppointmentId = await _appointmentRepository.GetAppointmentByIdAsync(createdAppointment.AppointmentId, cancellationToken);

                var appointmentDetail = new AppointmentDetail
                {
                    AppointmentId = createdAppointment.AppointmentId,
                    AppointmentDate = createdAppointment.AppointmentDate,
                    ServiceType = createdAppointment.ServiceType,
                    AppointmentStatus = createdAppointment.AppointmentStatus,
                    AppointmentDetailCode = "AD" + random.Next(0, 1000000).ToString("D6"),
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "System",
                };

                var createdAppointmentDetail = await _appointmentDetailRepository.AddAppointmentDetailAsync(appointmentDetail, cancellationToken);
                if (createdAppointmentDetail == null)
                {
                    await _appointmentRepository.DeleteAppointmentAsync(createdAppointment.AppointmentId, cancellationToken);
                    return new BaseResponse<AppointmentWithMicorchipResponseDTO>
                    {
                        Code = 500,
                        Success = false,
                        Message = "Không thể tạo chi tiết cuộc hẹn cấy microchip.",
                        Data = null
                    };
                }

                var createdAppointmentDetailId = await _appointmentDetailRepository.GetAppointmentDetailByIdAsync(createdAppointmentDetail.AppointmentDetailId, cancellationToken);
                if (createdAppointmentId == null || createdAppointmentDetailId == null)
                {
                    return new BaseResponse<AppointmentWithMicorchipResponseDTO>
                    {
                        Code = 500,
                        Success = false,
                        Message = "Lỗi khi lấy thông tin cuộc hẹn đã tạo.",
                        Data = null
                    };
                }
                return new BaseResponse<AppointmentWithMicorchipResponseDTO>
                {
                    Code = 201,
                    Success = true,
                    Message = "Tạo cuộc hẹn tiêm phòng thành công.",
                    Data = new AppointmentWithMicorchipResponseDTO
                    {
                        Appointment = _mapper.Map<AppointmentResponseDTO>(createdAppointmentId),
                        Microchip = _mapper.Map<AppointmentMicrochipResponseDTO>(createdAppointmentDetailId)
                    }
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<AppointmentWithMicorchipResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã xảy ra lỗi khi tạo cuộc hẹn tiêm phòng. " + ex.Message,
                    Data = null
                };
            }
        }
        public async Task<BaseResponse<AppointmentMicrochipResponseDTO>> UpdateAppointmentMicrochip(UpdateAppointmentMicrochipDTO updateAppointmentMicrochipDTO, CancellationToken cancellationToken)
        {
            if (updateAppointmentMicrochipDTO == null)
            {
                return new BaseResponse<AppointmentMicrochipResponseDTO>
                {
                    Code = 400,
                    Success = false,
                    Message = "Dữ liệu cập nhật không hợp lệ.",
                    Data = null
                };
            }
            try
            {
                var appointment = await _appointmentRepository.GetAppointmentByIdAsync(updateAppointmentMicrochipDTO.AppointmentId, cancellationToken);
                if (appointment == null)
                {
                    return new BaseResponse<AppointmentMicrochipResponseDTO>
                    {
                        Code = 200,
                        Success = false,
                        Message = "Cuộc hẹn không tồn tại.",
                        Data = null
                    };
                }

                // Validate allowed status transitions
                var currentStatus = appointment.AppointmentStatus;
                var newStatus = updateAppointmentMicrochipDTO.AppointmentStatus ?? currentStatus;
                bool isValidTransition = false;
                switch (currentStatus)
                {
                    case EnumList.AppointmentStatus.Processing:
                        isValidTransition = newStatus == EnumList.AppointmentStatus.Confirmed || newStatus == EnumList.AppointmentStatus.Cancelled;
                        break;
                    case EnumList.AppointmentStatus.Confirmed:
                        isValidTransition = newStatus == EnumList.AppointmentStatus.CheckedIn || newStatus == EnumList.AppointmentStatus.Rejected;
                        break;
                    case EnumList.AppointmentStatus.CheckedIn:
                        isValidTransition = newStatus == EnumList.AppointmentStatus.Processed || newStatus == EnumList.AppointmentStatus.Cancelled;
                        break;
                    case EnumList.AppointmentStatus.Processed:
                        isValidTransition = newStatus == EnumList.AppointmentStatus.Completed;
                        break;
                    case EnumList.AppointmentStatus.Paid:
                        isValidTransition = newStatus == EnumList.AppointmentStatus.Completed;
                        break;
                    default:
                        isValidTransition = false;
                        break;
                }
                if (newStatus != currentStatus && !isValidTransition)
                {
                    return new BaseResponse<AppointmentMicrochipResponseDTO>
                    {
                        Code = 200,
                        Success = false,
                        Message = $"Không thể chuyển trạng thái từ {currentStatus} sang {newStatus}.",
                        Data = null
                    };
                }

                if (appointment.ServiceType != EnumList.ServiceType.Microchip)
                {
                    return new BaseResponse<AppointmentMicrochipResponseDTO>
                    {
                        Code = 400,
                        Success = false,
                        Message = "Cuộc hẹn này không phải là dịch vụ cấy microchip.",
                        Data = null
                    };
                }

                var appointmentDetail = await _appointmentDetailRepository.GetAppointmentDetailsByAppointmentIdAsync(updateAppointmentMicrochipDTO.AppointmentId, cancellationToken);
                if (appointmentDetail == null)
                {
                    return new BaseResponse<AppointmentMicrochipResponseDTO>
                    {
                        Code = 200,
                        Success = false,
                        Message = "Chi tiết cuộc hẹn không tồn tại.",
                        Data = null
                    };
                }

                // Check and update VetSchedule (same as UpdateAppointmentVaccination)
                if (updateAppointmentMicrochipDTO.VetId.HasValue)
                {
                    var appointmentDate = appointmentDetail.AppointmentDate;
                    var slotNumber = GetSlotNumberFromAppointmentDate(appointmentDate);

                    var vetSchedules = await _vetScheduleRepository.GetVetSchedulesByVetIdAsync(updateAppointmentMicrochipDTO.VetId.Value, cancellationToken);

                    var isValidSchedule = vetSchedules.Any(s =>
                        s.ScheduleDate.Date == appointmentDate.Date &&
                        s.SlotNumber == slotNumber &&
                        (s.Status == EnumList.VetScheduleStatus.Available ||
                         s.Status == EnumList.VetScheduleStatus.Scheduled));

                    if (!isValidSchedule)
                    {
                        return new BaseResponse<AppointmentMicrochipResponseDTO>
                        {
                            Code = 200,
                            Success = false,
                            Message = "Bác sĩ không có lịch làm việc vào thời gian này.",
                            Data = null
                        };
                    }

                    // Không cho chọn nếu VetScheduleStatus là Scheduled
                    var vetScheduleToCheck = vetSchedules.FirstOrDefault(s =>
                        s.ScheduleDate.Date == appointmentDate.Date &&
                        s.SlotNumber == slotNumber);

                    if (vetScheduleToCheck != null && vetScheduleToCheck.Status == EnumList.VetScheduleStatus.Scheduled)
                    {
                        return new BaseResponse<AppointmentMicrochipResponseDTO>
                        {
                            Code = 200,
                            Success = false,
                            Message = "Bác sĩ đã có lịch hẹn khác vào khung giờ này.",
                            Data = null
                        };
                    }

                    // Chuyển trạng thái VetSchedule sang Scheduled nếu vet được chọn cho slot này
                    var vetScheduleToUpdate = vetSchedules.FirstOrDefault(s =>
                        s.ScheduleDate.Date == appointmentDate.Date &&
                        s.SlotNumber == slotNumber &&
                        s.Status == EnumList.VetScheduleStatus.Available);

                    if (vetScheduleToUpdate != null)
                    {
                        vetScheduleToUpdate.Status = EnumList.VetScheduleStatus.Scheduled;
                        await _vetScheduleRepository.UpdateVetScheduleAsync(vetScheduleToUpdate, cancellationToken);
                    }
                }

                // Update microchip item if provided
                if (updateAppointmentMicrochipDTO.MicrochipItemId > 0)
                {
                    var appointmentDetailExist = await _appointmentDetailRepository.GetAppointmentDetailsByMicrochipItemIdAsync(updateAppointmentMicrochipDTO.MicrochipItemId, cancellationToken);
                    var microchipItem = await _microchipItemRepository.GetMicrochipItemByIdAsync(updateAppointmentMicrochipDTO.MicrochipItemId, cancellationToken);
                    if (appointmentDetailExist != null && appointmentDetailExist.AppointmentId != updateAppointmentMicrochipDTO.AppointmentId)
                    {
                        return new BaseResponse<AppointmentMicrochipResponseDTO>
                        {
                            Code = 200,
                            Success = false,
                            Message = "Microchip đã được sử dụng trong cuộc hẹn khác.",
                            Data = null
                        };
                    }
                    if (microchipItem == null)
                    {
                        return new BaseResponse<AppointmentMicrochipResponseDTO>
                        {
                            Code = 200,
                            Success = false,
                            Message = "Microchip không tồn tại.",
                            Data = null
                        };
                    }
                    microchipItem.Location = updateAppointmentMicrochipDTO.Description;
                    int rowEffected = await _microchipItemRepository.UpdateMicrochipItemAsync(microchipItem, cancellationToken);
                }

                appointmentDetail.VetId = updateAppointmentMicrochipDTO.VetId ?? appointmentDetail.VetId;
                appointmentDetail.MicrochipItemId = updateAppointmentMicrochipDTO.MicrochipItemId ?? appointmentDetail.MicrochipItemId;
                appointmentDetail.AppointmentStatus = newStatus;
                appointmentDetail.Notes = updateAppointmentMicrochipDTO.Note ?? appointmentDetail.Notes;
                appointmentDetail.ModifiedAt = DateTime.UtcNow;
                appointmentDetail.ModifiedBy = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "System";

                appointment.AppointmentStatus = newStatus;
                appointment.ModifiedAt = DateTime.UtcNow;
                appointment.ModifiedBy = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "System";

                using (var transaction = await _appointmentRepository.BeginTransactionAsync())
                {
                    try
                    {
                        var rowEffected = await _appointmentDetailRepository.UpdateAppointmentDetailAsync(appointmentDetail, cancellationToken);
                        var updatedAppointment = await _appointmentRepository.UpdateAppointmentAsync(appointment, cancellationToken);

                        if (updatedAppointment == null)
                        {
                            await transaction.RollbackAsync();
                            return new BaseResponse<AppointmentMicrochipResponseDTO>
                            {
                                Code = 500,
                                Success = false,
                                Message = "Không thể cập nhật thông tin microchip cho cuộc hẹn.",
                                Data = null
                            };
                        }
                        var appointmentDetailResponse = await _appointmentDetailRepository.GetAppointmentDetailsByAppointmentIdAsync(updateAppointmentMicrochipDTO.AppointmentId, cancellationToken);

                        await transaction.CommitAsync();

                        return new BaseResponse<AppointmentMicrochipResponseDTO>
                        {
                            Code = 200,
                            Success = true,
                            Message = "Cập nhật thông tin microchip cho cuộc hẹn thành công.",
                            Data = _mapper.Map<AppointmentMicrochipResponseDTO>(appointmentDetailResponse)
                        };
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        _logger.LogError(ex, "Đã xảy ra lỗi khi cập nhật thông tin  cho cuộc hẹn.");
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                return new BaseResponse<AppointmentMicrochipResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã xảy ra lỗi khi cập nhật thông tin microchip cho cuộc hẹn." + ex.Message + ex.InnerException,
                    Data = null
                };
            }
        }
        public async Task<BaseResponse<AppointmentWithMicorchipResponseDTO>> UpdateAppointmentMicrochipAsync(int appointmentId, UpdateAppointmentDTO updateAppointmentDTO, CancellationToken cancellationToken)
        {
            try
            {
                var appointmentExist = await _appointmentRepository.GetAppointmentByIdAsync(appointmentId, cancellationToken);
                if (appointmentExist == null)
                {
                    return new BaseResponse<AppointmentWithMicorchipResponseDTO>
                    {
                        Code = 200,
                        Success = false,
                        Message = "Cuộc hẹn không tồn tại.",
                        Data = null
                    };
                }
                if (appointmentExist.AppointmentStatus == AppointmentStatus.Confirmed)
                {
                    return new BaseResponse<AppointmentWithMicorchipResponseDTO>
                    {
                        Code = 200,
                        Success = false,
                        Message = "Cuộc hẹn đã được xác nhận, không thể cập nhật!",
                        Data = null
                    };
                }
                if (updateAppointmentDTO == null)
                {
                    return new BaseResponse<AppointmentWithMicorchipResponseDTO>
                    {
                        Code = 200,
                        Success = false,
                        Message = "Dữ liệu tạo cuộc hẹn cấy microchip không hợp lệ.",
                        Data = null
                    };
                }
                if (updateAppointmentDTO.Location == EnumList.Location.HomeVisit)
                {
                    if (string.IsNullOrWhiteSpace(updateAppointmentDTO.Address))
                    {
                        return new BaseResponse<AppointmentWithMicorchipResponseDTO>
                        {
                            Code = 200,
                            Success = false,
                            Message = "Vui lòng nhập địa chỉ khi chọn dịch vụ tại nhà.",
                            Data = null
                        };
                    }
                    // Check if address is in Ho Chi Minh City
                    bool isInHCM = await IsAddressInHoChiMinhCity(updateAppointmentDTO.Address);
                    if (!isInHCM)
                    {
                        return new BaseResponse<AppointmentWithMicorchipResponseDTO>
                        {
                            Code = 400,
                            Success = false,
                            Message = "Địa chỉ bạn nhập không thuộc khu vực Thành phố Hồ Chí Minh. Vui lòng nhập địa chỉ hợp lệ trong khu vực này.",
                            Data = null
                        };
                    }
                }
                else
                {
                    // Nếu không phải HomeVisit, vẫn kiểm tra nếu có Address truyền vào thì phải thuộc HCM
                    if (!string.IsNullOrWhiteSpace(updateAppointmentDTO.Address))
                    {
                        bool isInHCM = await IsAddressInHoChiMinhCity(updateAppointmentDTO.Address);
                        if (!isInHCM)
                        {
                            return new BaseResponse<AppointmentWithMicorchipResponseDTO>
                            {
                                Code = 400,
                                Success = false,
                                Message = "Địa chỉ bạn nhập không thuộc khu vực Thành phố Hồ Chí Minh. Vui lòng nhập địa chỉ hợp lệ trong khu vực này.",
                                Data = null
                            };
                        }
                    }
                }
                if (updateAppointmentDTO.Location == EnumList.Location.Clinic)
                {
                    var addresses = await _addressRepository.GetAllAddressesAsync(CancellationToken.None);
                    var defaultAddress = addresses.FirstOrDefault()?.Location;
                    updateAppointmentDTO.Address = defaultAddress;
                }

                appointmentExist.AppointmentDate = updateAppointmentDTO.AppointmentDate ?? appointmentExist.AppointmentDate;
                appointmentExist.ServiceType = updateAppointmentDTO.ServiceType ?? appointmentExist.ServiceType;
                appointmentExist.Location = updateAppointmentDTO.Location ?? appointmentExist.Location;
                appointmentExist.Address = updateAppointmentDTO.Address ?? appointmentExist.Address;
                appointmentExist.ModifiedAt = DateTime.UtcNow;
                appointmentExist.ModifiedBy = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "System";

                var updateAppointment = await _appointmentRepository.UpdateAppointmentAsync(appointmentExist, cancellationToken);
                if (updateAppointment == null)
                {
                    return new BaseResponse<AppointmentWithMicorchipResponseDTO>
                    {
                        Code = 500,
                        Success = false,
                        Message = "Không thể cập nhật cuộc hẹn cấy microchip.",
                        Data = null
                    };
                }

                var appoimentCheck = await _appointmentRepository.GetAppointmentByIdAsync(updateAppointment.AppointmentId, cancellationToken);

                if (appoimentCheck == null)
                {
                    return new BaseResponse<AppointmentWithMicorchipResponseDTO>
                    {
                        Code = 500,
                        Success = false,
                        Message = "Không tìm thấy cuộc hẹn cấy microchip vừa mới cập nhật",
                        Data = null
                    };
                }

                var appointmentDetail = await _appointmentDetailRepository.GetAppointmentDetailsByAppointmentIdAsync(appoimentCheck.AppointmentId, cancellationToken);
                if (appointmentDetail == null)
                {
                    return new BaseResponse<AppointmentWithMicorchipResponseDTO>
                    {
                        Code = 500,
                        Success = false,
                        Message = "Không tìm thấy chi tiết cuộc hẹn cấy microchip.",
                        Data = null
                    };
                }

                appointmentDetail.AppointmentId = appoimentCheck.AppointmentId;
                appointmentDetail.AppointmentDate = appoimentCheck.AppointmentDate;
                appointmentDetail.ServiceType = appoimentCheck.ServiceType;
                appointmentDetail.AppointmentStatus = appoimentCheck.AppointmentStatus;
                appointmentDetail.ModifiedAt = DateTime.UtcNow;
                appointmentDetail.ModifiedBy = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "System";

                var updatedAppointmentDetail = await _appointmentDetailRepository.UpdateAppointmentDetailAsync(appointmentDetail, cancellationToken);
                if (updatedAppointmentDetail == null)
                {
                    await _appointmentRepository.DeleteAppointmentAsync(appoimentCheck.AppointmentId, cancellationToken);
                    return new BaseResponse<AppointmentWithMicorchipResponseDTO>
                    {
                        Code = 500,
                        Success = false,
                        Message = "Không thể cập nhật chi tiết cuộc hẹn cấy microchip.",
                        Data = null
                    };
                }

                var createdAppointmentDetailId = await _appointmentDetailRepository.GetAppointmentDetailsByAppointmentIdAsync(appoimentCheck.AppointmentId, cancellationToken);
                if (updatedAppointmentDetail == null || createdAppointmentDetailId == null)
                {
                    return new BaseResponse<AppointmentWithMicorchipResponseDTO>
                    {
                        Code = 500,
                        Success = false,
                        Message = "Lỗi khi lấy thông tin cuộc hẹn đã tạo.",
                        Data = null
                    };
                }
                return new BaseResponse<AppointmentWithMicorchipResponseDTO>
                {
                    Code = 201,
                    Success = true,
                    Message = "Cập nhật cuộc hẹn cấy microchip thành công.",
                    Data = new AppointmentWithMicorchipResponseDTO
                    {
                        Appointment = _mapper.Map<AppointmentResponseDTO>(appoimentCheck),
                        Microchip = _mapper.Map<AppointmentMicrochipResponseDTO>(createdAppointmentDetailId)
                    }
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<AppointmentWithMicorchipResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã xảy ra lỗi khi tạo cuộc hẹn cấy microchip. " + ex.Message + ex.InnerException,
                    Data = null
                };
            }
        }
        public async Task<BaseResponse<AppointmentResponseDTO>> GetAppointmentMicrochipByAppointmentId(int appointmentId, CancellationToken cancellationToken)
        {
            try
            {
                var appointment = await _appointmentRepository.GetAppointmentByIdAsync(appointmentId, cancellationToken);
                if (appointment == null || appointment.ServiceType != EnumList.ServiceType.Microchip)
                {
                    return new BaseResponse<AppointmentResponseDTO>
                    {
                        Code = 200,
                        Success = false,
                        Message = "Không tìm thấy cuộc hẹn cấy microchip với ID đã cung cấp.",
                        Data = null
                    };
                }
                var appointmentDetail = await _appointmentDetailRepository.GetAppointmentDetailsByAppointmentIdAsync(appointmentId, cancellationToken);
                if (appointmentDetail == null || appointmentDetail.ServiceType != EnumList.ServiceType.Microchip)
                {
                    return new BaseResponse<AppointmentResponseDTO>
                    {
                        Code = 200,
                        Success = false,
                        Message = "Không tìm thấy chi tiết cuộc hẹn cấy microchip với ID đã cung cấp.",
                        Data = null
                    };
                }
                return new BaseResponse<AppointmentResponseDTO>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy thông tin cuộc hẹn cấy microchip thành công.",
                    Data = _mapper.Map<AppointmentResponseDTO>(appointment)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Đã xảy ra lỗi khi lấy thông tin cuộc hẹn cấy microchip.");
                return new BaseResponse<AppointmentResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã xảy ra lỗi khi lấy thông tin cuộc hẹn cấy microchip.",
                    Data = null
                };
            }
        }
        #endregion

        #region Vaccination Certificate Appointment Service
        public async Task<BaseResponse<AppointmentWithVaccinationCertificateResponseDTO>> CreateAppointmentVaccinationCertificate(CreateAppointmentVaccinationCertificateDTO createAppointmentVaccinationCertificateDTO, CancellationToken cancellationToken)
        {
            if (createAppointmentVaccinationCertificateDTO == null)
            {
                return new BaseResponse<AppointmentWithVaccinationCertificateResponseDTO>
                {
                    Code = 400,
                    Success = false,
                    Message = "Dữ liệu cuộc hẹn không hợp lệ.",
                    Data = null
                };
            }
            if (createAppointmentVaccinationCertificateDTO.AppointmentDTO.ServiceType != EnumList.ServiceType.VaccinationCertificate)
            {
                return new BaseResponse<AppointmentWithVaccinationCertificateResponseDTO>
                {
                    Code = 400,
                    Success = false,
                    Message = "Loại dịch vụ không hợp lệ. Chỉ cho phép tạo cuộc hẹn với dịch vụ là 4 - VaccinationCertificate.",
                    Data = null
                };
            }
            if (createAppointmentVaccinationCertificateDTO.AppointmentDTO.Location == EnumList.Location.HomeVisit && string.IsNullOrWhiteSpace(createAppointmentVaccinationCertificateDTO.AppointmentDTO.Address))
            {
                return new BaseResponse<AppointmentWithVaccinationCertificateResponseDTO>
                {
                    Code = 400,
                    Success = false,
                    Message = "Vui lòng nhập địa chỉ khi chọn dịch vụ tại nhà.",
                    Data = null
                };
            }
            if (createAppointmentVaccinationCertificateDTO.AppointmentDTO.Location == EnumList.Location.Clinic)
            {
                createAppointmentVaccinationCertificateDTO.AppointmentDTO.Address = "Đường D1, Long Bình, 71200, Quận 9, Ho Chi Minh City, Vietnam";
            }
            var pet = await _petRepository.GetPetByIdAsync(createAppointmentVaccinationCertificateDTO.AppointmentDTO.PetId, cancellationToken);
            if (pet == null || pet.CustomerId != createAppointmentVaccinationCertificateDTO.AppointmentDTO.CustomerId)
            {
                return new BaseResponse<AppointmentWithVaccinationCertificateResponseDTO>
                {
                    Code = 404,
                    Success = false,
                    Message = "Thú cưng này không thuộc quyền sở hữu của chủ nuôi này",
                    Data = null
                };
            }
            if (createAppointmentVaccinationCertificateDTO.AppointmentDetailVaccination.DiseaseId == null || createAppointmentVaccinationCertificateDTO.AppointmentDetailVaccination.DiseaseId <= 0)
            {
                return new BaseResponse<AppointmentWithVaccinationCertificateResponseDTO>
                {
                    Code = 400,
                    Success = false,
                    Message = "Vui lòng cung cấp DiseaseId cho dịch vụ cấp giấy chứng nhận tiêm phòng.",
                    Data = null
                };
            }
            var disease = await _diseaseRepository.GetDiseaseByIdAsync(createAppointmentVaccinationCertificateDTO.AppointmentDetailVaccination.DiseaseId, cancellationToken);
            if (disease == null)
            {
                return new BaseResponse<AppointmentWithVaccinationCertificateResponseDTO>
                {
                    Code = 404,
                    Success = false,
                    Message = "Không tìm thấy bệnh với ID đã cung cấp.",
                    Data = null
                };
            }

            // Lấy danh sách vaccine profile của pet
            var vaccineProfiles = await _vaccineProfileRepository.GetListVaccineProfileByPetIdAsync(createAppointmentVaccinationCertificateDTO.AppointmentDTO.PetId, cancellationToken);
            // Chỉ lấy profile của đúng DiseaseId
            var completedProfiles = vaccineProfiles?.Where(vp => vp.DiseaseId == createAppointmentVaccinationCertificateDTO.AppointmentDetailVaccination.DiseaseId && vp.IsCompleted == true).ToList();

            // Nếu chưa có profile nào hoàn thành cho DiseaseId này thì báo lỗi
            if (completedProfiles == null || !completedProfiles.Any())
            {
                return new BaseResponse<AppointmentWithVaccinationCertificateResponseDTO>
                {
                    Code = 400,
                    Success = false,
                    Message = $"Thú cưng này chưa có mũi tiêm nào cho bệnh {disease.Name} đã hoàn thành để xuất giấy chứng nhận.",
                    Data = null
                };
            }

            try
            {
                var random = new Random();
                var appointment = _mapper.Map<Appointment>(createAppointmentVaccinationCertificateDTO.AppointmentDTO);
                appointment.AppointmentCode = "AP" + random.Next(0, 1000000).ToString("D6");
                appointment.AppointmentDate = createAppointmentVaccinationCertificateDTO.AppointmentDTO.AppointmentDate;
                appointment.ServiceType = EnumList.ServiceType.VaccinationCertificate;
                appointment.Location = createAppointmentVaccinationCertificateDTO.AppointmentDTO.Location;
                appointment.Address = createAppointmentVaccinationCertificateDTO.AppointmentDTO.Address;
                appointment.AppointmentStatus = EnumList.AppointmentStatus.Processing;
                appointment.CreatedAt = DateTime.UtcNow;
                appointment.CreatedBy = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "System";

                var createdAppointment = await _appointmentRepository.CreateAppointmentAsync(appointment, cancellationToken);
                if (createdAppointment == null)
                {
                    return new BaseResponse<AppointmentWithVaccinationCertificateResponseDTO>
                    {
                        Code = 500,
                        Success = false,
                        Message = "Không thể tạo cuộc hẹn xuất giấy chứng nhận tiêm phòng.",
                        Data = null
                    };
                }

                var createdAppointmentId = await _appointmentRepository.GetAppointmentByIdAsync(createdAppointment.AppointmentId, cancellationToken);
                var appointmentDetail = new AppointmentDetail
                {
                    AppointmentId = createdAppointment.AppointmentId,
                    AppointmentDate = createdAppointment.AppointmentDate,
                    ServiceType = createdAppointment.ServiceType,
                    AppointmentStatus = createdAppointment.AppointmentStatus,
                    AppointmentDetailCode = "AD" + random.Next(0, 1000000).ToString("D6"),
                    DiseaseId = createAppointmentVaccinationCertificateDTO.AppointmentDetailVaccination.DiseaseId,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "System",
                };

                var createdAppointmentDetail = await _appointmentDetailRepository.AddAppointmentDetailAsync(appointmentDetail, cancellationToken);
                if (createdAppointmentDetail == null)
                {
                    await _appointmentRepository.DeleteAppointmentAsync(createdAppointment.AppointmentId, cancellationToken);
                    return new BaseResponse<AppointmentWithVaccinationCertificateResponseDTO>
                    {
                        Code = 500,
                        Success = false,
                        Message = "Không thể tạo chi tiết cuộc hẹn xuất giấy chứng nhận tiêm phòng.",
                        Data = null
                    };
                }
                var createdAppointmentDetailId = await _appointmentDetailRepository.GetAppointmentDetailByIdAsync(createdAppointmentDetail.AppointmentDetailId, cancellationToken);
                if (createdAppointmentId == null || createdAppointmentDetailId == null)
                {
                    return new BaseResponse<AppointmentWithVaccinationCertificateResponseDTO>
                    {
                        Code = 500,
                        Success = false,
                        Message = "Lỗi khi lấy thông tin cuộc hẹn đã tạo.",
                        Data = null
                    };
                }
                return new BaseResponse<AppointmentWithVaccinationCertificateResponseDTO>
                {
                    Code = 201,
                    Success = true,
                    Message = "Tạo cuộc hẹn xuất giấy chứng nhận tiêm phòng thành công.",
                    Data = new AppointmentWithVaccinationCertificateResponseDTO
                    {
                        Appointment = _mapper.Map<AppointmentResponseDTO>(createdAppointmentId),
                        VaccinationCertificate = _mapper.Map<AppointmentVaccinationCertificateResponseDTO>(createdAppointmentDetailId),
                    }
                };

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Đã xảy ra lỗi khi tạo cuộc hẹn xuất giấy chứng nhận tiêm phòng.");
                return new BaseResponse<AppointmentWithVaccinationCertificateResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã xảy ra lỗi khi tạo cuộc hẹn xuất giấy chứng nhận tiêm phòng.",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<AppointmentResponseDTO>> UpdateAppointmentVaccinationCertificate(int appointmentId, UpdateAppointmentDTO updateAppointmentDTO, CancellationToken cancellationToken)
        {
            if (updateAppointmentDTO == null)
            {
                return new BaseResponse<AppointmentResponseDTO>
                {
                    Code = 400,
                    Success = false,
                    Message = "Dữ liệu cập nhật không hợp lệ.",
                    Data = null
                };
            }
            try
            {
                var appointment = await _appointmentRepository.GetAppointmentByIdAsync(appointmentId, cancellationToken);
                if (appointment == null)
                {
                    return new BaseResponse<AppointmentResponseDTO>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Cuộc hẹn không tồn tại.",
                        Data = null
                    };
                }
                if (updateAppointmentDTO.AppointmentDate.HasValue)
                {
                    appointment.AppointmentDate = updateAppointmentDTO.AppointmentDate.Value;
                }
                appointment.Address = updateAppointmentDTO.Address;
                appointment.ModifiedAt = DateTimeHelper.Now();
                appointment.ModifiedBy = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "System";
                var updatedAppointment = await _appointmentRepository.UpdateAppointmentAsync(appointment, cancellationToken);
                if (updatedAppointment == null)
                {
                    return new BaseResponse<AppointmentResponseDTO>
                    {
                        Code = 500,
                        Success = false,
                        Message = "Không thể cập nhật cuộc hẹn xuất giấy chứng nhận tiêm phòng.",
                        Data = null
                    };
                }
                return new BaseResponse<AppointmentResponseDTO>
                {
                    Code = 200,
                    Success = true,
                    Message = "Cập nhật cuộc hẹn xuất giấy chứng nhận tiêm phòng thành công.",
                    Data = _mapper.Map<AppointmentResponseDTO>(updatedAppointment)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Đã xảy ra lỗi khi cập nhật cuộc hẹn xuất giấy chứng nhận tiêm phòng.");
                return new BaseResponse<AppointmentResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã xảy ra lỗi khi cập nhật cuộc hẹn xuất giấy chứng nhận tiêm phòng.",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<AppointmentVaccinationCertificateResponseDTO>> UpdateAppointmentDetailVaccinationCertificate(int appointmentId, UpdateAppointmentVaccinationCertificateDTO updateAppointmentVaccinationCertificateDTO, CancellationToken cancellationToken)
        {
            if (updateAppointmentVaccinationCertificateDTO == null)
            {
                return new BaseResponse<AppointmentVaccinationCertificateResponseDTO>
                {
                    Code = 400,
                    Success = false,
                    Message = "Dữ liệu cập nhật không hợp lệ.",
                    Data = null
                };
            }
            try
            {
                var appointmentDetail = await _appointmentDetailRepository.GetAppointmentDetailByIdAsync(appointmentId, cancellationToken);
                if (appointmentDetail == null)
                {
                    return new BaseResponse<AppointmentVaccinationCertificateResponseDTO>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Chi tiết cuộc hẹn không tồn tại.",
                        Data = null
                    };
                }

                // Chỉ cho phép chuyển trạng thái theo quy tắc:
                // 1 (Processing) -> 2 (Confirmed) hoặc 10 (Cancelled)
                // 2 (Confirmed) -> 3 (CheckedIn) hoặc 10 (Cancelled)
                // 3 (CheckedIn) -> 4 (Processed)
                // 4 (Processed) -> 9 (Completed)
                var currentStatus = appointmentDetail.AppointmentStatus;
                var newStatus = updateAppointmentVaccinationCertificateDTO.AppointmentStatus ?? currentStatus;
                bool isValidTransition = false;
                switch (currentStatus)
                {
                    case EnumList.AppointmentStatus.Processing:
                        isValidTransition = newStatus == EnumList.AppointmentStatus.Confirmed || newStatus == EnumList.AppointmentStatus.Cancelled;
                        break;
                    case EnumList.AppointmentStatus.Confirmed:
                        isValidTransition = newStatus == EnumList.AppointmentStatus.CheckedIn || newStatus == EnumList.AppointmentStatus.Cancelled;
                        break;
                    case EnumList.AppointmentStatus.CheckedIn:
                        isValidTransition = newStatus == EnumList.AppointmentStatus.Processed;
                        break;
                    case EnumList.AppointmentStatus.Processed:
                        isValidTransition = newStatus == EnumList.AppointmentStatus.Completed;
                        break;
                    default:
                        isValidTransition = false;
                        break;
                }
                if (newStatus != currentStatus && !isValidTransition)
                {
                    return new BaseResponse<AppointmentVaccinationCertificateResponseDTO>
                    {
                        Code = 400,
                        Success = false,
                        Message = $"Không thể chuyển trạng thái từ {currentStatus} sang {newStatus}.",
                        Data = null
                    };
                }

                // Cập nhật các trường, nếu không truyền thì giữ giá trị cũ
                appointmentDetail.VetId = updateAppointmentVaccinationCertificateDTO.VetId > 0
                    ? updateAppointmentVaccinationCertificateDTO.VetId
                    : appointmentDetail.VetId;
                appointmentDetail.DiseaseId = updateAppointmentVaccinationCertificateDTO.DiseaseId > 0
                    ? updateAppointmentVaccinationCertificateDTO.DiseaseId
                    : appointmentDetail.DiseaseId;

                appointmentDetail.AppointmentStatus = newStatus;

                // Nếu có các trường khác cần cập nhật, thêm vào đây (giữ giá trị cũ nếu không truyền)
                appointmentDetail.Notes = updateAppointmentVaccinationCertificateDTO.Notes ?? appointmentDetail.Notes;

                appointmentDetail.ModifiedAt = DateTimeHelper.Now();
                appointmentDetail.ModifiedBy = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "System";
                var rowEffected = await _appointmentDetailRepository.UpdateAppointmentDetailAsync(appointmentDetail, cancellationToken);
                if (rowEffected <= 0)
                {
                    return new BaseResponse<AppointmentVaccinationCertificateResponseDTO>
                    {
                        Code = 500,
                        Success = false,
                        Message = "Không thể cập nhật chi tiết cuộc hẹn xuất giấy chứng nhận tiêm phòng.",
                        Data = null
                    };
                }

                // Nếu chuyển sang CheckedIn (3) thì tạo mới VaccinationCertificate
                if (newStatus == EnumList.AppointmentStatus.CheckedIn)
                {
                    // Lấy thông tin petId, vetId, diseaseId
                    int? petId = null;
                    int? vetId = appointmentDetail.VetId;
                    int? diseaseId = appointmentDetail.DiseaseId;

                    // Lấy appointment để lấy petId
                    var appointment = appointmentDetail.Appointment;
                    if (appointment == null)
                    {
                        // Nếu không có navigation, lấy từ repository
                        var appointmentRepo = await _appointmentRepository.GetAppointmentByIdAsync(appointmentDetail.AppointmentId, cancellationToken);
                        petId = appointmentRepo?.PetId;
                    }
                    else
                    {
                        petId = appointment.PetId;
                    }

                    if (petId.HasValue && vetId.HasValue && diseaseId.HasValue)
                    {
                        // Lấy vaccineProfile của pet và disease
                        var vaccineProfiles = await _vaccineProfileRepository.GetListVaccineProfileByPetIdAsync(petId.Value, cancellationToken);
                        var profilesForDisease = vaccineProfiles?
                            .Where(p => p.DiseaseId == diseaseId.Value)
                            .ToList();

                        if (profilesForDisease != null && profilesForDisease.Any())
                        {
                            // Lấy doseNumber lớn nhất
                            int maxDose = profilesForDisease
                                .Where(p => p.Dose.HasValue)
                                .Select(p => p.Dose.Value)
                                .DefaultIfEmpty(1)
                                .Max();

                            // Lấy ngày tiêm của vaccineProfile có dose lớn nhất
                            var profileWithMaxDose = profilesForDisease
                                .Where(p => p.Dose == maxDose)
                                .OrderByDescending(p => p.VaccinationDate)
                                .FirstOrDefault();

                            DateTime? vaccinationDate = profileWithMaxDose?.VaccinationDate;

                            // Tạo certificateCode random
                            var random = new Random();
                            string certificateCode = "VC" + random.Next(0, 1000000).ToString("D6");

                            // Tạo mới VaccinationCertificate
                            var vaccinationCertificate = new VaccinationCertificate
                            {
                                PetId = petId.Value,
                                VetId = vetId.Value,
                                DiseaseId = diseaseId.Value,
                                CertificateCode = certificateCode,
                                DoseNumber = maxDose,
                                VaccinationDate = vaccinationDate ?? DateTime.UtcNow,
                                ClinicName = "Trung tâm tiêm chủng cho thú cưng PetVax",
                                ClinicAddress = "Đường D1, Long Bình, 71200, Quận 9, Ho Chi Minh City, Vietnam",
                                CreatedAt = DateTimeHelper.Now(),
                                CreatedBy = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "System"
                            };

                            // Lưu vào DB
                            await _vaccinationCertificateRepository.AddVaccinationCertificateAsync(vaccinationCertificate, cancellationToken);

                            // Gán lại vào appointmentDetail nếu cần
                            appointmentDetail.VaccinationCertificate = vaccinationCertificate;
                        }
                    }
                }

                // Lấy lại bản ghi đã cập nhật
                var updatedAppointmentDetail = await _appointmentDetailRepository.GetAppointmentDetailByIdAsync(appointmentId, cancellationToken);
                return new BaseResponse<AppointmentVaccinationCertificateResponseDTO>
                {
                    Code = 200,
                    Success = true,
                    Message = "Cập nhật chi tiết cuộc hẹn xuất giấy chứng nhận tiêm phòng thành công.",
                    Data = _mapper.Map<AppointmentVaccinationCertificateResponseDTO>(updatedAppointmentDetail)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Đã xảy ra lỗi khi cập nhật chi tiết cuộc hẹn xuất giấy chứng nhận tiêm phòng.");
                return new BaseResponse<AppointmentVaccinationCertificateResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã xảy ra lỗi khi cập nhật chi tiết cuộc hẹn xuất giấy chứng nhận tiêm phòng.",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<AppointmentWithVaccinationCertificateResponseDTO>> GetAppointmentVaccinationCertificateById(int appointmentId, CancellationToken cancellationToken)
        {
            if (appointmentId <= 0)
            {
                return new BaseResponse<AppointmentWithVaccinationCertificateResponseDTO>
                {
                    Code = 400,
                    Success = false,
                    Message = "ID cuộc hẹn không hợp lệ.",
                    Data = null
                };
            }
            try
            {
                var appointment = await _appointmentRepository.GetAppointmentByIdAsync(appointmentId, cancellationToken);
                if (appointment == null)
                {
                    return new BaseResponse<AppointmentWithVaccinationCertificateResponseDTO>
                    {
                        Code = 200,
                        Success = false,
                        Message = "Không tìm thấy cuộc hẹn với ID đã cung cấp.",
                        Data = null
                    };
                }
                var appointmentDetail = await _appointmentDetailRepository.GetAppointmentDetailByIdAsync(appointmentId, cancellationToken);
                if (appointmentDetail == null)
                {
                    return new BaseResponse<AppointmentWithVaccinationCertificateResponseDTO>
                    {
                        Code = 200,
                        Success = false,
                        Message = "Không tìm thấy chi tiết cuộc hẹn với ID đã cung cấp.",
                        Data = null
                    };
                }
                return new BaseResponse<AppointmentWithVaccinationCertificateResponseDTO>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy thông tin cuộc hẹn xuất giấy chứng nhận tiêm phòng thành công.",
                    Data = new AppointmentWithVaccinationCertificateResponseDTO
                    {
                        Appointment = _mapper.Map<AppointmentResponseDTO>(appointment),
                        VaccinationCertificate = _mapper.Map<AppointmentVaccinationCertificateResponseDTO>(appointmentDetail)
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Đã xảy ra lỗi khi lấy thông tin cuộc hẹn xuất giấy chứng nhận tiêm phòng.");
                return new BaseResponse<AppointmentWithVaccinationCertificateResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã xảy ra lỗi khi lấy thông tin cuộc hẹn xuất giấy chứng nhận tiêm phòng.",
                    Data = null
                };
            }
        }

        public async Task<DynamicResponse<AppointmentWithVaccinationCertificateResponseDTO>> GetAllAppointmentVaccinationCertificateAsync(GetAllItemsDTO getAllItemsDTO, CancellationToken cancellationToken)
        {
            try
            {
                var appointments = await _appointmentDetailRepository.GetAllAppointmentDetailsVaccinationCertificateAsync(cancellationToken);
                if (!string.IsNullOrWhiteSpace(getAllItemsDTO.KeyWord))
                {
                    var keyword = getAllItemsDTO.KeyWord.ToLower();
                    appointments = appointments
                        .Where(d =>
                            // Search by AppointmentDetailCode
                            (d.AppointmentDetailCode != null && d.AppointmentDetailCode.ToLower().Contains(keyword)) ||

                            // Search by Vet name
                            (d.Vet != null && d.Vet.Name != null && d.Vet.Name.ToLower().Contains(keyword)) ||

                            // Search by Vaccine batch code
                            (d.VaccineBatch != null && d.VaccineBatch.Vaccine.Name != null && d.VaccineBatch.Vaccine.Name.ToLower().Contains(keyword)) ||

                            // Search by Disease name
                            (d.Disease != null && d.Disease.Name != null && d.Disease.Name.ToLower().Contains(keyword)) ||

                            // Search by Appointment status (enum)
                            (d.AppointmentStatus.ToString().ToLower().Contains(keyword))
                        )
                        .ToList();
                }
                int pageNumber = getAllItemsDTO?.PageNumber > 0 ? getAllItemsDTO.PageNumber : 1;
                int pageSize = getAllItemsDTO?.PageSize > 0 ? getAllItemsDTO.PageSize : 10;
                int skip = (pageNumber - 1) * pageSize;
                int totalItem = appointments.Count;
                int totalPage = (int)Math.Ceiling((double)totalItem / pageSize);
                var pagedAppointments = appointments
                    .Skip(skip)
                    .Take(pageSize)
                    .ToList();
                var responseData = new MegaData<AppointmentWithVaccinationCertificateResponseDTO>
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
                        keyWord = getAllItemsDTO.KeyWord,
                        status = getAllItemsDTO.Status
                    },
                    PageData = pagedAppointments.Select(d => new AppointmentWithVaccinationCertificateResponseDTO
                    {
                        Appointment = _mapper.Map<AppointmentResponseDTO>(d.Appointment),
                        VaccinationCertificate = _mapper.Map<AppointmentVaccinationCertificateResponseDTO>(d)
                    }).ToList()
                };
                if (!pagedAppointments.Any())
                {
                    return new DynamicResponse<AppointmentWithVaccinationCertificateResponseDTO>
                    {
                        Code = 200,
                        Success = false,
                        Message = "Không tìm thấy cuộc hẹn xuất giấy chứng nhận tiêm phòng nào.",
                        Data = null
                    };
                }
                return new DynamicResponse<AppointmentWithVaccinationCertificateResponseDTO>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy danh sách cuộc hẹn xuất giấy chứng nhận tiêm phòng thành công.",
                    Data = responseData
                };

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Đã xảy ra lỗi khi lấy danh sách cuộc hẹn xuất giấy chứng nhận tiêm phòng.");
                return new DynamicResponse<AppointmentWithVaccinationCertificateResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã xảy ra lỗi khi lấy danh sách cuộc hẹn xuất giấy chứng nhận tiêm phòng.",
                    Data = null
                };
            }

        }
        private async Task UpdateNextVaccinationInfoForAllDiseases(AppointmentDetail appointmentDetail, int petId, CancellationToken cancellationToken)
        {
            // Lấy các diseaseId liên quan đến vaccine vừa tiêm
            List<int> diseaseIds = new();
            if (appointmentDetail.VaccineBatchId.HasValue)
            {
                var vaccineBatch = await _vaccineBatchRepository.GetVaccineBatchByIdAsync(appointmentDetail.VaccineBatchId.Value, cancellationToken);
                if (vaccineBatch != null)
                {
                    var vaccineDiseases = await _vaccineDiseaseRepository.GetVaccineDiseaseByVaccineIdAsync(vaccineBatch.VaccineId, cancellationToken);
                    diseaseIds = vaccineDiseases?.Select(vd => vd.DiseaseId).Distinct().ToList() ?? new();
                }
            }
            // Nếu không có vaccine batch, chỉ cập nhật cho disease của appointmentDetail
            if (!diseaseIds.Any() && appointmentDetail.DiseaseId.HasValue)
                diseaseIds.Add(appointmentDetail.DiseaseId.Value);

            var vaccineProfiles = await _vaccineProfileRepository.GetListVaccineProfileByPetIdAsync(petId, cancellationToken);

            foreach (var diseaseId in diseaseIds)
            {
                var profilesForDisease = vaccineProfiles?
                    .Where(p => p.DiseaseId == diseaseId)
                    .OrderBy(p => p.Dose ?? 0)
                    .ToList() ?? new List<VaccineProfile>();

                // Tìm mũi đã tiêm cuối cùng
                var lastCompleted = profilesForDisease
                    .Where(p => p.IsCompleted == true)
                    .OrderByDescending(p => p.Dose ?? 0)
                    .FirstOrDefault();

                // Tìm mũi tiếp theo (dose lớn hơn, chưa tiêm)
                VaccineProfile nextProfile = null;
                if (lastCompleted != null)
                {
                    nextProfile = profilesForDisease
                        .Where(p => (p.Dose ?? 0) > (lastCompleted.Dose ?? 0) && p.IsCompleted != true)
                        .OrderBy(p => p.Dose ?? 0)
                        .FirstOrDefault();
                }
                else
                {
                    nextProfile = profilesForDisease.FirstOrDefault(p => p.IsCompleted != true);
                }

                // Truyền thông tin nextVaccinationInfo vào tất cả các mũi đã tiêm của bệnh này
                if (nextProfile != null && nextProfile.PreferedDate.HasValue)
                {
                    var info = $"Dự kiến mũi tiêm tiếp theo sẽ vào ngày {nextProfile.PreferedDate.Value:dd/MM/yyyy}";
                    var completedProfiles = profilesForDisease.Where(p => p.IsCompleted == true).ToList();
                    foreach (var completedProfile in completedProfiles)
                    {
                        completedProfile.NextVaccinationInfo = info;
                        completedProfile.Disease = null;
                        completedProfile.AppointmentDetail = null;
                        await _vaccineProfileRepository.UpdateVaccineProfileAsync(completedProfile, cancellationToken);
                    }
                    if (diseaseId == appointmentDetail.DiseaseId)
                    {
                        appointmentDetail.NextVaccinationInfo = info;
                    }
                }
                // Nếu chưa tiêm mũi nào, không có lastCompleted, không set nextVaccinationInfo
            }
        }



        #endregion
        public async Task NotifyCustomerIfCancelledOrRejectedAsync(Appointment appointment, CancellationToken cancellationToken)
        {
            if (appointment.AppointmentStatus == AppointmentStatus.Cancelled || appointment.AppointmentStatus == AppointmentStatus.Rejected)
            {
                try
                {
                    var pet = await _petRepository.GetPetByIdAsync(appointment.PetId, cancellationToken);
                    var customerEmail = pet?.Customer?.Account?.Email;
                    var customerName = pet?.Customer?.FullName ?? "Quý khách hàng";
                    var petName = pet?.Name ?? "thú cưng của bạn";

                    if (!string.IsNullOrEmpty(customerEmail))
                    {
                        // Prepare SMTP configuration
                        var smtpHost = _configuration["Smtp:Host"];
                        var smtpPort = int.Parse(_configuration["Smtp:Port"]);
                        var smtpUser = _configuration["Smtp:User"];
                        var smtpPass = _configuration["Smtp:Pass"];
                        var fromEmail = _configuration["Smtp:From"];

                        using var client = new SmtpClient(smtpHost, smtpPort)
                        {
                            Credentials = new NetworkCredential(smtpUser, smtpPass),
                            EnableSsl = true
                        };

                        var statusText = appointment.AppointmentStatus == AppointmentStatus.Cancelled ? "hủy" : "từ chối";
                        var mail = new MailMessage(fromEmail, customerEmail)
                        {
                            Subject = "Thông báo hủy/từ chối cuộc hẹn",
                            Body = $@"
                        <html>
                        <body style='font-family: Arial, sans-serif; background-color: #f6f6f6; padding: 30px;'>
                        <div style='max-width: 500px; margin: auto; background: #fff; border-radius: 8px; box-shadow: 0 2px 8px rgba(0,0,0,0.05); padding: 32px;'>
                        <h2 style='color: #2d8cf0; text-align: center;'>Thông báo cập nhật lịch hẹn</h2>
                        <p style='font-size: 16px; color: #333;'>Gửi {customerName},</p>
                        <p style='font-size: 16px; color: #333;'>Chúng tôi rất tiếc phải thông báo với bạn rằng cuộc hẹn của bạn với {petName} đã bị {statusText}:</p>
                        <div style='text-align: center; margin: 24px 0;'>
                        <span style='display: inline-block; font-size: 24px; color: #2d8cf0; font-weight: bold; background: #f0f7ff; padding: 16px 32px; border-radius: 6px;'>
                            Mã lịch hẹn: {appointment.AppointmentCode}
                        </span>
                        </div>
                        <p style='font-size: 16px; color: #333;'>
                            <b>Ngày giờ:</b> {appointment.AppointmentDate:dd/MM/yyyy HH:mm}<br/>
                            <b>Trạng thái:</b> {appointment.AppointmentStatus.ToString()}
                        </p>
                        <p style='font-size: 15px; color: #555;'>
                            Vui lòng liên hệ với chúng tôi để sắp xếp lại cuộc hẹn mới hoặc để được hỗ trợ thêm.
                        </p>
                        <p style='font-size: 14px; color: #aaa; margin-top: 32px;'>
                            Nếu có bất kỳ thắc mắc nào, vui lòng liên hệ với chúng tôi qua <a href='mailto:support@petvax.com' style='color: #2d8cf0;'>support@petvax.com</a> hoặc gọi cho chúng tôi theo số 0987654321.
                        </p>
                        <p style='font-size: 14px; color: #aaa;'>Xin cảm ơn,<br/>PetVax</p>
                        </div>
                        </body>
                        </html>",
                            IsBodyHtml = true
                        };

                        // Send email
                        await client.SendMailAsync(mail, cancellationToken);
                        _logger.LogInformation("Sent {status} notification email to {email} for appointment {code}",
                            statusText, customerEmail, appointment.AppointmentCode);
                    }
                    else
                    {
                        _logger.LogWarning("No valid email found for appointment {code}", appointment.AppointmentCode);
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to send {status} notification email for appointment {code}",
                        appointment.AppointmentStatus.ToString().ToLower(), appointment.AppointmentCode);
                }
            }
        }
        #region Comment
        //public async Task<BaseResponse<AppointmentResponseDTO>> CreateAppointmentAsync(CreateAppointmentDTO createAppointmentDTO, CancellationToken cancellationToken)
        //{
        //    if (createAppointmentDTO == null)
        //    {
        //        return new BaseResponse<AppointmentResponseDTO>
        //        {
        //            Code = 400,
        //            Success = false,
        //            Message = "Dữ liệu cuộc hẹn không hợp lệ.",
        //            Data = null
        //        };
        //    }

        //    // Validate address based on location
        //    if (createAppointmentDTO.Location == EnumList.Location.HomeVisit && string.IsNullOrWhiteSpace(createAppointmentDTO.Address))
        //    {
        //        return new BaseResponse<AppointmentResponseDTO>
        //        {
        //            Code = 400,
        //            Success = false,
        //            Message = "Vui lòng nhập địa chỉ khi chọn dịch vụ tại nhà.",
        //            Data = null
        //        };
        //    }
        //    if (createAppointmentDTO.Location == EnumList.Location.Clinic)
        //    {
        //        // If Clinic, ignore address (set to null or empty)
        //        createAppointmentDTO.Address = "Đại học FPT TP. Hồ Chí Minh";
        //    }

        //    var pet = await _petRepository.GetPetByIdAsync(createAppointmentDTO.PetId, cancellationToken);
        //    if (pet == null || pet.CustomerId != createAppointmentDTO.CustomerId)
        //    {
        //        return new BaseResponse<AppointmentResponseDTO>
        //        {
        //            Code = 404,
        //            Success = false,
        //            Message = "Thú cưng này không thuộc quyền sở hữu của chủ nuôi này",
        //            Data = null
        //        };
        //    }

        //    try
        //    {
        //        var random = new Random();

        //        var appointment = _mapper.Map<Appointment>(createAppointmentDTO);
        //        appointment.AppointmentCode = "AP" + random.Next(0, 1000000).ToString("D6");
        //        appointment.AppointmentDate = createAppointmentDTO.AppointmentDate;
        //        appointment.ServiceType = createAppointmentDTO.ServiceType;
        //        appointment.Location = createAppointmentDTO.Location;
        //        appointment.Address = createAppointmentDTO.Address;
        //        appointment.AppointmentStatus = EnumList.AppointmentStatus.Processing;
        //        appointment.CreatedAt = DateTime.UtcNow;
        //        appointment.CreatedBy = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "System";

        //        var createdAppointment = await _appointmentRepository.CreateAppointmentAsync(appointment, cancellationToken);
        //        if (createdAppointment == null)
        //        {
        //            return new BaseResponse<AppointmentResponseDTO>
        //            {
        //                Code = 500,
        //                Success = false,
        //                Message = "Không thể tạo cuộc hẹn.",
        //                Data = null
        //            };
        //        }

        //        var appointmentResponse = _mapper.Map<AppointmentResponseDTO>(appointment);
        //        return new BaseResponse<AppointmentResponseDTO>
        //        {
        //            Code = 201,
        //            Success = true,
        //            Message = "Tạo cuộc hẹn thành công.",
        //            Data = appointmentResponse
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Đã xảy ra lỗi khi tạo cuộc hẹn.");
        //        return new BaseResponse<AppointmentResponseDTO>
        //        {
        //            Code = 500,
        //            Success = false,
        //            Message = "Đã xảy ra lỗi khi tạo cuộc hẹn.",
        //            Data = null
        //        };
        //    }
        //}

        //public async Task<BaseResponse<AppointmentWithDetailResponseDTO>> CreateFullAppointmentAsync(CreateFullAppointmentDTO createFullAppointmentDTO, CancellationToken cancellationToken)
        //{
        //    if (createFullAppointmentDTO == null)
        //    {
        //        return new BaseResponse<AppointmentWithDetailResponseDTO>
        //        {
        //            Code = 400,
        //            Success = false,
        //            Message = "Dữ liệu để tạo cuộc hẹn không hợp lệ.",
        //            Data = default!
        //        };
        //    }
        //    // Validate address based on location
        //    if (createFullAppointmentDTO.Appointment.Location == EnumList.Location.HomeVisit && string.IsNullOrWhiteSpace(createFullAppointmentDTO.Appointment.Address))
        //    {
        //        return new BaseResponse<AppointmentWithDetailResponseDTO>
        //        {
        //            Code = 400,
        //            Success = false,
        //            Message = "Vui lòng nhập địa chỉ khi chọn dịch vụ tại nhà.",
        //            Data = default!
        //        };
        //    }
        //    if (createFullAppointmentDTO.Appointment.Location == EnumList.Location.Clinic)
        //    {
        //        // If Clinic, ignore address (set to null or empty)
        //        createFullAppointmentDTO.Appointment.Address = "Đại học FPT TP. Hồ Chí Minh";
        //    }

        //    var pet = await _petRepository.GetPetByIdAsync(createFullAppointmentDTO.Appointment.PetId, cancellationToken);
        //    if (pet == null || pet.CustomerId != createFullAppointmentDTO.Appointment.CustomerId)
        //    {
        //        return new BaseResponse<AppointmentWithDetailResponseDTO>
        //        {
        //            Code = 404,
        //            Success = false,
        //            Message = "Thú cưng này không thuộc quyền sở hữu của chủ nuôi này",
        //            Data = default!
        //        };
        //    }

        //    // Check DiseaseId before creating Appointment
        //    if (createFullAppointmentDTO.Appointment.ServiceType == EnumList.ServiceType.Vaccination)
        //    {
        //        if (createFullAppointmentDTO.AppointmentDetail.DiseaseId == null || createFullAppointmentDTO.AppointmentDetail.DiseaseId <= 0)
        //        {
        //            return new BaseResponse<AppointmentWithDetailResponseDTO>
        //            {
        //                Code = 400,
        //                Success = false,
        //                Message = "Vui lòng cung cấp DiseaseId cho dịch vụ tiêm phòng.",
        //                Data = default!
        //            };
        //        }
        //        // Kiểm tra xem DiseaseId có tồn tại không
        //        var disease = await _diseaseRepository.GetDiseaseByIdAsync(createFullAppointmentDTO.AppointmentDetail.DiseaseId.Value, cancellationToken);
        //        if (disease == null)
        //        {
        //            return new BaseResponse<AppointmentWithDetailResponseDTO>
        //            {
        //                Code = 404,
        //                Success = false,
        //                Message = "Không tìm thấy bệnh với ID đã cung cấp.",
        //                Data = default!
        //            };
        //        }
        //    }

        //    try
        //    {
        //        var random = new Random();

        //        var appointment = _mapper.Map<Appointment>(createFullAppointmentDTO.Appointment);
        //        appointment.AppointmentCode = "AP" + random.Next(0, 1000000).ToString("D6");
        //        appointment.AppointmentDate = createFullAppointmentDTO.Appointment.AppointmentDate;
        //        appointment.ServiceType = createFullAppointmentDTO.Appointment.ServiceType;
        //        appointment.Location = createFullAppointmentDTO.Appointment.Location;
        //        appointment.Address = createFullAppointmentDTO.Appointment.Address;
        //        appointment.AppointmentStatus = EnumList.AppointmentStatus.Processing;
        //        appointment.CreatedAt = DateTime.UtcNow;
        //        appointment.CreatedBy = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "System";

        //        var createdAppointmentId = await _appointmentRepository.CreateAppointmentAsync(appointment, cancellationToken);
        //        if (createdAppointmentId == null)
        //        {
        //            return new BaseResponse<AppointmentWithDetailResponseDTO>
        //            {
        //                Code = 500,
        //                Success = false,
        //                Message = "Không thể tạo cuộc hẹn.",
        //                Data = default!
        //            };
        //        }

        //        // Lấy lại bản ghi Appointment vừa tạo để đảm bảo có đầy đủ thông tin (ID, ...)
        //        var createdAppointment = await _appointmentRepository.GetAppointmentByIdAsync(createdAppointmentId.AppointmentId, cancellationToken);
        //        if (createdAppointment == null)
        //        {
        //            return new BaseResponse<AppointmentWithDetailResponseDTO>
        //            {
        //                Code = 500,
        //                Success = false,
        //                Message = "Không thể lấy thông tin cuộc hẹn vừa tạo.",
        //                Data = default!
        //            };
        //        }
        //        var appointmentResponse = _mapper.Map<AppointmentResponseDTO>(createdAppointment);

        //        var appointmentDetail = new AppointmentDetail
        //        {
        //            AppointmentId = appointmentResponse.AppointmentId,
        //            AppointmentDate = appointmentResponse.AppointmentDate,
        //            ServiceType = appointmentResponse.ServiceType,
        //            AppointmentStatus = appointmentResponse.AppointmentStatus,
        //            AppointmentDetailCode = "AD" + random.Next(0, 1000000).ToString("D6"),
        //            CreatedAt = DateTime.UtcNow,
        //            CreatedBy = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "System",
        //        };

        //        if (appointmentResponse.ServiceType == EnumList.ServiceType.Vaccination)
        //        {
        //            // Gán DiseaseId nếu có
        //            var diseaseIdProp = typeof(AppointmentDetail).GetProperty("DiseaseId");
        //            if (diseaseIdProp != null && createFullAppointmentDTO.AppointmentDetail.DiseaseId != null)
        //            {
        //                diseaseIdProp.SetValue(appointmentDetail, createFullAppointmentDTO.AppointmentDetail.DiseaseId);
        //            }
        //        }

        //        var createdAppointmentDetailId = await _appointmentDetailRepository.AddAppointmentDetailAsync(appointmentDetail, cancellationToken);
        //        if (createdAppointmentDetailId == null)
        //        {
        //            return new BaseResponse<AppointmentWithDetailResponseDTO>
        //            {
        //                Code = 500,
        //                Success = false,
        //                Message = "Không thể tạo chi tiết cuộc hẹn.",
        //                Data = default!
        //            };
        //        }

        //        // Lấy lại bản ghi AppointmentDetail vừa tạo
        //        var createdAppointmentDetail = await _appointmentDetailRepository.GetAppointmentDetailByIdAsync(createdAppointmentDetailId.AppointmentDetailId, cancellationToken);
        //        if (createdAppointmentDetail == null)
        //        {
        //            return new BaseResponse<AppointmentWithDetailResponseDTO>
        //            {
        //                Code = 500,
        //                Success = false,
        //                Message = "Không thể lấy thông tin chi tiết cuộc hẹn vừa tạo.",
        //                Data = default!
        //            };
        //        }
        //        var appointmentDetailResponse = _mapper.Map<AppointmentDetailResponseDTO>(createdAppointmentDetail);
        //        // Map the appointment and appointment detail to the response DTO
        //        var appointmentWithDetailResponse = new AppointmentWithDetailResponseDTO
        //        {
        //            Appointment = appointmentResponse,
        //            AppointmentDetail = appointmentDetailResponse
        //        };
        //        return new BaseResponse<AppointmentWithDetailResponseDTO>
        //        {
        //            Code = 201,
        //            Success = true,
        //            Message = "Tạo cuộc hẹn và chi tiết cuộc hẹn thành công.",
        //            Data = appointmentWithDetailResponse
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error occurred while creating appointment.");
        //        return new BaseResponse<AppointmentWithDetailResponseDTO>
        //        {
        //            Code = 500,
        //            Success = false,
        //            Message = "Đã xảy ra lỗi khi tạo cuộc hẹn.",
        //            Data = default!
        //        };
        //    }
        //}

        //public async Task<BaseResponse<AppointmentResponseDTO>> UpdateAppointmentAsync(int appointmentId, UpdateAppointmentDTO updateAppointmentDTO, CancellationToken cancellationToken)
        //{
        //    if (updateAppointmentDTO == null)
        //    {
        //        return new BaseResponse<AppointmentResponseDTO>
        //        {
        //            Code = 400,
        //            Success = false,
        //            Message = "Dữ liệu cuộc hẹn không hợp lệ.",
        //            Data = null
        //        };
        //    }
        //    try
        //    {
        //        var appointment = await _appointmentRepository.GetAppointmentByIdAsync(appointmentId, cancellationToken);
        //        if (appointment == null)
        //        {
        //            return new BaseResponse<AppointmentResponseDTO>
        //            {
        //                Code = 404,
        //                Success = false,
        //                Message = "Cuộc hẹn không tồn tại.",
        //                Data = null
        //            };
        //        }

        //        if (updateAppointmentDTO.AppointmentDate.HasValue)
        //            appointment.AppointmentDate = updateAppointmentDTO.AppointmentDate.Value;
        //        if (updateAppointmentDTO.ServiceType.HasValue)
        //            appointment.ServiceType = updateAppointmentDTO.ServiceType.Value;
        //        if (updateAppointmentDTO.Location.HasValue)
        //            appointment.Location = updateAppointmentDTO.Location.Value;
        //        if (!string.IsNullOrWhiteSpace(updateAppointmentDTO.Address))
        //            appointment.Address = updateAppointmentDTO.Address;
        //        appointment.ModifiedAt = DateTime.UtcNow;
        //        appointment.ModifiedBy = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "System";
        //        var updatedAppointment = await _appointmentRepository.UpdateAppointmentAsync(appointment, cancellationToken);
        //        if (updatedAppointment == null)
        //        {
        //            return new BaseResponse<AppointmentResponseDTO>
        //            {
        //                Code = 500,
        //                Success = false,
        //                Message = "Không thể cập nhật cuộc hẹn.",
        //                Data = null
        //            };
        //        }

        //        var appointmentResponse = _mapper.Map<AppointmentResponseDTO>(updatedAppointment);
        //        return new BaseResponse<AppointmentResponseDTO>
        //        {
        //            Code = 200,
        //            Success = true,
        //            Message = "Cập nhật cuộc hẹn thành công.",
        //            Data = appointmentResponse
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Đã xảy ra lỗi khi cập nhật cuộc hẹn.");
        //        return new BaseResponse<AppointmentResponseDTO>
        //        {
        //            Code = 500,
        //            Success = false,
        //            Message = "Đã xảy ra lỗi khi cập nhật cuộc hẹn.",
        //            Data = null
        //        };
        //    }
        //}

        //private DateTime? ExtractDateFromVaccinationInfo(string vaccinationInfo)
        //{
        //    if (string.IsNullOrWhiteSpace(vaccinationInfo))
        //        return null;

        //    // Tìm ngày theo định dạng yyyy-MM-dd trong chuỗi
        //    var regex = new Regex(@"\b\d{4}-\d{2}-\d{2}\b");
        //    var match = regex.Match(vaccinationInfo);

        //    if (match.Success && DateTime.TryParse(match.Value, out var date))
        //    {
        //        return date;
        //    }
        //    return null;
        //}

        //    private async Task UpdateVaccineProfilesForInjectedVaccinationAppointment(
        //AppointmentDetail appointmentDetail,
        //CancellationToken cancellationToken)
        //    {
        //        try
        //        {
        //            // 1. Kiểm tra null chặt chẽ
        //            if (appointmentDetail == null)
        //            {
        //                _logger.LogError("AppointmentDetail is null");
        //                throw new ArgumentNullException(nameof(appointmentDetail));
        //            }

        //            if (appointmentDetail.Appointment == null)
        //            {
        //                _logger.LogError("Appointment is null for AppointmentDetailId: {AppointmentDetailId}",
        //                    appointmentDetail.AppointmentDetailId);
        //                return;
        //            }

        //            if (!appointmentDetail.DiseaseId.HasValue)
        //            {
        //                _logger.LogInformation("No DiseaseId, skipping update");
        //                return;
        //            }

        //            // 2. Lấy danh sách vaccine liên quan đến disease
        //            var vaccineDiseases = await _vaccineDiseaseRepository.GetVaccineDiseaseByDiseaseIdAsync(
        //                appointmentDetail.DiseaseId.Value,
        //                cancellationToken);

        //            if (vaccineDiseases == null || !vaccineDiseases.Any())
        //            {
        //                _logger.LogWarning("No vaccine diseases found for DiseaseId: {DiseaseId}",
        //                    appointmentDetail.DiseaseId);
        //                return;
        //            }

        //            var vaccineIds = vaccineDiseases.Select(vd => vd.VaccineId).ToList();

        //            // 3. Lấy vaccine profiles của pet
        //            var petProfiles = await _vaccineProfileRepository.GetVaccineProfileByPetIdAsync(
        //                appointmentDetail.Appointment.PetId,
        //                cancellationToken);

        //            if (petProfiles == null || !petProfiles.Any())
        //            {
        //                _logger.LogInformation("No vaccine profiles found for PetId: {PetId}",
        //                    appointmentDetail.Appointment.PetId);
        //                return;
        //            }

        //            // 4. Cập nhật từng profile
        //            foreach (var vaccineId in vaccineIds)
        //            {
        //                var profileToUpdate = petProfiles
        //                    .FirstOrDefault(vp => vp.DiseaseId == appointmentDetail.DiseaseId &&
        //                                        vp.IsCompleted == false);

        //                if (profileToUpdate != null)
        //                {
        //                    profileToUpdate.AppointmentDetailId = appointmentDetail.AppointmentDetailId;
        //                    profileToUpdate.VaccinationDate = appointmentDetail.AppointmentDate;
        //                    profileToUpdate.Dose = appointmentDetail.Dose ?? profileToUpdate.Dose;
        //                    profileToUpdate.Reaction = appointmentDetail.Reaction ?? profileToUpdate.Reaction;
        //                    profileToUpdate.NextVaccinationInfo = appointmentDetail.NextVaccinationInfo ?? profileToUpdate.NextVaccinationInfo;
        //                    profileToUpdate.IsCompleted = true;
        //                    profileToUpdate.ModifiedAt = DateTime.UtcNow;
        //                    profileToUpdate.ModifiedBy = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "System";

        //                    await _vaccineProfileRepository.UpdateVaccineProfileAsync(profileToUpdate, cancellationToken);

        //                    _logger.LogInformation("Updated VaccineProfile {VaccineProfileId} for Pet {PetId}",
        //                        profileToUpdate.VaccineProfileId, appointmentDetail.Appointment.PetId);
        //                }
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            _logger.LogError(ex, "Error updating VaccineProfile for AppointmentDetailId: {AppointmentDetailId}",
        //                appointmentDetail?.AppointmentDetailId);
        //            throw new Exception("Failed to update vaccine profiles for injected vaccination appointment", ex);
        //        }
        //    }

        //private async Task UpdateVaccineProfilesForInjectedVaccinationAppointment(AppointmentDetail appointmentDetail, CancellationToken cancellationToken)
        //{
        //    try
        //    {
        //        // 1. Kiểm tra null và điều kiện tiên quyết
        //        if (appointmentDetail == null)
        //        {
        //            _logger.LogError("AppointmentDetail is null");
        //            throw new ArgumentNullException(nameof(appointmentDetail));
        //        }

        //        if (appointmentDetail.Appointment == null)
        //        {
        //            _logger.LogError("Appointment is null for AppointmentDetailId: {Id}",
        //                appointmentDetail.AppointmentDetailId);
        //            return;
        //        }

        //        if (_vaccineProfileRepository == null)
        //        {
        //            _logger.LogError("VaccineProfileRepository is not initialized");
        //            throw new InvalidOperationException("VaccineProfileRepository is not initialized");
        //        }
        //        var petId = appointmentDetail.Appointment.PetId;
        //        _logger.LogInformation("Updating VaccineProfile for PetId: {PetId}, AppointmentDetailId: {AppointmentDetailId}",
        //            petId, appointmentDetail.AppointmentDetailId);
        //        if (petId <= 0)
        //        {
        //            _logger.LogError("Invalid PetId: {PetId} for AppointmentDetailId: {AppointmentDetailId}",
        //                petId, appointmentDetail.AppointmentDetailId);
        //            throw new ArgumentException("Invalid PetId", nameof(appointmentDetail.Appointment.PetId));
        //        }

        //        // 2. Lấy VaccineProfile hiện tại hoặc tạo mới nếu không tồn tại
        //        var vaccineProfile = await _vaccineProfileRepository.GetVaccineProfileByPetIdAsync(petId, cancellationToken);

        //        bool isNewProfile = false;
        //        if (vaccineProfile == null)
        //        {
        //            _logger.LogInformation("Creating new VaccineProfile for PetId: {PetId}",
        //                appointmentDetail.Appointment.PetId);

        //            vaccineProfile = new VaccineProfile
        //            {
        //                PetId = appointmentDetail.Appointment.PetId,
        //                CreatedAt = DateTime.UtcNow,
        //                CreatedBy = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "System",
        //                IsActive = true,
        //                IsCompleted = false
        //            };
        //            isNewProfile = true;
        //        }

        //        // 3. Cập nhật thông tin từ AppointmentDetail
        //        vaccineProfile.AppointmentDetailId = appointmentDetail.AppointmentDetailId;
        //        vaccineProfile.DiseaseId = appointmentDetail.DiseaseId;
        //        vaccineProfile.VaccinationDate = appointmentDetail.AppointmentDate;
        //        vaccineProfile.Dose = appointmentDetail.Dose;
        //        vaccineProfile.Reaction = appointmentDetail.Reaction;
        //        vaccineProfile.NextVaccinationInfo = appointmentDetail.NextVaccinationInfo;
        //        vaccineProfile.IsCompleted = true; // Đánh dấu đã hoàn thành tiêm
        //        vaccineProfile.ModifiedAt = DateTime.UtcNow;
        //        vaccineProfile.ModifiedBy = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "System";

        //        // 4. Xử lý ngày tiêm nhắc lại nếu có
        //        if (!string.IsNullOrEmpty(appointmentDetail.NextVaccinationInfo))
        //        {
        //            vaccineProfile.PreferedDate = ExtractDateFromVaccinationInfo(appointmentDetail.NextVaccinationInfo);
        //        }

        //        // 5. Lưu thay đổi
        //        if (isNewProfile)
        //        {
        //            await _vaccineProfileRepository.UpdateVaccineProfileAsync(vaccineProfile, cancellationToken);
        //        }

        //        _logger.LogInformation("Successfully updated VaccineProfile (ID: {VaccineProfileId}) for PetId: {PetId}",
        //            vaccineProfile.VaccineProfileId, appointmentDetail.Appointment.PetId);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error updating VaccineProfile for AppointmentDetailId: {Id}",
        //            appointmentDetail?.AppointmentDetailId);
        //        throw new Exception("Failed to update vaccine profile", ex);
        //    }
        //}
        #endregion


        public async Task<BaseResponse<AppointmenWithHealthConditionResponseDTO>> CreateAppointmentHealConditionAsync(CreateAppointmentHealthConditionDTO createAppointmentHealConditionDTO, CancellationToken cancellationToken)
        {
            if (createAppointmentHealConditionDTO == null)
            {
                return new BaseResponse<AppointmenWithHealthConditionResponseDTO>
                {
                    Code = 200,
                    Success = false,
                    Message = "Dữ liệu tạo cuộc hẹn khám bệnh không hợp lệ.",
                    Data = null
                };
            }

            if (createAppointmentHealConditionDTO.Appointment.Location == EnumList.Location.HomeVisit)
            {
                if (string.IsNullOrWhiteSpace(createAppointmentHealConditionDTO.Appointment.Address))
                {
                    return new BaseResponse<AppointmenWithHealthConditionResponseDTO>
                    {
                        Code = 400,
                        Success = false,
                        Message = "Vui lòng nhập địa chỉ khi chọn dịch vụ tại nhà.",
                        Data = null
                    };
                }
                // Check if address is in Ho Chi Minh City
                bool isInHCM = await IsAddressInHoChiMinhCity(createAppointmentHealConditionDTO.Appointment.Address);
                if (!isInHCM)
                {
                    return new BaseResponse<AppointmenWithHealthConditionResponseDTO>
                    {
                        Code = 400,
                        Success = false,
                        Message = "Địa chỉ bạn nhập không thuộc khu vực Thành phố Hồ Chí Minh. Vui lòng nhập địa chỉ hợp lệ trong khu vực này.",
                        Data = null
                    };
                }
            }

            if (createAppointmentHealConditionDTO.Appointment.Location == EnumList.Location.Clinic)
            {
                var addresses = await _addressRepository.GetAllAddressesAsync(CancellationToken.None);
                var defaultAddress = addresses.FirstOrDefault()?.Location;
                createAppointmentHealConditionDTO.Appointment.Address = defaultAddress;
            }

            if (createAppointmentHealConditionDTO.Appointment.ServiceType != EnumList.ServiceType.HealthCondition)
            {
                return new BaseResponse<AppointmenWithHealthConditionResponseDTO>
                {
                    Code = 400,
                    Success = false,
                    Message = "Loại dịch vụ không hợp lệ. Chỉ cho phép tạo cuộc hẹn với dịch vụ là 3 - HealthCondition.",
                    Data = null
                };
            }
            var appointmentDate = createAppointmentHealConditionDTO.Appointment.AppointmentDate;
            var now = DateTimeHelper.Now();
            if (appointmentDate.Date == now.Date)
            {
                return new BaseResponse<AppointmenWithHealthConditionResponseDTO>
                {
                    Code = 400,
                    Success = false,
                    Message = "Không cho phép đặt lịch trong ngày. Vui lòng chọn ngày khác.",
                    Data = null
                };
            }
            var hour = createAppointmentHealConditionDTO.Appointment.AppointmentDate.Hour;
            if (!((hour >= 8 && hour <= 11) || (hour >= 13 && hour <= 16)))
            {
                return new BaseResponse<AppointmenWithHealthConditionResponseDTO>
                {
                    Code = 400,
                    Success = false,
                    Message = "Chỉ cho phép đặt lịch trong khung giờ từ 8h-11h và 13h-16h.",
                    Data = null
                };
            }

            var pet = await _petRepository.GetPetByIdAsync(createAppointmentHealConditionDTO.Appointment.PetId, cancellationToken);
            if (pet == null || pet.CustomerId != createAppointmentHealConditionDTO.Appointment.CustomerId)
            {
                return new BaseResponse<AppointmenWithHealthConditionResponseDTO>
                {
                    Code = 200,
                    Success = false,
                    Message = "Thú cưng này không thuộc quyền sở hữu của chủ nuôi này.",
                    Data = null
                };
            }

            try
            {
                var random = new Random();
                var appointment = _mapper.Map<Appointment>(createAppointmentHealConditionDTO.Appointment);

                appointment.AppointmentCode = "AP" + random.Next(0, 1000000).ToString("D6");
                appointment.AppointmentDate = createAppointmentHealConditionDTO.Appointment.AppointmentDate;
                appointment.ServiceType = createAppointmentHealConditionDTO.Appointment.ServiceType;
                appointment.Location = createAppointmentHealConditionDTO.Appointment.Location;
                appointment.Address = createAppointmentHealConditionDTO.Appointment.Address;
                appointment.AppointmentStatus = EnumList.AppointmentStatus.Processing;
                appointment.CreatedAt = DateTime.UtcNow;
                appointment.CreatedBy = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "System";

                var createdAppointment = await _appointmentRepository.CreateAppointmentAsync(appointment, cancellationToken);
                if (createdAppointment == null)
                {
                    return new BaseResponse<AppointmenWithHealthConditionResponseDTO>
                    {
                        Code = 500,
                        Success = false,
                        Message = "Không thể tạo cuộc hẹn khám bệnh.",
                        Data = null
                    };
                }

                var createdAppointmentId = await _appointmentRepository.GetAppointmentByIdAsync(createdAppointment.AppointmentId, cancellationToken);

                var appointmentDetail = new AppointmentDetail
                {
                    AppointmentId = createdAppointment.AppointmentId,
                    AppointmentDate = createdAppointment.AppointmentDate,
                    ServiceType = createdAppointment.ServiceType,
                    AppointmentStatus = createdAppointment.AppointmentStatus,
                    AppointmentDetailCode = "AD" + random.Next(0, 1000000).ToString("D6"),
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "System",
                };

                var createdAppointmentDetail = await _appointmentDetailRepository.AddAppointmentDetailAsync(appointmentDetail, cancellationToken);
                if (createdAppointmentDetail == null)
                {
                    await _appointmentRepository.DeleteAppointmentAsync(createdAppointment.AppointmentId, cancellationToken);
                    return new BaseResponse<AppointmenWithHealthConditionResponseDTO>
                    {
                        Code = 500,
                        Success = false,
                        Message = "Không thể tạo chi tiết cuộc hẹn khám bệnh.",
                        Data = null
                    };
                }

                var createdAppointmentDetailId = await _appointmentDetailRepository.GetAppointmentDetailByIdAsync(createdAppointmentDetail.AppointmentDetailId, cancellationToken);
                if (createdAppointmentId == null || createdAppointmentDetailId == null)
                {
                    return new BaseResponse<AppointmenWithHealthConditionResponseDTO>
                    {
                        Code = 500,
                        Success = false,
                        Message = "Lỗi khi lấy thông tin cuộc hẹn đã tạo.",
                        Data = null
                    };
                }

                return new BaseResponse<AppointmenWithHealthConditionResponseDTO>
                {
                    Code = 201,
                    Success = true,
                    Message = "Tạo cuộc hẹn khám bệnh thành công.",
                    Data = new AppointmenWithHealthConditionResponseDTO
                    {
                        Appointment = _mapper.Map<AppointmentResponseDTO>(createdAppointmentId),
                        HealthCondition = _mapper.Map<AppointmentHealthConditionResponseDTO>(createdAppointmentDetailId)
                    }
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<AppointmenWithHealthConditionResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã xảy ra lỗi khi tạo cuộc hẹn khám bệnh. " + ex.Message,
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<AppointmentHealthConditionResponseDTO>> UpdateAppointmentHealthConditionAsync(int AppointmentId, UpdateAppointmentHealthConditionDTO updateDTO, CancellationToken cancellationToken)
        {
            if (updateDTO == null)
            {
                return new BaseResponse<AppointmentHealthConditionResponseDTO>
                {
                    Code = 400,
                    Success = false,
                    Message = "Dữ liệu cập nhật không hợp lệ.",
                    Data = null
                };
            }

            try
            {
                var appointment = await _appointmentRepository.GetAppointmentByIdAsync(AppointmentId, cancellationToken);
                if (appointment == null)
                {
                    return new BaseResponse<AppointmentHealthConditionResponseDTO>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Cuộc hẹn không tồn tại.",
                        Data = null
                    };
                }

                if (appointment.ServiceType != EnumList.ServiceType.HealthCondition)
                {
                    return new BaseResponse<AppointmentHealthConditionResponseDTO>
                    {
                        Code = 400,
                        Success = false,
                        Message = "Cuộc hẹn này không phải là dịch vụ khám bệnh.",
                        Data = null
                    };
                }

                // Validate allowed status transitions
                var currentStatus = appointment.AppointmentStatus;
                var newStatus = updateDTO.AppointmentStatus ?? currentStatus;
                bool isValidTransition = false;
                switch (currentStatus)
                {
                    case EnumList.AppointmentStatus.Processing:
                        isValidTransition = newStatus == EnumList.AppointmentStatus.Confirmed || newStatus == EnumList.AppointmentStatus.Cancelled;
                        break;
                    case EnumList.AppointmentStatus.Confirmed:
                        isValidTransition = newStatus == EnumList.AppointmentStatus.CheckedIn || newStatus == EnumList.AppointmentStatus.Rejected;
                        break;
                    case EnumList.AppointmentStatus.CheckedIn:
                        isValidTransition = newStatus == EnumList.AppointmentStatus.Processed || newStatus == EnumList.AppointmentStatus.Cancelled;
                        break;
                    case EnumList.AppointmentStatus.Processed:
                        isValidTransition = newStatus == EnumList.AppointmentStatus.Completed;
                        break;
                    case EnumList.AppointmentStatus.Paid:
                        isValidTransition = newStatus == EnumList.AppointmentStatus.Completed;
                        break;
                    default:
                        isValidTransition = false;
                        break;
                }
                if (newStatus != currentStatus && !isValidTransition)
                {
                    return new BaseResponse<AppointmentHealthConditionResponseDTO>
                    {
                        Code = 200,
                        Success = false,
                        Message = $"Không thể chuyển trạng thái từ {currentStatus} sang {newStatus}.",
                        Data = null
                    };
                }

                bool isStatusChangeToProcessed = updateDTO.AppointmentStatus.HasValue &&
                                                 updateDTO.AppointmentStatus.Value == EnumList.AppointmentStatus.Processed &&
                                                 appointment.AppointmentStatus != EnumList.AppointmentStatus.Processed;

                if (appointment.ServiceType != EnumList.ServiceType.HealthCondition)
                {
                    return new BaseResponse<AppointmentHealthConditionResponseDTO>
                    {
                        Code = 400,
                        Success = false,
                        Message = "Cuộc hẹn này không phải là dịch vụ khám sức khỏe.",
                        Data = null
                    };
                }

                var appointmentDetail = await _appointmentDetailRepository.GetAppointmentDetailsByAppointmentIdAsync(AppointmentId, cancellationToken);
                if (appointmentDetail == null)
                {
                    return new BaseResponse<AppointmentHealthConditionResponseDTO>
                    {
                        Code = 200,
                        Success = false,
                        Message = "Chi tiết cuộc hẹn không tồn tại.",
                        Data = null
                    };
                }

                // VetSchedule check and update (like UpdateAppointmentVaccination)
                if (updateDTO.VetId.HasValue)
                {
                    var appointmentDate = appointmentDetail.AppointmentDate;
                    var slotNumber = GetSlotNumberFromAppointmentDate(appointmentDate);

                    var vetSchedules = await _vetScheduleRepository.GetVetSchedulesByVetIdAsync(updateDTO.VetId.Value, cancellationToken);

                    var isValidSchedule = vetSchedules.Any(s =>
                        s.ScheduleDate.Date == appointmentDate.Date &&
                        s.SlotNumber == slotNumber &&
                        (s.Status == EnumList.VetScheduleStatus.Available ||
                         s.Status == EnumList.VetScheduleStatus.Scheduled));

                    if (!isValidSchedule)
                    {
                        return new BaseResponse<AppointmentHealthConditionResponseDTO>
                        {
                            Code = 200,
                            Success = false,
                            Message = "Bác sĩ không có lịch làm việc vào thời gian này.",
                            Data = null
                        };
                    }

                    // Không cho chọn nếu VetScheduleStatus là Scheduled
                    var vetScheduleToCheck = vetSchedules.FirstOrDefault(s =>
                        s.ScheduleDate.Date == appointmentDate.Date &&
                        s.SlotNumber == slotNumber);

                    if (vetScheduleToCheck != null && vetScheduleToCheck.Status == EnumList.VetScheduleStatus.Scheduled)
                    {
                        return new BaseResponse<AppointmentHealthConditionResponseDTO>
                        {
                            Code = 200,
                            Success = false,
                            Message = "Bác sĩ đã có lịch hẹn khác vào khung giờ này.",
                            Data = null
                        };
                    }

                    // Chuyển trạng thái VetSchedule sang Scheduled nếu vet được chọn cho slot này
                    var vetScheduleToUpdate = vetSchedules.FirstOrDefault(s =>
                        s.ScheduleDate.Date == appointmentDate.Date &&
                        s.SlotNumber == slotNumber &&
                        s.Status == EnumList.VetScheduleStatus.Available);

                    if (vetScheduleToUpdate != null)
                    {
                        vetScheduleToUpdate.Status = EnumList.VetScheduleStatus.Scheduled;
                        await _vetScheduleRepository.UpdateVetScheduleAsync(vetScheduleToUpdate, cancellationToken);
                    }
                }

                if (updateDTO.VetId > 0)
                {
                    appointmentDetail.VetId = updateDTO.VetId;
                }

                appointmentDetail.Notes = updateDTO.Note;
                appointmentDetail.AppointmentStatus = newStatus;
                appointmentDetail.ModifiedAt = DateTime.UtcNow;
                appointmentDetail.ModifiedBy = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "System";

                appointment.AppointmentStatus = newStatus;
                appointment.AppointmentDate = updateDTO.AppointmentDate ?? appointment.AppointmentDate;
                appointment.ModifiedAt = DateTime.UtcNow;
                appointment.ModifiedBy = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "System";

                //create health condition
                HealthCondition healthConditiont = new HealthCondition();
                if (currentStatus != EnumList.AppointmentStatus.Confirmed && newStatus == EnumList.AppointmentStatus.Confirmed)
                {
                    var random = new Random();
                    healthConditiont.Price = 10000;
                    healthConditiont.ConditionCode = "HE" + random.Next(0, 1000000).ToString("D6");
                    healthConditiont.CreatedAt = DateTime.UtcNow;
                    healthConditiont.CreatedBy = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "System";
                    var createHealthcondition = await _healthConditionRepository.AddHealthConditionAsync(healthConditiont, cancellationToken);
                    appointmentDetail.HealthConditionId = createHealthcondition.HealthConditionId;
                    if (createHealthcondition == null)
                    {
                        return new BaseResponse<AppointmentHealthConditionResponseDTO>
                        {
                            Code = 500,
                            Success = false,
                            Message = "Không thể tạo tình trạng sức khỏe.",
                            Data = null
                        };
                    }
                }

                if (updateDTO.PetId > 0 && updateDTO.HealthConditionId > 0)
                {
                    var pet = await _petRepository.GetPetByIdAsync(appointment.PetId, cancellationToken);
                    if (pet == null)
                    {
                        return new BaseResponse<AppointmentHealthConditionResponseDTO>
                        {
                            Code = 404,
                            Success = false,
                            Message = "Không tìm thấy thú cưng.",
                            Data = null
                        };
                    }

                    var species = pet.Species?.ToLower();
                    if (species != "dog" && species != "cat")
                    {
                        return new BaseResponse<AppointmentHealthConditionResponseDTO>
                        {
                            Code = 400,
                            Success = false,
                            Message = "Loài thú cưng không được hỗ trợ. Chỉ hỗ trợ 'cat' và 'dog'.",
                            Data = null
                        };
                    }
                    if (pet != null)
                    {
                        string Conclusion = string.Empty;
                        string Status = string.Empty;

                        var getHealthCondition = await _healthConditionRepository.GetHealthConditionByIdAsync(updateDTO.HealthConditionId, cancellationToken);

                        var healthIssues = new List<string>();

                        // Validate dog
                        if (species == "dog")
                        {
                            if (!string.IsNullOrEmpty(updateDTO.Temperature) &&
                                decimal.TryParse(updateDTO.Temperature.Replace("°C", "").Trim(), out decimal tempC))
                            {
                                if (tempC < 37.5m || tempC > 39.2m)
                                    healthIssues.Add($"Nhiệt độ bất thường: {tempC} °C");
                            }

                            if (!string.IsNullOrEmpty(updateDTO.HeartRate) &&
                                int.TryParse(updateDTO.HeartRate.Trim(), out int heartRate))
                            {
                                if (heartRate < 60 || heartRate > 140)
                                    healthIssues.Add($"Nhịp tim bất thường: {heartRate} bpm");
                            }

                            if (!string.IsNullOrEmpty(updateDTO.BreathingRate) &&
                                int.TryParse(updateDTO.BreathingRate.Trim(), out int breathingRate))
                            {
                                if (breathingRate < 10 || breathingRate > 30)
                                    healthIssues.Add($"Nhịp thở bất thường: {breathingRate} lần/phút");
                            }
                        }
                        // Validate cat
                        else if (species == "cat")
                        {
                            if (!string.IsNullOrEmpty(updateDTO.Temperature) &&
                                decimal.TryParse(updateDTO.Temperature.Replace("°C", "").Trim(), out decimal tempC))
                            {
                                if (tempC < 38.0m || tempC > 39.5m)
                                    healthIssues.Add($"Nhiệt độ bất thường: {tempC} °C");
                            }

                            if (!string.IsNullOrEmpty(updateDTO.HeartRate) &&
                                int.TryParse(updateDTO.HeartRate.Trim(), out int heartRate))
                            {
                                if (heartRate < 140 || heartRate > 220)
                                    healthIssues.Add($"Nhịp tim bất thường: {heartRate} bpm");
                            }

                            if (!string.IsNullOrEmpty(updateDTO.BreathingRate) &&
                                int.TryParse(updateDTO.BreathingRate.Trim(), out int breathingRate))
                            {
                                if (breathingRate < 20 || breathingRate > 30)
                                    healthIssues.Add($"Nhịp thở bất thường: {breathingRate} lần/phút");
                            }
                        }

                        // check weight
                        if (!string.IsNullOrEmpty(updateDTO.Weight) &&
                            !decimal.TryParse(updateDTO.Weight.Trim(), out _))
                        {
                            healthIssues.Add("Cân nặng không hợp lệ.");
                        }
                        else
                        {
                            var existpet = await _petRepository.GetPetAndAppointmentByIdAsync(appointment.PetId, cancellationToken);
                            existpet.Weight = updateDTO.Weight;
                            await _petRepository.UpdatePetAsync(existpet, cancellationToken);
                        }

                        if (healthIssues.Any())
                        {
                            Conclusion = $"❌ Không đạt: {string.Join("; ", healthIssues)}, {updateDTO.Conclusion}";
                            Status = "FAIL";
                        }
                        else
                        {
                            Conclusion = $"Đạt: Tình trạng sức khỏe trong ngưỡng bình thường. {updateDTO.Conclusion}";
                            Status = "PASS";
                        }

                        getHealthCondition.PetId = updateDTO.PetId ?? getHealthCondition.PetId;
                        getHealthCondition.VetId = updateDTO.VetId ?? getHealthCondition.VetId;
                        getHealthCondition.HeartRate = updateDTO.HeartRate ?? getHealthCondition.HeartRate;
                        getHealthCondition.BreathingRate = updateDTO.BreathingRate ?? getHealthCondition.BreathingRate;
                        getHealthCondition.Weight = updateDTO.Weight ?? getHealthCondition.Weight;
                        getHealthCondition.Temperature = updateDTO.Temperature ?? getHealthCondition.Temperature;
                        getHealthCondition.EHNM = updateDTO.EHNM ?? getHealthCondition.EHNM;
                        getHealthCondition.SkinAFur = updateDTO.SkinAFur ?? getHealthCondition.SkinAFur;
                        getHealthCondition.Digestion = updateDTO.Digestion ?? getHealthCondition.Digestion;
                        getHealthCondition.Respiratory = updateDTO.Respiratory ?? getHealthCondition.Respiratory;
                        getHealthCondition.Excrete = updateDTO.Excrete ?? getHealthCondition.Excrete;
                        getHealthCondition.Behavior = updateDTO.Behavior ?? getHealthCondition.Behavior;
                        getHealthCondition.Psycho = updateDTO.Psycho ?? getHealthCondition.Psycho;
                        getHealthCondition.Different = updateDTO.Different ?? getHealthCondition.Different;
                        getHealthCondition.Conclusion = Conclusion;
                        getHealthCondition.Status = Status ?? getHealthCondition.Status;
                        getHealthCondition.CheckDate = DateTime.UtcNow;
                        getHealthCondition.ModifiedAt = DateTime.UtcNow;
                        getHealthCondition.ModifiedBy = _httpContextAccessor.HttpContext?.User?.Identity?.Name;

                        var updated = await _healthConditionRepository.UpdateHealthConditionAsync(getHealthCondition, cancellationToken);
                        if (updated == null)
                        {
                            return new BaseResponse<AppointmentHealthConditionResponseDTO>
                            {
                                Code = 500,
                                Success = false,
                                Message = "Không thể cập nhật tình trạng sức khỏe.",
                                Data = null
                            };
                        }
                    }
                }

                using (var transaction = await _appointmentRepository.BeginTransactionAsync())
                {
                    try
                    {
                        var updatedDetail = await _appointmentDetailRepository.UpdateAppointmentDetailAsync(appointmentDetail, cancellationToken);
                        var updatedAppointment = await _appointmentRepository.UpdateAppointmentAsync(appointment, cancellationToken);

                        if (updatedAppointment == null)
                        {
                            await transaction.RollbackAsync();
                            return new BaseResponse<AppointmentHealthConditionResponseDTO>
                            {
                                Code = 500,
                                Success = false,
                                Message = "Không thể cập nhật cuộc hẹn khám bệnh.",
                                Data = null
                            };
                        }

                        await transaction.CommitAsync();

                        var responseDetail = await _appointmentDetailRepository.GetAppointmentDetailByIdAsync(appointmentDetail.AppointmentDetailId, cancellationToken);

                        return new BaseResponse<AppointmentHealthConditionResponseDTO>
                        {
                            Code = 200,
                            Success = true,
                            Message = "Cập nhật cuộc hẹn khám bệnh thành công.",
                            Data = _mapper.Map<AppointmentHealthConditionResponseDTO>(responseDetail)
                        };
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        _logger.LogError(ex, "Lỗi khi cập nhật cuộc hẹn khám bệnh.");
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                return new BaseResponse<AppointmentHealthConditionResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã xảy ra lỗi khi cập nhật cuộc hẹn khám bệnh: " + ex.Message,
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<AppointmentResponseDTO>> UpdateAppointmentHealConditionAsync(int appointmentId, UpdateAppointmentDTO updateAppointmentHealConditionDTO, CancellationToken cancellationToken)
        {
            if (updateAppointmentHealConditionDTO == null)
            {
                return new BaseResponse<AppointmentResponseDTO>
                {
                    Code = 400,
                    Success = false,
                    Message = "Dữ liệu cập nhật cuộc hẹn khám bệnh không hợp lệ.",
                    Data = null
                };
            }
            var appointment = await _appointmentRepository.GetAppointmentByIdAsync(appointmentId, cancellationToken);
            if (appointment == null)
            {
                return new BaseResponse<AppointmentResponseDTO>
                {
                    Code = 404,
                    Success = false,
                    Message = "Không tìm thấy cuộc hẹn với ID đã cung cấp.",
                    Data = null
                };
            }
            var appointmentDetail = await _appointmentDetailRepository.GetAppointmentDetailsByAppointmentIdAsync(appointmentId, cancellationToken);
            if (appointmentDetail == null)
            {
                return new BaseResponse<AppointmentResponseDTO>
                {
                    Code = 404,
                    Success = false,
                    Message = "Không tìm thấy chi tiết cuộc hẹn với ID đã cung cấp.",
                    Data = null
                };
            }
            if (updateAppointmentHealConditionDTO.Location == EnumList.Location.HomeVisit)
            {
                if (string.IsNullOrWhiteSpace(updateAppointmentHealConditionDTO.Address))
                {
                    return new BaseResponse<AppointmentResponseDTO>
                    {
                        Code = 400,
                        Success = false,
                        Message = "Vui lòng nhập địa chỉ khi chọn dịch vụ tại nhà.",
                        Data = null
                    };
                }
                // Check if address is in Ho Chi Minh City
                bool isInHCM = await IsAddressInHoChiMinhCity(updateAppointmentHealConditionDTO.Address);
                if (!isInHCM)
                {
                    return new BaseResponse<AppointmentResponseDTO>
                    {
                        Code = 400,
                        Success = false,
                        Message = "Địa chỉ bạn nhập không thuộc khu vực Thành phố Hồ Chí Minh. Vui lòng nhập địa chỉ hợp lệ trong khu vực này.",
                        Data = null
                    };
                }
            }
            else
            {
                // Nếu không phải HomeVisit, vẫn kiểm tra nếu có Address truyền vào thì phải thuộc HCM
                if (!string.IsNullOrWhiteSpace(updateAppointmentHealConditionDTO.Address))
                {
                    bool isInHCM = await IsAddressInHoChiMinhCity(updateAppointmentHealConditionDTO.Address);
                    if (!isInHCM)
                    {
                        return new BaseResponse<AppointmentResponseDTO>
                        {
                            Code = 400,
                            Success = false,
                            Message = "Địa chỉ bạn nhập không thuộc khu vực Thành phố Hồ Chí Minh. Vui lòng nhập địa chỉ hợp lệ trong khu vực này.",
                            Data = null
                        };
                    }
                }
            }
            if (updateAppointmentHealConditionDTO.Location == EnumList.Location.Clinic)
            {
                var addresses = await _addressRepository.GetAllAddressesAsync(CancellationToken.None);
                var defaultAddress = addresses.FirstOrDefault()?.Location;
                updateAppointmentHealConditionDTO.Address = defaultAddress;
            }
            try
            {
                // Update Appointment fields
                var updateApp = updateAppointmentHealConditionDTO;
                if (updateApp.AppointmentDate.HasValue)
                    appointment.AppointmentDate = updateApp.AppointmentDate.Value;
                if (updateApp.Location.HasValue)
                    appointment.Location = updateApp.Location.Value;
                if (!string.IsNullOrWhiteSpace(updateApp.Address))
                    appointment.Address = updateApp.Address;
                appointment.ModifiedAt = DateTime.UtcNow;
                appointment.ModifiedBy = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "System";

                // Update AppointmentDetail fields
                appointmentDetail.AppointmentDate = updateApp.AppointmentDate ?? appointmentDetail.AppointmentDate;
                appointmentDetail.ModifiedAt = DateTimeHelper.Now();
                appointmentDetail.ModifiedBy = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "System";

                await _appointmentRepository.UpdateAppointmentAsync(appointment, cancellationToken);
                await _appointmentDetailRepository.UpdateAppointmentDetailAsync(appointmentDetail, cancellationToken);

                // Lấy lại thông tin đã cập nhật
                var updatedAppointment = await _appointmentRepository.GetAppointmentByIdAsync(appointment.AppointmentId, cancellationToken);
                var updatedAppointmentDetail = await _appointmentDetailRepository.GetAppointmentDetailByIdAsync(appointmentDetail.AppointmentDetailId, cancellationToken);

                return new BaseResponse<AppointmentResponseDTO>
                {
                    Code = 200,
                    Success = true,
                    Message = "Cập nhật thông tin cuộc hẹn khám bệnh thành công.",
                    Data = _mapper.Map<AppointmentResponseDTO>(updatedAppointment)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Đã xảy ra lỗi khi cập nhật cuộc hẹn khám bệnh.");
                return new BaseResponse<AppointmentResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã xảy ra lỗi khi cập nhật cuộc hẹn khám bệnh.",
                    Data = null
                };
            }
        }
        public async Task<BaseResponse<AppointmentForVaccinationResponseDTO>> UpdateAppointmentHealthConditionForVaccinationAsync(int appointmentId, UpdateAppointmentForVaccinationDTO updateAppointmentForVaccinationDTO, CancellationToken cancellationToken)
        {
            if (updateAppointmentForVaccinationDTO == null || updateAppointmentForVaccinationDTO.Appointment == null)
            {
                return new BaseResponse<AppointmentForVaccinationResponseDTO>
                {
                    Code = 400,
                    Success = false,
                    Message = "Dữ liệu cập nhật cuộc hẹn khám bệnh không hợp lệ.",
                    Data = null
                };
            }
            var appointment = await _appointmentRepository.GetAppointmentByIdAsync(appointmentId, cancellationToken);
            if (appointment == null)
            {
                return new BaseResponse<AppointmentForVaccinationResponseDTO>
                {
                    Code = 404,
                    Success = false,
                    Message = "Không tìm thấy cuộc hẹn với ID đã cung cấp.",
                    Data = null
                };
            }
            var appointmentDetail = await _appointmentDetailRepository.GetAppointmentDetailByIdAsync(appointmentId, cancellationToken);
            if (appointmentDetail == null)
            {
                return new BaseResponse<AppointmentForVaccinationResponseDTO>
                {
                    Code = 404,
                    Success = false,
                    Message = "Không tìm thấy chi tiết cuộc hẹn với ID đã cung cấp.",
                    Data = null
                };
            }
            try
            {
                // Update Appointment fields
                var updateApp = updateAppointmentForVaccinationDTO.Appointment;
                if (updateApp.AppointmentDate.HasValue)
                    appointment.AppointmentDate = updateApp.AppointmentDate.Value;
                if (updateApp.Location.HasValue)
                    appointment.Location = updateApp.Location.Value;
                if (!string.IsNullOrWhiteSpace(updateApp.Address))
                    appointment.Address = updateApp.Address;
                appointment.ModifiedAt = DateTime.UtcNow;
                appointment.ModifiedBy = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "System";

                // Update AppointmentDetail fields
                appointmentDetail.AppointmentDate = updateApp.AppointmentDate ?? appointmentDetail.AppointmentDate;
                appointmentDetail.ModifiedAt = DateTimeHelper.Now();
                appointmentDetail.ModifiedBy = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "System";

                await _appointmentRepository.UpdateAppointmentAsync(appointment, cancellationToken);
                await _appointmentDetailRepository.UpdateAppointmentDetailAsync(appointmentDetail, cancellationToken);

                // Lấy lại thông tin đã cập nhật
                var updatedAppointment = await _appointmentRepository.GetAppointmentByIdAsync(appointment.AppointmentId, cancellationToken);
                var updatedAppointmentDetail = await _appointmentDetailRepository.GetAppointmentDetailByIdAsync(appointmentDetail.AppointmentDetailId, cancellationToken);

                return new BaseResponse<AppointmentForVaccinationResponseDTO>
                {
                    Code = 200,
                    Success = true,
                    Message = "Cập nhật thông tin cuộc hẹn khám bệnh thành công.",
                    Data = new AppointmentForVaccinationResponseDTO
                    {
                        Appointment = _mapper.Map<AppointmentResponseDTO>(updatedAppointment),
                        AppointmentHasDiseaseResponseDTO = null // Không trả về UpdateDiseaseForAppointmentDTO
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Đã xảy ra lỗi khi cập nhật cuộc hẹn khám bệnh.");
                return new BaseResponse<AppointmentForVaccinationResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã xảy ra lỗi khi cập nhật cuộc hẹn khám bệnh.",
                    Data = null
                };
            }
        }
        private async Task<bool> IsAddressInHoChiMinhCity(string address)
        {
            try
            {
                string mapboxApiKey = _configuration["Mapbox:AccessToken"];
                string encodedAddress = Uri.EscapeDataString(address);
                string requestUrl = $"https://api.mapbox.com/geocoding/v5/mapbox.places/{encodedAddress}.json?access_token={mapboxApiKey}&country=vn&limit=1";

                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync(requestUrl);
                    if (!response.IsSuccessStatusCode)
                    {
                        _logger.LogWarning("Mapbox API returned non-success status code: {StatusCode}", response.StatusCode);
                        return false;
                    }

                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    using (var document = System.Text.Json.JsonDocument.Parse(jsonResponse))
                    {
                        var features = document.RootElement.GetProperty("features");
                        if (features.GetArrayLength() == 0)
                        {
                            _logger.LogWarning("No features found for address: {Address}", address);
                            return false;
                        }

                        var feature = features[0];
                        // Lấy tọa độ (longitude, latitude)
                        if (!feature.TryGetProperty("center", out var center) || center.GetArrayLength() != 2)
                        {
                            _logger.LogWarning("No center coordinates found for address: {Address}", address);
                            return false;
                        }
                        double longitude = center[0].GetDouble();
                        double latitude = center[1].GetDouble();

                        // Khu vực Thành phố Hồ Chí Minh (theo wikipedia https://vi.wikipedia.org/wiki/Th%C3%A0nh_ph%E1%BB%91_H%E1%BB%93_Ch%C3%AD_Minh#V%E1%BB%8B_tr%C3%AD_%C4%91%E1%BB%8Ba_l%C3%BD)
                        double minLat = 10.10, maxLat = 11.160;
                        double minLng = 106.22, maxLng = 107.010;

                        bool isInHCM = latitude >= minLat && latitude <= maxLat && longitude >= minLng && longitude <= maxLng;
                        if (!isInHCM)
                        {
                            _logger.LogInformation("Address coordinates ({Lat}, {Lng}) are not in Ho Chi Minh City area.", latitude, longitude);
                        }
                        return isInHCM;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while validating address with Mapbox API.");
                return false;
            }
        }
    }
}
