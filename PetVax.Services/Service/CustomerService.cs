using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PetVax.BusinessObjects.DTO;
using PetVax.BusinessObjects.DTO.AccountDTO;
using PetVax.BusinessObjects.DTO.CustomerDTO;
using PetVax.BusinessObjects.Helpers;
using PetVax.BusinessObjects.Models;
using PetVax.Repositories.IRepository;
using PetVax.Services.ExternalService;
using PetVax.Services.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PetVax.BusinessObjects.DTO.ResponseModel;

namespace PetVax.Services.Service
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CustomerService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICloudinariService _cloudinariService;
        private readonly IConfiguration _configuration;

        public CustomerService(ICustomerRepository customerRepository, IMapper mapper, ILogger<CustomerService> logger, IHttpContextAccessor httpContextAccessor, ICloudinariService cloudinariService, IConfiguration configuration)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _cloudinariService = cloudinariService;
            _configuration = configuration;
        }

        public async Task<BaseResponse<bool>> DeleteCustomerAsync(int customerId, CancellationToken cancellationToken)
        {
            try
            {
                var customer = await _customerRepository.GetCustomerByIdAsync(customerId, cancellationToken);
                if (customer == null)
                {
                    return new BaseResponse<bool>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Không tìm thấy khách hàng với ID đã cho.",
                        Data = false
                    };
                }
                customer.isDeleted = true;
                customer.ModifiedAt = DateTimeHelper.Now();
                customer.ModifiedBy = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "System";

                int result = await _customerRepository.UpdateCustomerAsync(customer, cancellationToken);
                if (result > 0)
                {
                    return new BaseResponse<bool>
                    {
                        Code = 200,
                        Success = true,
                        Message = "Xóa khách hàng thành công.",
                        Data = true
                    };
                }
                else
                {
                    return new BaseResponse<bool>
                    {
                        Code = 500,
                        Success = false,
                        Message = "Không thể xóa khách hàng. Vui lòng thử lại sau.",
                        Data = false
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting customer.");
                return new BaseResponse<bool>
                {
                    Code = 500,
                    Success = false,
                    Message = "Lỗi xảy ra khi xóa khách hàng.",
                    Data = false
                };
            }
        }

        public async Task<DynamicResponse<CustomerResponseDTO>> GetAllCustomersAsync(GetAllCustomerRequestDTO getAllCustomerRequestDTO, CancellationToken cancellationToken)
        {
            try
            {
                var customers = await _customerRepository.GetAllCustomersAsync(cancellationToken);
                if (!string.IsNullOrWhiteSpace(getAllCustomerRequestDTO?.KeyWord))
                {
                    var keyword = getAllCustomerRequestDTO.KeyWord.Trim().ToLower();
                    customers = customers
                        .Where(c => c.FullName.ToLower().Contains(keyword))
                    .ToList();
                }

                int pageNumber = getAllCustomerRequestDTO?.PageNumber > 0 ? getAllCustomerRequestDTO.PageNumber : 1;
                int pageSize = getAllCustomerRequestDTO?.PageSize > 0 ? getAllCustomerRequestDTO.PageSize : 10;
                int skip = (pageNumber - 1) * pageSize;
                int totalItem = customers.Count;
                int totalPage = (int)Math.Ceiling((double)totalItem / pageSize);

                var pagedCustomers = customers
                    .Skip(skip)
                    .Take(pageSize)
                    .ToList();

                var responseData = new MegaData<CustomerResponseDTO>
                {
                    PageInfo = new PagingMetaData
                    {
                        Page = pageNumber,
                        Size = pageSize,
                        TotalItem = totalItem,
                        TotalPage = totalPage,
                        Sort = null,
                        Order = null
                    },
                    SearchInfo = new SearchCondition
                    {
                        keyWord = getAllCustomerRequestDTO?.KeyWord,
                        status = getAllCustomerRequestDTO?.Status,
                    },
                    PageData = _mapper.Map<List<CustomerResponseDTO>>(pagedCustomers)
                };
                if (!pagedCustomers.Any())
                {
                    return new DynamicResponse<CustomerResponseDTO>
                    {
                        Code = 200,
                        Success = false,
                        Message = "Không tìm thấy khách hàng nào phù hợp với tiêu chí tìm kiếm.",
                        Data = null
                    };
                }
                return new DynamicResponse<CustomerResponseDTO>
                {
                    Code = 200,
                    Success = true,
                    Message = "Danh sách khách hàng đã được lấy thành công.",
                    Data = responseData
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting all customers.");
                return new DynamicResponse<CustomerResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Lỗi xảy ra khi lấy danh sách khách hàng.",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<CustomerResponseDTO>> GetCustomerByAccountIdAsync(int accountId, CancellationToken cancellationToken)
        {
            try
            {
                var customer = await _customerRepository.GetCustomerByAccountId(accountId, cancellationToken);
                if (customer == null)
                {
                    return new BaseResponse<CustomerResponseDTO>
                    {
                        Code = 200,
                        Success = false,
                        Message = "Không tìm thấy khách hàng với ID tài khoản đã cho.",
                        Data = null
                    };
                }
                var customerResponse = _mapper.Map<CustomerResponseDTO>(customer);
                return new BaseResponse<CustomerResponseDTO>
                {
                    Code = 200,
                    Success = true,
                    Message = "Khách hàng đã được lấy thành công.",
                    Data = customerResponse
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting customer by account ID.");
                return new BaseResponse<CustomerResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Lỗi xảy ra khi lấy khách hàng.",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<CustomerResponseDTO>> GetCustomerByIdAsync(int customerId, CancellationToken cancellationToken)
        {
            try
            {
                var customer = await _customerRepository.GetCustomerByIdAsync(customerId, cancellationToken);
                if (customer == null)
                {
                    return new BaseResponse<CustomerResponseDTO>
                    {
                        Code = 200,
                        Success = false,
                        Message = "Không tìm thấy khách hàng với ID đã cho.",
                        Data = null
                    };
                }
                var customerResponse = _mapper.Map<CustomerResponseDTO>(customer);
                return new BaseResponse<CustomerResponseDTO>
                {
                    Code = 200,
                    Success = true,
                    Message = "Khách hàng đã được lấy thành công.",
                    Data = customerResponse
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting customer by ID.");
                return new BaseResponse<CustomerResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Lỗi xảy ra khi lấy khách hàng.",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<bool>> UpdateCustomerAsync(int customerId, UpdateCustomerDTO updateCustomerDTO, CancellationToken cancellationToken)
        {
            try
            {
                var customer = await _customerRepository.GetCustomerByIdAsync(customerId, cancellationToken);
                if (customer == null)
                {
                    return new BaseResponse<bool>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Không tìm thấy khách hàng với ID đã cho.",
                        Data = false
                    };
                }

                // Handle image upload if a new image is provided
                if (updateCustomerDTO.Image != null)
                {
                    var imageUrl = await _cloudinariService.UploadImage(updateCustomerDTO.Image);
                    customer.Image = imageUrl;
                }

                // Update other fields if provided
                if (!string.IsNullOrWhiteSpace(updateCustomerDTO.FullName))
                    customer.FullName = updateCustomerDTO.FullName;

                if (!string.IsNullOrWhiteSpace(updateCustomerDTO.UserName))
                    customer.UserName = updateCustomerDTO.UserName;

                if (!string.IsNullOrWhiteSpace(updateCustomerDTO.PhoneNumber))
                    customer.PhoneNumber = updateCustomerDTO.PhoneNumber;

                if (!string.IsNullOrWhiteSpace(updateCustomerDTO.DateOfBirth))
                    customer.DateOfBirth = updateCustomerDTO.DateOfBirth;

                if (!string.IsNullOrWhiteSpace(updateCustomerDTO.Gender))
                    customer.Gender = updateCustomerDTO.Gender;

                // No address validation, just update if provided
                if (!string.IsNullOrWhiteSpace(updateCustomerDTO.Address))
                {
                    customer.Address = updateCustomerDTO.Address;
                }

                customer.ModifiedAt = DateTimeHelper.Now();

                int updatedCustomerId = await _customerRepository.UpdateCustomerAsync(customer, cancellationToken);
                if (updatedCustomerId <= 0)
                {
                    return new BaseResponse<bool>
                    {
                        Code = 500,
                        Success = false,
                        Message = "Không thể cập nhật khách hàng. Vui lòng thử lại sau.",
                        Data = false
                    };
                }
                return new BaseResponse<bool>
                {
                    Code = 200,
                    Success = true,
                    Message = "Cập nhật khách hàng thành công.",
                    Data = true
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating customer.");
                return new BaseResponse<bool>
                {
                    Code = 500,
                    Success = false,
                    Message = "Lỗi xảy ra khi cập nhật khách hàng.",
                    Data = false
                };
            }
        }
        
    }
}

    
