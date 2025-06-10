using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PetVax.BusinessObjects.DTO;
using PetVax.BusinessObjects.DTO.AccountDTO;
using PetVax.BusinessObjects.DTO.CustomerDTO;
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
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CustomerService> _logger;

        public CustomerService(ICustomerRepository customerRepository, IMapper mapper, ILogger<CustomerService> logger)
        {
            _customerRepository = customerRepository;
            _mapper = mapper;
            _logger = logger;
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
                        Message = "Customer not found.",
                        Data = false
                    };
                }
                bool isDeleted = await _customerRepository.DeleteCustomerAsync(customerId, cancellationToken);
                if (isDeleted)
                {
                    return new BaseResponse<bool>
                    {
                        Code = 200,
                        Success = true,
                        Message = "Customer deleted successfully.",
                        Data = true
                    };
                }
                else
                {
                    return new BaseResponse<bool>
                    {
                        Code = 500,
                        Success = false,
                        Message = "Failed to delete customer.",
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
                    Message = "An error occurred while deleting the customer.",
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
                    },
                    PageData = _mapper.Map<List<CustomerResponseDTO>>(pagedCustomers)
                };
                if (!pagedCustomers.Any())
                {
                    return new DynamicResponse<CustomerResponseDTO>
                    {
                        Code = 404,
                        Success = false,
                        Message = "No customers found.",
                        Data = null
                    };
                }
                return new DynamicResponse<CustomerResponseDTO>
                {
                    Code = 200,
                    Success = true,
                    Message = "Customers retrieved successfully.",
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
                    Message = "An error occurred while retrieving customers.",
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
                        Code = 404,
                        Success = false,
                        Message = "Customer not found.",
                        Data = null
                    };
                }
                var customerResponse = _mapper.Map<CustomerResponseDTO>(customer);
                return new BaseResponse<CustomerResponseDTO>
                {
                    Code = 200,
                    Success = true,
                    Message = "Customer retrieved successfully.",
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
                    Message = "An error occurred while retrieving the customer.",
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
                        Code = 404,
                        Success = false,
                        Message = "Customer not found.",
                        Data = null
                    };
                }
                var customerResponse = _mapper.Map<CustomerResponseDTO>(customer);
                return new BaseResponse<CustomerResponseDTO>
                {
                    Code = 200,
                    Success = true,
                    Message = "Customer retrieved successfully.",
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
                    Message = "An error occurred while retrieving the customer.",
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
                        Message = "Customer not found.",
                        Data = false
                    };
                }
                // Map the update DTO to the customer entity
                _mapper.Map(updateCustomerDTO, customer);
                // Update the customer in the repository
                int updatedCustomerId = await _customerRepository.UpdateCustomerAsync(customer, cancellationToken);
                if (updatedCustomerId <= 0)
                {
                    return new BaseResponse<bool>
                    {
                        Code = 500,
                        Success = false,
                        Message = "Failed to update customer.",
                        Data = false
                    };
                }
                return new BaseResponse<bool>
                {
                    Code = 200,
                    Success = true,
                    Message = "Customer updated successfully.",
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
                    Message = "An error occurred while updating the customer.",
                    Data = false
                };
            }
        }
    }
}
