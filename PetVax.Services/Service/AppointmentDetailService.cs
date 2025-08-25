using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PetVax.BusinessObjects.DTO;
using PetVax.BusinessObjects.DTO.AccountDTO;
using PetVax.BusinessObjects.DTO.AppointmentDetailDTO;
using PetVax.BusinessObjects.DTO.AppointmentDTO;
using PetVax.BusinessObjects.Enum;
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
using static PetVax.BusinessObjects.Enum.EnumList;

namespace PetVax.Services.Service
{
    public class AppointmentDetailService : IAppointmentDetailService
    {
        private readonly IAppointmentDetailRepository _appointmentDetailRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<AppointmentDetailService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IVetRepository _vetRepository;
        private readonly IDiseaseRepository _diseaseRepository;
        private readonly IMicrochipItemRepository _microchipItemRepository;
        private readonly IVaccinationCertificateRepository _petPassportRepository;
        private readonly IHealthConditionRepository _healthConditionRepository;
        private readonly IVaccineBatchRepository _vaccineBatchRepository;

        public AppointmentDetailService(
            IAppointmentDetailRepository appointmentDetailRepository,
            IMapper mapper,
            ILogger<AppointmentDetailService> logger,
            IHttpContextAccessor httpContextAccessor,
            IAppointmentRepository appointmentRepository,
            IVetRepository vetRepository,
            IDiseaseRepository diseaseRepository,
            IMicrochipItemRepository microchipItemRepository,
            IVaccinationCertificateRepository petPassportRepository,
            IHealthConditionRepository healthConditionRepository,
            IVaccineBatchRepository vaccineBatchRepository)
        {
            _appointmentDetailRepository = appointmentDetailRepository;
            _mapper = mapper;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _appointmentRepository = appointmentRepository;
            _vetRepository = vetRepository;
            _diseaseRepository = diseaseRepository;
            _microchipItemRepository = microchipItemRepository;
            _petPassportRepository = petPassportRepository;
            _healthConditionRepository = healthConditionRepository;
            _vaccineBatchRepository = vaccineBatchRepository;
        }

        //public async Task<BaseResponse<AppointmentDetailResponseDTO>> CreateAppointmentDetail(CreateAppointmentDetailDTO createAppointmentDetailDTO, CancellationToken cancellationToken)
        //{
        //    if (createAppointmentDetailDTO == null || createAppointmentDetailDTO.AppointmentId <= 0)
        //    {
        //        return new BaseResponse<AppointmentDetailResponseDTO>
        //        {
        //            Code = 400,
        //            Success = false,
        //            Message = "Dữ liệu chi tiết cuộc hẹn không hợp lệ.",
        //            Data = null
        //        };
        //    }
        //    try
        //    {
        //        // Lấy thông tin Appointment từ repository (giả định có repository hoặc context)
        //        var appointment = await _appointmentRepository.GetAppointmentByIdAsync(createAppointmentDetailDTO.AppointmentId, cancellationToken);
        //        if (appointment == null)
        //        {
        //            return new BaseResponse<AppointmentDetailResponseDTO>
        //            {
        //                Code = 404,
        //                Success = false,
        //                Message = "Không tìm thấy cuộc hẹn.",
        //                Data = null
        //            };
        //        }

        //        if (appointment.ServiceType == EnumList.ServiceType.Vaccination)
        //        {
        //            if (createAppointmentDetailDTO.DiseaseId == null || createAppointmentDetailDTO.DiseaseId <= 0)
        //            {
        //                return new BaseResponse<AppointmentDetailResponseDTO>
        //                {
        //                    Code = 400,
        //                    Success = false,
        //                    Message = "Vui lòng cung cấp DiseaseId cho dịch vụ tiêm phòng.",
        //                    Data = null
        //                };
        //            }
        //            // Kiểm tra xem DiseaseId có tồn tại không
        //            var disease = await _diseaseRepository.GetDiseaseByIdAsync(createAppointmentDetailDTO.DiseaseId.Value, cancellationToken);
        //            if (disease == null)
        //            {
        //                return new BaseResponse<AppointmentDetailResponseDTO>
        //                {
        //                    Code = 404,
        //                    Success = false,
        //                    Message = "Không tìm thấy bệnh với ID đã cung cấp.",
        //                    Data = null
        //                };
        //            }
        //        }
        //        else if (appointment.ServiceType == EnumList.ServiceType.Microchip || appointment.ServiceType == EnumList.ServiceType.HealthCertificate)
        //        {
        //            // Kiểm tra các trường hợp khác nếu cần thiết
        //            if (createAppointmentDetailDTO.DiseaseId != null && createAppointmentDetailDTO.DiseaseId > 0)
        //            {
        //                return new BaseResponse<AppointmentDetailResponseDTO>
        //                {
        //                    Code = 400,
        //                    Success = false,
        //                    Message = "Không cần cung cấp DiseaseId cho dịch vụ Microchip hoặc Health Certificate.",
        //                    Data = null
        //                };
        //            }
        //            var random = new Random();

