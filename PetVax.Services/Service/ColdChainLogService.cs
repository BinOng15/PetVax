using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PetVax.BusinessObjects.DTO;
using PetVax.BusinessObjects.DTO.ColdChainLogDTO;
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
    public class ColdChainLogService : IColdChainLogService
    {
        private readonly IColdChainLogRepository _coldChainLogRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<ColdChainLogService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ColdChainLogService(
            IColdChainLogRepository coldChainLogRepository,
            IMapper mapper,
            ILogger<ColdChainLogService> logger,
            IHttpContextAccessor httpContextAccessor)
        {
            _coldChainLogRepository = coldChainLogRepository;
            _mapper = mapper;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<BaseResponse<ColdChainLogResponseDTO>> CreateColdChainLogAsync(CreateColdChainLogDTO createColdChainLogDTO, CancellationToken cancellationToken)
        {
            if (createColdChainLogDTO == null)
            {
                return new BaseResponse<ColdChainLogResponseDTO>
                {
                    Code = 400,
                    Success = false,
                    Message = "Dữ liệu không hợp lệ.",
                    Data = null
                };
            }
            try
            {
                var coldChainLog = _mapper.Map<ColdChainLog>(createColdChainLogDTO);
                var createdLog = await _coldChainLogRepository.CreateColdChainLogAsync(coldChainLog, cancellationToken);
                if (createdLog == null)
                {
                    return new BaseResponse<ColdChainLogResponseDTO>
                    {
                        Code = 500,
                        Success = false,
                        Message = "Không thể tạo nhật ký lạnh.",
                        Data = null
                    };
                }
                var responseDTO = _mapper.Map<ColdChainLogResponseDTO>(createdLog);
                return new BaseResponse<ColdChainLogResponseDTO>
                {
                    Code = 201,
                    Success = true,
                    Message = "Nhật ký lạnh đã được tạo thành công.",
                    Data = responseDTO
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi tạo nhật ký lạnh.");
                return new BaseResponse<ColdChainLogResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã xảy ra lỗi khi tạo nhật ký lạnh.",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<ColdChainLogResponseDTO>> DeleteColdChainLogAsync(int coldChainLogId, CancellationToken cancellationToken)
        {
            if (coldChainLogId <= 0)
            {
                return new BaseResponse<ColdChainLogResponseDTO>
                {
                    Code = 400,
                    Success = false,
                    Message = "ID nhật ký lạnh không hợp lệ.",
                    Data = null
                };
            }
            try
            {
                var deleted = await _coldChainLogRepository.DeleteColdChainLogAsync(coldChainLogId, cancellationToken);
                if (!deleted)
                {
                    return new BaseResponse<ColdChainLogResponseDTO>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Nhật ký lạnh không tìm thấy.",
                        Data = null
                    };
                }
                return new BaseResponse<ColdChainLogResponseDTO>
                {
                    Code = 200,
                    Success = true,
                    Message = "Nhật ký lạnh đã được xóa thành công.",
                    Data = null
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi xóa nhật ký lạnh.");
                return new BaseResponse<ColdChainLogResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã xảy ra lỗi khi xóa nhật ký lạnh.",
                    Data = null
                };
            }
        }

        public async Task<DynamicResponse<ColdChainLogResponseDTO>> GetAllColdChainLogsAsync(GetAllItemsDTO getAllItemsDTO, CancellationToken cancellationToken)
        {
            try
            {
                var coldChainLogs = await _coldChainLogRepository.GetAllColdChainLogsAsync(cancellationToken);
                if (!string.IsNullOrWhiteSpace(getAllItemsDTO.KeyWord))
                {
                    coldChainLogs = coldChainLogs
                     .Where(log => log.Event.Contains(getAllItemsDTO.KeyWord, StringComparison.OrdinalIgnoreCase))
                     .ToList();
                }
                int pageNumber = getAllItemsDTO?.PageNumber > 0 ? getAllItemsDTO.PageNumber : 1;
                int pageSize = getAllItemsDTO?.PageSize > 0 ? getAllItemsDTO.PageSize : 10;
                int skip = (pageNumber - 1) * pageSize;
                int totalItem = coldChainLogs.Count;
                int totalPage = (int)Math.Ceiling((double)totalItem / pageSize);

                var pagedLogs = coldChainLogs
                    .Skip(skip)
                    .Take(pageSize)
                    .Select(log => _mapper.Map<ColdChainLogResponseDTO>(log))
                    .ToList();

                var responseData = new MegaData<ColdChainLogResponseDTO>
                {
                    PageInfo = new PagingMetaData
                    {
                        Page = pageNumber,
                        Size = pageSize,
                        TotalItem = totalItem,
                        TotalPage = totalPage,
                    },
                    SearchInfo = new SearchCondition
                    {
                        keyWord = getAllItemsDTO.KeyWord,
                        status = getAllItemsDTO.Status
                    },
                    PageData = _mapper.Map<List<ColdChainLogResponseDTO>>(pagedLogs)
                };
                if (!pagedLogs.Any())
                {
                    return new DynamicResponse<ColdChainLogResponseDTO>
                    {
                        Code = 200,
                        Success = true,
                        Message = "Không tìm thấy nhật ký lạnh nào.",
                        Data = responseData
                    };
                }
                return new DynamicResponse<ColdChainLogResponseDTO>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy tất cả nhật ký lạnh thành công.",
                    Data = responseData
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy tất cả nhật ký lạnh.");
                return new DynamicResponse<ColdChainLogResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã xảy ra lỗi khi lấy tất cả nhật ký lạnh.",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<ColdChainLogResponseDTO>> GetColdchainLogById(int coldChainLogId, CancellationToken cancellationToken)
        {
            if (coldChainLogId <= 0)
            {
                return new BaseResponse<ColdChainLogResponseDTO>
                {
                    Code = 400,
                    Success = false,
                    Message = "ID nhật ký lạnh không hợp lệ.",
                    Data = null
                };
            }
            try
            {
                var coldChainLog = await _coldChainLogRepository.GetColdChainLogByIdAsync(coldChainLogId, cancellationToken);
                if (coldChainLog == null)
                {
                    return new BaseResponse<ColdChainLogResponseDTO>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Nhật ký lạnh không tìm thấy.",
                        Data = null
                    };
                }
                var responseDTO = _mapper.Map<ColdChainLogResponseDTO>(coldChainLog);
                return new BaseResponse<ColdChainLogResponseDTO>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy nhật ký lạnh thành công.",
                    Data = responseDTO
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy nhật ký lạnh theo ID.");
                return new BaseResponse<ColdChainLogResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã xảy ra lỗi khi lấy nhật ký lạnh theo ID.",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<List<ColdChainLogResponseDTO>>> GetColdChainLogsByVaccineBatchIdAsync(int vaccineBatchId, GetAllItemsDTO getAllItemsDTO, CancellationToken cancellationToken)
        {
            if (vaccineBatchId <= 0)
            {
                return new BaseResponse<List<ColdChainLogResponseDTO>>
                {
                    Code = 400,
                    Success = false,
                    Message = "ID lô vắc xin không hợp lệ.",
                    Data = null
                };
            }
            try
            {
                var coldChainLogs = await _coldChainLogRepository.GetColdChainLogsByVaccineBatchIdAsync(vaccineBatchId, cancellationToken);
                if (!string.IsNullOrWhiteSpace(getAllItemsDTO.KeyWord))
                {
                    coldChainLogs = coldChainLogs
                     .Where(log => log.Event.Contains(getAllItemsDTO.KeyWord, StringComparison.OrdinalIgnoreCase))
                     .ToList();
                }
                var responseDTOs = _mapper.Map<List<ColdChainLogResponseDTO>>(coldChainLogs);
                return new BaseResponse<List<ColdChainLogResponseDTO>>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy nhật ký lạnh theo ID lô vắc xin thành công.",
                    Data = responseDTOs
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy nhật ký lạnh theo ID lô vắc xin.");
                return new BaseResponse<List<ColdChainLogResponseDTO>>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã xảy ra lỗi khi lấy nhật ký lạnh theo ID lô vắc xin.",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<ColdChainLogResponseDTO>> UpdateColdChainLogAsync(int coldChainLogId, UpdateColdChainLogDTO updateColdChainLogDTO, CancellationToken cancellationToken)
        {
            if (updateColdChainLogDTO == null || coldChainLogId <= 0)
            {
                return new BaseResponse<ColdChainLogResponseDTO>
                {
                    Code = 400,
                    Success = false,
                    Message = "Dữ liệu không hợp lệ.",
                    Data = null
                };
            }
            try
            {
                var existingLog = await _coldChainLogRepository.GetColdChainLogByIdAsync(coldChainLogId, cancellationToken);
                if (existingLog == null)
                {
                    return new BaseResponse<ColdChainLogResponseDTO>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Nhật ký lạnh không tìm thấy.",
                        Data = null
                    };
                }
                var updatedLog = _mapper.Map(updateColdChainLogDTO, existingLog);
                var result = await _coldChainLogRepository.UpdateColdChainLogAsync(updatedLog, cancellationToken);
                if (result <= 0)
                {
                    return new BaseResponse<ColdChainLogResponseDTO>
                    {
                        Code = 500,
                        Success = false,
                        Message = "Không thể cập nhật nhật ký lạnh.",
                        Data = null
                    };
                }
                var responseDTO = _mapper.Map<ColdChainLogResponseDTO>(updatedLog);
                return new BaseResponse<ColdChainLogResponseDTO>
                {
                    Code = 200,
                    Success = true,
                    Message = "Nhật ký lạnh đã được cập nhật thành công.",
                    Data = responseDTO
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi cập nhật nhật ký lạnh.");
                return new BaseResponse<ColdChainLogResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã xảy ra lỗi khi cập nhật nhật ký lạnh.",
                    Data = null
                };
            }
        }
    }
}
