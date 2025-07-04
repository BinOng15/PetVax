using AutoMapper;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Net.payOS.Types;
using PetVax.BusinessObjects.DTO;
using PetVax.BusinessObjects.DTO.PaymentDTO;
using PetVax.BusinessObjects.Enum;
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
    public class PaymentService : IPaymentService
    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IAppointmentDetailRepository _appointmentDetailRepository;
        private readonly IVaccineBatchRepository _vaccineBatchRepository;
        private readonly IMicrochipRepository _microchipRepository;
        private readonly IVaccinationCertificateRepository _vaccinationCertificateRepository;
        private readonly IHealthConditionRepository _healthConditionRepository;
        private readonly PayOsService _payOsService;
        private readonly ILogger<PaymentService> _logger;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PaymentService(
            IPaymentRepository paymentRepository,
            IAppointmentDetailRepository appointmentDetailRepository,
            IVaccineBatchRepository vaccineBatchRepository,
            IMicrochipRepository microchipRepository,
            IVaccinationCertificateRepository vaccinationCertificateRepository,
            IHealthConditionRepository healthConditionRepository,
            PayOsService payOsService,
            ILogger<PaymentService> logger,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            _paymentRepository = paymentRepository;
            _appointmentDetailRepository = appointmentDetailRepository;
            _vaccineBatchRepository = vaccineBatchRepository;
            _microchipRepository = microchipRepository;
            _vaccinationCertificateRepository = vaccinationCertificateRepository;
            _healthConditionRepository = healthConditionRepository;
            _payOsService = payOsService;
            _logger = logger;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<BaseResponse<PaymentResponseDTO>> CreatePaymentAsync(CreatePaymentRequestDTO createPaymentRequest, CancellationToken cancellationToken)
        {
            if (createPaymentRequest == null)
            {
                _logger.LogError("CreatePaymentAsync: createPaymentRequest is null");
                return new BaseResponse<PaymentResponseDTO>
                {
                    Code = 400,
                    Success = false,
                    Message = "Dữ liệu thanh toán không hợp lệ, vui lòng thử lại!",
                    Data = default!
                };
            }
            try
            {
                var appointmentDetail = await _appointmentDetailRepository.GetAppointmentDetailByIdAsync(createPaymentRequest.AppointmentDetailId, cancellationToken);
                if (appointmentDetail == null)
                {
                    _logger.LogError("CreatePaymentAsync: Appointment detail not found for ID {AppointmentDetailId}", createPaymentRequest.AppointmentDetailId);
                    return new BaseResponse<PaymentResponseDTO>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Chi tiết cuộc hẹn không tồn tại, vui lòng kiểm tra lại!",
                        Data = default!
                    };
                }

                var payment = _mapper.Map<Payment>(createPaymentRequest);
                decimal amount = 0;

                if (appointmentDetail.VaccineBatchId.HasValue)
                {
                    var vaccineBatch = await _vaccineBatchRepository.GetVaccineBatchByIdAsync(appointmentDetail.VaccineBatchId.Value, cancellationToken);
                    if (vaccineBatch == null || vaccineBatch.Vaccine == null)
                    {
                        _logger.LogError("CreatePaymentAsync: Vaccine batch not found for ID {VaccineBatchId}", appointmentDetail.VaccineBatchId);
                        return new BaseResponse<PaymentResponseDTO>
                        {
                            Code = 404,
                            Success = false,
                            Message = "Lô vắc xin không tồn tại, vui lòng kiểm tra lại!",
                            Data = default!
                        };
                    }
                    amount = vaccineBatch.Vaccine.Price;
                    payment.VaccineBatchId = vaccineBatch.VaccineBatchId;
                }
                else if (appointmentDetail.MicrochipItemId.HasValue)
                {
                    var microchipItem = appointmentDetail.MicrochipItem;
                    if (microchipItem == null)
                    {
                        _logger.LogError("CreatePaymentAsync: MicrochipItem not found for ID {MicrochipItemId}", appointmentDetail.MicrochipItemId);
                        return new BaseResponse<PaymentResponseDTO>
                        {
                            Code = 404,
                            Success = false,
                            Message = "Vi mạch không tồn tại, vui lòng kiểm tra lại!",
                            Data = default!
                        };
                    }
                    var microchip = await _microchipRepository.GetMicrochipByIdAsync(microchipItem.MicrochipId, cancellationToken);
                    if (microchip == null)
                    {
                        _logger.LogError("CreatePaymentAsync: Microchip not found for ID {MicrochipId}", microchipItem.MicrochipId);
                        return new BaseResponse<PaymentResponseDTO>
                        {
                            Code = 404,
                            Success = false,
                            Message = "Vi mạch không tồn tại, vui lòng kiểm tra lại!",
                            Data = default!
                        };
                    }
                    amount = microchip.Price;
                    payment.MicrochipId = microchipItem.MicrochipId;
                }
                else if (appointmentDetail.VaccinationCertificateId.HasValue)
                {
                    var certificate = await _vaccinationCertificateRepository.GetVaccinationCertificateByIdAsync(appointmentDetail.VaccinationCertificateId.Value, cancellationToken);
                    if (certificate == null)
                    {
                        _logger.LogError("CreatePaymentAsync: Vaccination certificate not found for ID {VaccinationCertificateId}", appointmentDetail.VaccinationCertificateId);
                        return new BaseResponse<PaymentResponseDTO>
                        {
                            Code = 404,
                            Success = false,
                            Message = "Chứng nhận tiêm chủng không tồn tại, vui lòng kiểm tra lại!",
                            Data = default!
                        };
                    }
                    amount = certificate.Price;
                    payment.VaccinationCertificateId = certificate.CertificateId;
                }
                else if (appointmentDetail.HealthConditionId.HasValue)
                {
                    var healthCondition = await _healthConditionRepository.GetHealthConditionByIdAsync(appointmentDetail.HealthConditionId.Value, cancellationToken);
                    if (healthCondition == null)
                    {
                        _logger.LogError("CreatePaymentAsync: Health condition not found for ID {HealthConditionId}", appointmentDetail.HealthConditionId);
                        return new BaseResponse<PaymentResponseDTO>
                        {
                            Code = 404,
                            Success = false,
                            Message = "Tình trạng sức khỏe không tồn tại, vui lòng kiểm tra lại!",
                            Data = default!
                        };
                    }
                    amount = healthCondition.Price;
                    payment.HealthConditionId = healthCondition.HealthConditionId;
                }
                else
                {
                    _logger.LogError("CreatePaymentAsync: No valid payment items found for AppointmentDetailId {AppointmentDetailId}", createPaymentRequest.AppointmentDetailId);
                    return new BaseResponse<PaymentResponseDTO>
                    {
                        Code = 400,
                        Success = false,
                        Message = "Không có mục thanh toán hợp lệ, vui lòng kiểm tra lại!",
                        Data = default!
                    };
                }

                if (amount < 0.01m)
                {
                    _logger.LogError("CreatePaymentAsync: Payment amount must be greater than or equal to 0.01 for AppointmentDetailId {AppointmentDetailId}", createPaymentRequest.AppointmentDetailId);
                    return new BaseResponse<PaymentResponseDTO>
                    {
                        Code = 400,
                        Success = false,
                        Message = "Số tiền thanh toán phải lớn hơn hoặc bằng 0.01, vui lòng kiểm tra lại!",
                        Data = default!
                    };
                }

                payment.Amount = amount;
                payment.PaymentCode = $"PAY{DateTime.UtcNow:yyyyMMddHHmmssfff}{Guid.NewGuid().ToString("N")[..6]}";
                payment.PaymentDate = DateTimeHelper.Now();
                payment.CreatedAt = DateTimeHelper.Now();
                payment.CreatedBy = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "system";

                string? paymentUrl = null;
                if (createPaymentRequest.PaymentMethod == EnumList.PaymentMethod.BankTransfer)
                {
                    payment.PaymentStatus = EnumList.PaymentStatus.Pending;
                    var paymentLink = await GeneratePaymentLinkForAppointmentDetailAsync(createPaymentRequest.AppointmentDetailId, cancellationToken);
                    payment.CheckoutUrl = paymentLink;
                }

                else if (createPaymentRequest.PaymentMethod == EnumList.PaymentMethod.Cash)
                {
                    payment.PaymentStatus = EnumList.PaymentStatus.Completed;
                    // Optionally, update appointment status to Paid
                    appointmentDetail.AppointmentStatus = EnumList.AppointmentStatus.Paid;
                    await _appointmentDetailRepository.UpdateAppointmentDetailAsync(appointmentDetail, cancellationToken);
                }

                var result = await _paymentRepository.AddPaymentAsync(payment, cancellationToken);
                if (result <= 0)
                {
                    _logger.LogError("CreatePaymentAsync: Failed to add payment to database");
                    return new BaseResponse<PaymentResponseDTO>
                    {
                        Code = 500,
                        Success = false,
                        Message = "Không thể tạo thanh toán, vui lòng thử lại sau!",
                        Data = default!
                    };
                }

                var paymentResponse = _mapper.Map<PaymentResponseDTO>(payment);
                return new BaseResponse<PaymentResponseDTO>
                {
                    Code = 201,
                    Success = true,
                    Message = "Tạo thanh toán thành công!",
                    Data = paymentResponse
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CreatePaymentAsync: An error occurred while creating payment");
                return new BaseResponse<PaymentResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã xảy ra lỗi khi tạo thanh toán, vui lòng thử lại sau!",
                    Data = default!
                };
            }
        }

        public async Task<BaseResponse<PaymentResponseDTO>> DeletePaymentByIdAsync(int paymentId, CancellationToken cancellationToken)
        {
            var payment = await _paymentRepository.GetPaymentByIdAsync(paymentId, cancellationToken);
            if (payment == null)
            {
                _logger.LogError("DeletePaymentByIdAsync: Payment not found for ID {PaymentId}", paymentId);
                return new BaseResponse<PaymentResponseDTO>
                {
                    Code = 404,
                    Success = false,
                    Message = "Thanh toán không tồn tại, vui lòng kiểm tra lại!",
                    Data = null
                };
            }
            payment.isDeleted = true;
            var result = await _paymentRepository.UpdatePaymentAsync(payment, cancellationToken);
            if (result <= 0)
            {
                _logger.LogError("DeletePaymentByIdAsync: Failed to delete payment with ID {PaymentId}", paymentId);
                return new BaseResponse<PaymentResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Không thể xóa thanh toán, vui lòng thử lại sau!",
                    Data = null
                };
            }
            var paymentResponse = _mapper.Map<PaymentResponseDTO>(payment);
            return new BaseResponse<PaymentResponseDTO>
            {
                Code = 200,
                Success = true,
                Message = "Xóa thanh toán thành công!",
                Data = paymentResponse
            };
        }

        public async Task<DynamicResponse<PaymentResponseDTO>> GetAllPaymentsAsync(GetAllItemsDTO getAllItemsRequest, CancellationToken cancellationToken)
        {
            try
            {
                var payments = await _paymentRepository.GetAllPaymentsAsync(cancellationToken);
                payments = payments
                    .Where(p => p.isDeleted == false || p.isDeleted == null)
                    .ToList();

                if (!string.IsNullOrWhiteSpace(getAllItemsRequest.KeyWord))
                {
                    var keyword = getAllItemsRequest.KeyWord.ToLower();
                    payments = payments
                        .Where(p =>
                        (p.PaymentCode != null && p.PaymentCode.ToLower().Contains(keyword)) ||
                        (p.Customer != null && p.Customer.FullName != null && p.Customer.FullName.ToLower().Contains(keyword)) ||
                        (p.AppointmentDetail != null && p.AppointmentDetail.AppointmentDetailCode != null && p.AppointmentDetail.AppointmentDetailCode.ToLower().Contains(keyword)) ||
                        (p.VaccineBatch != null && p.VaccineBatch.Vaccine.Name != null && p.VaccineBatch.Vaccine.Name.ToLower().Contains(keyword)) ||
                        (p.VaccineBatch != null && p.VaccineBatch.Vaccine.VaccineCode != null && p.VaccineBatch.Vaccine.VaccineCode.ToLower().Contains(keyword)))
                        .ToList();
                }
                int pageNumber = getAllItemsRequest?.PageNumber > 0 ? getAllItemsRequest.PageNumber : 1;
                int pageSize = getAllItemsRequest?.PageSize > 0 ? getAllItemsRequest.PageSize : 10;
                int skip = (pageNumber - 1) * pageSize;
                int totalItem = payments.Count;
                int totalPage = (int)Math.Ceiling((double)totalItem / pageSize);
                var pagedPayments = payments
                    .Skip(skip)
                    .Take(pageSize)
                    .Select(p => _mapper.Map<PaymentResponseDTO>(p))
                    .ToList();
                var response = new MegaData<PaymentResponseDTO>
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
                        keyWord = getAllItemsRequest?.KeyWord,
                        status = getAllItemsRequest?.Status,
                    },
                    PageData = _mapper.Map<List<PaymentResponseDTO>>(pagedPayments)
                };
                if (!pagedPayments.Any())
                {
                    _logger.LogInformation("GetAllPaymentsAsync: No payments found");
                    return new DynamicResponse<PaymentResponseDTO>
                    {
                        Code = 200,
                        Success = false,
                        Message = "Không tìm thấy thanh toán nào!",
                        Data = null
                    };
                }
                _logger.LogInformation("GetAllPaymentsAsync: Successfully retrieved {Count} payments", pagedPayments.Count);
                return new DynamicResponse<PaymentResponseDTO>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy danh sách thanh toán thành công!",
                    Data = response
                };

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetAllPaymentsAsync: An error occurred while retrieving payments");
                return new DynamicResponse<PaymentResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã xảy ra lỗi khi lấy danh sách thanh toán, vui lòng thử lại sau!",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<PaymentResponseDTO>> GetPaymentByIdAsync(int paymentId, CancellationToken cancellationToken)
        {
            var payment = await _paymentRepository.GetPaymentByIdAsync(paymentId, cancellationToken);
            if (payment == null)
            {
                _logger.LogError("GetPaymentByIdAsync: Payment not found for ID {PaymentId}", paymentId);
                return new BaseResponse<PaymentResponseDTO>
                {
                    Code = 200,
                    Success = false,
                    Message = "Thông tin thanh toán không tồn tại, vui lòng kiểm tra lại!",
                    Data = null
                };
            }
            var paymentResponse = _mapper.Map<PaymentResponseDTO>(payment);
            return new BaseResponse<PaymentResponseDTO>
            {
                Code = 200,
                Success = true,
                Message = "Lấy thông tin thanh toán thành công!",
                Data = paymentResponse
            };
        }

        public async Task<BaseResponse<List<PaymentResponseDTO>>> GetPaymentsByAppointmentDetailIdAsync(int appointmentDetailId, CancellationToken cancellationToken)
        {
            var payments = await _paymentRepository.GetPaymentsByAppointmentIdAsync(appointmentDetailId, cancellationToken);
            if (payments == null || !payments.Any())
            {
                _logger.LogError("GetPaymentsByAppointmentDetailIdAsync: No payments found for AppointmentDetailId {AppointmentDetailId}", appointmentDetailId);
                return new BaseResponse<List<PaymentResponseDTO>>
                {
                    Code = 404,
                    Success = false,
                    Message = "Không tìm thấy thông tin thanh toán nào cho chi tiết cuộc hẹn này!",
                    Data = null
                };
            }
            var paymentResponses = payments.Select(p => _mapper.Map<PaymentResponseDTO>(p)).ToList();
            return new BaseResponse<List<PaymentResponseDTO>>
            {
                Code = 200,
                Success = true,
                Message = "Lấy danh sách thanh toán thành công!",
                Data = paymentResponses
            };
        }

        public async Task<BaseResponse<List<PaymentResponseDTO>>> GetPaymentsByCustomerIdAsync(int customerId, CancellationToken cancellationToken)
        {
            var payments = await _paymentRepository.GetPaymentsByAccountIdAsync(customerId, cancellationToken);
            if (payments == null || !payments.Any())
            {
                _logger.LogError("GetPaymentsByCustomerIdAsync: No payments found for CustomerId {CustomerId}", customerId);
                return new BaseResponse<List<PaymentResponseDTO>>
                {
                    Code = 404,
                    Success = false,
                    Message = "Không tìm thấy thông tin thanh toán nào cho khách hàng này!",
                    Data = null
                };
            }
            var paymentResponses = payments.Select(p => _mapper.Map<PaymentResponseDTO>(p)).ToList();
            return new BaseResponse<List<PaymentResponseDTO>>
            {
                Code = 200,
                Success = true,
                Message = "Lấy danh sách thanh toán thành công!",
                Data = paymentResponses
            };
        }

        public async Task<BaseResponse<PaymentResponseDTO>> UpdatePaymentAsync(int paymentId, UpdatePaymentRequestDTO updatePaymentRequest, CancellationToken cancellationToken)
        {
            if (updatePaymentRequest == null)
            {
                _logger.LogError("UpdatePaymentAsync: updatePaymentRequest is null");
                return new BaseResponse<PaymentResponseDTO>
                {
                    Code = 400,
                    Success = false,
                    Message = "Dữ liệu cập nhật thanh toán không hợp lệ, vui lòng thử lại!",
                    Data = null
                };
            }
            var payment = await _paymentRepository.GetPaymentByIdAsync(paymentId, cancellationToken);
            if (payment == null)
            {
                _logger.LogError("UpdatePaymentAsync: Payment not found for ID {PaymentId}", paymentId);
                return new BaseResponse<PaymentResponseDTO>
                {
                    Code = 404,
                    Success = false,
                    Message = "Thanh toán không tồn tại, vui lòng kiểm tra lại!",
                    Data = null
                };
            }
            // Cập nhật các trường cần thiết từ updatePaymentRequest
            payment.PaymentStatus = updatePaymentRequest.PaymentStatus ?? payment.PaymentStatus;
            var result = await _paymentRepository.UpdatePaymentAsync(payment, cancellationToken);
            if (result <= 0)
            {
                _logger.LogError("UpdatePaymentAsync: Failed to update payment with ID {PaymentId}", paymentId);
                return new BaseResponse<PaymentResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Không thể cập nhật thanh toán, vui lòng thử lại sau!",
                    Data = null
                };
            }
            var paymentResponse = _mapper.Map<PaymentResponseDTO>(payment);
            return new BaseResponse<PaymentResponseDTO>
            {
                Code = 200,
                Success = true,
                Message = "Cập nhật thanh toán thành công!",
                Data = paymentResponse
            };
        }
        public async Task<string> GeneratePaymentLinkForAppointmentDetailAsync(int appointmentDetailId, CancellationToken cancellationToken)
        {
            var appointmentDetail = await _appointmentDetailRepository.GetAppointmentDetailByIdAsync(appointmentDetailId, cancellationToken);
            if (appointmentDetail == null)
            {
                _logger.LogError("GeneratePaymentLinkForAppointmentDetailAsync: Appointment detail not found for ID {AppointmentDetailId}", appointmentDetailId);
                return null;
            }

            // Tạo một CreatePaymentRequestDTO giả định để truyền vào hàm tính amount
            var createPaymentRequest = new CreatePaymentRequestDTO
            {
                AppointmentDetailId = appointmentDetail.AppointmentDetailId,
                VaccineBatchId = appointmentDetail.VaccineBatchId,
                MicrochipId = appointmentDetail.MicrochipItem?.Microchip?.MicrochipId,
                VaccinationCertificateId = appointmentDetail.VaccinationCertificate?.CertificateId,
                HealthConditionId = appointmentDetail.HealthCondition?.HealthConditionId,
                // Có thể bổ sung các trường khác nếu cần thiết
            };

            decimal amount = 0;

            if (appointmentDetail.VaccineBatchId.HasValue)
            {
                var vaccineBatch = await _vaccineBatchRepository.GetVaccineBatchByIdAsync(appointmentDetail.VaccineBatchId.Value, cancellationToken);
                amount = vaccineBatch.Vaccine.Price;
            }
            else if (appointmentDetail.MicrochipItemId.HasValue)
            {
                var microchipItem = appointmentDetail.MicrochipItem;

                var microchip = await _microchipRepository.GetMicrochipByIdAsync(microchipItem.MicrochipId, cancellationToken);

                amount = microchip.Price;
            }
            else if (appointmentDetail.VaccinationCertificateId.HasValue)
            {
                var certificate = await _vaccinationCertificateRepository.GetVaccinationCertificateByIdAsync(appointmentDetail.VaccinationCertificateId.Value, cancellationToken);
                amount = certificate.Price;
            }
            else if (appointmentDetail.HealthConditionId.HasValue)
            {
                var healthCondition = await _healthConditionRepository.GetHealthConditionByIdAsync(appointmentDetail.HealthConditionId.Value, cancellationToken);
                amount = healthCondition.Price;
            }

            var paymentData = new PaymentData(
                orderCode: (long)appointmentDetail.AppointmentDetailId,
                amount: (int)amount,
                description: $"VaxPet #{appointmentDetail.AppointmentDetailCode}",
                items: new List<ItemData>(),
                cancelUrl: "http://localhost:5173/staff/vaccination-appointments/cancel",
                returnUrl: "http://localhost:5173/staff/vaccination-appointments/success"
            );

            _logger.LogInformation("Amount: {amount}",
                        amount);
                var paymentLink = await _payOsService.CreatePaymentLink(paymentData);
            _logger.LogInformation("PayOs checkoutUrl: {CheckoutUrl}", paymentLink.checkoutUrl);
            return paymentLink.checkoutUrl;
        }
        
        public async Task HandlePayOsCallBackAsync(PaymentCallBackDTO paymentCallBackDTO, CancellationToken cancellationToken)
        {
            var payment = await _paymentRepository.GetPaymentByIdAsync(paymentCallBackDTO.PaymentId, cancellationToken);
            if (payment == null) return;

            if (paymentCallBackDTO.PaymentStatus == EnumList.PaymentStatus.Completed)
            {
                payment.PaymentStatus = EnumList.PaymentStatus.Completed;

                // Update AppointmentDetail status to Paid
                var appointmentDetail = await _appointmentDetailRepository.GetAppointmentDetailByIdAsync(payment.AppointmentDetailId, cancellationToken);
                if (appointmentDetail != null)
                {
                    appointmentDetail.AppointmentStatus = EnumList.AppointmentStatus.Paid;
                    await _appointmentDetailRepository.UpdateAppointmentDetailAsync(appointmentDetail, cancellationToken);

                    // Update Appointment status to Paid if exists
                    if (appointmentDetail.Appointment != null)
                    {
                        appointmentDetail.Appointment.AppointmentStatus = EnumList.AppointmentStatus.Paid;
                        // If you have a repository for Appointment, update it here, e.g.:
                        // await _appointmentRepository.UpdateAppointmentAsync(appointmentDetail.Appointment, cancellationToken);
                    }
                }
            }
            else if (paymentCallBackDTO.PaymentStatus == EnumList.PaymentStatus.Failed)
            {
                payment.PaymentStatus = EnumList.PaymentStatus.Failed;
                // No need to update appointment status
            }

            await _paymentRepository.UpdatePaymentAsync(payment, cancellationToken);
        }
    }
}
