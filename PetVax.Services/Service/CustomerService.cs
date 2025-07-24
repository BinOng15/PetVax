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

                // Validate address is in Ho Chi Minh City
                if (!string.IsNullOrWhiteSpace(updateCustomerDTO.Address))
                {
                    bool isValidAddress = await IsAddressInHoChiMinhCity(updateCustomerDTO.Address);
                    if (!isValidAddress)
                    {
                        return new BaseResponse<bool>
                        {
                            Code = 400,
                            Success = false,
                            Message = "Địa chỉ phải nằm trong khu vực Thành phố Hồ Chí Minh.",
                            Data = false
                        };
                    }
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
        private async Task<bool> IsAddressInHoChiMinhCity(string address)
        {
            try
            {
                string mapboxApiKey = _configuration["Mapbox:AccessToken"];
                string encodedAddress = Uri.EscapeDataString(address);
                string requestUrl = $"https://api.mapbox.com/geocoding/v5/mapbox.places/{encodedAddress}.json?access_token={mapboxApiKey}&country=vn&limit=1";

                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = await client.GetAsync(requestUrl);
                    if (!response.IsSuccessStatusCode)
                    {
                        _logger.LogWarning("Mapbox API returned non-success status code: {StatusCode}", response.StatusCode);
                        return false;
                    }

                    string jsonResponse = await response.Content.ReadAsStringAsync();
                    using (var document = System.Text.Json.JsonDocument.Parse(jsonResponse))
                    {
                        var features = document.RootElement.GetProperty("features");
                        if (features.GetArrayLength() == 0)
                        {
                            _logger.LogWarning("No features found for address: {Address}", address);
                            return false;
                        }

                        var feature = features[0];
                        // Lấy tọa độ (longitude, latitude)
                        if (!feature.TryGetProperty("center", out var center) || center.GetArrayLength() != 2)
                        {
                            _logger.LogWarning("No center coordinates found for address: {Address}", address);
                            return false;
                        }
                        double longitude = center[0].GetDouble();
                        double latitude = center[1].GetDouble();

                        // Khu vực Thành phố Hồ Chí Minh (ước lượng)
                        // Lat: 10.35 - 11.20, Lng: 106.35 - 107.05
                        double minLat = 10.35, maxLat = 11.20;
                        double minLng = 106.35, maxLng = 107.05;

                        bool isInHCM = latitude >= minLat && latitude <= maxLat && longitude >= minLng && longitude <= maxLng;
                        if (!isInHCM)
                        {
                            _logger.LogInformation("Address coordinates ({Lat}, {Lng}) are not in Ho Chi Minh City area.", latitude, longitude);
                        }
                        return isInHCM;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while validating address with Mapbox API.");
                return false;
            }
        }
    }
}

    
