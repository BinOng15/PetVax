using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PetVax.BusinessObjects.DTO;
using PetVax.BusinessObjects.DTO.VaccineBatchDTO;
using PetVax.BusinessObjects.DTO.VaccineReceipDetailDTO;
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
    public class VaccineReceiptDetailService : IVaccineReceiptDetailService
    {
        private readonly IVaccineReceiptDetailRepository _vaccineReceiptDetailRepository;
        private readonly IColdChainLogRepository _coldChainLogRepository;
        private readonly IVaccineBatchRepository _vaccineBatchRepository;
        private readonly IVaccineReceiptRepository _vaccineReceiptRepository;
        private readonly ILogger<VaccineReceiptDetailService> _logger;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public VaccineReceiptDetailService(
            IVaccineReceiptDetailRepository vaccineReceiptDetailRepository,
            IColdChainLogRepository coldChainLogRepository,
            IVaccineBatchRepository vaccineBatchRepository,
            IVaccineReceiptRepository vaccineReceiptRepository,
            ILogger<VaccineReceiptDetailService> logger,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            _vaccineReceiptDetailRepository = vaccineReceiptDetailRepository;
            _coldChainLogRepository = coldChainLogRepository;
            _vaccineBatchRepository = vaccineBatchRepository;
            _vaccineReceiptRepository = vaccineReceiptRepository;
            _logger = logger;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<BaseResponse<VaccineReceiptDetailResponseDTO>> CreateFullVaccineReceiptAsync(CreateFullVaccineReceiptDTO createFullVaccineReceiptDTO, CancellationToken cancellationToken)
        {
            if (createFullVaccineReceiptDTO == null)
            {
                return new BaseResponse<VaccineReceiptDetailResponseDTO>
                {
                    Code = 400,
                    Success = false,
                    Message = "Dữ liệu không hợp lệ",
                    Data = null
                };
            }
            try
            {
                // 1. Tạo VaccineReceipt
                var vaccineReceipt = new VaccineReceipt
                {
                    ReceiptCode = "RECEIPT" + new Random().Next(100000, 1000000).ToString(),
                    ReceiptDate = createFullVaccineReceiptDTO.ReceiptDate,
                    CreatedAt = DateTimeHelper.Now(),
                    CreatedBy = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "System",
                };
                var vaccineReceiptId = await _vaccineReceiptRepository.CreateVaccineReceiptAsync(vaccineReceipt, cancellationToken);

                // 2. Kiểm tra VaccineBatchId có hợp lệ không
                var vaccineBatch = await _vaccineBatchRepository.GetVaccineBatchByIdAsync(createFullVaccineReceiptDTO.VaccineBatchId, cancellationToken);
                if (vaccineBatch == null)
                {
                    return new BaseResponse<VaccineReceiptDetailResponseDTO>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Lô vắc xin không tồn tại.",
                        Data = null
                    };
                }

                // 3. Tạo VaccineReceiptDetail
                var vaccineReceiptDetail = new VaccineReceiptDetail
                {
                    VaccineReceiptId = vaccineReceiptId,
                    VaccineBatchId = createFullVaccineReceiptDTO.VaccineBatchId,
                    Suppiler = createFullVaccineReceiptDTO.Suppiler,
                    Quantity = createFullVaccineReceiptDTO.Quantity,
                    VaccineStatus = createFullVaccineReceiptDTO.VaccineStatus,
                    Notes = createFullVaccineReceiptDTO.Notes,
                    CreatedAt = DateTimeHelper.Now(),
                    CreatedBy = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "System",
                    isDeleted = false
                };
                var vaccineReceiptDetailId = await _vaccineReceiptDetailRepository.CreateVaccineReceiptDetailAsync(vaccineReceiptDetail, cancellationToken);

                // 4. Tăng quantity của VaccineBatch
                vaccineBatch.Quantity += createFullVaccineReceiptDTO.Quantity;
                await _vaccineBatchRepository.UpdateVaccineBatchAsync(vaccineBatch, cancellationToken);

                // 5. Tạo ColdChainLog
                var coldChainLogDTO = createFullVaccineReceiptDTO.ColdChainLog;
                var coldChainLog = new ColdChainLog
                {
                    VaccineBatchId = coldChainLogDTO?.VaccineBatchId ?? createFullVaccineReceiptDTO.VaccineBatchId,
                    LogTime = coldChainLogDTO?.LogTime ?? DateTimeHelper.Now(),
                    Temperature = coldChainLogDTO?.Temperature ?? 0,
                    Humidity = coldChainLogDTO?.Humidity ?? 0,
                    Event = coldChainLogDTO?.Event ?? "Nhập kho",
                    Notes = coldChainLogDTO?.Notes ?? $"Tạo log cho chi tiết nhập kho có id: {vaccineReceiptDetailId}",
                    RecordedAt = DateTimeHelper.Now(),
                    RecordedBy = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "System",
                    isDeleted = false
                };
                await _coldChainLogRepository.CreateColdChainLogAsync(coldChainLog, cancellationToken);

                // 6. Lấy lại VaccineReceiptDetail vừa tạo để trả về
                var createdDetail = await _vaccineReceiptDetailRepository.GetVaccineReceiptDetailByIdAsync(vaccineReceiptDetailId, cancellationToken);
                var responseDTO = _mapper.Map<VaccineReceiptDetailResponseDTO>(createdDetail);

                return new BaseResponse<VaccineReceiptDetailResponseDTO>
                {
                    Code = 201,
                    Success = true,
                    Message = "Tạo phiếu nhập kho và chi tiết thành công.",
                    Data = responseDTO
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating full Vaccine Receipt");
                return new BaseResponse<VaccineReceiptDetailResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã có lỗi khi tạo phiếu nhập kho và chi tiết, vui lòng thử lại sau!",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<VaccineReceiptDetailResponseDTO>> CreateVaccineReceiptDetailAsync(CreateVaccineReceiptDetailDTO createVaccineReceiptDetailDTO, CancellationToken cancellationToken)
        {
            if (createVaccineReceiptDetailDTO == null)
            {
                return new BaseResponse<VaccineReceiptDetailResponseDTO>
                {
                    Code = 400,
                    Success = false,
                    Message = "Dữ liệu không hợp lệ",
                    Data = null
                };
            }
            try
            {
                // 1. Kiểm tra VaccineReceiptId có hợp lệ không
                var vaccineReceipt = await _vaccineReceiptRepository.GetVaccineReceiptByIdAsync(createVaccineReceiptDetailDTO.VaccineReceiptId, cancellationToken);
                if (vaccineReceipt == null)
                {
                    return new BaseResponse<VaccineReceiptDetailResponseDTO>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Phiếu nhập kho không tồn tại.",
                        Data = null
                    };
                }
                // 1.1. Kiểm tra VaccineBatchId có hợp lệ không
                var vaccineBatch = await _vaccineBatchRepository.GetVaccineBatchByIdAsync(createVaccineReceiptDetailDTO.VaccineBatchId, cancellationToken);
                if (vaccineBatch == null)
                {
                    return new BaseResponse<VaccineReceiptDetailResponseDTO>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Lô vắc xin không tồn tại.",
                        Data = null
                    };
                }
                // 2.Tạo VaccineReceiptDetail
                var vaccineReceiptDetail = new VaccineReceiptDetail
                {
                    VaccineReceiptId = createVaccineReceiptDetailDTO.VaccineReceiptId,
                    VaccineBatchId = createVaccineReceiptDetailDTO.VaccineBatchId,
                    Suppiler = createVaccineReceiptDetailDTO.Suppiler,
                    Quantity = createVaccineReceiptDetailDTO.Quantity,
                    VaccineStatus = createVaccineReceiptDetailDTO.VaccineStatus,
                    Notes = createVaccineReceiptDetailDTO.Notes,
                    CreatedAt = DateTimeHelper.Now(),
                    CreatedBy = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "System",
                    isDeleted = false
                };
                var vaccineReceiptDetailId = await _vaccineReceiptDetailRepository.CreateVaccineReceiptDetailAsync(vaccineReceiptDetail, cancellationToken);

                // 2.1. Tăng quantity của VaccineBatch
                if (vaccineBatch != null)
                {
                    vaccineBatch.Quantity += createVaccineReceiptDetailDTO.Quantity;
                    await _vaccineBatchRepository.UpdateVaccineBatchAsync(vaccineBatch, cancellationToken);
                }

                // 3. Tạo ColdChainLog
                var coldChainLogDTO = createVaccineReceiptDetailDTO.ColdChainLog;
                var coldChainLog = new ColdChainLog
                {
                    VaccineBatchId = coldChainLogDTO?.VaccineBatchId ?? createVaccineReceiptDetailDTO.VaccineBatchId,
                    LogTime = coldChainLogDTO?.LogTime ?? DateTimeHelper.Now(),
                    Temperature = coldChainLogDTO?.Temperature ?? 0,
                    Humidity = coldChainLogDTO?.Humidity ?? 0,
                    Event = coldChainLogDTO?.Event ?? "Nhập kho",
                    Notes = coldChainLogDTO?.Notes ?? $"Tạo log cho chi tiết nhập kho có id: {vaccineReceiptDetailId}",
                    RecordedAt = DateTimeHelper.Now(),
                    RecordedBy = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "System",
                    isDeleted = false
                };
                await _coldChainLogRepository.CreateColdChainLogAsync(coldChainLog, cancellationToken);

                // 4. Lấy lại VaccineReceiptDetail vừa tạo để trả về
                var createdDetail = await _vaccineReceiptDetailRepository.GetVaccineReceiptDetailByIdAsync(vaccineReceiptDetailId, cancellationToken);
                var responseDTO = _mapper.Map<VaccineReceiptDetailResponseDTO>(createdDetail);

                return new BaseResponse<VaccineReceiptDetailResponseDTO>
                {
                    Code = 201,
                    Success = true,
                    Message = "Tạo chi tiết phiếu nhập kho thành công.",
                    Data = responseDTO
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating Vaccine Receipt Detail");
                return new BaseResponse<VaccineReceiptDetailResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã có lỗi khi tạo phiếu nhập kho cho lô vắc xin, vui lòng thử lại sau!",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<bool>> DeleteVaccineReceiptDetailAsync(int vaccineReceiptDetailId, CancellationToken cancellationToken)
        {
            if (vaccineReceiptDetailId <= 0)
            {
                return new BaseResponse<bool>
                {
                    Code = 400,
                    Success = false,
                    Message = "ID chi tiết phiếu nhập kho không hợp lệ.",
                    Data = false
                };
            }
            try
            {
                var detailToDelete = await _vaccineReceiptDetailRepository.GetVaccineReceiptDetailByIdAsync(vaccineReceiptDetailId, cancellationToken);
                if (detailToDelete == null || detailToDelete.isDeleted == true)
                {
                    return new BaseResponse<bool>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Chi tiết phiếu nhập kho không tồn tại hoặc đã bị xóa.",
                        Data = false
                    };
                }
                detailToDelete.isDeleted = true;
                var result = await _vaccineReceiptDetailRepository.UpdateVaccineReceiptDetailAsync(detailToDelete, cancellationToken);
                return new BaseResponse<bool>
                {
                    Code = 200,
                    Success = true,
                    Message = "Xóa chi tiết phiếu nhập kho thành công.",
                    Data = result > 0
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting Vaccine Receipt Detail");
                return new BaseResponse<bool>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã có lỗi khi xóa chi tiết phiếu nhập kho, vui lòng thử lại sau!",
                    Data = false
                };
            }
        }

        public async Task<DynamicResponse<VaccineReceiptDetailResponseDTO>> GetAllVaccineReceiptDetailsAsync(GetAllItemsDTO getAllItemsDTO, CancellationToken cancellationToken)
        {
            try
            {
                var vaccineReceiptDetails = await _vaccineReceiptDetailRepository.GetAllVaccineReceiptDetailsAsync(cancellationToken);
                if (!string.IsNullOrWhiteSpace(getAllItemsDTO.KeyWord))
                {
                    vaccineReceiptDetails = vaccineReceiptDetails
                        .Where(vrd => vrd.VaccineBatch?.Vaccine?.Name.Contains(getAllItemsDTO.KeyWord, StringComparison.OrdinalIgnoreCase) == true ||
                                      vrd.VaccineBatch?.Vaccine?.VaccineCode.Contains(getAllItemsDTO.KeyWord, StringComparison.OrdinalIgnoreCase) == true ||
                                      vrd.Suppiler.Contains(getAllItemsDTO.KeyWord, StringComparison.OrdinalIgnoreCase) ||
                                      vrd.VaccineStatus.Contains(getAllItemsDTO.KeyWord, StringComparison.OrdinalIgnoreCase) ||
                                      vrd.Quantity.ToString().Contains(getAllItemsDTO.KeyWord, StringComparison.OrdinalIgnoreCase) ||
                                      vrd.Notes.Contains(getAllItemsDTO.KeyWord, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                }
                int pageNumber = getAllItemsDTO?.PageNumber > 0 ? getAllItemsDTO.PageNumber : 1;
                int pageSize = getAllItemsDTO?.PageSize > 0 ? getAllItemsDTO.PageSize : 10;
                int skip = (pageNumber - 1) * pageSize;
                int totalItem = vaccineReceiptDetails.Count;
                int totalPage = (int)Math.Ceiling((double)totalItem / pageSize);

                var pagedDetails = vaccineReceiptDetails
                    .Skip(skip)
                    .Take(pageSize)
                    .Select(vrd => _mapper.Map<VaccineReceiptDetailResponseDTO>(vrd))
                    .ToList();

                var response = new MegaData<VaccineReceiptDetailResponseDTO>
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
                        status = getAllItemsDTO?.Status,
                    },
                    PageData = _mapper.Map<List<VaccineReceiptDetailResponseDTO>>(pagedDetails)
                };
                if (pagedDetails.Any())
                {
                    return new DynamicResponse<VaccineReceiptDetailResponseDTO>
                    {
                        Code = 200,
                        Success = true,
                        Message = "Lấy danh sách chi tiết phiếu nhập kho thành công.",
                        Data = response
                    };
                }
                else
                {
                    return new DynamicResponse<VaccineReceiptDetailResponseDTO>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Không tìm thấy chi tiết phiếu nhập kho nào.",
                        Data = response
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all Vaccine Receipt Details");
                return new DynamicResponse<VaccineReceiptDetailResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã có lỗi khi lấy danh sách chi tiết phiếu nhập kho, vui lòng thử lại sau!",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<VaccineReceiptDetailResponseDTO>> GetVaccineReceiptDetailByIdAsync(int vaccineReceiptDetailId, CancellationToken cancellationToken)
        {
            if (vaccineReceiptDetailId <= 0)
            {
                return new BaseResponse<VaccineReceiptDetailResponseDTO>
                {
                    Code = 400,
                    Success = false,
                    Message = "ID chi tiết phiếu nhập kho không hợp lệ.",
                    Data = null
                };
            }
            try
            {
                var detail = await _vaccineReceiptDetailRepository.GetVaccineReceiptDetailByIdAsync(vaccineReceiptDetailId, cancellationToken);
                if (detail == null)
                {
                    return new BaseResponse<VaccineReceiptDetailResponseDTO>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Chi tiết phiếu nhập kho không tồn tại.",
                        Data = null
                    };
                }
                var responseDTO = _mapper.Map<VaccineReceiptDetailResponseDTO>(detail);
                return new BaseResponse<VaccineReceiptDetailResponseDTO>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy chi tiết phiếu nhập kho thành công.",
                    Data = responseDTO
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving Vaccine Receipt Detail by ID");
                return new BaseResponse<VaccineReceiptDetailResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã có lỗi khi lấy chi tiết phiếu nhập kho, vui lòng thử lại sau!",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<VaccineReceiptDetailResponseDTO>> GetVaccineReceiptDetailByVaccineBatchId(int vaccineBatchId, CancellationToken cancellationToken)
        {
            if (vaccineBatchId <= 0)
            {
                return new BaseResponse<VaccineReceiptDetailResponseDTO>
                {
                    Code = 400,
                    Success = false,
                    Message = "ID lô vắc xin không hợp lệ.",
                    Data = null
                };
            }
            try
            {
                var detail = await _vaccineReceiptDetailRepository.GetVaccineReceiptDetailByVaccineBatchIdAsync(vaccineBatchId, cancellationToken);
                if (detail == null)
                {
                    return new BaseResponse<VaccineReceiptDetailResponseDTO>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Chi tiết phiếu nhập kho cho lô vắc xin này không tồn tại.",
                        Data = null
                    };
                }
                var responseDTO = _mapper.Map<VaccineReceiptDetailResponseDTO>(detail);
                return new BaseResponse<VaccineReceiptDetailResponseDTO>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy chi tiết phiếu nhập kho theo lô vắc xin thành công.",
                    Data = responseDTO
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving Vaccine Receipt Detail by Vaccine Batch ID");
                return new BaseResponse<VaccineReceiptDetailResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã có lỗi khi lấy chi tiết phiếu nhập kho theo lô vắc xin, vui lòng thử lại sau!",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<List<VaccineReceiptDetailResponseDTO>>> GetVaccineReceiptDetailsByVaccineReceiptIdAsync(int vaccineReceiptId, CancellationToken cancellationToken)
        {
            if (vaccineReceiptId <= 0)
            {
                return new BaseResponse<List<VaccineReceiptDetailResponseDTO>>
                {
                    Code = 400,
                    Success = false,
                    Message = "ID phiếu nhập kho không hợp lệ.",
                    Data = null
                };
            }
            try
            {
                var details = await _vaccineReceiptDetailRepository.GetVaccineReceiptDetailsByVaccineReceiptIdAsync(vaccineReceiptId, cancellationToken);
                if (details == null || !details.Any())
                {
                    return new BaseResponse<List<VaccineReceiptDetailResponseDTO>>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Không tìm thấy chi tiết phiếu nhập kho nào cho phiếu nhập kho này.",
                        Data = null
                    };
                }
                var responseDTOs = _mapper.Map<List<VaccineReceiptDetailResponseDTO>>(details);
                return new BaseResponse<List<VaccineReceiptDetailResponseDTO>>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy danh sách chi tiết phiếu nhập kho theo phiếu nhập kho thành công.",
                    Data = responseDTOs
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving Vaccine Receipt Details by Vaccine Receipt ID");
                return new BaseResponse<List<VaccineReceiptDetailResponseDTO>>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã có lỗi khi lấy danh sách chi tiết phiếu nhập kho theo phiếu nhập kho, vui lòng thử lại sau!",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<VaccineReceiptDetailResponseDTO>> UpdateVaccineReceiptDetailAsync(int vaccineReceiptDetailId, UpdateVaccineReceiptDetailDTO updateVaccineReceiptDetailDTO, CancellationToken cancellationToken)
        {
            if (updateVaccineReceiptDetailDTO == null || vaccineReceiptDetailId <= 0)
            {
                return new BaseResponse<VaccineReceiptDetailResponseDTO>
                {
                    Code = 400,
                    Success = false,
                    Message = "Dữ liệu cập nhật không hợp lệ.",
                    Data = null
                };
            }
            try
            {
                var existingDetail = await _vaccineReceiptDetailRepository.GetVaccineReceiptDetailByIdAsync(vaccineReceiptDetailId, cancellationToken);
                if (existingDetail == null)
                {
                    return new BaseResponse<VaccineReceiptDetailResponseDTO>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Chi tiết phiếu nhập kho không tồn tại.",
                        Data = null
                    };
                }

                // Nếu Quantity có giá trị cập nhật, cập nhật cả VaccineBatch.Quantity
                if (updateVaccineReceiptDetailDTO.Quantity.HasValue)
                {
                    var vaccineBatch = await _vaccineBatchRepository.GetVaccineBatchByIdAsync(existingDetail.VaccineBatchId, cancellationToken);
                    if (vaccineBatch != null)
                    {
                        // Trừ quantity cũ, cộng quantity mới
                        vaccineBatch.Quantity = vaccineBatch.Quantity - existingDetail.Quantity + updateVaccineReceiptDetailDTO.Quantity.Value;
                        await _vaccineBatchRepository.UpdateVaccineBatchAsync(vaccineBatch, cancellationToken);
                    }
                }

                // Cập nhật thông tin chi tiết phiếu nhập kho
                existingDetail.VaccineReceiptId = updateVaccineReceiptDetailDTO.VaccineReceiptId ?? existingDetail.VaccineReceiptId;
                existingDetail.VaccineBatchId = updateVaccineReceiptDetailDTO.VaccineBatchId ?? existingDetail.VaccineBatchId;
                existingDetail.Suppiler = updateVaccineReceiptDetailDTO.Suppiler ?? existingDetail.Suppiler;
                existingDetail.Quantity = updateVaccineReceiptDetailDTO.Quantity ?? existingDetail.Quantity;
                existingDetail.VaccineStatus = updateVaccineReceiptDetailDTO.VaccineStatus ?? existingDetail.VaccineStatus;
                existingDetail.Notes = updateVaccineReceiptDetailDTO.Notes ?? existingDetail.Notes;
                existingDetail.ModifiedAt = DateTimeHelper.Now();
                existingDetail.ModifiedBy = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "System";
                var updatedId = await _vaccineReceiptDetailRepository.UpdateVaccineReceiptDetailAsync(existingDetail, cancellationToken);

                // Cập nhật ColdChainLog nếu có
                if (updateVaccineReceiptDetailDTO.ColdChainLog != null)
                {
                    var coldChainLogs = await _coldChainLogRepository.GetColdChainLogsByVaccineBatchIdAsync(existingDetail.VaccineBatchId, cancellationToken);
                    var coldChainLog = coldChainLogs.FirstOrDefault();
                    if (coldChainLog != null)
                    {
                        coldChainLog.VaccineBatchId = updateVaccineReceiptDetailDTO.ColdChainLog.VaccineBatchId ?? coldChainLog.VaccineBatchId;
                        coldChainLog.LogTime = updateVaccineReceiptDetailDTO.ColdChainLog.LogTime ?? coldChainLog.LogTime;
                        coldChainLog.Temperature = updateVaccineReceiptDetailDTO.ColdChainLog.Temperature ?? coldChainLog.Temperature;
                        coldChainLog.Humidity = updateVaccineReceiptDetailDTO.ColdChainLog.Humidity ?? coldChainLog.Humidity;
                        coldChainLog.Event = updateVaccineReceiptDetailDTO.ColdChainLog.Event ?? coldChainLog.Event;
                        coldChainLog.Notes = updateVaccineReceiptDetailDTO.ColdChainLog.Notes ?? coldChainLog.Notes;
                        coldChainLog.ModifiedAt = DateTimeHelper.Now();
                        coldChainLog.ModifiedBy = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "System";
                        await _coldChainLogRepository.UpdateColdChainLogAsync(coldChainLog, cancellationToken);
                    }
                }

                // Lấy lại chi tiết đã cập nhật để trả về
                var updatedDetail = await _vaccineReceiptDetailRepository.GetVaccineReceiptDetailByIdAsync(updatedId, cancellationToken);
                var responseDTO = _mapper.Map<VaccineReceiptDetailResponseDTO>(updatedDetail);
                return new BaseResponse<VaccineReceiptDetailResponseDTO>
                {
                    Code = 200,
                    Success = true,
                    Message = "Cập nhật chi tiết phiếu nhập kho thành công.",
                    Data = responseDTO
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating Vaccine Receipt Detail");
                return new BaseResponse<VaccineReceiptDetailResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã có lỗi khi cập nhật chi tiết phiếu nhập kho, vui lòng thử lại sau!",
                    Data = null
                };
            }
        }
    }
}
