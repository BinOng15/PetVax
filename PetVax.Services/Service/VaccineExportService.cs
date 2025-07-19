using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PetVax.BusinessObjects.DTO;
using PetVax.BusinessObjects.DTO.VaccineExportDTO;
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
    public class VaccineExportService : IVaccineExportService
    {
        private readonly IVaccineExportRepository _vaccineExportRepository;
        private readonly ILogger<VaccineExportService> _logger;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public VaccineExportService(
            IVaccineExportRepository vaccineExportRepository,
            ILogger<VaccineExportService> logger,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            _vaccineExportRepository = vaccineExportRepository;
            _logger = logger;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<BaseResponse<VaccineExportResponseDTO>> CreateVaccineExportAsync(CreateVaccineExportDTO createVaccineExportDTO, CancellationToken cancellationToken)
        {
            if (createVaccineExportDTO == null)
            {
                _logger.LogError("CreateVaccineExportAsync: createVaccineExportDTO is null");
                return new BaseResponse<VaccineExportResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Dữ liệu để tạo phiếu xuất kho không hợp lệ",
                    Data = null
                };
            }
            try
            {
                var vaccineExport = _mapper.Map<VaccineExport>(createVaccineExportDTO);
                vaccineExport.ExportDate = DateTimeHelper.Now();
                vaccineExport.ExportCode = "EXPORT" + new Random().Next(100000, 1000000).ToString();
                vaccineExport.CreatedAt = DateTimeHelper.Now();
                vaccineExport.CreatedBy = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "System";

                int createdId = await _vaccineExportRepository.CreateVaccineExportAsync(vaccineExport, cancellationToken);
                if (createdId <= 0)
                {
                    _logger.LogError("CreateVaccineExportAsync: Failed to create vaccine export");
                    return new BaseResponse<VaccineExportResponseDTO>
                    {
                        Code = 500,
                        Success = false,
                        Message = "Không thể tạo phiếu xuất kho",
                        Data = null
                    };
                }

                var responseDTO = _mapper.Map<VaccineExportResponseDTO>(vaccineExport);
                responseDTO.VaccineExportId = createdId;
                return new BaseResponse<VaccineExportResponseDTO>
                {
                    Code = 200,
                    Success = true,
                    Message = "Tạo phiếu xuất kho thành công",
                    Data = responseDTO
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CreateVaccineExportAsync: An error occurred while creating vaccine export");
                return new BaseResponse<VaccineExportResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã xảy ra lỗi khi tạo phiếu xuất kho",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<bool>> DeleteVaccineExportAsync(int vaccineExportId, CancellationToken cancellationToken)
        {
            if (vaccineExportId <= 0)
            {
                _logger.LogError("DeleteVaccineExportAsync: Invalid vaccineExportId");
                return new BaseResponse<bool>
                {
                    Code = 400,
                    Success = false,
                    Message = "ID phiếu xuất kho không hợp lệ",
                    Data = false
                };
            }
            try
            {
                var vaccineExport = await _vaccineExportRepository.GetVaccineExportByIdAsync(vaccineExportId, cancellationToken);
                if (vaccineExport == null)
                {
                    _logger.LogError("DeleteVaccineExportAsync: Vaccine export not found with ID {vaccineExportId}", vaccineExportId);
                    return new BaseResponse<bool>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Phiếu xuất kho không tồn tại",
                        Data = false
                    };
                }

                vaccineExport.isDeleted = true;
                int result = await _vaccineExportRepository.UpdateVaccineExportAsync(vaccineExport, cancellationToken);
                if (result <= 0)
                {
                    _logger.LogError("DeleteVaccineExportAsync: Failed to delete vaccine export with ID {vaccineExportId}", vaccineExportId);
                    return new BaseResponse<bool>
                    {
                        Code = 500,
                        Success = false,
                        Message = "Không thể xóa phiếu xuất kho",
                        Data = false
                    };
                }
                return new BaseResponse<bool>
                {
                    Code = 200,
                    Success = true,
                    Message = "Xóa phiếu xuất kho thành công",
                    Data = true
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "DeleteVaccineExportAsync: An error occurred while deleting vaccine export with ID {vaccineExportId}", vaccineExportId);
                return new BaseResponse<bool>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã xảy ra lỗi khi xóa phiếu xuất kho",
                    Data = false
                };
            }
        }

        public async Task<DynamicResponse<VaccineExportResponseDTO>> GetAllVaccineExportsAsync(GetAllItemsDTO getAllItemsDTO, CancellationToken cancellationToken)
        {
            try
            {
                var vaccineExports = await _vaccineExportRepository.GetAllVaccineExportsAsync(cancellationToken);
                if (!string.IsNullOrWhiteSpace(getAllItemsDTO.KeyWord))
                {
                    vaccineExports = vaccineExports
                        .Where(ve => ve.ExportCode.Contains(getAllItemsDTO.KeyWord, StringComparison.OrdinalIgnoreCase) ||
                                     ve.ExportDate.ToString("dd/MM/yyyy").Contains(getAllItemsDTO.KeyWord))
                        .ToList();
                }

                int pageNumber = getAllItemsDTO?.PageNumber > 0 ? getAllItemsDTO.PageNumber : 1;
                int pageSize = getAllItemsDTO?.PageSize > 0 ? getAllItemsDTO.PageSize : 10;
                int skip = (pageNumber - 1) * pageSize;
                int totalItem = vaccineExports.Count;
                int totalPage = (int)Math.Ceiling((double)totalItem / pageSize);

                var pagedExports = vaccineExports
                    .Skip(skip)
                    .Take(pageSize)
                    .Select(ve => _mapper.Map<VaccineExportResponseDTO>(ve))
                    .ToList();

                var response = new MegaData<VaccineExportResponseDTO>
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
                    PageData = _mapper.Map<List<VaccineExportResponseDTO>>(pagedExports)
                };
                if (pagedExports.Any())
                {
                    return new DynamicResponse<VaccineExportResponseDTO>
                    {
                        Code = 200,
                        Success = true,
                        Message = "Lấy danh sách phiếu xuất kho thành công",
                        Data = response
                    };
                }
                else
                {
                    return new DynamicResponse<VaccineExportResponseDTO>
                    {
                        Code = 200,
                        Success = false,
                        Message = "Không tìm thấy phiếu xuất kho nào",
                        Data = null
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetAllVaccineExportsAsync: An error occurred while retrieving all vaccine exports");
                return new DynamicResponse<VaccineExportResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã xảy ra lỗi khi lấy danh sách phiếu xuất kho",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<VaccineExportResponseDTO>> GetVaccineExportByExportCodeAsync(string exportCode, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(exportCode))
            {
                _logger.LogError("GetVaccineExportByExportCodeAsync: exportCode is null or empty");
                return new BaseResponse<VaccineExportResponseDTO>
                {
                    Code = 400,
                    Success = false,
                    Message = "Mã phiếu xuất kho không hợp lệ",
                    Data = null
                };
            }
            try
            {
                var vaccineExport = await _vaccineExportRepository.GetVaccineExportByExportCodeAsync(exportCode, cancellationToken);
                if (vaccineExport == null)
                {
                    _logger.LogError("GetVaccineExportByExportCodeAsync: Vaccine export not found with export code {exportCode}", exportCode);
                    return new BaseResponse<VaccineExportResponseDTO>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Phiếu xuất kho không tồn tại",
                        Data = null
                    };
                }
                var responseDTO = _mapper.Map<VaccineExportResponseDTO>(vaccineExport);
                return new BaseResponse<VaccineExportResponseDTO>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy phiếu xuất kho thành công",
                    Data = responseDTO
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetVaccineExportByExportCodeAsync: An error occurred while retrieving vaccine export by export code {exportCode}", exportCode);
                return new BaseResponse<VaccineExportResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã xảy ra lỗi khi lấy phiếu xuất kho",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<VaccineExportResponseDTO>> GetVaccineExportByExportDateAsync(DateTime exportDate, CancellationToken cancellationToken)
        {
            try
            {
                var vaccineExports = await _vaccineExportRepository.GetAllVaccineExportsAsync(cancellationToken);
                var vaccineExport = vaccineExports.FirstOrDefault(ve => ve.ExportDate.Date == exportDate.Date);
                if (vaccineExport == null)
                {
                    _logger.LogError("GetVaccineExportByExportDateAsync: No vaccine export found for date {exportDate}", exportDate);
                    return new BaseResponse<VaccineExportResponseDTO>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Không tìm thấy phiếu xuất kho cho ngày này",
                        Data = null
                    };
                }
                var responseDTO = _mapper.Map<VaccineExportResponseDTO>(vaccineExport);
                return new BaseResponse<VaccineExportResponseDTO>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy phiếu xuất kho theo ngày thành công",
                    Data = responseDTO
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetVaccineExportByExportDateAsync: An error occurred while retrieving vaccine export by export date {exportDate}", exportDate);
                return new BaseResponse<VaccineExportResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã xảy ra lỗi khi lấy phiếu xuất kho theo ngày",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<VaccineExportResponseDTO>> GetVaccineExportByIdAsync(int vaccineExportId, CancellationToken cancellationToken)
        {
            if (vaccineExportId <= 0)
            {
                _logger.LogError("GetVaccineExportByIdAsync: Invalid vaccineExportId");
                return new BaseResponse<VaccineExportResponseDTO>
                {
                    Code = 400,
                    Success = false,
                    Message = "ID phiếu xuất kho không hợp lệ",
                    Data = null
                };
            }
            try
            {
                var vaccineExport = await _vaccineExportRepository.GetVaccineExportByIdAsync(vaccineExportId, cancellationToken);
                if (vaccineExport == null)
                {
                    _logger.LogError("GetVaccineExportByIdAsync: Vaccine export not found with ID {vaccineExportId}", vaccineExportId);
                    return new BaseResponse<VaccineExportResponseDTO>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Phiếu xuất kho không tồn tại",
                        Data = null
                    };
                }
                var responseDTO = _mapper.Map<VaccineExportResponseDTO>(vaccineExport);
                return new BaseResponse<VaccineExportResponseDTO>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy phiếu xuất kho thành công",
                    Data = responseDTO
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetVaccineExportByIdAsync: An error occurred while retrieving vaccine export by ID {vaccineExportId}", vaccineExportId);
                return new BaseResponse<VaccineExportResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã xảy ra lỗi khi lấy phiếu xuất kho",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<VaccineExportResponseDTO>> UpdateVaccineExportAsync(int exportId, UpdateVaccineExportDTO updateVaccineExportDTO, CancellationToken cancellationToken)
        {
            if (updateVaccineExportDTO == null || exportId <= 0)
            {
                _logger.LogError("UpdateVaccineExportAsync: Invalid updateVaccineExportDTO");
                return new BaseResponse<VaccineExportResponseDTO>
                {
                    Code = 400,
                    Success = false,
                    Message = "Dữ liệu để cập nhật phiếu xuất kho không hợp lệ",
                    Data = null
                };
            }
            try
            {
                var existingExport = await _vaccineExportRepository.GetVaccineExportByIdAsync(exportId, cancellationToken);
                if (existingExport == null)
                {
                    _logger.LogError("UpdateVaccineExportAsync: Vaccine export not found with ID {vaccineExportId}", exportId);
                    return new BaseResponse<VaccineExportResponseDTO>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Phiếu xuất kho không tồn tại",
                        Data = null
                    };
                }
                existingExport.ExportDate = updateVaccineExportDTO.ExportDate ?? existingExport.ExportDate;
                existingExport.ModifiedAt = DateTimeHelper.Now();
                existingExport.ModifiedBy = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "System";
                int result = await _vaccineExportRepository.UpdateVaccineExportAsync(existingExport, cancellationToken);
                if (result <= 0)
                {
                    _logger.LogError("UpdateVaccineExportAsync: Failed to update vaccine export with ID {vaccineExportId}", exportId);
                    return new BaseResponse<VaccineExportResponseDTO>
                    {
                        Code = 500,
                        Success = false,
                        Message = "Không thể cập nhật phiếu xuất kho",
                        Data = null
                    };
                }
                var responseDTO = _mapper.Map<VaccineExportResponseDTO>(existingExport);
                return new BaseResponse<VaccineExportResponseDTO>
                {
                    Code = 200,
                    Success = true,
                    Message = "Cập nhật phiếu xuất kho thành công",
                    Data = responseDTO
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "UpdateVaccineExportAsync: An error occurred while updating vaccine export with ID {vaccineExportId}", exportId);
                return new BaseResponse<VaccineExportResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã xảy ra lỗi khi cập nhật phiếu xuất kho",
                    Data = null
                };
            }
        }
    }
}
