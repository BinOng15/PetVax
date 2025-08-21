using AutoMapper;
using Microsoft.Extensions.Logging;
using PetVax.BusinessObjects.DTO;
using PetVax.BusinessObjects.DTO.CustomerVoucherDTO;
using PetVax.BusinessObjects.Enum;
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
    public class CustomerVoucherService : ICustomerVoucherService
    {
        private readonly ICustomerVoucherRepository _customerVoucherRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IVoucherRepository _voucherRepository;
        private readonly ILogger<CustomerVoucherService> _logger;
        private readonly IMapper _mapper;

        public CustomerVoucherService(ICustomerVoucherRepository customerVoucherRepository, ICustomerRepository customerRepository, IVoucherRepository voucherRepository, ILogger<CustomerVoucherService> logger, IMapper mapper)
        {
            _customerVoucherRepository = customerVoucherRepository;
            _customerRepository = customerRepository;
            _voucherRepository = voucherRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<DynamicResponse<CustomerVoucherResponseDTO>> GetAllCustomerVouchersAsync(GetAllItemsDTO getAllItemsDTO, EnumList.VoucherStatus? voucherStatus, CancellationToken cancellationToken)
        {
            try
            {
                var customerVouchers = await _customerVoucherRepository.GetAllCustomerVouchersAsync(cancellationToken);

                // Filter by keyword
                if (!string.IsNullOrWhiteSpace(getAllItemsDTO.KeyWord))
                {
                    customerVouchers = customerVouchers.Where(cv =>
                        (cv.Customer.FullName != null && cv.Customer.FullName.Contains(getAllItemsDTO.KeyWord, StringComparison.OrdinalIgnoreCase)) ||
                        (cv.Voucher.VoucherCode != null && cv.Voucher.VoucherCode.Contains(getAllItemsDTO.KeyWord, StringComparison.OrdinalIgnoreCase))
                    ).ToList();
                }

                // Filter by voucher status: Status == null (all), true (valid), false (expired)
                if (voucherStatus.HasValue)
                {
                    customerVouchers = customerVouchers
                        .Where(cv => cv.Status == voucherStatus.Value)
                        .ToList();
                }

                int pageNumber = getAllItemsDTO?.PageNumber > 0 ? getAllItemsDTO.PageNumber : 1;
                int pageSize = getAllItemsDTO?.PageSize > 0 ? getAllItemsDTO.PageSize : 10;
                int skip = (pageNumber - 1) * pageSize;
                int totalItem = customerVouchers.Count;
                int totalPage = (int)Math.Ceiling((double)totalItem / pageSize);

                var pagedCustomerVouchers = customerVouchers
                    .Skip(skip)
                    .Take(pageSize)
                    .ToList();

                var responseData = new MegaData<CustomerVoucherResponseDTO>
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
                    PageData = _mapper.Map<List<CustomerVoucherResponseDTO>>(pagedCustomerVouchers)
                };
                if (pagedCustomerVouchers.Any())
                {
                    return new DynamicResponse<CustomerVoucherResponseDTO>
                    {
                        Code = 200,
                        Success = true,
                        Message = "Lấy tất cả CustomerVoucher thành công",
                        Data = responseData
                    };
                }
                else
                {
                    return new DynamicResponse<CustomerVoucherResponseDTO>
                    {
                        Code = 200,
                        Success = true,
                        Message = "Không tìm thấy CustomerVoucher nào",
                        Data = null
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting all customer vouchers");
                return new DynamicResponse<CustomerVoucherResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã có lỗi khi lấy tất cả CustomerVoucher",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<CustomerVoucherResponseDTO>> GetCustomerVoucherByCustomerIdAndVoucherIdAsync(int customerId, int voucherId, CancellationToken cancellationToken)
        {
            try
            {
                var customerVoucher = await _customerVoucherRepository.GetCustomerVoucherByCustomerIdAndVoucherIdAsync(customerId, voucherId, cancellationToken);
                if (customerVoucher == null)
                {
                    return new BaseResponse<CustomerVoucherResponseDTO>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Không tìm thấy CustomerVoucher",
                        Data = null
                    };
                }
                var responseData = _mapper.Map<CustomerVoucherResponseDTO>(customerVoucher);
                return new BaseResponse<CustomerVoucherResponseDTO>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy CustomerVoucher thành công",
                    Data = responseData
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting customer voucher by customer ID and voucher ID");
                return new BaseResponse<CustomerVoucherResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã có lỗi khi lấy CustomerVoucher",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<CustomerVoucherResponseDTO>> GetCustomerVoucherByIdAsync(int customerVoucherId, CancellationToken cancellationToken)
        {
            try
            {
                var customerVoucher = await _customerVoucherRepository.GetCustomerVoucherByIdAsync(customerVoucherId, cancellationToken);
                if (customerVoucher == null)
                {
                    return new BaseResponse<CustomerVoucherResponseDTO>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Không tìm thấy CustomerVoucher",
                        Data = null
                    };
                }
                var responseData = _mapper.Map<CustomerVoucherResponseDTO>(customerVoucher);
                return new BaseResponse<CustomerVoucherResponseDTO>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy CustomerVoucher thành công",
                    Data = responseData
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting customer voucher by ID");
                return new BaseResponse<CustomerVoucherResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã có lỗi khi lấy CustomerVoucher",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<List<CustomerVoucherResponseDTO>>> GetCustomerVoucherByVoucherIdAsync(int voucherId, CancellationToken cancellationToken)
        {
            try
            {
                var customerVouchers = await _customerVoucherRepository.GetCustomerVoucherByVoucherIdAsync(voucherId, cancellationToken);
                if (customerVouchers == null || !customerVouchers.Any())
                {
                    return new BaseResponse<List<CustomerVoucherResponseDTO>>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Không tìm thấy CustomerVoucher nào cho Voucher này",
                        Data = null
                    };
                }
                var responseData = _mapper.Map<List<CustomerVoucherResponseDTO>>(customerVouchers);
                return new BaseResponse<List<CustomerVoucherResponseDTO>>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy CustomerVoucher thành công",
                    Data = responseData
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting customer vouchers by voucher ID");
                return new BaseResponse<List<CustomerVoucherResponseDTO>>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã có lỗi khi lấy CustomerVoucher",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<List<CustomerVoucherResponseDTO>>> GetCustomerVouchersByCustomerIdAsync(int customerId, CancellationToken cancellationToken)
        {
            try
            {
                var customerVouchers = await _customerVoucherRepository.GetCustomerVouchersByCustomerIdAsync(customerId, cancellationToken);
                if (customerVouchers == null || !customerVouchers.Any())
                {
                    return new BaseResponse<List<CustomerVoucherResponseDTO>>
                    {
                        Code = 200,
                        Success = true,
                        Message = "Khách hàng hiện chưa có voucher nào, vui lòng đổi điểm để có voucher!",
                        Data = null
                    };
                }
                var responseData = _mapper.Map<List<CustomerVoucherResponseDTO>>(customerVouchers);
                return new BaseResponse<List<CustomerVoucherResponseDTO>>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy CustomerVoucher thành công",
                    Data = responseData
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting customer vouchers by customer ID");
                return new BaseResponse<List<CustomerVoucherResponseDTO>>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã có lỗi khi lấy CustomerVoucher",
                    Data = null
                };
            }
        }
    }
}
