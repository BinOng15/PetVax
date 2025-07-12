using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PetVax.BusinessObjects.DTO;
using PetVax.BusinessObjects.DTO.AppointmentDetailDTO;
using PetVax.BusinessObjects.DTO.AppointmentDTO;
using PetVax.BusinessObjects.DTO.CustomerDTO;
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
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static PetVax.BusinessObjects.DTO.ResponseModel;
using static PetVax.BusinessObjects.Enum.EnumList;
using Microsoft.Extensions.Configuration;

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
        private readonly IConfiguration _configuration;
        private readonly ILogger<AppointmentService> _logger;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMicrochipItemRepository _microchipItemRepository;

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
            IConfiguration configuration,
            ILogger<AppointmentService> logger,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            IMicrochipItemRepository microchipItemRepository)
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
            _configuration = configuration;
            _logger = logger;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _microchipItemRepository = microchipItemRepository;
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
        #endregion

        #region Vaccination Appointment Service
        public async Task<DynamicResponse<AppointmentForVaccinationResponseDTO>> GetAllAppointmentVaccinationAsync(GetAllItemsDTO getAllItemsDTO, CancellationToken cancellationToken)
        {
            try
            {
                var appointments = await _appointmentDetailRepository.GetAllAppointmentDetailsForVaccinationAsync(cancellationToken);
                // Only show items where isDeleted is false or null (not deleted)
                appointments = appointments
                    .Where(d => d.isDeleted == false || d.isDeleted == null)
                    .ToList();

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
                        Success = false,
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
            if (createAppointmentVaccinationDTO.Appointment.Location == EnumList.Location.HomeVisit && string.IsNullOrWhiteSpace(createAppointmentVaccinationDTO.Appointment.Address))
            {
                return new BaseResponse<AppointmentWithVaccinationResponseDTO>
                {
                    Code = 400,
                    Success = false,
                    Message = "Vui lòng nhập địa chỉ khi chọn dịch vụ tại nhà.",
                    Data = null
                };
            }
            if (createAppointmentVaccinationDTO.Appointment.Location == EnumList.Location.Clinic)
            {
                createAppointmentVaccinationDTO.Appointment.Address = "Đại học FPT TP. Hồ Chí Minh";
            }
            // Validate allowed appointment hours: 8-11 and 13-16
            var hour = createAppointmentVaccinationDTO.Appointment.AppointmentDate.Hour;
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
            var disease = await _diseaseRepository.GetDiseaseByIdAsync(createAppointmentVaccinationDTO.AppointmentDetailVaccination.DiseaseId, cancellationToken);
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

            // Check if the pet has already completed all doses for this disease
            var vaccinationSchedules = await _vaccinationScheduleRepository.GetAllVaccinationSchedulesAsync(cancellationToken);
            var schedulesForDisease = vaccinationSchedules
                .Where(s => s.DiseaseId == createAppointmentVaccinationDTO.AppointmentDetailVaccination.DiseaseId)
                .ToList();
            int totalDose = schedulesForDisease.Sum(s => s.DoseNumber);

            if (totalDose > 0)
            {
                var vaccineProfiles = await _vaccineProfileRepository.GetListVaccineProfileByPetIdAsync(pet.PetId, cancellationToken);
                var profilesForDisease = vaccineProfiles?.Where(p => p.DiseaseId == createAppointmentVaccinationDTO.AppointmentDetailVaccination.DiseaseId).ToList() ?? new List<VaccineProfile>();
                int completedDoses = profilesForDisease.Count(p => p.IsCompleted == true);
                if (completedDoses >= totalDose)
                {
                    return new BaseResponse<AppointmentWithVaccinationResponseDTO>
                    {
                        Code = 400,
                        Success = false,
                        Message = "Bệnh này đã được tiêm đủ liều lượng theo lịch tiêm chủng cho thú cưng. Không thể tạo thêm lịch tiêm cho bệnh này.",
                        Data = null
                    };
                }
            }
            if (vaccinationSchedules != null)
            {
                var vaccineProfiles = await _vaccineProfileRepository.GetListVaccineProfileByPetIdAsync(pet.PetId, cancellationToken);
                var profilesForDisease = vaccineProfiles?.Where(p => p.DiseaseId == createAppointmentVaccinationDTO.AppointmentDetailVaccination.DiseaseId).ToList() ?? new List<VaccineProfile>();

                var totalDoseProp = vaccinationSchedules.GetType().GetProperty("TotalDose");
                int totalDoses = totalDoseProp != null ? (int)(totalDoseProp.GetValue(vaccinationSchedules) ?? 0) : 0;

                int completedDoses = profilesForDisease.Count(p => p.IsCompleted == true);
                if (totalDose > 0 && completedDoses >= totalDose)
                {
                    return new BaseResponse<AppointmentWithVaccinationResponseDTO>
                    {
                        Code = 400,
                        Success = false,
                        Message = "Bệnh này đã được tiêm đủ liều lượng theo lịch tiêm chủng. Không thể tạo thêm lịch tiêm cho bệnh này.",
                        Data = null
                    };
                }
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
                    DiseaseId = createAppointmentVaccinationDTO.AppointmentDetailVaccination.DiseaseId,
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
                        s.Status == EnumList.VetScheduleStatus.Available);

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
                }



                // Cập nhật thông tin tiêm phòng, giữ giá trị cũ nếu không truyền mới
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
                                                    await _vaccineProfileRepository.UpdateVaccineProfileAsync(doseProfile, cancellationToken);
                                                    break;
                                                }
                                            }
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
                var appointmentDetail = await _appointmentDetailRepository.GetAppointmentDetailByIdAsync(appointmentId, cancellationToken);
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
        #endregion

        #region Microchip Appointment Service
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
            if (createAppointmentMicrochipDTO.Appointment.Location == EnumList.Location.HomeVisit && string.IsNullOrWhiteSpace(createAppointmentMicrochipDTO.Appointment.Address))
            {
                return new BaseResponse<AppointmentWithMicorchipResponseDTO>
                {
                    Code = 200,
                    Success = false,
                    Message = "Vui lòng nhập địa chỉ khi chọn dịch vụ tại nhà.",
                    Data = null
                };
            }
            if (createAppointmentMicrochipDTO.Appointment.Location == EnumList.Location.Clinic)
            {
                createAppointmentMicrochipDTO.Appointment.Address = "Đại học FPT TP. Hồ Chí Minh";
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
                    return new BaseResponse<AppointmentMicrochipResponseDTO>
                    {
                        Code = 200,
                        Success = false,
                        Message = $"Không thể chuyển trạng thái từ {currentStatus} sang {newStatus}.",
                        Data = null
                    };
                }

                bool isStatusChangeToProcessed = updateAppointmentMicrochipDTO.AppointmentStatus.HasValue &&
                                                 updateAppointmentMicrochipDTO.AppointmentStatus.Value == EnumList.AppointmentStatus.Processed &&
                                                 appointment.AppointmentStatus != EnumList.AppointmentStatus.Processed;

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

                if (updateAppointmentMicrochipDTO.VetId.HasValue)
                {
                    var appointmentDate = appointmentDetail.AppointmentDate;
                    var slotNumber = GetSlotNumberFromAppointmentDate(appointmentDate);

                    var vetSchedules = await _vetScheduleRepository.GetVetSchedulesByVetIdAsync(updateAppointmentMicrochipDTO.VetId.Value, cancellationToken);

                    var isValidSchedule = vetSchedules.Any(s =>
                        s.ScheduleDate.Date == appointmentDate.Date &&
                        s.SlotNumber == slotNumber &&
                        s.Status == EnumList.VetScheduleStatus.Available);

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
                }

                // Update microchip item if provided

                if (updateAppointmentMicrochipDTO.MicrochipItemId > 0)
                {
                    var microchipItem = await _microchipItemRepository.GetMicrochipItemByIdAsync(updateAppointmentMicrochipDTO.MicrochipItemId, cancellationToken);
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
                    else if (microchipItem.IsUsed == true)
                    {
                        return new BaseResponse<AppointmentMicrochipResponseDTO>
                        {
                            Code = 200,
                            Success = false,
                            Message = "Microchip đã được sử dụng.",
                            Data = null
                        };
                    }
                    else
                    {   

                        microchipItem.Location = updateAppointmentMicrochipDTO.Description;
                        int rowEffected = await _microchipItemRepository.UpdateMicrochipItemAsync(microchipItem, cancellationToken);

                    }
                    ;
                }

                appointmentDetail.VetId = updateAppointmentMicrochipDTO.VetId;
                appointmentDetail.MicrochipItemId = updateAppointmentMicrochipDTO.MicrochipItemId;
                appointmentDetail.AppointmentStatus = newStatus;
                appointmentDetail.Notes = updateAppointmentMicrochipDTO.Note;
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
                    Message = "Đã xảy ra lỗi khi cập nhật thông tin microchip cho cuộc hẹn." + ex.Message,
                    Data = null
                };
            }
        }
        public async Task<BaseResponse<AppointmentWithMicorchipResponseDTO>> UpdateAppointmentMicrochipAsync(int appointmentId, CreateAppointmentMicrochipDTO createAppointmentMicrochipDTO, CancellationToken cancellationToken)
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
            if (createAppointmentMicrochipDTO.Appointment.Location == EnumList.Location.HomeVisit && string.IsNullOrWhiteSpace(createAppointmentMicrochipDTO.Appointment.Address))
            {
                return new BaseResponse<AppointmentWithMicorchipResponseDTO>
                {
                    Code = 200,
                    Success = false,
                    Message = "Vui lòng nhập địa chỉ khi chọn dịch vụ tại nhà.",
                    Data = null
                };
            }
            if (createAppointmentMicrochipDTO.Appointment.Location == EnumList.Location.Clinic)
            {
                createAppointmentMicrochipDTO.Appointment.Address = "Đại học FPT TP. Hồ Chí Minh";
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

                appointmentExist = _mapper.Map<Appointment>(createAppointmentMicrochipDTO.Appointment);

                appointmentExist.AppointmentDate = createAppointmentMicrochipDTO.Appointment.AppointmentDate;
                appointmentExist.ServiceType = createAppointmentMicrochipDTO.Appointment.ServiceType;
                appointmentExist.Location = createAppointmentMicrochipDTO.Appointment.Location;
                appointmentExist.Address = createAppointmentMicrochipDTO.Appointment.Address;
                appointmentExist.AppointmentStatus = EnumList.AppointmentStatus.Processing;
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

                var appointmentDetail = new AppointmentDetail
                {
                    AppointmentId = appoimentCheck.AppointmentId,
                    AppointmentDate = appoimentCheck.AppointmentDate,
                    ServiceType = appoimentCheck.ServiceType,
                    AppointmentStatus = appoimentCheck.AppointmentStatus,
                    ModifiedAt = DateTime.UtcNow,
                    ModifiedBy = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "System",
                };


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

                var createdAppointmentDetailId = await _appointmentDetailRepository.GetAppointmentDetailByIdAsync(updatedAppointmentDetail, cancellationToken);
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
                    Message = "Cập nhật cuộc hẹn tiêm phòng thành công.",
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
                    Message = "Đã xảy ra lỗi khi tạo cuộc hẹn tiêm phòng. " + ex.Message,
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
                createAppointmentVaccinationCertificateDTO.AppointmentDTO.Address = "Đại học FPT TP. Hồ Chí Minh";
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
                                ClinicAddress = "Đại học FPT TP. Hồ Chí Minh",
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

            if (createAppointmentHealConditionDTO.Appointment.Location == EnumList.Location.HomeVisit &&
                string.IsNullOrWhiteSpace(createAppointmentHealConditionDTO.Appointment.Address))
            {
                return new BaseResponse<AppointmenWithHealthConditionResponseDTO>
                {
                    Code = 200,
                    Success = false,
                    Message = "Vui lòng nhập địa chỉ khi chọn dịch vụ tại nhà.",
                    Data = null
                };
            }

            if (createAppointmentHealConditionDTO.Appointment.Location == EnumList.Location.Clinic)
            {
                createAppointmentHealConditionDTO.Appointment.Address = "Đại học FPT TP. Hồ Chí Minh";
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

        public async Task<BaseResponse<AppointmentHealthConditionResponseDTO>> UpdateAppointmentHealthConditionAsync(UpdateAppointmentHealthConditionDTO updateDTO, CancellationToken cancellationToken)
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
                var appointment = await _appointmentRepository.GetAppointmentByIdAsync(updateDTO.AppointmentId, cancellationToken);
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


                var appointmentDetail = await _appointmentDetailRepository.GetAppointmentDetailsByAppointmentIdAsync(updateDTO.AppointmentId, cancellationToken);
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

                if (updateDTO.VetId.HasValue)
                {
                    var appointmentDate = appointmentDetail.AppointmentDate;
                    var slotNumber = GetSlotNumberFromAppointmentDate(appointmentDate);

                    var vetSchedules = await _vetScheduleRepository.GetVetSchedulesByVetIdAsync(updateDTO.VetId.Value, cancellationToken);

                    var isValidSchedule = vetSchedules.Any(s =>
                        s.ScheduleDate.Date == appointmentDate.Date &&
                        s.SlotNumber == slotNumber &&
                        s.Status == EnumList.VetScheduleStatus.Available);

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
                }
           

                appointmentDetail.VetId = updateDTO.VetId ?? appointmentDetail.VetId;
                appointmentDetail.HealthConditionId = updateDTO.HealthConditionId ?? appointmentDetail.HealthConditionId;
                appointmentDetail.Notes = updateDTO.Note;
                appointmentDetail.AppointmentStatus = newStatus;
                appointmentDetail.ModifiedAt = DateTime.UtcNow;
                appointmentDetail.ModifiedBy = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "System";

                appointment.AppointmentStatus = newStatus;
                appointment.AppointmentDate = updateDTO.AppointmentDate ?? appointment.AppointmentDate;
                appointment.ModifiedAt = DateTime.UtcNow;
                appointment.ModifiedBy = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "System";

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

        public async Task<BaseResponse<AppointmenWithHealthConditionResponseDTO>> UpdateAppointmentHealConditionAsync(int appointmentId, CreateAppointmentHealthConditionDTO createAppointmentHealConditionDTO, CancellationToken cancellationToken)
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

            var appointment = await _appointmentRepository.GetAppointmentByIdAsync(appointmentId, cancellationToken);

            if (appointment == null)
            {
                return new BaseResponse<AppointmenWithHealthConditionResponseDTO>
                {
                    Code = 200,
                    Success = false,
                    Message = "Cuộc hẹn không tồn tại.",
                    Data = new AppointmenWithHealthConditionResponseDTO()
                };
            }

            if (createAppointmentHealConditionDTO.Appointment.Location == EnumList.Location.HomeVisit &&
                string.IsNullOrWhiteSpace(createAppointmentHealConditionDTO.Appointment.Address))
            {
                return new BaseResponse<AppointmenWithHealthConditionResponseDTO>
                {
                    Code = 200,
                    Success = false,
                    Message = "Vui lòng nhập địa chỉ khi chọn dịch vụ tại nhà.",
                    Data = null
                };
            }

            if (createAppointmentHealConditionDTO.Appointment.Location == EnumList.Location.Clinic)
            {
                createAppointmentHealConditionDTO.Appointment.Address = "Đại học FPT TP. Hồ Chí Minh";
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
     
                 appointment = _mapper.Map<Appointment>(createAppointmentHealConditionDTO.Appointment);


                appointment.AppointmentDate = createAppointmentHealConditionDTO.Appointment.AppointmentDate;
                appointment.ServiceType = createAppointmentHealConditionDTO.Appointment.ServiceType;
                appointment.Location = createAppointmentHealConditionDTO.Appointment.Location;
                appointment.Address = createAppointmentHealConditionDTO.Appointment.Address;
                appointment.AppointmentStatus = EnumList.AppointmentStatus.Processing;
                appointment.ModifiedAt = DateTime.UtcNow;
                appointment.ModifiedBy = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "System";

                var updated = await _appointmentRepository.UpdateAppointmentAsync(appointment, cancellationToken);
                if (updated == null)
                {
                    return new BaseResponse<AppointmenWithHealthConditionResponseDTO>
                    {
                        Code = 500,
                        Success = false,
                        Message = "Không thể cập nhật cuộc hẹn khám bệnh.",
                        Data = null
                    };
                }

                var updatedAppointmentId = await _appointmentRepository.GetAppointmentByIdAsync(updated.AppointmentId, cancellationToken);

                var appointmentDetail = new AppointmentDetail
                {
                    AppointmentId = updatedAppointmentId.AppointmentId,
                    AppointmentDate = updated.AppointmentDate,
                    ServiceType = updated.ServiceType,
                    AppointmentStatus = updated.AppointmentStatus,
                    ModifiedAt = DateTime.UtcNow,
                    ModifiedBy = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "System",
                };

                var createdAppointmentDetail = await _appointmentDetailRepository.AddAppointmentDetailAsync(appointmentDetail, cancellationToken);
                if (createdAppointmentDetail == null)
                {
                    await _appointmentRepository.DeleteAppointmentAsync(updated.AppointmentId, cancellationToken);
                    return new BaseResponse<AppointmenWithHealthConditionResponseDTO>
                    {
                        Code = 500,
                        Success = false,
                        Message = "Không thể cập nhật chi tiết cuộc hẹn khám bệnh.",
                        Data = null
                    };
                }

                var updateedAppointmentDetailId = await _appointmentDetailRepository.GetAppointmentDetailByIdAsync(createdAppointmentDetail.AppointmentDetailId, cancellationToken);
                if (updatedAppointmentId == null || updateedAppointmentDetailId == null)
                {
                    return new BaseResponse<AppointmenWithHealthConditionResponseDTO>
                    {
                        Code = 500,
                        Success = false,
                        Message = "Lỗi khi lấy thông tin cuộc hẹn đã cập nhật.",
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
                        Appointment = _mapper.Map<AppointmentResponseDTO>(updatedAppointmentId),
                        HealthCondition = _mapper.Map<AppointmentHealthConditionResponseDTO>(updateedAppointmentDetailId)
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



    }
}
