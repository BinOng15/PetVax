using AutoMapper;
using Microsoft.Extensions.Logging;
using PetVax.BusinessObjects.DTO;
using PetVax.BusinessObjects.DTO.PointTransactionDTO;
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
    public class PointTransactionService : IPointTransactionService
    {
        private readonly IPointTransactionRepository _pointTransactionRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<PointTransactionService> _logger;

        public PointTransactionService(IPointTransactionRepository pointTransactionRepository, ICustomerRepository customerRepository, IMapper mapper, ILogger<PointTransactionService> logger)
        {
            _pointTransactionRepository = pointTransactionRepository;
            _customerRepository = customerRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<DynamicResponse<PointTransactionResponseDTO>> GetAllPointTransactionsAsync(GetAllItemsDTO getAllItemsDTO, CancellationToken cancellationToken)
        {
            try
            {
                var pointTransactions = await _pointTransactionRepository.GetAllPointTransactionsAsync(cancellationToken);
                if (!string.IsNullOrWhiteSpace(getAllItemsDTO.KeyWord))
                {
                    pointTransactions = pointTransactions.Where(pt => pt.Customer.FullName.Contains(getAllItemsDTO.KeyWord, StringComparison.OrdinalIgnoreCase)).ToList();
                }

                int pageNumber = getAllItemsDTO?.PageNumber > 0 ? getAllItemsDTO.PageNumber : 1;
                int pageSize = getAllItemsDTO?.PageSize > 0 ? getAllItemsDTO.PageSize : 10;
                int skip = (pageNumber - 1) * pageSize;
                int totalItem = pointTransactions.Count;
                int totalPage = (int)Math.Ceiling((double)totalItem / pageSize);

                var pagedTransactions = pointTransactions
                    .Skip(skip)
                    .Take(pageSize)
                    .Select(pt => _mapper.Map<PointTransactionResponseDTO>(pt))
                    .ToList();

                var responseData = new MegaData<PointTransactionResponseDTO>
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
                    PageData = _mapper.Map<List<PointTransactionResponseDTO>>(pagedTransactions)
                };
                if (pagedTransactions.Any())
                {
                    return new DynamicResponse<PointTransactionResponseDTO>
                    {
                        Code = 200,
                        Success = true,
                        Message = "Lấy tất cả lịch sử điểm thành công",
                        Data = responseData
                    };
                }
                else
                {
                    return new DynamicResponse<PointTransactionResponseDTO>
                    {
                        Code = 200,
                        Success = false,
                        Message = "Không tìm thấy lịch sử điểm nào",
                        Data = null
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting all point transactions.");
                return new DynamicResponse<PointTransactionResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã có lỗi khi lấy tất cả lịch sử điểm",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<List<PointTransactionResponseDTO>>> GetPointTransactionByCustomerIdAsync(int customerId, CancellationToken cancellationToken)
        {
            try
            {
                var customer = await _customerRepository.GetCustomerByIdAsync(customerId, cancellationToken);
                if (customer == null)
                {
                    return new BaseResponse<List<PointTransactionResponseDTO>>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Khách hàng không tồn tại",
                        Data = null
                    };
                }
                var pointTransactions = await _pointTransactionRepository.GetPointTransactionsByCustomerIdAsync(customerId, cancellationToken);
                if (pointTransactions == null || !pointTransactions.Any())
                {
                    return new BaseResponse<List<PointTransactionResponseDTO>>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Không tìm thấy lịch sử điểm cho khách hàng này",
                        Data = null
                    };
                }
                var responseData = _mapper.Map<List<PointTransactionResponseDTO>>(pointTransactions);
                return new BaseResponse<List<PointTransactionResponseDTO>>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy lịch sử điểm thành công",
                    Data = responseData
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting point transactions by customer ID.");
                return new BaseResponse<List<PointTransactionResponseDTO>>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã có lỗi khi lấy lịch sử điểm của khách hàng",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<PointTransactionResponseDTO>> GetPointTransactionByIdAsync(int pointTransactionId, CancellationToken cancellationToken)
        {
            if (pointTransactionId <= 0)
            {
                return new BaseResponse<PointTransactionResponseDTO>
                {
                    Code = 400,
                    Success = false,
                    Message = "ID lịch sử điểm không hợp lệ",
                    Data = null
                };
            }
            try
            {
                var pointTransaction = await _pointTransactionRepository.GetPointTransactionByIdAsync(pointTransactionId, cancellationToken);
                if (pointTransaction == null)
                {
                    return new BaseResponse<PointTransactionResponseDTO>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Lịch sử điểm không tồn tại",
                        Data = null
                    };
                }
                var responseData = _mapper.Map<PointTransactionResponseDTO>(pointTransaction);
                return new BaseResponse<PointTransactionResponseDTO>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy lịch sử điểm thành công",
                    Data = responseData
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting point transaction by ID.");
                return new BaseResponse<PointTransactionResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã có lỗi khi lấy lịch sử điểm",
                    Data = null
                };
            }
        }
    }
}