        //            var appointmentDetail = new AppointmentDetail
        //            {
        //                AppointmentId = createAppointmentDetailDTO.AppointmentId,
        //                AppointmentDate = appointment.AppointmentDate,
        //                AppointmentStatus = appointment.AppointmentStatus,
        //                AppointmentDetailCode = "AD" + random.Next(0, 1000000).ToString("D6"),
        //                CreatedAt = DateTime.UtcNow,
        //                ServiceType = appointment.ServiceType
        //            };

        //            // Lấy userId từ context
        //            var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier);
        //            if (userIdClaim != null)
        //            {
        //                appointmentDetail.CreatedBy = userIdClaim.Value;
        //            }

        //            var createdId = await _appointmentDetailRepository.AddAppointmentDetailAsync(appointmentDetail, cancellationToken);
        //            if (createdId > 0)
        //            {
        //                var responseData = _mapper.Map<AppointmentDetailResponseDTO>(appointmentDetail);
        //                return new BaseResponse<AppointmentDetailResponseDTO>
        //                {
        //                    Code = 201,
        //                    Success = true,
        //                    Message = "Tạo chi tiết cuộc hẹn thành công.",
        //                    Data = responseData
        //                };
        //            }
        //            else
        //            {
        //                return new BaseResponse<AppointmentDetailResponseDTO>
        //                {
        //                    Code = 500,
        //                    Success = false,
        //                    Message = "Không thể tạo chi tiết cuộc hẹn.",
        //                    Data = null
        //                };
        //            }
        //        }

        //        // Default return for unsupported service types
        //        return new BaseResponse<AppointmentDetailResponseDTO>
        //        {
        //            Code = 400,
        //            Success = false,
        //            Message = "Loại dịch vụ không được hỗ trợ.",
        //            Data = null
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error occurred while creating appointment detail.");
        //        return new BaseResponse<AppointmentDetailResponseDTO>
        //        {
        //            Code = 500,
        //            Success = false,
        //            Message = "Đã xảy ra lỗi khi tạo chi tiết cuộc hẹn.",
        //            Data = null
        //        };
        //    }
        //}

        public async Task<BaseResponse<bool>> DeleteAppointmentDetail(int appointmentDetailId, CancellationToken cancellationToken)
        {
            try
            {
                var appointmentDetail = await _appointmentDetailRepository.GetAppointmentDetailByIdAsync(appointmentDetailId, cancellationToken);
                if (appointmentDetail == null)
                {
                    return new BaseResponse<bool>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Không tìm thấy chi tiết cuộc hẹn với ID đã cung cấp.",
                        Data = false
                    };
                }

                appointmentDetail.isDeleted = true;
                appointmentDetail.ModifiedAt = DateTimeHelper.Now();
                appointmentDetail.ModifiedBy = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "System";

                var updateResult = await _appointmentDetailRepository.UpdateAppointmentDetailAsync(appointmentDetail, cancellationToken);
                if (updateResult > 0)
                {
                    return new BaseResponse<bool>
                    {
                        Code = 200,
                        Success = true,
                        Message = "Xóa chi tiết cuộc hẹn (mềm) thành công.",
                        Data = true
                    };
                }
                else
                {
                    return new BaseResponse<bool>
                    {
                        Code = 500,
                        Success = false,
                        Message = "Không thể xóa chi tiết cuộc hẹn (mềm).",
                        Data = false
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while soft deleting appointment detail.");
                return new BaseResponse<bool>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã xảy ra lỗi khi xóa chi tiết cuộc hẹn (mềm).",
                    Data = false
                };
            }
        }

