using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PetVax.BusinessObjects.DTO;
using PetVax.BusinessObjects.DTO.CustomerVoucherDTO;
using PetVax.BusinessObjects.DTO.VoucherDTO;
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
    public class VoucherService : IVoucherService
    {
        private readonly IVoucherRepository _voucherRepository;
        private readonly IPointTransactionRepository _pointTransactionRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly ICustomerVoucherRepository _customerVoucherRepository;
        private readonly ILogger<VoucherService> _logger;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public VoucherService(
            IVoucherRepository voucherRepository, 
            IPointTransactionRepository pointTransactionRepository,
            ICustomerVoucherRepository customerVoucherRepository,
            ILogger<VoucherService> logger, 
            IMapper mapper, 
            IHttpContextAccessor httpContextAccessor,
            ICustomerRepository customerRepository)
        {
            _voucherRepository = voucherRepository;
            _pointTransactionRepository = pointTransactionRepository;
            _customerVoucherRepository = customerVoucherRepository;
            _logger = logger;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _customerRepository = customerRepository;
        }

        public async Task<BaseResponse<CreateUpdateVoucherResponseDTO>> CreateVoucherAsync(CreateVoucherDTO createVoucherDTO, CancellationToken cancellationToken)
        {
            if (createVoucherDTO == null)
            {
                _logger.LogError("CreateVoucherAsync: createVoucherDTO is null");
                return new BaseResponse<CreateUpdateVoucherResponseDTO>
                {
                    Code = 400,
                    Success = false,
                    Message = "Dữ liệu tạo voucher không hợp lệ!",
                    Data = null
                };
            }
            try
            {
                var voucher = _mapper.Map<Voucher>(createVoucherDTO);
                voucher.VoucherCode = "VCH" + new Random().Next(100000, 1000000).ToString();
                voucher.VoucherName = createVoucherDTO.VoucherName;
                voucher.PointsRequired = createVoucherDTO.PointsRequired;
                voucher.Description = createVoucherDTO.Description;
                voucher.DiscountAmount = createVoucherDTO.DiscountAmount;
                voucher.ExpirationDate = createVoucherDTO.ExpirationDate;
                voucher.CreatedAt = DateTimeHelper.Now();
                voucher.CreatedBy = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "System";
                var createdVoucherId = await _voucherRepository.CreateVoucherAsync(voucher, cancellationToken);
                if (createdVoucherId <= 0)
                {
                    _logger.LogError("CreateVoucherAsync: Failed to create voucher");
                    return new BaseResponse<CreateUpdateVoucherResponseDTO>
                    {
                        Code = 500,
                        Success = false,
                        Message = "Không thể tạo mới voucher, vui lòng thử lại!",
                        Data = null
                    };
                }

                var createdVoucher = await _voucherRepository.GetVoucherByIdAsync(createdVoucherId, cancellationToken);

                var responseDTO = _mapper.Map<CreateUpdateVoucherResponseDTO>(createdVoucher);
                return new BaseResponse<CreateUpdateVoucherResponseDTO>
                {
                    Code = 201,
                    Success = true,
                    Message = "Tạo mới voucher thành công!",
                    Data = responseDTO
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CreateVoucherAsync: An error occurred while creating voucher");
                return new BaseResponse<CreateUpdateVoucherResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã có lỗi khi tạo mới voucher, vui lòng thử lại!",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<bool>> DeleteVoucherAsync(int voucherId, CancellationToken cancellationToken)
        {
            if (voucherId <= 0)
            {
                _logger.LogError("DeleteVoucherAsync: Invalid voucherId");
                return new BaseResponse<bool>
                {
                    Code = 400,
                    Success = false,
                    Message = "ID voucher không hợp lệ!",
                    Data = false
                };
            }
            try
            {
                var voucher = await _voucherRepository.GetVoucherByIdAsync(voucherId, cancellationToken);
                if (voucher == null)
                {
                    _logger.LogError($"DeleteVoucherAsync: Voucher with ID {voucherId} not found");
                    return new BaseResponse<bool>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Voucher không tồn tại!",
                        Data = false
                    };
                }
                var isDeleted = await _voucherRepository.DeleteVoucherAsync(voucherId, cancellationToken);
                if (!isDeleted)
                {
                    _logger.LogError($"DeleteVoucherAsync: Failed to delete voucher with ID {voucherId}");
                    return new BaseResponse<bool>
                    {
                        Code = 500,
                        Success = false,
                        Message = "Không thể xóa voucher, vui lòng thử lại!",
                        Data = false
                    };
                }
                return new BaseResponse<bool>
                {
                    Code = 200,
                    Success = true,
                    Message = "Xóa voucher thành công!",
                    Data = true
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"DeleteVoucherAsync: An error occurred while deleting voucher with ID {voucherId}");
                return new BaseResponse<bool>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã có lỗi khi xóa voucher, vui lòng thử lại!",
                    Data = false
                };
            }
        }

        public async Task<DynamicResponse<VoucherResponseDTO>> GetAllVoucherAsync(GetAllItemsDTO getAllItemsDTO, CancellationToken cancellationToken)
        {
            try
            {
                var vouchers = await _voucherRepository.GetAllVoucherAsync(cancellationToken);
                if (!string.IsNullOrWhiteSpace(getAllItemsDTO.KeyWord))
                {
                    var keyword = getAllItemsDTO.KeyWord.ToLower();
                    vouchers = vouchers.Where(v => v.VoucherName.ToLower().Contains(keyword) || v.VoucherCode.ToLower().Contains(keyword)).ToList();
                }

                int pageNumber = getAllItemsDTO?.PageNumber > 0 ? getAllItemsDTO.PageNumber : 1;
                int pageSize = getAllItemsDTO?.PageSize > 0 ? getAllItemsDTO.PageSize : 10;
                int skip = (pageNumber - 1) * pageSize;
                int totalItem = vouchers.Count;
                int totalPage = (int)Math.Ceiling((double)totalItem / pageSize);

                var pagedVouchers = vouchers
                    .Skip(skip)
                    .Take(pageSize)
                    .ToList();

                var responseData = new MegaData<VoucherResponseDTO>
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
                    PageData = _mapper.Map<List<VoucherResponseDTO>>(pagedVouchers)
                };
                if (pagedVouchers.Any())
                {
                    return new DynamicResponse<VoucherResponseDTO>
                    {
                        Code = 200,
                        Success = true,
                        Message = "Lấy danh sách voucher thành công!",
                        Data = responseData
                    };
                }
                else
                {
                    return new DynamicResponse<VoucherResponseDTO>
                    {
                        Code = 200,
                        Success = true,
                        Message = "Không tìm thấy voucher nào!",
                        Data = null
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetAllVoucherAsync: An error occurred while retrieving vouchers");
                return new DynamicResponse<VoucherResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã có lỗi khi lấy danh sách voucher, vui lòng thử lại!",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<VoucherResponseDTO>> GetVoucherByCodeAsync(string voucherCode, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(voucherCode))
            {
                _logger.LogError("GetVoucherByCodeAsync: voucherCode is null or empty");
                return new BaseResponse<VoucherResponseDTO>
                {
                    Code = 400,
                    Success = false,
                    Message = "Mã voucher không hợp lệ!",
                    Data = null
                };
            }
            try
            {
                var voucher = await _voucherRepository.GetVoucherByCodeAsync(voucherCode, cancellationToken);
                if (voucher == null)
                {
                    _logger.LogError($"GetVoucherByCodeAsync: Voucher with code {voucherCode} not found");
                    return new BaseResponse<VoucherResponseDTO>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Voucher không tồn tại!",
                        Data = null
                    };
                }
                var responseDTO = _mapper.Map<VoucherResponseDTO>(voucher);
                return new BaseResponse<VoucherResponseDTO>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy voucher thành công!",
                    Data = responseDTO
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"GetVoucherByCodeAsync: An error occurred while retrieving voucher with code {voucherCode}");
                return new BaseResponse<VoucherResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã có lỗi khi lấy voucher, vui lòng thử lại!",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<VoucherResponseDTO>> GetVoucherByIdAsync(int voucherId, CancellationToken cancellationToken)
        {
            if (voucherId <= 0)
            {
                _logger.LogError("GetVoucherByIdAsync: Invalid voucherId");
                return new BaseResponse<VoucherResponseDTO>
                {
                    Code = 400,
                    Success = false,
                    Message = "ID voucher không hợp lệ!",
                    Data = null
                };
            }
            try
            {
                var voucher = await _voucherRepository.GetVoucherByIdAsync(voucherId, cancellationToken);
                if (voucher == null)
                {
                    _logger.LogError($"GetVoucherByIdAsync: Voucher with ID {voucherId} not found");
                    return new BaseResponse<VoucherResponseDTO>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Voucher không tồn tại!",
                        Data = null
                    };
                }
                var responseDTO = _mapper.Map<VoucherResponseDTO>(voucher);
                return new BaseResponse<VoucherResponseDTO>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy voucher thành công!",
                    Data = responseDTO
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"GetVoucherByIdAsync: An error occurred while retrieving voucher with ID {voucherId}");
                return new BaseResponse<VoucherResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã có lỗi khi lấy voucher, vui lòng thử lại!",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<VoucherResponseDTO>> GetVoucherByTransactionIdAsync(int transactionId, CancellationToken cancellationToken)
        {
            if (transactionId <= 0)
            {
                _logger.LogError("GetVoucherByTransactionIdAsync: Invalid transactionId");
                return new BaseResponse<VoucherResponseDTO>
                {
                    Code = 400,
                    Success = false,
                    Message = "ID giao dịch không hợp lệ!",
                    Data = null
                };
            }
            try
            {
                var voucher = await _voucherRepository.GetVouchersByTransactionIdAsync(transactionId, cancellationToken);
                if (voucher == null)
                {
                    _logger.LogError($"GetVoucherByTransactionIdAsync: Voucher with transaction ID {transactionId} not found");
                    return new BaseResponse<VoucherResponseDTO>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Voucher không tồn tại cho giao dịch này!",
                        Data = null
                    };
                }
                var responseDTO = _mapper.Map<VoucherResponseDTO>(voucher);
                return new BaseResponse<VoucherResponseDTO>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy voucher thành công!",
                    Data = responseDTO
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"GetVoucherByTransactionIdAsync: An error occurred while retrieving voucher for transaction ID {transactionId}");
                return new BaseResponse<VoucherResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã có lỗi khi lấy voucher, vui lòng thử lại!",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<CustomerVoucherResponseDTO>> RedeemPointsForVoucherAsync(int customerId, int voucherId, CancellationToken cancellationToken)
        {
            if (customerId <= 0 || voucherId <= 0)
            {
                _logger.LogError("RedeemPointsForVoucherAsync: Invalid customerId or voucherId");
                return new BaseResponse<CustomerVoucherResponseDTO>
                {
                    Code = 400,
                    Success = false,
                    Message = "Khách hàng hoặc voucher không hợp lệ!",
                    Data = null
                };
            }

            var customer = await _customerRepository.GetCustomerByIdAsync(customerId, cancellationToken);
            if (customer == null)
            {
                _logger.LogError($"RedeemPointsForVoucherAsync: Customer with ID {customerId} not found");
                return new BaseResponse<CustomerVoucherResponseDTO>
                {
                    Code = 404,
                    Success = false,
                    Message = "Khách hàng không tồn tại!",
                    Data = null
                };
            }

            var voucher = await _voucherRepository.GetVoucherByIdAsync(voucherId, cancellationToken);
            if (voucher == null)
            {
                _logger.LogError($"RedeemPointsForVoucherAsync: Voucher with ID {voucherId} not found");
                return new BaseResponse<CustomerVoucherResponseDTO>
                {
                    Code = 404,
                    Success = false,
                    Message = "Voucher không tồn tại!",
                    Data = null
                };
            }

            int pointsRequired = voucher.PointsRequired;
            int redeemablePoints = customer.RedeemablePoints ?? 0;

            if (redeemablePoints < pointsRequired)
            {
                _logger.LogError($"RedeemPointsForVoucherAsync: Customer with ID {customerId} does not have enough points to redeem voucher {voucherId}");
                return new BaseResponse<CustomerVoucherResponseDTO>
                {
                    Code = 400,
                    Success = false,
                    Message = "Khách hàng không đủ điểm để đổi voucher này!",
                    Data = null
                };
            }

            try
            {
                // Create a point transaction for the redemption
                var pointTransaction = new PointTransaction
                {
                    CustomerId = customerId,
                    VoucherId = voucherId,
                    Change = pointsRequired.ToString(),
                    TransactionType = "Redeem",
                    Description = $"Đổi {pointsRequired} điểm để nhận voucher {voucher.VoucherName}",
                    TransactionDate = DateTimeHelper.Now(),
                    CreatedAt = DateTimeHelper.Now(),
                    CreatedBy = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "System"
                };
                await _pointTransactionRepository.CreatePointTransactionAsync(pointTransaction, cancellationToken);

                // Update customer's redeemable points
                customer.RedeemablePoints = redeemablePoints - pointsRequired;
                await _customerRepository.UpdateCustomerAsync(customer, cancellationToken);

                // Create a CustomerVoucher record
                var customerVoucher = new CustomerVoucher
                {
                    CustomerId = customerId,
                    VoucherId = voucherId,
                    Status = EnumList.VoucherStatus.Available,
                    RedeemedAt = DateTimeHelper.Now(),
                    RedeemedBy = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "System",
                    ExpirationDate = voucher.ExpirationDate,
                    CreatedAt = DateTimeHelper.Now(),
                    CreatedBy = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "System",
                };
                var customerVoucherId = await _customerVoucherRepository.CreateCustomerVoucherAsync(customerVoucher, cancellationToken);


                var createdCustomerVoucher = await _customerVoucherRepository.GetCustomerVoucherByIdAsync(customerVoucherId, cancellationToken);
                // Return the voucher details
                var responseDTO = _mapper.Map<CustomerVoucherResponseDTO>(createdCustomerVoucher);
                return new BaseResponse<CustomerVoucherResponseDTO>
                {
                    Code = 200,
                    Success = true,
                    Message = "Đổi điểm thành công! Voucher đã được cấp.",
                    Data = responseDTO
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"RedeemPointsForVoucherAsync: An error occurred while redeeming points for customer ID {customerId} and voucher ID {voucherId}");
                return new BaseResponse<CustomerVoucherResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã có lỗi khi đổi điểm, vui lòng thử lại!",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<CreateUpdateVoucherResponseDTO>> UpdateVoucherAsync(int voucherId, UpdateVoucherDTO updateVoucherDTO, CancellationToken cancellationToken)
        {
            if (voucherId <= 0 || updateVoucherDTO == null)
            {
                _logger.LogError("UpdateVoucherAsync: Invalid voucherId or updateVoucherDTO is null");
                return new BaseResponse<CreateUpdateVoucherResponseDTO>
                {
                    Code = 400,
                    Success = false,
                    Message = "ID voucher hoặc dữ liệu cập nhật không hợp lệ!",
                    Data = null
                };
            }
            try
            {
                var existingVoucher = await _voucherRepository.GetVoucherByIdAsync(voucherId, cancellationToken);
                if (existingVoucher == null)
                {
                    _logger.LogError($"UpdateVoucherAsync: Voucher with ID {voucherId} not found");
                    return new BaseResponse<CreateUpdateVoucherResponseDTO>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Voucher không tồn tại!",
                        Data = null
                    };
                }
                // Update voucher properties
                existingVoucher.VoucherName = updateVoucherDTO.VoucherName ?? existingVoucher.VoucherName;
                existingVoucher.PointsRequired = updateVoucherDTO.PointsRequired ?? existingVoucher.PointsRequired;
                existingVoucher.Description = updateVoucherDTO.Description ?? existingVoucher.Description;
                existingVoucher.DiscountAmount = updateVoucherDTO.DiscountAmount ?? existingVoucher.DiscountAmount;
                existingVoucher.ExpirationDate = updateVoucherDTO.ExpirationDate ?? existingVoucher.ExpirationDate;
                existingVoucher.ModifiedAt = DateTimeHelper.Now();
                existingVoucher.ModifiedBy = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "System";
                var updatedRows = await _voucherRepository.UpdateVoucherAsync(existingVoucher, cancellationToken);
                if (updatedRows <= 0)
                {
                    _logger.LogError($"UpdateVoucherAsync: Failed to update voucher with ID {voucherId}");
                    return new BaseResponse<CreateUpdateVoucherResponseDTO>
                    {
                        Code = 500,
                        Success = false,
                        Message = "Không thể cập nhật voucher, vui lòng thử lại!",
                        Data = null
                    };
                }
                var updatedVoucher = await _voucherRepository.GetVoucherByIdAsync(voucherId, cancellationToken);
                var responseDTO = _mapper.Map<CreateUpdateVoucherResponseDTO>(updatedVoucher);
                return new BaseResponse<CreateUpdateVoucherResponseDTO>
                {
                    Code = 200,
                    Success = true,
                    Message = "Cập nhật voucher thành công!",
                    Data = responseDTO
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"UpdateVoucherAsync: An error occurred while updating voucher with ID {voucherId}");
                return new BaseResponse<CreateUpdateVoucherResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã có lỗi khi cập nhật voucher, vui lòng thử lại!",
                    Data = null
                };
            }
        }
    }
}
