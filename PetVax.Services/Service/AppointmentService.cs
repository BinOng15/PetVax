using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PetVax.BusinessObjects.DTO;
using PetVax.BusinessObjects.DTO.AppointmentDetailDTO;
using PetVax.BusinessObjects.DTO.AppointmentDTO;
using PetVax.BusinessObjects.DTO.CustomerDTO;
using PetVax.BusinessObjects.DTO.VaccineProfileDTO;
using PetVax.BusinessObjects.Enum;
using PetVax.BusinessObjects.Models;
using PetVax.Repositories.IRepository;
using PetVax.Repositories.Repository;
using PetVax.Services.IService;
using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly ILogger<AppointmentService> _logger;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IMicrochipItemRepository _microchipItemRepository;
        private readonly IMicrochipRepository _microchipRepository;

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
            ILogger<AppointmentService> logger,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            IMicrochipItemRepository microchipItemRepository,
            IMicrochipRepository microchipRepository)
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
            _logger = logger;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _microchipItemRepository = microchipItemRepository;
            _microchipRepository = microchipRepository;
        }

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
                bool isDeleted = await _appointmentRepository.DeleteAppointmentAsync(appointmentId, cancellationToken);
                if (isDeleted)
                {
                    return new BaseResponse<bool>
                    {
                        Code = 200,
                        Success = true,
                        Message = "Xóa cuộc hẹn thành công.",
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
                    return new BaseResponse<AppointmentVaccinationDetailResponseDTO>
                    {
                        Code = 400,
                        Success = false,
                        Message = $"Không thể chuyển trạng thái từ {currentStatus} sang {newStatus}.",
                        Data = null
                    };
                }

                bool isStatusChangeToProcessed = updateAppointmentVaccinationDTO.AppointmentStatus.HasValue &&
                                                 updateAppointmentVaccinationDTO.AppointmentStatus.Value == EnumList.AppointmentStatus.Processed &&
                                                 appointment.AppointmentStatus != EnumList.AppointmentStatus.Processed;

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

                if (isStatusChangeToProcessed && appointmentDetail.VaccineBatchId.HasValue)
                {
                    var vaccineBatch = await _vaccineBatchRepository.GetVaccineBatchByIdAsync(appointmentDetail.VaccineBatchId.Value, cancellationToken);
                    if (vaccineBatch != null)
                    {
                        if (vaccineBatch.Quantity <= 0)
                        {
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
                }

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

                // Cập nhật thông tin tiêm phòng
                appointmentDetail.DiseaseId = updateAppointmentVaccinationDTO.DiseaseId;
                appointmentDetail.VaccineBatchId = updateAppointmentVaccinationDTO.VaccineBatchId;
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
                appointmentDetail.VetId = updateAppointmentVaccinationDTO.VetId;
                appointmentDetail.Reaction = updateAppointmentVaccinationDTO.Reaction;
                appointmentDetail.Temperature = updateAppointmentVaccinationDTO.Temperature;
                appointmentDetail.HeartRate = updateAppointmentVaccinationDTO.HeartRate;
                appointmentDetail.Others = updateAppointmentVaccinationDTO.Others;
                appointmentDetail.GeneralCondition = updateAppointmentVaccinationDTO.GeneralCondition;
                appointmentDetail.Notes = updateAppointmentVaccinationDTO.Notes;
                appointmentDetail.AppointmentStatus = newStatus;
                appointmentDetail.NextVaccinationInfo = updateAppointmentVaccinationDTO.NextVaccinationInfo;
                appointmentDetail.ModifiedAt = DateTime.UtcNow;
                appointmentDetail.ModifiedBy = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "System";

                appointment.AppointmentStatus = newStatus;
                appointment.ModifiedAt = DateTime.UtcNow;
                appointment.ModifiedBy = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "System";

                using (var transaction = await _appointmentRepository.BeginTransactionAsync())
                {
                    try
                    {
                        // Xác định Dose gần nhất chưa hoàn thành cho pet và disease
                        int? newDose = null;
                        if (appointmentDetail.DiseaseId.HasValue)
                        {
                            var existingProfiles = await _vaccineProfileRepository.GetListVaccineProfileByPetIdAsync(appointment.PetId, cancellationToken);
                            var profilesForDisease = existingProfiles?
                                .Where(p => p.DiseaseId == appointmentDetail.DiseaseId)
                                .ToList() ?? new List<VaccineProfile>();

                            // Tìm profile chưa hoàn thành có số dose nhỏ nhất
                            var unfinishedProfile = profilesForDisease
                                .Where(p => p.IsCompleted.HasValue && !p.IsCompleted.Value)
                                .OrderBy(p => p.Dose ?? 0)
                                .FirstOrDefault();

                            if (unfinishedProfile != null && unfinishedProfile.Dose.HasValue)
                            {
                                newDose = unfinishedProfile.Dose.Value;
                            }
                            else
                            {
                                // Nếu tất cả đã hoàn thành, lấy max dose + 1
                                var maxDose = profilesForDisease.Where(p => p.Dose.HasValue).Select(p => p.Dose.Value).DefaultIfEmpty(0).Max();
                                newDose = maxDose + 1;
                            }
                        }
                        appointmentDetail.Dose = newDose;

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

                        if (isStatusChangeToProcessed && appointmentDetail.VaccineBatchId.HasValue)
                        {
                            var vaccineBatch = await _vaccineBatchRepository.GetVaccineBatchByIdAsync(appointmentDetail.VaccineBatchId.Value, cancellationToken);
                            if (vaccineBatch != null)
                            {
                                var vaccineDiseases = await _vaccineDiseaseRepository.GetVaccineDiseaseByVaccineIdAsync(vaccineBatch.VaccineId, cancellationToken);
                                var diseaseIds = vaccineDiseases?.Select(vd => vd.DiseaseId).Distinct().ToList() ?? new List<int>();
                                var existingProfiles = await _vaccineProfileRepository.GetListVaccineProfileByPetIdAsync(appointment.PetId, cancellationToken);

                                foreach (var diseaseId in diseaseIds)
                                {
                                    var vaccinationSchedule = await _vaccinationScheduleRepository.GetVaccinationScheduleByDiseaseIdAsync(diseaseId, cancellationToken);
                                    // Tìm profile có dose = doseNumber, isCompleted = false
                                    int doseNumber = newDose ?? 1;
                                    var profileToUpdate = existingProfiles?
                                        .FirstOrDefault(p => p.DiseaseId == diseaseId && (p.Dose ?? 0) == doseNumber && (p.IsCompleted.HasValue && !p.IsCompleted.Value));

                                    if (profileToUpdate != null)
                                    {
                                        profileToUpdate.AppointmentDetailId = appointmentDetail.AppointmentDetailId;
                                        profileToUpdate.VaccinationDate = appointmentDetail.AppointmentDate;
                                        profileToUpdate.Dose = doseNumber;
                                        profileToUpdate.Reaction = appointmentDetail.Reaction ?? profileToUpdate.Reaction;
                                        profileToUpdate.NextVaccinationInfo = appointmentDetail.NextVaccinationInfo ?? profileToUpdate.NextVaccinationInfo;
                                        profileToUpdate.IsActive = true;
                                        profileToUpdate.IsCompleted = true;
                                        profileToUpdate.VaccinationScheduleId = vaccinationSchedule?.VaccinationScheduleId;
                                        profileToUpdate.ModifiedAt = DateTime.UtcNow;
                                        profileToUpdate.ModifiedBy = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "System";
                                        await _vaccineProfileRepository.UpdateVaccineProfileAsync(profileToUpdate, cancellationToken);
                                    }
                                    // Không tạo mới profile nếu không tìm thấy
                                }
                            }
                        }

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
            var vaccinationSchedule = await _vaccinationScheduleRepository.GetVaccinationScheduleByDiseaseIdAsync(createAppointmentVaccinationDTO.AppointmentDetailVaccination.DiseaseId, cancellationToken);
            if (vaccinationSchedule != null)
            {
                var vaccineProfiles = await _vaccineProfileRepository.GetListVaccineProfileByPetIdAsync(pet.PetId, cancellationToken);
                var profilesForDisease = vaccineProfiles?.Where(p => p.DiseaseId == createAppointmentVaccinationDTO.AppointmentDetailVaccination.DiseaseId).ToList() ?? new List<VaccineProfile>();

                // Calculate totalDose from DoseNumber of VaccinationSchedule
                int totalDose = vaccinationSchedule.DoseNumber;

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
            }
            if (vaccinationSchedule != null)
            {
                var vaccineProfiles = await _vaccineProfileRepository.GetListVaccineProfileByPetIdAsync(pet.PetId, cancellationToken);
                var profilesForDisease = vaccineProfiles?.Where(p => p.DiseaseId == createAppointmentVaccinationDTO.AppointmentDetailVaccination.DiseaseId).ToList() ?? new List<VaccineProfile>();

                // Assume that the number of doses required is stored in a property called "TotalDose" in VaccinationSchedule
                // If not, adjust this logic to match your data model
                var totalDoseProp = vaccinationSchedule.GetType().GetProperty("TotalDose");
                int totalDose = totalDoseProp != null ? (int)(totalDoseProp.GetValue(vaccinationSchedule) ?? 0) : 0;

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

        private int GetSlotNumberFromTime(TimeOnly time)
        {
            var hour = time.Hour;

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
                // Chỉ cập nhật DiseaseId cho appointmentDetail
                if (updateAppointmentForVaccinationDTO.UpdateDiseaseForAppointmentDTO.DiseaseId > 0)
                {
                    appointmentDetail.DiseaseId = updateAppointmentForVaccinationDTO.UpdateDiseaseForAppointmentDTO.DiseaseId;
                    appointmentDetail.ModifiedAt = DateTime.UtcNow;
                    appointmentDetail.ModifiedBy = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "System";
                    await _appointmentDetailRepository.UpdateAppointmentDetailAsync(appointmentDetail, cancellationToken);
                }
                // Lấy lại thông tin đã cập nhật
                var updatedAppointment = await _appointmentRepository.GetAppointmentByIdAsync(appointment.AppointmentId, cancellationToken);
                var updatedAppointmentDetail = await _appointmentDetailRepository.GetAppointmentDetailByIdAsync(appointmentDetail.AppointmentDetailId, cancellationToken);

                return new BaseResponse<AppointmentForVaccinationResponseDTO>
                {
                    Code = 200,
                    Success = true,
                    Message = "Cập nhật DiseaseId cho chi tiết cuộc hẹn thành công.",
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

                // Cập nhật thông tin microchip

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
                }else if (microchipItem.IsUsed == true)
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
                    microchipItem.IsUsed = true;
                    int rowEffected = await _microchipItemRepository.UpdateMicrochipItemAsync(microchipItem, cancellationToken);
                }

                appointmentDetail.VetId = updateAppointmentMicrochipDTO.VetId;
                appointmentDetail.MicrochipItemId = updateAppointmentMicrochipDTO.MicrochipItemId;
                appointmentDetail.AppointmentStatus = newStatus;
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

                        if ( updatedAppointment == null)
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
                if (updateAppointment == null )
                {
                    return new BaseResponse<AppointmentWithMicorchipResponseDTO>
                    {
                        Code = 500,
                        Success = false,
                        Message = "Không thể tạo cuộc hẹn cấy microchip.",
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
                        Message = "Không thể tạo chi tiết cuộc hẹn cấy microchip.",
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
                    Message = "Tạo cuộc hẹn tiêm phòng thành công.",
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


    }
}