        public async Task<DynamicResponse<AppointmentDetailResponseDTO>> GetAllAppointmentDetail(GetAllItemsDTO getAllItemsDTO, CancellationToken cancellationToken)
        {
            try
            {
                var details = await _appointmentDetailRepository.GetAllAppointmentDetailsAsync(cancellationToken);
                if (!string.IsNullOrWhiteSpace(getAllItemsDTO.KeyWord))
                {
                    var keyword = getAllItemsDTO.KeyWord.ToLower();
                    details = details
                        .Where(d =>
                            // Search by AppointmentDetailCode
                            (d.AppointmentDetailCode != null && d.AppointmentDetailCode.ToLower().Contains(keyword)) ||

                            // Search by Vet name
                            (d.Vet != null && d.Vet.Name != null && d.Vet.Name.ToLower().Contains(keyword)) ||

                            // Search by Vaccine batch code
                            (d.VaccineBatch != null && d.VaccineBatch.Vaccine.Name != null && d.VaccineBatch.Vaccine.Name.ToLower().Contains(keyword)) ||

                            // Search by Disease name
                            (d.Disease != null && d.Disease.Name != null && d.Disease.Name.ToLower().Contains(keyword)) ||

                            // Search by Microchip code
                            (d.MicrochipItem != null && d.MicrochipItem.Name != null && d.MicrochipItem.Name.ToLower().Contains(keyword)) ||

                            // Search by ServiceType (enum)
                            (d.ServiceType.ToString().ToLower().Contains(keyword)) ||

                            // Search by Appointment status (enum)
                            (d.AppointmentStatus.ToString().ToLower().Contains(keyword))

                        // Search by Dose information
                        )
                        .ToList();
                }
                // Filter by status if provided
                if (getAllItemsDTO?.Status.HasValue == true)
                {
                    details = details
                        .Where(a => a.isDeleted == getAllItemsDTO.Status.Value)
                        .ToList();
                }
                int pageNumber = getAllItemsDTO?.PageNumber > 0 ? getAllItemsDTO.PageNumber : 1;
                int pageSize = getAllItemsDTO?.PageSize > 0 ? getAllItemsDTO.PageSize : 10;
                int skip = (pageNumber - 1) * pageSize;
                int totalItem = details.Count;
                int totalPage = (int)Math.Ceiling((double)totalItem / pageSize);

                var pagedDetails = details
                    .Skip(skip)
                    .Take(pageSize)
                    .ToList();

                var responseData = new MegaData<AppointmentDetailResponseDTO>
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
                    PageData = _mapper.Map<List<AppointmentDetailResponseDTO>>(pagedDetails)
                };

                if (!pagedDetails.Any())
                {
                    return new DynamicResponse<AppointmentDetailResponseDTO>
                    {
                        Code = 200,
                        Success = false,
                        Message = "Không tìm thấy chi tiết cuộc hẹn nào.",
                        Data = null
                    };
                }
                return new DynamicResponse<AppointmentDetailResponseDTO>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy tất cả chi tiết cuộc hẹn thành công.",
                    Data = responseData
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting all appointment details.");
                return new DynamicResponse<AppointmentDetailResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã xảy ra lỗi khi lấy tất cả chi tiết cuộc hẹn.",
                    Data = null
                };
            }
        }

        //public async Task<BaseResponse<AppointmentDetailResponseDTO>> GetAppointmentDetailByAppointmentId(int appointmentId, CancellationToken cancellationToken)
        //{
        //    try
        //    {
        //        var appointmentDetail = await _appointmentDetailRepository.GetAppointmentDetailsByAppointmentIdAsync(appointmentId, cancellationToken);
        //        if (appointmentDetail == null)
        //        {
        //            return new BaseResponse<AppointmentDetailResponseDTO>
        //            {
        //                Code = 200,
        //                Success = false,
        //                Message = "Không tìm thấy chi tiết cuộc hẹn cho ID đã cung cấp.",
        //                Data = null
        //            };
        //        }
        //        var responseData = _mapper.Map<AppointmentDetailResponseDTO>(appointmentDetail);
        //        return new BaseResponse<AppointmentDetailResponseDTO>
        //        {
        //            Code = 200,
        //            Success = true,
        //            Message = "Lấy chi tiết cuộc hẹn thành công.",
        //            Data = responseData
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error occurred while getting appointment detail by appointment ID.");
        //        return new BaseResponse<AppointmentDetailResponseDTO>
        //        {
        //            Code = 500,
        //            Success = false,
        //            Message = "Đã xảy ra lỗi khi lấy chi tiết cuộc hẹn.",
        //            Data = null
        //        };
        //    }
        //}

        //public async Task<BaseResponse<AppointmentDetailResponseDTO>> GetAppointmentDetailById(int appointmentDetailId, CancellationToken cancellationToken)
        //{
        //    try
        //    {
        //        var appointmentDetail = await _appointmentDetailRepository.GetAppointmentDetailByIdAsync(appointmentDetailId, cancellationToken);
        //        if (appointmentDetail == null)
        //        {
        //            return new BaseResponse<AppointmentDetailResponseDTO>
        //            {
        //                Code = 200,
        //                Success = false,
        //                Message = "Không tìm thấy chi tiết cuộc hẹn cho ID đã cung cấp.",
        //                Data = null
        //            };
        //        }
        //        var responseData = _mapper.Map<AppointmentDetailResponseDTO>(appointmentDetail);
        //        return new BaseResponse<AppointmentDetailResponseDTO>
        //        {
        //            Code = 200,
        //            Success = true,
        //            Message = "Lấy chi tiết cuộc hẹn thành công.",
        //            Data = responseData
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error occurred while getting appointment detail by ID.");
        //        return new BaseResponse<AppointmentDetailResponseDTO>
        //        {
        //            Code = 500,
        //            Success = false,
        //            Message = "Đã xảy ra lỗi khi lấy chi tiết cuộc hẹn.",
        //            Data = null
        //        };
        //    }
        //}

        //public async Task<BaseResponse<AppointmentDetailResponseDTO>> GetAppointmentDetailByPetId(int petId, CancellationToken cancellationToken)
        //{
        //    try
        //    {
        //        var appointmentDetails = await _appointmentDetailRepository.GetAppointmentDetailByPetIdAsync(petId, cancellationToken);
        //        if (appointmentDetails == null)
        //        {
        //            return new BaseResponse<AppointmentDetailResponseDTO>
        //            {
        //                Code = 200,
        //                Success = false,
        //                Message = "Không tìm thấy chi tiết cuộc hẹn cho Pet ID đã cung cấp.",
        //                Data = null
        //            };
        //        }
        //        var responseData = _mapper.Map<List<AppointmentDetailResponseDTO>>(appointmentDetails);
        //        return new BaseResponse<AppointmentDetailResponseDTO>
        //        {
        //            Code = 200,
        //            Success = true,
        //            Message = "Lấy chi tiết cuộc hẹn theo Pet ID thành công.",
        //            Data = responseData.FirstOrDefault() // Assuming you want the first detail if multiple exist
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error occurred while getting appointment detail by pet ID.");
        //        return new BaseResponse<AppointmentDetailResponseDTO>
        //        {
        //            Code = 500,
        //            Success = false,
        //            Message = "Đã xảy ra lỗi khi lấy chi tiết cuộc hẹn theo Pet ID.",
        //            Data = null
        //        };
        //    }
        //}

        public async Task<BaseResponse<List<AppointmentDetailResponseDTO>>> GetAppointmentDetailByServiceType(EnumList.ServiceType serviceType, CancellationToken cancellationToken)
        {
            try
            {
                var appointmentDetails = await _appointmentDetailRepository.GetAppointmentDetailsByServiceTypeAsync(serviceType, cancellationToken);
                if (appointmentDetails == null || !appointmentDetails.Any())
                {
                    return new BaseResponse<List<AppointmentDetailResponseDTO>>
                    {
                        Code = 200,
                        Success = false,
                        Message = "Không tìm thấy chi tiết cuộc hẹn cho loại dịch vụ đã cung cấp.",
                        Data = null
                    };
                }
                var responseData = _mapper.Map<List<AppointmentDetailResponseDTO>>(appointmentDetails);
                return new BaseResponse<List<AppointmentDetailResponseDTO>>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy chi tiết cuộc hẹn theo loại dịch vụ thành công.",
                    Data = responseData
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting appointment detail by service type.");
                return new BaseResponse<List<AppointmentDetailResponseDTO>>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã xảy ra lỗi khi lấy chi tiết cuộc hẹn theo loại dịch vụ.",
                    Data = null
                };
            }
        }

        //public async Task<BaseResponse<AppointmentDetailResponseDTO>> GetAppointmentDetailByStatus(EnumList.AppointmentStatus status, CancellationToken cancellationToken)
        //{
        //    try
        //    {
        //        var appointmentDetails = await _appointmentDetailRepository.GetAppointmentDetailsByStatusAsync(status, cancellationToken);
        //        if (appointmentDetails == null || !appointmentDetails.Any())
        //        {
        //            return new BaseResponse<AppointmentDetailResponseDTO>
        //            {
        //                Code = 200,
        //                Success = false,
        //                Message = "Không tìm thấy chi tiết cuộc hẹn cho trạng thái đã cung cấp.",
        //                Data = null
        //            };
        //        }
        //        var responseData = _mapper.Map<List<AppointmentDetailResponseDTO>>(appointmentDetails);
        //        return new BaseResponse<AppointmentDetailResponseDTO>
        //        {
        //            Code = 200,
        //            Success = true,
        //            Message = "Lấy chi tiết cuộc hẹn theo trạng thái thành công.",
        //            Data = responseData.FirstOrDefault() // Assuming you want the first detail if multiple exist
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error occurred while getting appointment detail by status.");
        //        return new BaseResponse<AppointmentDetailResponseDTO>
        //        {
        //            Code = 500,
        //            Success = false,
        //            Message = "Đã xảy ra lỗi khi lấy chi tiết cuộc hẹn theo trạng thái.",
        //            Data = null
        //        };
        //    }
        //}

        //public async Task<BaseResponse<AppointmentDetailResponseDTO>> GetAppointmentDetailByVetId(int vetId, CancellationToken cancellationToken)
        //{
        //    try
        //    {
        //        var appointmentDetails = await _appointmentDetailRepository.GetAppointmentDetailsByVetIdAsync(vetId, cancellationToken);
        //        if (appointmentDetails == null)
        //        {
        //            return new BaseResponse<AppointmentDetailResponseDTO>
        //            {
        //                Code = 200,
        //                Success = false,
        //                Message = "Không tìm thấy chi tiết cuộc hẹn cho Vet ID đã cung cấp.",
        //                Data = null
        //            };
        //        }
        //        var responseData = _mapper.Map<List<AppointmentDetailResponseDTO>>(appointmentDetails);
        //        return new BaseResponse<AppointmentDetailResponseDTO>
        //        {
        //            Code = 200,
        //            Success = true,
        //            Message = "Lấy chi tiết cuộc hẹn theo Vet ID thành công.",
        //            Data = responseData.FirstOrDefault() // Assuming you want the first detail if multiple exist
        //        };
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, "Error occurred while getting appointment detail by vet ID.");
        //        return new BaseResponse<AppointmentDetailResponseDTO>
        //        {
        //            Code = 500,
        //            Success = false,
        //            Message = "Đã xảy ra lỗi khi lấy chi tiết cuộc hẹn theo Vet ID.",
        //            Data = null
        //        };
        //    }
        //}

        //public Task<BaseResponse<AppointmentDetailResponseDTO>> UpdateAppointmentDetail(int appointmentDetailId, UpdateAppointmentDetailDTO updateAppointmentDetailDTO, CancellationToken cancellationToken)
        //{
        //    throw new NotImplementedException();
        //}

        //public Task<BaseResponse<AppointmentVaccinationDetailResponseDTO>> UpdateAppointmentVaccination(int appointmentDetailId, UpdateAppointmentVaccinationDTO updateAppointmentVaccinationDTO, CancellationToken cancellationToken)
        //{
        //    throw new NotImplementedException();
        //}
        public async Task<BaseResponse<List<AppointmentVaccinationDetailResponseDTO>>> GetAppointmentVaccinationByPetId(int petId, CancellationToken cancellationToken)
        {
            try
            {
                var appointmentDetails = await _appointmentDetailRepository.GetAppointmentVaccinationDetailByPetId(petId, cancellationToken);
                if (appointmentDetails == null)
                {
                    return new BaseResponse<List<AppointmentVaccinationDetailResponseDTO>>
                    {
                        Code = 200,
                        Success = false,
                        Message = "Không tìm thấy chi tiết cuộc hẹn tiêm phòng cho thú cưng này.",
                        Data = null
                    };
                }
                var appointmentVaccinationDetailResponse = _mapper.Map<List<AppointmentVaccinationDetailResponseDTO>>(appointmentDetails);
                return new BaseResponse<List<AppointmentVaccinationDetailResponseDTO>>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy thông tin tiêm phòng theo thú cưng thành công.",
                    Data = appointmentVaccinationDetailResponse
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Đã xảy ra lỗi khi lấy thông tin tiêm phòng theo ID thú cưng.");
                return new BaseResponse<List<AppointmentVaccinationDetailResponseDTO>>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã xảy ra lỗi khi lấy thông tin tiêm phòng theo ID thú cưng.",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<List<AppointmentVaccinationDetailResponseDTO>>> GetAppointmentVaccinationByPetIdAndStatus(int petId, EnumList.AppointmentStatus status, CancellationToken cancellationToken)
        {
            try
            {
                var appointmentDetails = await _appointmentDetailRepository.GetAppointmentVaccinationDetailByPetIdAndStatus(petId, status, cancellationToken);
                if (appointmentDetails == null)
                {
                    return new BaseResponse<List<AppointmentVaccinationDetailResponseDTO>>
                    {
                        Code = 200,
                        Success = false,
                        Message = "Không tìm thấy chi tiết cuộc hẹn tiêm phòng cho thú cưng này với trạng thái đã cung cấp.",
                        Data = null
                    };
                }
                var appointmentVaccinationDetailResponse = _mapper.Map<List<AppointmentVaccinationDetailResponseDTO>>(appointmentDetails);
                return new BaseResponse<List<AppointmentVaccinationDetailResponseDTO>>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy thông tin tiêm phòng theo thú cưng và trạng thái thành công.",
                    Data = appointmentVaccinationDetailResponse
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Đã xảy ra lỗi khi lấy thông tin tiêm phòng theo ID thú cưng và trạng thái.");
                return new BaseResponse<List<AppointmentVaccinationDetailResponseDTO>>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã xảy ra lỗi khi lấy thông tin tiêm phòng theo ID thú cưng và trạng thái.",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<AppointmentVaccinationDetailResponseDTO>> GetAppointmentVaccinationByAppointmentId(int appointmentId, CancellationToken cancellationToken)
        {
            try
            {
                var vaccinations = await _appointmentDetailRepository.GetAppointmentVaccinationByAppointmentId(appointmentId, cancellationToken);
                if (vaccinations == null)
                {
                    return new BaseResponse<AppointmentVaccinationDetailResponseDTO>
                    {
                        Code = 200,
                        Success = false,
                        Message = "Không tìm thấy thông tin lịch tiêm theo id lịch hẹn",
                        Data = null
                    };
                }

                var vaccinationResponse = _mapper.Map<AppointmentVaccinationDetailResponseDTO>(vaccinations);

                // Filter Vet.ScheduleResponse to only include schedules matching AppointmentDate and Slot
                if (vaccinationResponse?.Vet?.ScheduleResponse != null)
                {
                    var appointmentDate = vaccinations.AppointmentDate.Date;
                    var appointmentSlot = vaccinations.AppointmentDate.Hour; // Assuming AppointmentDetail has AppointmentSlot property

                    vaccinationResponse.Vet.ScheduleResponse = vaccinationResponse.Vet.ScheduleResponse
                        .Where(s => s.ScheduleDate.Date == appointmentDate && s.SlotNumber == appointmentSlot)
                        .ToList();
                }

                return new BaseResponse<AppointmentVaccinationDetailResponseDTO>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy thông tin lịch tiêm thành công",
                    Data = vaccinationResponse
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Đã xảy ra lỗi khi lấy thông tin tiêm phòng theo id lịch hẹn");
                return new BaseResponse<AppointmentVaccinationDetailResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã xảy ra lỗi khi lấy thông tin tiêm phòng theo id lịch hẹn",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<List<AppointmenDetialMicorchipResponseDTO>>> GetAppointmentMicrochipByPetId(int petId, CancellationToken cancellationToken)
        {
            try
            {
                var appointmentDetails = await _appointmentDetailRepository.GetAppointmentDetaiMicrochiplsByPetIdAsync(petId, cancellationToken);
                if (appointmentDetails == null || !appointmentDetails.Any())
                {
                    return new BaseResponse<List<AppointmenDetialMicorchipResponseDTO>>
                    {
                        Code = 200,
                        Success = false,
                        Message = "Không tìm thấy chi tiết cuộc hẹn microchip cho thú cưng này.",
                        Data = new List<AppointmenDetialMicorchipResponseDTO>()
                    };
                }

                // Manually map AppointmentDetail to AppointmenDetialMicorchipResponseDTO
                var appointmentMicrochipResponse = _mapper.Map<List<AppointmenDetialMicorchipResponseDTO>>(appointmentDetails);

                return new BaseResponse<List<AppointmenDetialMicorchipResponseDTO>>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy thông tin microchip theo thú cưng thành công.",
                    Data = appointmentMicrochipResponse
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<AppointmenDetialMicorchipResponseDTO>>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã xảy ra lỗi khi lấy thông tin microchip theo ID thú cưng." + ex.Message,
                    Data = new List<AppointmenDetialMicorchipResponseDTO>()
                };
            }
        }

        public async Task<BaseResponse<AppointmenDetialMicorchipResponseDTO>> GetAppointmentMicrochipByAppointmentDetailId(int appointmentId, CancellationToken cancellationToken)
        {
            try
            {
                var appointmentDetail = await _appointmentDetailRepository.GetAppointmentDetailMicrochipByAppointmentIdAsync(appointmentId, cancellationToken);
                if (appointmentDetail == null)
                {
                    return new BaseResponse<AppointmenDetialMicorchipResponseDTO>
                    {
                        Code = 200,
                        Success = false,
                        Message = "Không tìm thấy chi tiết cuộc hẹn microchip với AppointmentDetailId này.",
                        Data = null
                    };
                }

                var appointmentMicrochipResponse = _mapper.Map<AppointmenDetialMicorchipResponseDTO>(appointmentDetail);

                return new BaseResponse<AppointmenDetialMicorchipResponseDTO>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy thông tin microchip theo AppointmentDetailId thành công.",
                    Data = appointmentMicrochipResponse
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<AppointmenDetialMicorchipResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã xảy ra lỗi khi lấy thông tin microchip theo AppointmentDetailId. " + ex.Message,
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<List<AppointmenDetialMicorchipResponseDTO>>> GetAppointmentMicrochipByPetIdAndStatus(int petId, AppointmentStatus status, CancellationToken cancellationToken)
        {
            try
            {
                var appointmentDetails = await _appointmentDetailRepository.GetAllAppointmentDetailsMicrochipByPetIdAndStatusAsync(petId, status, cancellationToken);
                if (appointmentDetails == null)
                {
                    return new BaseResponse<List<AppointmenDetialMicorchipResponseDTO>>
                    {
                        Code = 200,
                        Success = false,
                        Message = "Không tìm thấy chi tiết cuộc hẹn microchip cho thú cưng này với trạng thái đã cung cấp.",
                        Data = null
                    };
                }
                var appointmentMicrochipResponses = _mapper.Map<List<AppointmenDetialMicorchipResponseDTO>>(appointmentDetails);
                return new BaseResponse<List<AppointmenDetialMicorchipResponseDTO>>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy thông tin microchip theo thú cưng và trạng thái thành công.",
                    Data = appointmentMicrochipResponses
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Đã xảy ra lỗi khi lấy thông tin microchip theo ID thú cưng và trạng thái.");
                return new BaseResponse<List<AppointmenDetialMicorchipResponseDTO>>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã xảy ra lỗi khi lấy thông tin microchip theo ID thú cưng và trạng thái.",
                    Data = null
                };
            }
        }

        public async Task<DynamicResponse<AppointmenDetialMicorchipResponseDTO>> GetAllAppointmemtMicrochipAsync(GetAllItemsDTO getAllItemsDTO, int? vetId, CancellationToken cancellationToken)
        {
            try
            {
                var appointmentDetails = await _appointmentDetailRepository.GetAllAppointmentDetailsMicrochipAsync(cancellationToken);
                if (appointmentDetails == null || !appointmentDetails.Any())
                {
                    return new DynamicResponse<AppointmenDetialMicorchipResponseDTO>
                    {
                        Code = 200,
                        Success = true,
                        Message = "Không tìm thấy chi tiết cuộc hẹn microchip nào.",
                        Data = null
                    };
                }

                // Filter by vetId if provided
                if (vetId.HasValue && vetId > 0)
                {
                    appointmentDetails = appointmentDetails
                        .Where(d => d.VetId.HasValue && d.VetId.Value == vetId.Value)
                        .ToList();
                }

                // Filtering by keyword if provided
                if (!string.IsNullOrWhiteSpace(getAllItemsDTO?.KeyWord))
                {
                    var keyword = getAllItemsDTO.KeyWord.ToLower();
                    appointmentDetails = appointmentDetails
                        .Where(d =>
                            (d.AppointmentDetailCode != null && d.AppointmentDetailCode.ToLower().Contains(keyword)) ||
                            (d.MicrochipItem != null && d.MicrochipItem.Name != null && d.MicrochipItem.Name.ToLower().Contains(keyword)) ||
                            (d.ServiceType.ToString().ToLower().Contains(keyword))
                        )
                        .ToList();
                }

                int pageNumber = getAllItemsDTO?.PageNumber > 0 ? getAllItemsDTO.PageNumber : 1;
                int pageSize = getAllItemsDTO?.PageSize > 0 ? getAllItemsDTO.PageSize : 10;
                int skip = (pageNumber - 1) * pageSize;
                int totalItem = appointmentDetails.Count;
                int totalPage = (int)Math.Ceiling((double)totalItem / pageSize);

                var pagedDetails = appointmentDetails
                    .Skip(skip)
                    .Take(pageSize)
                    .ToList();

                var responseData = _mapper.Map<List<AppointmenDetialMicorchipResponseDTO>>(pagedDetails);

                var megaData = new MegaData<AppointmenDetialMicorchipResponseDTO>
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
                    PageData = responseData
                };

                return new DynamicResponse<AppointmenDetialMicorchipResponseDTO>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy tất cả chi tiết cuộc hẹn microchip thành công.",
                    Data = megaData
                };
            }
            catch (Exception ex)
            {
                return new DynamicResponse<AppointmenDetialMicorchipResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã xảy ra lỗi khi lấy tất cả chi tiết cuộc hẹn microchip.",
                    Data = null
                };
            }
        }



        public async Task<BaseResponse<AppointmentHealthConditionResponseDTO>> GetAppointmentDetailHealthConditionByAppointmentIdAsync(int id, CancellationToken cancellationToken)
        {
            try
            {
                var appointmentDetail = await _appointmentDetailRepository.GetAppointmentDetailHealthConditionByAppointmentIdAsync(id, cancellationToken);
                if (appointmentDetail == null)
                {
                    return new BaseResponse<AppointmentHealthConditionResponseDTO>
                    {
                        Code = 200,
                        Success = false,
                        Message = "Không tìm thấy chi tiết cuộc hẹn sức khỏe với ID đã cung cấp.",
                        Data = new AppointmentHealthConditionResponseDTO()
                    };
                }
                var responseData = _mapper.Map<AppointmentHealthConditionResponseDTO>(appointmentDetail);
                return new BaseResponse<AppointmentHealthConditionResponseDTO>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy chi tiết cuộc hẹn sức khỏe thành công.",
                    Data = responseData
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting appointment detail health condition by ID.");
                return new BaseResponse<AppointmentHealthConditionResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã xảy ra lỗi khi lấy chi tiết cuộc hẹn sức khỏe.",
                    Data = new AppointmentHealthConditionResponseDTO()
                };
            }
        }

        public async Task<BaseResponse<List<AppointmentHealthConditionResponseDTO>>> GetAppointmentDetailHealthConditionByPetIdAsync(int petId, CancellationToken cancellationToken)
        {
            try
            {
                var appointmentDetails = await _appointmentDetailRepository.GetAppointmentDetailHealthConditionByPetIdAsync(petId, cancellationToken);
                if (appointmentDetails == null || !appointmentDetails.Any())
                {
                    return new BaseResponse<List<AppointmentHealthConditionResponseDTO>>
                    {
                        Code = 200,
                        Success = false,
                        Message = "Không tìm thấy chi tiết cuộc hẹn sức khỏe cho Pet ID đã cung cấp.",
                        Data = new List<AppointmentHealthConditionResponseDTO>()
                    };
                }
                var responseData = _mapper.Map<List<AppointmentHealthConditionResponseDTO>>(appointmentDetails);
                return new BaseResponse<List<AppointmentHealthConditionResponseDTO>>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy chi tiết cuộc hẹn sức khỏe theo Pet ID thành công.",
                    Data = responseData
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<AppointmentHealthConditionResponseDTO>>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã xảy ra lỗi khi lấy chi tiết cuộc hẹn sức khỏe theo Pet ID.",
                    Data = new List<AppointmentHealthConditionResponseDTO>()
                };
            }
        }

        public async Task<DynamicResponse<AppointmentHealthConditionResponseDTO>> GetAllAppointmentDetailHealthConditionAsync(GetAllItemsDTO getAllItemsDTO, int? vetId, CancellationToken cancellationToken)
        {
            try
            {
                var appointmentDetails = await _appointmentDetailRepository.GetAllAppointmentDetailHealthConditionAsync(cancellationToken);
                if (appointmentDetails == null || !appointmentDetails.Any())
                {
                    return new DynamicResponse<AppointmentHealthConditionResponseDTO>
                    {
                        Code = 200,
                        Success = true,
                        Message = "Không tìm thấy chi tiết cuộc hẹn sức khỏe nào.",
                        Data = null
                    };
                }

                // Filter by vetId if provided
                if (vetId.HasValue && vetId > 0)
                {
                    appointmentDetails = appointmentDetails
                        .Where(d => d.VetId == vetId.Value)
                        .ToList();
                }

                // Filtering by keyword if provided
                if (!string.IsNullOrWhiteSpace(getAllItemsDTO?.KeyWord))
                {
                    var keyword = getAllItemsDTO.KeyWord.ToLower();
                    appointmentDetails = appointmentDetails
                        .Where(d => d.AppointmentDetailCode != null && d.AppointmentDetailCode.ToLower().Contains(keyword))
                        .ToList();
                }

                int pageNumber = getAllItemsDTO?.PageNumber > 0 ? getAllItemsDTO.PageNumber : 1;
                int pageSize = getAllItemsDTO?.PageSize > 0 ? getAllItemsDTO.PageSize : 10;
                int skip = (pageNumber - 1) * pageSize;
                int totalItem = appointmentDetails.Count;
                int totalPage = (int)Math.Ceiling((double)totalItem / pageSize);
                var pagedDetails = appointmentDetails
                    .Skip(skip)
                    .Take(pageSize)
                    .ToList();
                var responseData = _mapper.Map<List<AppointmentHealthConditionResponseDTO>>(pagedDetails);
                var megaData = new MegaData<AppointmentHealthConditionResponseDTO>
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
                    PageData = responseData
                };
                return new DynamicResponse<AppointmentHealthConditionResponseDTO>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy tất cả chi tiết cuộc hẹn sức khỏe thành công.",
                    Data = megaData
                };
            }
            catch (Exception ex)
            {
                return new DynamicResponse<AppointmentHealthConditionResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã xảy ra lỗi khi lấy tất cả chi tiết cuộc hẹn sức khỏe." + ex.InnerException,
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<List<AppointmentHealthConditionResponseDTO>>> GetAppointmentDetailHealthConditionByPetIdAndStatusAsync(int petId, AppointmentStatus status, CancellationToken cancellationToken)
        {
            try
            {
                var appointmentDetails = await _appointmentDetailRepository.GetAllAppointmentDetailsHealthconditionByPetIdAndStatusAsync(petId, status, cancellationToken);
                if (appointmentDetails == null || !appointmentDetails.Any())
                {
                    return new BaseResponse<List<AppointmentHealthConditionResponseDTO>>
                    {
                        Code = 200,
                        Success = false,
                        Message = "Không tìm thấy chi tiết cuộc hẹn sức khỏe cho Pet ID và trạng thái đã cung cấp.",
                        Data = new List<AppointmentHealthConditionResponseDTO>()
                    };
                }
                var responseData = _mapper.Map<List<AppointmentHealthConditionResponseDTO>>(appointmentDetails);
                return new BaseResponse<List<AppointmentHealthConditionResponseDTO>>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy chi tiết cuộc hẹn sức khỏe theo Pet ID và trạng thái thành công.",
                    Data = responseData
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<AppointmentHealthConditionResponseDTO>>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã xảy ra lỗi khi lấy chi tiết cuộc hẹn sức khỏe theo Pet ID và trạng thái.",
                    Data = new List<AppointmentHealthConditionResponseDTO>()
                };
            }
        }
    }
}
