using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PetVax.BusinessObjects.DTO;
using PetVax.BusinessObjects.DTO.AppointmentDetailDTO;
using PetVax.BusinessObjects.DTO.AppointmentDTO;
using PetVax.BusinessObjects.DTO.CustomerDTO;
using PetVax.BusinessObjects.Enum;
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
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IPetRepository _petRepository;
        private readonly IDiseaseRepository _diseaseRepository;
        private readonly IAppointmentDetailRepository _appointmentDetailRepository;
        private readonly ILogger<AppointmentService> _logger;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AppointmentService(
            IAppointmentRepository appointmentRepository,
            IPetRepository petRepository,
            IDiseaseRepository diseaseRepository,
            IAppointmentDetailRepository appointmentDetailRepository,
            ILogger<AppointmentService> logger,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            _appointmentRepository = appointmentRepository;
            _petRepository = petRepository;
            _diseaseRepository = diseaseRepository;
            _appointmentDetailRepository = appointmentDetailRepository;
            _logger = logger;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<BaseResponse<AppointmentResponseDTO>> CreateAppointmentAsync(CreateAppointmentDTO createAppointmentDTO, CancellationToken cancellationToken)
        {
            if (createAppointmentDTO == null)
            {
                return new BaseResponse<AppointmentResponseDTO>
                {
                    Code = 400,
                    Success = false,
                    Message = "Dữ liệu cuộc hẹn không hợp lệ.",
                    Data = null
                };
            }

            // Validate address based on location
            if (createAppointmentDTO.Location == EnumList.Location.HomeVisit && string.IsNullOrWhiteSpace(createAppointmentDTO.Address))
            {
                return new BaseResponse<AppointmentResponseDTO>
                {
                    Code = 400,
                    Success = false,
                    Message = "Vui lòng nhập địa chỉ khi chọn dịch vụ tại nhà.",
                    Data = null
                };
            }
            if (createAppointmentDTO.Location == EnumList.Location.Clinic)
            {
                // If Clinic, ignore address (set to null or empty)
                createAppointmentDTO.Address = "Đại học FPT TP. Hồ Chí Minh";
            }

            var pet = await _petRepository.GetPetByIdAsync(createAppointmentDTO.PetId, cancellationToken);
            if (pet == null || pet.CustomerId != createAppointmentDTO.CustomerId)
            {
                return new BaseResponse<AppointmentResponseDTO>
                {
                    Code = 404,
                    Success = false,
                    Message = "Thú cưng này không thuộc quyền sở hữu của chủ nuôi này",
                    Data = null
                };
            }

            try
            {
                var random = new Random();

                var appointment = _mapper.Map<Appointment>(createAppointmentDTO);
                appointment.AppointmentCode = "AP" + random.Next(0, 1000000).ToString("D6");
                appointment.AppointmentDate = createAppointmentDTO.AppointmentDate;
                appointment.ServiceType = createAppointmentDTO.ServiceType;
                appointment.Location = createAppointmentDTO.Location;
                appointment.Address = createAppointmentDTO.Address;
                appointment.AppointmentStatus = EnumList.AppointmentStatus.Processing;
                appointment.CreatedAt = DateTime.UtcNow;
                appointment.CreatedBy = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "System";

                var createdAppointment = await _appointmentRepository.CreateAppointmentAsync(appointment, cancellationToken);
                if (createdAppointment == null)
                {
                    return new BaseResponse<AppointmentResponseDTO>
                    {
                        Code = 500,
                        Success = false,
                        Message = "Không thể tạo cuộc hẹn.",
                        Data = null
                    };
                }

                var appointmentResponse = _mapper.Map<AppointmentResponseDTO>(appointment);
                return new BaseResponse<AppointmentResponseDTO>
                {
                    Code = 201,
                    Success = true,
                    Message = "Tạo cuộc hẹn thành công.",
                    Data = appointmentResponse
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Đã xảy ra lỗi khi tạo cuộc hẹn.");
                return new BaseResponse<AppointmentResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã xảy ra lỗi khi tạo cuộc hẹn.",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<AppointmentWithDetailResponseDTO>> CreateFullAppointmentAsync(CreateFullAppointmentDTO createFullAppointmentDTO, CancellationToken cancellationToken)
        {
            if (createFullAppointmentDTO == null)
            {
                return new BaseResponse<AppointmentWithDetailResponseDTO>
                {
                    Code = 400,
                    Success = false,
                    Message = "Dữ liệu để tạo cuộc hẹn không hợp lệ.",
                    Data = default!
                };
            }
            // Validate address based on location
            if (createFullAppointmentDTO.Appointment.Location == EnumList.Location.HomeVisit && string.IsNullOrWhiteSpace(createFullAppointmentDTO.Appointment.Address))
            {
                return new BaseResponse<AppointmentWithDetailResponseDTO>
                {
                    Code = 400,
                    Success = false,
                    Message = "Vui lòng nhập địa chỉ khi chọn dịch vụ tại nhà.",
                    Data = default!
                };
            }
            if (createFullAppointmentDTO.Appointment.Location == EnumList.Location.Clinic)
            {
                // If Clinic, ignore address (set to null or empty)
                createFullAppointmentDTO.Appointment.Address = "Đại học FPT TP. Hồ Chí Minh";
            }

            var pet = await _petRepository.GetPetByIdAsync(createFullAppointmentDTO.Appointment.PetId, cancellationToken);
            if (pet == null || pet.CustomerId != createFullAppointmentDTO.Appointment.CustomerId)
            {
                return new BaseResponse<AppointmentWithDetailResponseDTO>
                {
                    Code = 404,
                    Success = false,
                    Message = "Thú cưng này không thuộc quyền sở hữu của chủ nuôi này",
                    Data = default!
                };
            }

            // Check DiseaseId before creating Appointment
            if (createFullAppointmentDTO.Appointment.ServiceType == EnumList.ServiceType.Vaccination)
            {
                if (createFullAppointmentDTO.AppointmentDetail.DiseaseId == null || createFullAppointmentDTO.AppointmentDetail.DiseaseId <= 0)
                {
                    return new BaseResponse<AppointmentWithDetailResponseDTO>
                    {
                        Code = 400,
                        Success = false,
                        Message = "Vui lòng cung cấp DiseaseId cho dịch vụ tiêm phòng.",
                        Data = default!
                    };
                }
                // Kiểm tra xem DiseaseId có tồn tại không
                var disease = await _diseaseRepository.GetDiseaseByIdAsync(createFullAppointmentDTO.AppointmentDetail.DiseaseId.Value, cancellationToken);
                if (disease == null)
                {
                    return new BaseResponse<AppointmentWithDetailResponseDTO>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Không tìm thấy bệnh với ID đã cung cấp.",
                        Data = default!
                    };
                }
            }

            try
            {
                var random = new Random();

                var appointment = _mapper.Map<Appointment>(createFullAppointmentDTO.Appointment);
                appointment.AppointmentCode = "AP" + random.Next(0, 1000000).ToString("D6");
                appointment.AppointmentDate = createFullAppointmentDTO.Appointment.AppointmentDate;
                appointment.ServiceType = createFullAppointmentDTO.Appointment.ServiceType;
                appointment.Location = createFullAppointmentDTO.Appointment.Location;
                appointment.Address = createFullAppointmentDTO.Appointment.Address;
                appointment.AppointmentStatus = EnumList.AppointmentStatus.Processing;
                appointment.CreatedAt = DateTime.UtcNow;
                appointment.CreatedBy = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "System";

                var createdAppointmentId = await _appointmentRepository.CreateAppointmentAsync(appointment, cancellationToken);
                if (createdAppointmentId <= 0)
                {
                    return new BaseResponse<AppointmentWithDetailResponseDTO>
                    {
                        Code = 500,
                        Success = false,
                        Message = "Không thể tạo cuộc hẹn.",
                        Data = default!
                    };
                }

                // Lấy lại bản ghi Appointment vừa tạo để đảm bảo có đầy đủ thông tin (ID, ...)
                var createdAppointment = await _appointmentRepository.GetAppointmentByIdAsync(createdAppointmentId, cancellationToken);
                if (createdAppointment == null)
                {
                    return new BaseResponse<AppointmentWithDetailResponseDTO>
                    {
                        Code = 500,
                        Success = false,
                        Message = "Không thể lấy thông tin cuộc hẹn vừa tạo.",
                        Data = default!
                    };
                }
                var appointmentResponse = _mapper.Map<AppointmentResponseDTO>(createdAppointment);

                var appointmentDetail = new AppointmentDetail
                {
                    AppointmentId = appointmentResponse.AppointmentId,
                    AppointmentDate = appointmentResponse.AppointmentDate,
                    ServiceType = appointmentResponse.ServiceType,
                    AppointmentStatus = appointmentResponse.AppointmentStatus,
                    AppointmentDetailCode = "AD" + random.Next(0, 1000000).ToString("D6"),
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "System",
                };

                if (appointmentResponse.ServiceType == EnumList.ServiceType.Vaccination)
                {
                    // Gán DiseaseId nếu có
                    var diseaseIdProp = typeof(AppointmentDetail).GetProperty("DiseaseId");
                    if (diseaseIdProp != null && createFullAppointmentDTO.AppointmentDetail.DiseaseId != null)
                    {
                        diseaseIdProp.SetValue(appointmentDetail, createFullAppointmentDTO.AppointmentDetail.DiseaseId);
                    }
                }

                var createdAppointmentDetailId = await _appointmentDetailRepository.AddAppointmentDetailAsync(appointmentDetail, cancellationToken);
                if (createdAppointmentDetailId <= 0)
                {
                    return new BaseResponse<AppointmentWithDetailResponseDTO>
                    {
                        Code = 500,
                        Success = false,
                        Message = "Không thể tạo chi tiết cuộc hẹn.",
                        Data = default!
                    };
                }

                // Lấy lại bản ghi AppointmentDetail vừa tạo
                var createdAppointmentDetail = await _appointmentDetailRepository.GetAppointmentDetailByIdAsync(createdAppointmentDetailId, cancellationToken);
                if (createdAppointmentDetail == null)
                {
                    return new BaseResponse<AppointmentWithDetailResponseDTO>
                    {
                        Code = 500,
                        Success = false,
                        Message = "Không thể lấy thông tin chi tiết cuộc hẹn vừa tạo.",
                        Data = default!
                    };
                }
                var appointmentDetailResponse = _mapper.Map<AppointmentDetailResponseDTO>(createdAppointmentDetail);
                // Map the appointment and appointment detail to the response DTO
                var appointmentWithDetailResponse = new AppointmentWithDetailResponseDTO
                {
                    Appointment = appointmentResponse,
                    AppointmentDetail = appointmentDetailResponse
                };
                return new BaseResponse<AppointmentWithDetailResponseDTO>
                {
                    Code = 201,
                    Success = true,
                    Message = "Tạo cuộc hẹn và chi tiết cuộc hẹn thành công.",
                    Data = appointmentWithDetailResponse
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating appointment.");
                return new BaseResponse<AppointmentWithDetailResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã xảy ra lỗi khi tạo cuộc hẹn.",
                    Data = default!
                };
            }
        }

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
                        .Where(a => a.AppointmentCode.ToLower().Contains(keyword) ||
                        a.Pet.Name.ToLower().Contains(keyword)).ToList();
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
                        Code = 404,
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
                        Code = 404,
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

        public async Task<BaseResponse<AppointmentResponseDTO>> GetAppointmentByPetIdAsync(int petId, CancellationToken cancellationToken)
        {
            try
            {
                var appointments = await _appointmentRepository.GetAppointmentsByPetIdAsync(petId, cancellationToken);
                if (appointments == null || !appointments.Any())
                {
                    return new BaseResponse<AppointmentResponseDTO>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Không tìm thấy cuộc hẹn cho thú cưng này.",
                        Data = null
                    };
                }
                var appointmentResponses = _mapper.Map<List<AppointmentResponseDTO>>(appointments);
                return new BaseResponse<AppointmentResponseDTO>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy cuộc hẹn theo thú cưng thành công.",
                    Data = appointmentResponses.FirstOrDefault() // Assuming you want the first appointment
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Đã xảy ra lỗi khi lấy cuộc hẹn theo ID thú cưng.");
                return new BaseResponse<AppointmentResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã xảy ra lỗi khi lấy cuộc hẹn theo ID thú cưng.",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<AppointmentResponseDTO>> UpdateAppointmentAsync(int appointmentId, UpdateAppointmentDTO updateAppointmentDTO, CancellationToken cancellationToken)
        {
            if (updateAppointmentDTO == null)
            {
                return new BaseResponse<AppointmentResponseDTO>
                {
                    Code = 400,
                    Success = false,
                    Message = "Dữ liệu cuộc hẹn không hợp lệ.",
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
                    appointment.AppointmentDate = updateAppointmentDTO.AppointmentDate.Value;
                if (updateAppointmentDTO.ServiceType.HasValue)
                    appointment.ServiceType = updateAppointmentDTO.ServiceType.Value;
                if (updateAppointmentDTO.Location.HasValue)
                    appointment.Location = updateAppointmentDTO.Location.Value;
                if (!string.IsNullOrWhiteSpace(updateAppointmentDTO.Address))
                    appointment.Address = updateAppointmentDTO.Address;
                appointment.ModifiedAt = DateTime.UtcNow;
                appointment.ModifiedBy = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "System";
                var updatedAppointment = await _appointmentRepository.UpdateAppointmentAsync(appointment, cancellationToken);
                if (updatedAppointment == null)
                {
                    return new BaseResponse<AppointmentResponseDTO>
                    {
                        Code = 500,
                        Success = false,
                        Message = "Không thể cập nhật cuộc hẹn.",
                        Data = null
                    };
                }

                var appointmentResponse = _mapper.Map<AppointmentResponseDTO>(updatedAppointment);
                return new BaseResponse<AppointmentResponseDTO>
                {
                    Code = 200,
                    Success = true,
                    Message = "Cập nhật cuộc hẹn thành công.",
                    Data = appointmentResponse
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Đã xảy ra lỗi khi cập nhật cuộc hẹn.");
                return new BaseResponse<AppointmentResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã xảy ra lỗi khi cập nhật cuộc hẹn.",
                    Data = null
                };
            }
        }
    }
}
