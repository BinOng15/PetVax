using AutoMapper;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
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
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IPointTransactionRepository _pointTransactionRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IMembershipRepository _membershipRepository;
        private readonly IVoucherRepository _voucherRepository;
        private readonly ICustomerVoucherRepository _customerVoucherRepository;
        private readonly PayOsService _payOsService;
        private readonly ILogger<PaymentService> _logger;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly string _mapboxToken;

        public PaymentService(
            IPaymentRepository paymentRepository,
            IAppointmentDetailRepository appointmentDetailRepository,
            IVaccineBatchRepository vaccineBatchRepository,
            IMicrochipRepository microchipRepository,
            IVaccinationCertificateRepository vaccinationCertificateRepository,
            IHealthConditionRepository healthConditionRepository,
            IAppointmentRepository appointmentRepository,
            IPointTransactionRepository pointTransactionRepository,
            ICustomerRepository customerRepository,
            IMembershipRepository membershipRepository,
            IVoucherRepository voucherRepository,
            ICustomerVoucherRepository customerVoucherRepository,
            PayOsService payOsService,
            ILogger<PaymentService> logger,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            IConfiguration configuration
            )
        {
            _paymentRepository = paymentRepository;
            _appointmentDetailRepository = appointmentDetailRepository;
            _vaccineBatchRepository = vaccineBatchRepository;
            _microchipRepository = microchipRepository;
            _vaccinationCertificateRepository = vaccinationCertificateRepository;
            _healthConditionRepository = healthConditionRepository;
            _appointmentRepository = appointmentRepository;
            _pointTransactionRepository = pointTransactionRepository;
            _customerRepository = customerRepository;
            _membershipRepository = membershipRepository;
            _customerVoucherRepository = customerVoucherRepository;
            _voucherRepository = voucherRepository;
            _payOsService = payOsService;
            _logger = logger;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _mapboxToken = configuration["Mapbox:AccessToken"];
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
                    Data = null
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
                        Data = null
                    };
                }

                var payment = _mapper.Map<Payment>(createPaymentRequest);
                decimal amount = 0;

                bool isRabiesVaccine = false;
                string membershipRank = null;

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
                            Data = null
                        };
                    }
                    amount = vaccineBatch.Vaccine.Price;
                    payment.VaccineBatchId = vaccineBatch.VaccineBatchId;

                    // Check if vaccine is rabies (case-insensitive, equals "rabisin" or "vaccine dại cho mèo")
                    if (!string.IsNullOrWhiteSpace(vaccineBatch.Vaccine.Name))
                    {
                        var vaccineName = vaccineBatch.Vaccine.Name.Trim().ToLower();
                        if (vaccineName == "rabisin" || vaccineName == "vaccine dại cho mèo")
                        {
                            isRabiesVaccine = true;
                        }
                    }
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
                            Data = null
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
                            Data = null
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
                            Data = null
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
                            Data = null
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
                        Data = null
                    };
                }

                // Add extra fee for HomeVisit
                var appointment = appointmentDetail.Appointment;
                if (appointment != null && appointment.Location == EnumList.Location.HomeVisit)
                {
                    var clinicAddress = "Trường Đại Học FPT Thành Phố Hồ Chí Minh";
                    var customerAddress = appointment.Address;
                    var clinicCoords = await GetLatLngFromAddressAsync(clinicAddress);
                    var customerCoords = await GetLatLngFromAddressAsync(customerAddress);
                    var distanceKm = await GetDistanceKmAsync(clinicCoords, customerCoords);
                    decimal extraFee = (decimal)distanceKm * 1000;
                    amount += extraFee;
                }

                // Get membership rank if available
                if (appointment != null && appointment.CustomerId > 0)
                {
                    var customer = await _customerRepository.GetCustomerByIdAsync(appointment.CustomerId, cancellationToken);
                    if (customer != null && customer.MembershipId.HasValue)
                    {
                        var membership = await _membershipRepository.GetMembershipByIdAsync(customer.MembershipId.Value, cancellationToken);
                        if (membership != null && !string.IsNullOrWhiteSpace(membership.Rank))
                        {
                            membershipRank = membership.Rank.Trim().ToLower();
                        }
                    }
                }

                // Apply discount logic
                if (!string.IsNullOrEmpty(membershipRank) && membershipRank == "gold" && isRabiesVaccine)
                {
                    amount -= amount * 0.70m;
                }
                else if (membershipRank == "silver")
                {
                    amount -= amount * 0.10m;
                }
                else if (membershipRank == "gold")
                {
                    amount -= amount * 0.20m;
                }

                // Add voucher
                if (createPaymentRequest.VoucherCode != null)
                {
                    var voucher = await _voucherRepository.GetVoucherByCodeAsync(createPaymentRequest.VoucherCode, cancellationToken);
                    if (voucher == null || voucher.isDeleted == true)
                    {
                        _logger.LogError("CreatePaymentAsync: Voucher not found or is deleted for code {VoucherCode}", createPaymentRequest.VoucherCode);
                        return new BaseResponse<PaymentResponseDTO>
                        {
                            Code = 404,
                            Success = false,
                            Message = "Mã giảm giá không hợp lệ hoặc đã bị xóa, vui lòng kiểm tra lại!",
                            Data = null
                        };
                    }
                    if (voucher.ExpirationDate < DateTimeHelper.Now())
                    {
                        _logger.LogError("CreatePaymentAsync: Voucher {VoucherCode} has expired", createPaymentRequest.VoucherCode);
                        return new BaseResponse<PaymentResponseDTO>
                        {
                            Code = 400,
                            Success = false,
                            Message = "Mã giảm giá đã hết hạn, vui lòng kiểm tra lại!",
                            Data = null
                        };
                    }
                    // Check if voucher belongs to customer and status == 1 (Available)
                    var customerVoucher = await _customerVoucherRepository.GetCustomerVoucherByCustomerIdAndVoucherIdAsync(createPaymentRequest.CustomerId, voucher.VoucherId, cancellationToken);
                    if (customerVoucher == null || customerVoucher.Status != EnumList.VoucherStatus.Available)
                    {
                        _logger.LogError("CreatePaymentAsync: Voucher {VoucherCode} does not belong to customer {CustomerId} or is not available", createPaymentRequest.VoucherCode, createPaymentRequest.CustomerId);
                        return new BaseResponse<PaymentResponseDTO>
                        {
                            Code = 400,
                            Success = false,
                            Message = "Mã giảm giá không thuộc về khách hàng này hoặc không còn hiệu lực, vui lòng kiểm tra lại!",
                            Data = null
                        };
                    }
                    var discountPercent = voucher.DiscountAmount;
                    var discountAmount = amount * (discountPercent / 100m);
                    amount -= discountAmount;
                    payment.VoucherCode = createPaymentRequest.VoucherCode;
                }

                if (amount < 0.01m)
                {
                    _logger.LogError("CreatePaymentAsync: Payment amount must be greater than or equal to 0.01 for AppointmentDetailId {AppointmentDetailId}", createPaymentRequest.AppointmentDetailId);
                    return new BaseResponse<PaymentResponseDTO>
                    {
                        Code = 400,
                        Success = false,
                        Message = "Số tiền thanh toán phải lớn hơn hoặc bằng 0.01, vui lòng kiểm tra lại!",
                        Data = null
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
                    var paymentLink = await GeneratePaymentLinkForAppointmentDetailAsync(createPaymentRequest.AppointmentDetailId, amount, cancellationToken);
                    payment.CheckoutUrl = paymentLink.paymentLink;
                    payment.QRCode = paymentLink.qrCode;
                }
                else if (createPaymentRequest.PaymentMethod == EnumList.PaymentMethod.Cash)
                {
                    payment.PaymentStatus = EnumList.PaymentStatus.Pending;
                    payment.CheckoutUrl = string.Empty;
                    payment.QRCode = string.Empty;
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
                        Data = null
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
                    Data = null
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
                        Success = true,
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
            var payments = await _paymentRepository.GetPaymentByAppointmentDetailIdAsync(appointmentDetailId, cancellationToken);
            if (payments == null)
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

            // Only allow status update from Pending (1) to Completed (2) or Failed (3)
            if (updatePaymentRequest.PaymentStatus.HasValue &&
                payment.PaymentStatus == EnumList.PaymentStatus.Pending &&
                (updatePaymentRequest.PaymentStatus == EnumList.PaymentStatus.Completed || updatePaymentRequest.PaymentStatus == EnumList.PaymentStatus.Failed))
            {
                if (updatePaymentRequest.PaymentStatus == EnumList.PaymentStatus.Completed)
                {
                    payment.PaymentStatus = EnumList.PaymentStatus.Completed;
                    // Update AppointmentDetail and Appointment status to Paid
                    var appointmentDetail = await _appointmentDetailRepository.GetAppointmentDetailByIdAsync(payment.AppointmentDetailId, cancellationToken);
                    if (appointmentDetail != null)
                    {
                        appointmentDetail.AppointmentStatus = EnumList.AppointmentStatus.Paid;
                        await _appointmentDetailRepository.UpdateAppointmentDetailAsync(appointmentDetail, cancellationToken);
                    }
                    var appointment = appointmentDetail != null
                        ? await _appointmentRepository.GetAppointmentByIdAsync(appointmentDetail.AppointmentId, cancellationToken)
                        : null;
                    if (appointment != null)
                    {
                        appointment.AppointmentStatus = EnumList.AppointmentStatus.Paid;
                        await _appointmentRepository.UpdateAppointmentAsync(appointment, cancellationToken);
                    }

                    // --- Begin: Update Customer's totalSpent, CurrentPoints, create PointTransaction, check Membership ---
                    var customer = payment.Customer;
                    if (customer != null)
                    {
                        decimal originalAmount = 0m;
                        if (payment.VaccineBatchId.HasValue)
                        {
                            var vaccineBatch = await _vaccineBatchRepository.GetVaccineBatchByIdAsync(payment.VaccineBatchId.Value, cancellationToken);
                            if (vaccineBatch != null && vaccineBatch.Vaccine != null)
                                originalAmount = vaccineBatch.Vaccine.Price;
                        }
                        else if (payment.MicrochipId.HasValue)
                        {
                            var microchip = await _microchipRepository.GetMicrochipByIdAsync(payment.MicrochipId.Value, cancellationToken);
                            if (microchip != null)
                                originalAmount = microchip.Price;
                        }
                        else if (payment.VaccinationCertificateId.HasValue)
                        {
                            var certificate = await _vaccinationCertificateRepository.GetVaccinationCertificateByIdAsync(payment.VaccinationCertificateId.Value, cancellationToken);
                            if (certificate != null)
                                originalAmount = certificate.Price;
                        }
                        else if (payment.HealthConditionId.HasValue)
                        {
                            var healthCondition = await _healthConditionRepository.GetHealthConditionByIdAsync(payment.HealthConditionId.Value, cancellationToken);
                            if (healthCondition != null)
                                originalAmount = healthCondition.Price;
                        }

                        if (appointment != null && appointment.Location == EnumList.Location.HomeVisit)
                        {
                            var clinicAddress = "Trường Đại Học FPT Thành Phố Hồ Chí Minh";
                            var customerAddress = appointment.Address;
                            var clinicCoords = await GetLatLngFromAddressAsync(clinicAddress);
                            var customerCoords = await GetLatLngFromAddressAsync(customerAddress);
                            var distanceKm = await GetDistanceKmAsync(clinicCoords, customerCoords);
                            decimal extraFee = (decimal)distanceKm * 1000;
                            originalAmount += extraFee;
                        }

                        if (customer.TotalSpent == null) customer.TotalSpent = 0;
                        customer.TotalSpent += originalAmount;

                        int earnedPoints = (int)(originalAmount / 10000m);
                        if (customer.CurrentPoints == null) customer.CurrentPoints = 0;
                        customer.CurrentPoints += earnedPoints;
                        customer.RedeemablePoints = customer.CurrentPoints;

                        var pointTransaction = new PointTransaction
                        {
                            CustomerId = customer.CustomerId,
                            Change = earnedPoints.ToString(),
                            TransactionType = "Earned",
                            TransactionDate = DateTimeHelper.Now(),
                            Description = $"Tích điểm từ thanh toán {payment.PaymentCode}",
                            CreatedAt = DateTimeHelper.Now(),
                            CreatedBy = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "system",
                        };
                        await _pointTransactionRepository.CreatePointTransactionAsync(pointTransaction, cancellationToken);

                        var memberships = await _membershipRepository.GetAllMembershipsAsync(cancellationToken);
                        var eligibleMembership = memberships
                            .Where(m => m.MinPoints <= customer.CurrentPoints)
                            .OrderByDescending(m => m.MinPoints)
                            .FirstOrDefault();
                        if (eligibleMembership != null && (customer.MembershipId != eligibleMembership.MembershipId))
                        {
                            customer.MembershipId = eligibleMembership.MembershipId;
                        }
                        var updatedCustomer = _mapper.Map<Customer>(customer);
                        await _customerRepository.UpdateCustomerAsync(updatedCustomer, cancellationToken);
                    }
                    // --- End: Update Customer's totalSpent, CurrentPoints, create PointTransaction, check Membership ---
                }
                else if (updatePaymentRequest.PaymentStatus == EnumList.PaymentStatus.Failed)
                {
                    payment.PaymentStatus = EnumList.PaymentStatus.Failed;
                    var appointmentDetail = await _appointmentDetailRepository.GetAppointmentDetailByIdAsync(payment.AppointmentDetailId, cancellationToken);
                    if (appointmentDetail != null)
                    {
                        appointmentDetail.AppointmentStatus = appointmentDetail.AppointmentStatus;
                        await _appointmentDetailRepository.UpdateAppointmentDetailAsync(appointmentDetail, cancellationToken);
                    }
                    var appointment = appointmentDetail != null
                        ? await _appointmentRepository.GetAppointmentByIdAsync(appointmentDetail.AppointmentId, cancellationToken)
                        : null;
                    if (appointment != null)
                    {
                        appointment.AppointmentStatus = appointment.AppointmentStatus;
                        await _appointmentRepository.UpdateAppointmentAsync(appointment, cancellationToken);
                    }
                }
            }
            else if (updatePaymentRequest.PaymentStatus.HasValue)
            {
                // Not allowed to update to other statuses or from non-pending
                _logger.LogWarning("UpdatePaymentAsync: Invalid status transition from {CurrentStatus} to {NewStatus} for PaymentId {PaymentId}",
                    payment.PaymentStatus, updatePaymentRequest.PaymentStatus, paymentId);
                return new BaseResponse<PaymentResponseDTO>
                {
                    Code = 400,
                    Success = false,
                    Message = "Chỉ cho phép cập nhật trạng thái thanh toán từ 'Pending - 1' sang 'Completed - 2' hoặc 'Failed - 3'!",
                    Data = null
                };
            }

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
        public async Task<(string paymentLink, string qrCode)> GeneratePaymentLinkForAppointmentDetailAsync(int appointmentDetailId, decimal amount, CancellationToken cancellationToken)
        {
            var appointmentDetail = await _appointmentDetailRepository.GetAppointmentDetailByIdAsync(appointmentDetailId, cancellationToken);
            if (appointmentDetail == null)
            {
                _logger.LogError("GeneratePaymentLinkForAppointmentDetailAsync: Appointment detail not found for ID {AppointmentDetailId}", appointmentDetailId);
                return (null, null);
            }

            var payment = await _paymentRepository.GetPaymentByAppointmentDetailIdAsync(appointmentDetailId, cancellationToken);

            var paymentData = new PaymentData(
                orderCode: (long)appointmentDetail.AppointmentDetailId,
                amount: (int)amount,
                description: $"VaxPet #{appointmentDetail.AppointmentDetailCode}",
                items: new List<ItemData>(),
                cancelUrl: "https://sep490-pvsm.vercel.app/staff/vaccination-appointments/cancel",
                returnUrl: "https://sep490-pvsm.vercel.app/staff/vaccination-appointments/success"
            );

            _logger.LogInformation("Amount: {amount}", amount);
            var paymentLink = await _payOsService.CreatePaymentLink(paymentData);
            _logger.LogInformation("PayOs checkoutUrl: {CheckoutUrl}", paymentLink.checkoutUrl);
            _logger.LogInformation("PayOs QRCode: {QRCode}", paymentLink.qrCode);
            return (paymentLink.checkoutUrl, paymentLink.qrCode);
        }

        public async Task<BaseResponse<PaymentResponseDTO>> HandlePayOsCallBackAsync(PaymentCallBackDTO paymentCallBackDTO, CancellationToken cancellationToken)
        {
            try
            {
                var payment = await _paymentRepository.GetPaymentByIdAsync(paymentCallBackDTO.PaymentId, cancellationToken);
                if (payment == null)
                {
                    _logger.LogError("HandlePayOsCallBackAsync: Payment not found for ID {PaymentId}", paymentCallBackDTO.PaymentId);
                    return new BaseResponse<PaymentResponseDTO>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Thông tin thanh toán không tồn tại, vui lòng kiểm tra lại!",
                        Data = null
                    };
                }

                // Only allow status update from Pending (1) to Completed (2) or Failed (3)
                if (payment.PaymentStatus != EnumList.PaymentStatus.Pending ||
                    (paymentCallBackDTO.PaymentStatus != EnumList.PaymentStatus.Completed && paymentCallBackDTO.PaymentStatus != EnumList.PaymentStatus.Failed))
                {
                    _logger.LogWarning("HandlePayOsCallBackAsync: Invalid status transition from {CurrentStatus} to {NewStatus} for PaymentId {PaymentId}",
                        payment.PaymentStatus, paymentCallBackDTO.PaymentStatus, paymentCallBackDTO.PaymentId);
                    return new BaseResponse<PaymentResponseDTO>
                    {
                        Code = 400,
                        Success = false,
                        Message = "Chỉ cho phép cập nhật trạng thái thanh toán từ 'Pending - 1' sang 'Completed - 2' hoặc 'Failed - 3'!",
                        Data = null
                    };
                }

                if (paymentCallBackDTO.PaymentStatus == EnumList.PaymentStatus.Completed)
                {
                    payment.PaymentStatus = EnumList.PaymentStatus.Completed;
                    // Update AppointmentDetail and Appointment status to Paid
                    var appointmentDetail = await _appointmentDetailRepository.GetAppointmentDetailByIdAsync(payment.AppointmentDetailId, cancellationToken);
                    if (appointmentDetail != null)
                    {
                        appointmentDetail.AppointmentStatus = EnumList.AppointmentStatus.Paid;
                        await _appointmentDetailRepository.UpdateAppointmentDetailAsync(appointmentDetail, cancellationToken);
                    }
                    var appointment = await _appointmentRepository.GetAppointmentByIdAsync(appointmentDetail.AppointmentId, cancellationToken);
                    if (appointment != null)
                    {
                        appointment.AppointmentStatus = EnumList.AppointmentStatus.Paid;
                        await _appointmentRepository.UpdateAppointmentAsync(appointment, cancellationToken);
                    }

                    // --- Begin: Update Customer's totalSpent, CurrentPoints, create PointTransaction, check Membership ---
                    // Get customer from payment
                    var customer = payment.Customer;
                    if (customer != null)
                    {
                        // Tính số tiền gốc trước giảm giá
                        decimal originalAmount = 0m;
                        if (payment.VaccineBatchId.HasValue)
                        {
                            var vaccineBatch = await _vaccineBatchRepository.GetVaccineBatchByIdAsync(payment.VaccineBatchId.Value, cancellationToken);
                            if (vaccineBatch != null && vaccineBatch.Vaccine != null)
                                originalAmount = vaccineBatch.Vaccine.Price;
                        }
                        else if (payment.MicrochipId.HasValue)
                        {
                            var microchip = await _microchipRepository.GetMicrochipByIdAsync(payment.MicrochipId.Value, cancellationToken);
                            if (microchip != null)
                                originalAmount = microchip.Price;
                        }
                        else if (payment.VaccinationCertificateId.HasValue)
                        {
                            var certificate = await _vaccinationCertificateRepository.GetVaccinationCertificateByIdAsync(payment.VaccinationCertificateId.Value, cancellationToken);
                            if (certificate != null)
                                originalAmount = certificate.Price;
                        }
                        else if (payment.HealthConditionId.HasValue)
                        {
                            var healthCondition = await _healthConditionRepository.GetHealthConditionByIdAsync(payment.HealthConditionId.Value, cancellationToken);
                            if (healthCondition != null)
                                originalAmount = healthCondition.Price;
                        }

                        // Nếu có phụ phí HomeVisit thì cộng thêm
                        if (appointment != null && appointment.Location == EnumList.Location.HomeVisit)
                        {
                            var clinicAddress = "Trường Đại Học FPT Thành Phố Hồ Chí Minh";
                            var customerAddress = appointment.Address;
                            var clinicCoords = await GetLatLngFromAddressAsync(clinicAddress);
                            var customerCoords = await GetLatLngFromAddressAsync(customerAddress);
                            var distanceKm = await GetDistanceKmAsync(clinicCoords, customerCoords);
                            decimal extraFee = (decimal)distanceKm * 1000;
                            originalAmount += extraFee;
                        }

                        // Update totalSpent
                        if (customer.TotalSpent == null) customer.TotalSpent = 0;
                        customer.TotalSpent += originalAmount;

                        // Calculate points: 1 point per 10,000 VND (dựa trên số tiền gốc)
                        int earnedPoints = (int)(originalAmount / 10000m);
                        if (customer.CurrentPoints == null) customer.CurrentPoints = 0;
                        customer.CurrentPoints += earnedPoints;
                        if (customer.RedeemablePoints == null) customer.RedeemablePoints = 0;
                        customer.RedeemablePoints += earnedPoints;

                        // Create point transaction
                        var pointTransaction = new PointTransaction
                        {
                            CustomerId = customer.CustomerId,
                            PaymentId = payment.PaymentId,
                            Change = earnedPoints.ToString(),
                            TransactionType = "Earned",
                            TransactionDate = DateTimeHelper.Now(),
                            Description = $"Tích điểm từ thanh toán {payment.PaymentCode}",
                            CreatedAt = DateTimeHelper.Now(),
                            CreatedBy = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "system",
                        };
                        // Save point transaction
                        await _pointTransactionRepository.CreatePointTransactionAsync(pointTransaction, cancellationToken);

                        // Check membership upgrade
                        var memberships = await _membershipRepository.GetAllMembershipsAsync(cancellationToken);
                        var eligibleMembership = memberships
                            .Where(m => m.MinPoints <= customer.CurrentPoints)
                            .OrderByDescending(m => m.MinPoints)
                            .FirstOrDefault();
                        if (eligibleMembership != null && (customer.MembershipId != eligibleMembership.MembershipId))
                        {
                            customer.MembershipId = eligibleMembership.MembershipId;
                        }
                        var updatedCustomer = _mapper.Map<Customer>(customer);
                        // Update customer (assume _customerRepository exists)
                        await _customerRepository.UpdateCustomerAsync(updatedCustomer, cancellationToken);
                    }
                    // --- End: Update Customer's totalSpent, CurrentPoints, create PointTransaction, check Membership ---

                    // --- Begin: Update CustomerVoucher status to 2 if voucher was used ---
                    if (!string.IsNullOrEmpty(payment.VoucherCode) && payment.CustomerId > 0)
                    {
                        var voucher = await _voucherRepository.GetVoucherByCodeAsync(payment.VoucherCode, cancellationToken);
                        if (voucher != null)
                        {
                            // Get all CustomerVoucher for this customer and voucher
                            var customerVouchers = await _customerVoucherRepository.GetCustomerVoucherByVoucherIdAsync(voucher.VoucherId, cancellationToken);
                            // Find one with Status == 1 (Available) and CustomerId matches
                            var availableCustomerVoucher = customerVouchers
                                .FirstOrDefault(cv => cv.Status == EnumList.VoucherStatus.Available && cv.CustomerId == payment.CustomerId);
                            if (availableCustomerVoucher != null)
                            {
                                availableCustomerVoucher.Status = EnumList.VoucherStatus.Used; // 2 = Used
                                await _customerVoucherRepository.UpdateCustomerVoucherAsync(availableCustomerVoucher, cancellationToken);
                            }
                        }
                    }
                    // --- End: Update CustomerVoucher status to 2 if voucher was used ---
                }
                else if (paymentCallBackDTO.PaymentStatus == EnumList.PaymentStatus.Failed)
                {
                    payment.PaymentStatus = EnumList.PaymentStatus.Failed;
                    var appointmentDetail = await _appointmentDetailRepository.GetAppointmentDetailByIdAsync(payment.AppointmentDetailId, cancellationToken);
                    if (appointmentDetail != null)
                    {
                        appointmentDetail.AppointmentStatus = appointmentDetail.AppointmentStatus;
                        await _appointmentDetailRepository.UpdateAppointmentDetailAsync(appointmentDetail, cancellationToken);
                    }
                    var appointment = await _appointmentRepository.GetAppointmentByIdAsync(appointmentDetail.AppointmentId, cancellationToken);
                    if (appointment != null)
                    {
                        appointment.AppointmentStatus = appointment.AppointmentStatus;
                        await _appointmentRepository.UpdateAppointmentAsync(appointment, cancellationToken);
                    }
                }

                await _paymentRepository.UpdatePaymentAsync(payment, cancellationToken);

                var paymentResponse = _mapper.Map<PaymentResponseDTO>(payment);
                return new BaseResponse<PaymentResponseDTO>
                {
                    Code = 200,
                    Success = true,
                    Message = "Xử lý callback từ PayOs thành công!",
                    Data = paymentResponse
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "HandlePayOsCallBackAsync: An error occurred while handling PayOs callback");
                return new BaseResponse<PaymentResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã xảy ra lỗi khi xử lý callback từ PayOs, vui lòng thử lại sau!",
                    Data = null
                };
            }
        }

        public async Task<(double lat, double lng)> GetLatLngFromAddressAsync(string address)
        {
            using var client = new HttpClient();
            var url = $"https://api.mapbox.com/geocoding/v5/mapbox.places/{Uri.EscapeDataString(address)}.json?access_token={_mapboxToken}";
            var response = await client.GetAsync(url);
            var json = await response.Content.ReadAsStringAsync();
            dynamic result = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
            var coordinates = result.features[0].geometry.coordinates;
            return ((double)coordinates[1], (double)coordinates[0]); // lat, lng
        }

        public async Task<double> GetDistanceKmAsync((double lat, double lng) origin, (double lat, double lng) destination)
        {
            using var client = new HttpClient();
            var url = $"https://api.mapbox.com/directions/v5/mapbox/driving/{origin.lng},{origin.lat};{destination.lng},{destination.lat}?access_token={_mapboxToken}";
            var response = await client.GetAsync(url);
            var json = await response.Content.ReadAsStringAsync();
            dynamic result = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
            var distanceMeters = result.routes[0].distance;
            return (double)distanceMeters / 1000.0;
        }

        public async Task<BaseResponse<PaymentResponseDTO>> RetryPaymentAsync(RetryPaymentRequestDTO retryPaymentRequest, CancellationToken cancellationToken)
        {
            if (retryPaymentRequest == null)
            {
                _logger.LogError("RetryPaymentAsync: retryPaymentRequest is null");
                return new BaseResponse<PaymentResponseDTO>
                {
                    Code = 400,
                    Success = false,
                    Message = "Dữ liệu yêu cầu thanh toán lại không hợp lệ, vui lòng thử lại!",
                    Data = null
                };
            }

            try
            {
                var appointmentDetail = await _appointmentDetailRepository.GetAppointmentDetailByIdAsync(retryPaymentRequest.AppointmentDetailId, cancellationToken);
                if (appointmentDetail == null)
                {
                    _logger.LogError("RetryPaymentAsync: Appointment detail not found for ID {AppointmentDetailId}", retryPaymentRequest.AppointmentDetailId);
                    return new BaseResponse<PaymentResponseDTO>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Chi tiết cuộc hẹn không tồn tại, vui lòng kiểm tra lại!",
                        Data = null
                    };
                }

                // Lấy payment gần nhất cho appointmentDetail
                var payments = await _paymentRepository.GetPaymentByAppointmentDetailIdAsync(retryPaymentRequest.AppointmentDetailId, cancellationToken);
                var latestPayment = payments?
                    .OrderByDescending(p => p.CreatedAt)
                    .FirstOrDefault();

                if (latestPayment == null || latestPayment.PaymentStatus != EnumList.PaymentStatus.Failed)
                {
                    _logger.LogError("RetryPaymentAsync: Latest payment for AppointmentDetailId {AppointmentDetailId} is not failed", retryPaymentRequest.AppointmentDetailId);
                    return new BaseResponse<PaymentResponseDTO>
                    {
                        Code = 400,
                        Success = false,
                        Message = "Chỉ cho phép tạo lại thanh toán nếu thanh toán gần nhất bị thất bại!",
                        Data = null
                    };
                }

                var newPayment = new Payment
                {
                    AppointmentDetailId = retryPaymentRequest.AppointmentDetailId,
                    CustomerId = appointmentDetail.Appointment.CustomerId,
                    VaccineBatchId = appointmentDetail.VaccineBatchId,
                    MicrochipId = appointmentDetail.MicrochipItem?.MicrochipId,
                    VaccinationCertificateId = appointmentDetail.VaccinationCertificateId,
                    HealthConditionId = appointmentDetail.HealthConditionId,
                    PaymentMethod = retryPaymentRequest.PaymentMethod.ToString(),
                    VoucherCode = retryPaymentRequest.VoucherCode,
                    PaymentStatus = EnumList.PaymentStatus.Pending,
                    isDeleted = false
                };

                newPayment.AppointmentDetail = null;
                newPayment.Customer = null;
                newPayment.VaccineBatch = null;
                newPayment.Microchip = null;
                newPayment.HealthCondition = null;
                newPayment.VaccinationCertificate = null;

                decimal amount = 0;
                if (appointmentDetail.VaccineBatchId.HasValue)
                {
                    var vaccineBatch = await _vaccineBatchRepository.GetVaccineBatchByIdAsync(appointmentDetail.VaccineBatchId.Value, cancellationToken);
                    if (vaccineBatch?.Vaccine != null)
                    {
                        amount = vaccineBatch.Vaccine.Price;
                    }
                }
                else if (appointmentDetail.MicrochipItemId.HasValue && appointmentDetail.MicrochipItem != null)
                {
                    var microchip = await _microchipRepository.GetMicrochipByIdAsync(appointmentDetail.MicrochipItem.MicrochipId, cancellationToken);
                    if (microchip != null)
                    {
                        amount = microchip.Price;
                    }
                }
                else if (appointmentDetail.VaccinationCertificateId.HasValue)
                {
                    var certificate = await _vaccinationCertificateRepository.GetVaccinationCertificateByIdAsync(appointmentDetail.VaccinationCertificateId.Value, cancellationToken);
                    if (certificate != null)
                    {
                        amount = certificate.Price;
                    }
                }
                else if (appointmentDetail.HealthConditionId.HasValue)
                {
                    var healthCondition = await _healthConditionRepository.GetHealthConditionByIdAsync(appointmentDetail.HealthConditionId.Value, cancellationToken);
                    if (healthCondition != null)
                    {
                        amount = healthCondition.Price;
                    }
                }

                if (amount <= 0)
                {
                    return new BaseResponse<PaymentResponseDTO>
                    {
                        Code = 400,
                        Success = false,
                        Message = "Không thể xác định số tiền thanh toán!",
                        Data = null
                    };
                }

                newPayment.Amount = amount;
                newPayment.PaymentCode = $"PAY{DateTime.UtcNow:yyyyMMddHHmmssfff}{Guid.NewGuid().ToString("N")[..6]}";
                newPayment.PaymentDate = DateTimeHelper.Now();
                newPayment.CreatedAt = DateTimeHelper.Now();
                newPayment.CreatedBy = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "system";

                if (retryPaymentRequest.PaymentMethod == EnumList.PaymentMethod.BankTransfer)
                {
                    var paymentLink = await GeneratePaymentLinkForAppointmentDetailAsync(retryPaymentRequest.AppointmentDetailId, amount, cancellationToken);
                    newPayment.CheckoutUrl = paymentLink.paymentLink;
                    newPayment.QRCode = paymentLink.qrCode;
                }
                else
                {
                    newPayment.CheckoutUrl = string.Empty;
                    newPayment.QRCode = string.Empty;
                }

                var newPaymentId = await _paymentRepository.AddPaymentAsync(newPayment, cancellationToken);

                if (newPaymentId <= 0)
                {
                    _logger.LogError("RetryPaymentAsync: Failed to create new payment for AppointmentDetailId {AppointmentDetailId}", retryPaymentRequest.AppointmentDetailId);
                    return new BaseResponse<PaymentResponseDTO>
                    {
                        Code = 500,
                        Success = false,
                        Message = "Không thể thử lại thanh toán, vui lòng kiểm tra lại thông tin và thử lại sau!",
                        Data = null
                    };
                }

                var createdPayment = await _paymentRepository.GetPaymentByIdAsync(newPaymentId, cancellationToken);
                var paymentResponse = _mapper.Map<PaymentResponseDTO>(createdPayment);

                _logger.LogInformation("RetryPaymentAsync: Successfully created new payment for AppointmentDetailId {AppointmentDetailId}", retryPaymentRequest.AppointmentDetailId);
                return new BaseResponse<PaymentResponseDTO>
                {
                    Code = 201,
                    Success = true,
                    Message = "Thử lại thanh toán thành công!",
                    Data = paymentResponse
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "RetryPaymentAsync: An error occurred while retrying payment");
                return new BaseResponse<PaymentResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã xảy ra lỗi khi thử lại thanh toán, vui lòng thử lại sau!",
                    Data = null
                };
            }
        }

    }
}
