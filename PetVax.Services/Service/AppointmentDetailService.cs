using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PetVax.BusinessObjects.DTO;
using PetVax.BusinessObjects.DTO.AppointmentDetailDTO;
using PetVax.BusinessObjects.DTO.AppointmentDTO;
using PetVax.BusinessObjects.Enum;
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
        private readonly IPetPassportRepository _petPassportRepository;
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
            IPetPassportRepository petPassportRepository,
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

        public Task<BaseResponse<bool>> DeleteAppointmentDetail(int appointmentDetailId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<DynamicResponse<AppointmentDetailResponseDTO>> GetAllAppointmentDetail(GetAllItemsDTO getAllItemsDTO, CancellationToken cancellationToken)
        {
            try
            {
                var details = await _appointmentDetailRepository.GetAllAppointmentDetailsAsync(cancellationToken);
                if (!string.IsNullOrWhiteSpace(getAllItemsDTO.KeyWord))
                {
                    details = details.Where(d => d.AppointmentDetailCode.Contains(getAllItemsDTO.KeyWord.ToLower(), StringComparison.OrdinalIgnoreCase)).ToList();
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
                    },
                    PageData = _mapper.Map<List<AppointmentDetailResponseDTO>>(pagedDetails)
                };

                if (!pagedDetails.Any())
                {
                    return new DynamicResponse<AppointmentDetailResponseDTO>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Không tìm thấy chi tiết cuộc hẹn nào.",
                        Data = responseData
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

        public async Task<BaseResponse<AppointmentDetailResponseDTO>> GetAppointmentDetailByAppointmentId(int appointmentId, CancellationToken cancellationToken)
        {
            try
            {
                var appointmentDetail = await _appointmentDetailRepository.GetAppointmentDetailsByAppointmentIdAsync(appointmentId, cancellationToken);
                if (appointmentDetail == null)
                {
                    return new BaseResponse<AppointmentDetailResponseDTO>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Không tìm thấy chi tiết cuộc hẹn cho ID đã cung cấp.",
                        Data = null
                    };
                }
                var responseData = _mapper.Map<AppointmentDetailResponseDTO>(appointmentDetail);
                return new BaseResponse<AppointmentDetailResponseDTO>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy chi tiết cuộc hẹn thành công.",
                    Data = responseData
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting appointment detail by appointment ID.");
                return new BaseResponse<AppointmentDetailResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã xảy ra lỗi khi lấy chi tiết cuộc hẹn.",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<AppointmentDetailResponseDTO>> GetAppointmentDetailById(int appointmentDetailId, CancellationToken cancellationToken)
        {
            try
            {
                var appointmentDetail = await _appointmentDetailRepository.GetAppointmentDetailByIdAsync(appointmentDetailId, cancellationToken);
                if (appointmentDetail == null)
                {
                    return new BaseResponse<AppointmentDetailResponseDTO>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Không tìm thấy chi tiết cuộc hẹn cho ID đã cung cấp.",
                        Data = null
                    };
                }
                var responseData = _mapper.Map<AppointmentDetailResponseDTO>(appointmentDetail);
                return new BaseResponse<AppointmentDetailResponseDTO>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy chi tiết cuộc hẹn thành công.",
                    Data = responseData
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting appointment detail by ID.");
                return new BaseResponse<AppointmentDetailResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã xảy ra lỗi khi lấy chi tiết cuộc hẹn.",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<AppointmentDetailResponseDTO>> GetAppointmentDetailByPetId(int petId, CancellationToken cancellationToken)
        {
            try
            {
                var appointmentDetails = await _appointmentDetailRepository.GetAppointmentDetailByPetIdAsync(petId, cancellationToken);
                if (appointmentDetails == null)
                {
                    return new BaseResponse<AppointmentDetailResponseDTO>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Không tìm thấy chi tiết cuộc hẹn cho Pet ID đã cung cấp.",
                        Data = null
                    };
                }
                var responseData = _mapper.Map<List<AppointmentDetailResponseDTO>>(appointmentDetails);
                return new BaseResponse<AppointmentDetailResponseDTO>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy chi tiết cuộc hẹn theo Pet ID thành công.",
                    Data = responseData.FirstOrDefault() // Assuming you want the first detail if multiple exist
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting appointment detail by pet ID.");
                return new BaseResponse<AppointmentDetailResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã xảy ra lỗi khi lấy chi tiết cuộc hẹn theo Pet ID.",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<AppointmentDetailResponseDTO>> GetAppointmentDetailByServiceType(EnumList.ServiceType serviceType, CancellationToken cancellationToken)
        {
            try
            {
                var appointmentDetails = await _appointmentDetailRepository.GetAppointmentDetailsByServiceTypeAsync(serviceType, cancellationToken);
                if (appointmentDetails == null || !appointmentDetails.Any())
                {
                    return new BaseResponse<AppointmentDetailResponseDTO>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Không tìm thấy chi tiết cuộc hẹn cho loại dịch vụ đã cung cấp.",
                        Data = null
                    };
                }
                var responseData = _mapper.Map<List<AppointmentDetailResponseDTO>>(appointmentDetails);
                return new BaseResponse<AppointmentDetailResponseDTO>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy chi tiết cuộc hẹn theo loại dịch vụ thành công.",
                    Data = responseData.FirstOrDefault() // Assuming you want the first detail if multiple exist
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting appointment detail by service type.");
                return new BaseResponse<AppointmentDetailResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã xảy ra lỗi khi lấy chi tiết cuộc hẹn theo loại dịch vụ.",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<AppointmentDetailResponseDTO>> GetAppointmentDetailByStatus(EnumList.AppointmentStatus status, CancellationToken cancellationToken)
        {
            try
            {
                var appointmentDetails = await _appointmentDetailRepository.GetAppointmentDetailsByStatusAsync(status, cancellationToken);
                if (appointmentDetails == null || !appointmentDetails.Any())
                {
                    return new BaseResponse<AppointmentDetailResponseDTO>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Không tìm thấy chi tiết cuộc hẹn cho trạng thái đã cung cấp.",
                        Data = null
                    };
                }
                var responseData = _mapper.Map<List<AppointmentDetailResponseDTO>>(appointmentDetails);
                return new BaseResponse<AppointmentDetailResponseDTO>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy chi tiết cuộc hẹn theo trạng thái thành công.",
                    Data = responseData.FirstOrDefault() // Assuming you want the first detail if multiple exist
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting appointment detail by status.");
                return new BaseResponse<AppointmentDetailResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã xảy ra lỗi khi lấy chi tiết cuộc hẹn theo trạng thái.",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<AppointmentDetailResponseDTO>> GetAppointmentDetailByVetId(int vetId, CancellationToken cancellationToken)
        {
            try
            {
                var appointmentDetails = await _appointmentDetailRepository.GetAppointmentDetailsByVetIdAsync(vetId, cancellationToken);
                if (appointmentDetails == null)
                {
                    return new BaseResponse<AppointmentDetailResponseDTO>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Không tìm thấy chi tiết cuộc hẹn cho Vet ID đã cung cấp.",
                        Data = null
                    };
                }
                var responseData = _mapper.Map<List<AppointmentDetailResponseDTO>>(appointmentDetails);
                return new BaseResponse<AppointmentDetailResponseDTO>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy chi tiết cuộc hẹn theo Vet ID thành công.",
                    Data = responseData.FirstOrDefault() // Assuming you want the first detail if multiple exist
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting appointment detail by vet ID.");
                return new BaseResponse<AppointmentDetailResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã xảy ra lỗi khi lấy chi tiết cuộc hẹn theo Vet ID.",
                    Data = null
                };
            }
        }

        public Task<BaseResponse<AppointmentDetailResponseDTO>> UpdateAppointmentDetail(int appointmentDetailId, UpdateAppointmentDetailDTO updateAppointmentDetailDTO, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
