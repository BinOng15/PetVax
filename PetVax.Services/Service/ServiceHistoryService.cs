using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PetVax.BusinessObjects.DTO;
using PetVax.BusinessObjects.DTO.ServiceHistoryDTO;
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
    public class ServiceHistoryService : IServiceHistoryService
    {
        private readonly IServiceHistoryRepository _serviceHistoryRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ServiceHistoryService(
            IServiceHistoryRepository serviceHistoryRepository,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            _serviceHistoryRepository = serviceHistoryRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<BaseResponse<List<ServiceHistoryResponseDTO>>> GetServiceHistoryByCustomerIdAsync(int customerId, CancellationToken cancellationToken)
        {
            try
            {
                var serviceHistories = await _serviceHistoryRepository.GetServiceHistoriesByCustomerIdAsync(customerId, cancellationToken);
                if (serviceHistories == null || !serviceHistories.Any())
                {
                    return new BaseResponse<List<ServiceHistoryResponseDTO>>
                    {
                        Code = 200,
                        Success = true,
                        Data = new List<ServiceHistoryResponseDTO>(),
                        Message = "Không tìm thấy lịch sử dịch vụ"
                    };

                }
                var response = _mapper.Map<List<ServiceHistoryResponseDTO>>(serviceHistories);
                return new BaseResponse<List<ServiceHistoryResponseDTO>>
                {
                    Code = 200,
                    Success = true,
                    Data = response
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<ServiceHistoryResponseDTO>>
                {
                    Code = 200,
                    Success = false,
                    Data = new List<ServiceHistoryResponseDTO>(),
                    Message = $"Lỗi khi lấy lịch sử dịch vụ: {ex.Message} {ex.InnerException}"
                };
            }
        }
        public async Task<DynamicResponse<ServiceHistoryResponseDTO>> GetAllServiceHistoryAsync(GetAllItemsDTO getAllItemsDTO, CancellationToken cancellationToken)
        {
            try
            {
                var serviceHistories = await _serviceHistoryRepository.GetAllServiceHistoriesAsync(cancellationToken);
                if (!string.IsNullOrWhiteSpace(getAllItemsDTO.KeyWord))
                {
                    var keyword = getAllItemsDTO.KeyWord.ToLower();
                    serviceHistories = serviceHistories
                        .Where(sh =>
                            // Search by ServiceType (convert enum to string)
                            sh.ServiceType.ToString().ToLower().Contains(keyword) ||

                            // Search by Status (case-insensitive)
                            (sh.Status != null && sh.Status.ToLower().Contains(keyword)) ||

                            // Search by Customer FullName (case-insensitive, check null)
                            (sh.Customer != null &&
                             sh.Customer.FullName != null &&
                             sh.Customer.FullName.ToLower().Contains(keyword))
                        )
                        .ToList();
                }

                int pageNumber = getAllItemsDTO?.PageNumber > 0 ? getAllItemsDTO.PageNumber : 1;
                int pageSize = getAllItemsDTO?.PageSize > 0 ? getAllItemsDTO.PageSize : 10;
                int skip = (pageNumber - 1) * pageSize;
                int totalItem = serviceHistories.Count;
                int totalPage = (int)Math.Ceiling((double)totalItem / pageSize);

                var pagedServiceHistories = serviceHistories
                    .Skip(skip)
                    .Take(pageSize)
                    .ToList();

                var responseData = new MegaData<ServiceHistoryResponseDTO>
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
                        status = getAllItemsDTO?.Status
                    },
                    PageData = _mapper.Map<List<ServiceHistoryResponseDTO>>(pagedServiceHistories)
                };

                if (!pagedServiceHistories.Any())
                {
                    return new DynamicResponse<ServiceHistoryResponseDTO>
                    {
                        Code = 200,
                        Success = false,
                        Message = "Không tìm thấy lịch sử dịch vụ nào.",
                        Data = null
                    };
                }
                return new DynamicResponse<ServiceHistoryResponseDTO>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy tất cả lịch sử dịch vụ thành công.",
                    Data = responseData
                };
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex, "Đã xảy ra lỗi khi lấy tất cả lịch sử dịch vụ.");
                return new DynamicResponse<ServiceHistoryResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã xảy ra lỗi khi lấy tất cả lịch sử dịch vụ.",
                    Data = null
                };
            }
        }
    }
}
