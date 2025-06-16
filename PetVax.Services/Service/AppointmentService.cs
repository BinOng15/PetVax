using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PetVax.BusinessObjects.DTO;
using PetVax.BusinessObjects.DTO.AppointmentDTO;
using PetVax.BusinessObjects.DTO.CustomerDTO;
using PetVax.BusinessObjects.Enum;
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
    public class AppointmentService : IAppointmentService
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IPetRepository _petRepository;
        private readonly ILogger<AppointmentService> _logger;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AppointmentService(
            IAppointmentRepository appointmentRepository,
            IPetRepository petRepository,
            ILogger<AppointmentService> logger,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            _appointmentRepository = appointmentRepository;
            _petRepository = petRepository;
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

                var pagedCustomers = appointments
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
                    PageData = _mapper.Map<List<AppointmentResponseDTO>>(pagedCustomers)
                };

                if (!pagedCustomers.Any())
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
                if (!string.IsNullOrWhiteSpace(updateAppointmentDTO.ServiceType))
                    appointment.ServiceType = updateAppointmentDTO.ServiceType;
                if (!string.IsNullOrWhiteSpace(updateAppointmentDTO.Location))
                    appointment.Location = updateAppointmentDTO.Location;
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
