using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PetVax.BusinessObjects.DTO;
using PetVax.BusinessObjects.DTO.VaccineReceiptDTO;
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
    public class VaccineReceiptService : IVaccineReceiptService
    {
        private readonly IVaccineReceiptRepository _vaccineReceiptRepository;
        private readonly ILogger<VaccineReceiptService> _logger;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public VaccineReceiptService(
            IVaccineReceiptRepository vaccineReceiptRepository,
            ILogger<VaccineReceiptService> logger,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            _vaccineReceiptRepository = vaccineReceiptRepository;
            _logger = logger;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<BaseResponse<VaccineReceiptResponseDTO>> CreateVaccineReceiptAsync(CreateVaccineReceiptDTO createVaccineReceiptDTO, CancellationToken cancellationToken)
        {
            if (createVaccineReceiptDTO == null)
            {
                _logger.LogError("CreateVaccineReceiptAsync: createVaccineReceiptDTO is null");
                return new BaseResponse<VaccineReceiptResponseDTO>
                {
                    Code = 400,
                    Success = false,
                    Message = "Dữ liệu để tạo phiếu nhập kho không hợp lệ.",
                    Data = null
                };
            }
            try
            {
                var vaccineReceipt = _mapper.Map<VaccineReceipt>(createVaccineReceiptDTO);
                vaccineReceipt.ReceiptDate = DateTimeHelper.Now();
                vaccineReceipt.ReceiptCode = "RECEIPT" + new Random().Next(100000, 1000000).ToString();
                vaccineReceipt.CreatedAt = DateTimeHelper.Now();
                vaccineReceipt.CreatedBy = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "System";
                // Call repository to create the vaccine receipt
                int createdId = await _vaccineReceiptRepository.CreateVaccineReceiptAsync(vaccineReceipt, cancellationToken);
                if (createdId <= 0)
                {
                    _logger.LogError("CreateVaccineReceiptAsync: Failed to create vaccine receipt");
                    return new BaseResponse<VaccineReceiptResponseDTO>
                    {
                        Code = 500,
                        Success = false,
                        Message = "Không thể tạo phiếu nhập kho.",
                        Data = null
                    };
                }
                var responseDTO = _mapper.Map<VaccineReceiptResponseDTO>(vaccineReceipt);
                responseDTO.VaccineReceiptId = createdId;
                return new BaseResponse<VaccineReceiptResponseDTO>
                {
                    Code = 201,
                    Success = true,
                    Message = "Phiếu xuất kho đã được tạo thành công.",
                    Data = responseDTO
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CreateVaccineReceiptAsync: An error occurred while creating vaccine receipt");
                return new BaseResponse<VaccineReceiptResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã xảy ra lỗi khi tạo phiếu nhập kho.",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<bool>> DeleteVaccineReceiptAsync(int vaccineReceiptId, CancellationToken cancellationToken)
        {
            if (vaccineReceiptId <= 0)
            {
                _logger.LogError("DeleteVaccineReceiptAsync: Invalid vaccineReceiptId");
                return new BaseResponse<bool>
                {
                    Code = 400,
                    Success = false,
                    Message = "ID phiếu nhập kho không hợp lệ.",
                    Data = false
                };
            }
            try
            {
                var vaccineReceipt = await _vaccineReceiptRepository.GetVaccineReceiptByIdAsync(vaccineReceiptId, cancellationToken);
                if (vaccineReceipt == null)
                {
                    _logger.LogError("DeleteVaccineReceiptAsync: Vaccine receipt not found");
                    return new BaseResponse<bool>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Phiếu nhập kho không tồn tại.",
                        Data = false
                    };
                }

                vaccineReceipt.isDeleted = true;
                int updated = await _vaccineReceiptRepository.UpdateVaccineReceiptAsync(vaccineReceipt, cancellationToken);
                if (updated <= 0)
                {
                    _logger.LogError("DeleteVaccineReceiptAsync: Failed to update isDeleted flag");
                    return new BaseResponse<bool>
                    {
                        Code = 500,
                        Success = false,
                        Message = "Không thể xóa phiếu nhập kho.",
                        Data = false
                    };
                }
                return new BaseResponse<bool>
                {
                    Code = 200,
                    Success = true,
                    Message = "Phiếu nhập kho đã được xóa thành công.",
                    Data = true
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "DeleteVaccineReceiptAsync: An error occurred while deleting vaccine receipt");
                return new BaseResponse<bool>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã xảy ra lỗi khi xóa phiếu nhập kho.",
                    Data = false
                };
            }
        }

        public async Task<DynamicResponse<VaccineReceiptResponseDTO>> GetAllVaccineReceiptsAsync(GetAllItemsDTO getAllItemsDTO, CancellationToken cancellationToken)
        {
            try
            {
                var vaccineReceipts = await _vaccineReceiptRepository.GetAllVaccineReceiptsAsync(cancellationToken);
                if (!string.IsNullOrWhiteSpace(getAllItemsDTO.KeyWord))
                {
                    vaccineReceipts = vaccineReceipts
                         .Where(vr => vr.ReceiptCode.Contains(getAllItemsDTO.KeyWord, StringComparison.OrdinalIgnoreCase) ||
                                      vr.ReceiptDate.ToString("dd/MM/yyyy").Contains(getAllItemsDTO.KeyWord))
                         .ToList();
                }
                int pageNumber = getAllItemsDTO?.PageNumber > 0 ? getAllItemsDTO.PageNumber : 1;
                int pageSize = getAllItemsDTO?.PageSize > 0 ? getAllItemsDTO.PageSize : 10;
                int skip = (pageNumber - 1) * pageSize;
                int totalItem = vaccineReceipts.Count;
                int totalPage = (int)Math.Ceiling((double)totalItem / pageSize);

                var pagedReceipts = vaccineReceipts
                    .Skip(skip)
                    .Take(pageSize)
                    .Select(vr => _mapper.Map<VaccineReceiptResponseDTO>(vr))
                    .ToList();

                var response = new MegaData<VaccineReceiptResponseDTO>
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
                    PageData = _mapper.Map<List<VaccineReceiptResponseDTO>>(pagedReceipts)
                };
                if (pagedReceipts.Any())
                {
                    return new DynamicResponse<VaccineReceiptResponseDTO>
                    {
                        Code = 200,
                        Success = true,
                        Message = "Danh sách phiếu nhập kho đã được lấy thành công.",
                        Data = response
                    };
                }
                else
                {
                    return new DynamicResponse<VaccineReceiptResponseDTO>
                    {
                        Code = 200,
                        Success = false,
                        Message = "Không tìm thấy phiếu nhập kho nào.",
                        Data = null
                    };

                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetAllVaccineReceiptsAsync: An error occurred while retrieving vaccine receipts");
                return new DynamicResponse<VaccineReceiptResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã xảy ra lỗi khi lấy danh sách phiếu nhập kho.",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<VaccineReceiptResponseDTO>> GetVaccineReceiptByIdAsync(int vaccineReceiptId, CancellationToken cancellationToken)
        {
            if (vaccineReceiptId <= 0)
            {
                _logger.LogError("GetVaccineReceiptByIdAsync: Invalid vaccineReceiptId");
                return new BaseResponse<VaccineReceiptResponseDTO>
                {
                    Code = 400,
                    Success = false,
                    Message = "ID phiếu nhập kho không hợp lệ.",
                    Data = null
                };
            }
            try
            {
                var vaccineReceipt = await _vaccineReceiptRepository.GetVaccineReceiptByIdAsync(vaccineReceiptId, cancellationToken);
                if (vaccineReceipt == null || vaccineReceipt.isDeleted == true)
                {
                    _logger.LogError("GetVaccineReceiptByIdAsync: Vaccine receipt not found or deleted");
                    return new BaseResponse<VaccineReceiptResponseDTO>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Phiếu nhập kho không tồn tại hoặc đã bị xóa.",
                        Data = null
                    };
                }
                var responseDTO = _mapper.Map<VaccineReceiptResponseDTO>(vaccineReceipt);
                return new BaseResponse<VaccineReceiptResponseDTO>
                {
                    Code = 200,
                    Success = true,
                    Message = "Phiếu nhập kho đã được lấy thành công.",
                    Data = responseDTO
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetVaccineReceiptByIdAsync: An error occurred while retrieving vaccine receipt");
                return new BaseResponse<VaccineReceiptResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã xảy ra lỗi khi lấy phiếu nhập kho.",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<VaccineReceiptResponseDTO>> GetVaccineReceiptByReceiptCodeAsync(string receiptCode, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(receiptCode))
            {
                _logger.LogError("GetVaccineReceiptByReceiptCodeAsync: Invalid receiptCode");
                return new BaseResponse<VaccineReceiptResponseDTO>
                {
                    Code = 400,
                    Success = false,
                    Message = "Mã phiếu nhập kho không hợp lệ.",
                    Data = null
                };
            }
            try
            {
                var vaccineReceipt = await _vaccineReceiptRepository.GetVaccineReceiptByReceiptCodeAsync(receiptCode, cancellationToken);
                if (vaccineReceipt == null || vaccineReceipt.isDeleted == true)
                {
                    _logger.LogError("GetVaccineReceiptByReceiptCodeAsync: Vaccine receipt not found or deleted");
                    return new BaseResponse<VaccineReceiptResponseDTO>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Phiếu nhập kho không tồn tại hoặc đã bị xóa.",
                        Data = null
                    };
                }
                var responseDTO = _mapper.Map<VaccineReceiptResponseDTO>(vaccineReceipt);
                return new BaseResponse<VaccineReceiptResponseDTO>
                {
                    Code = 200,
                    Success = true,
                    Message = "Phiếu nhập kho đã được lấy thành công.",
                    Data = responseDTO
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetVaccineReceiptByReceiptCodeAsync: An error occurred while retrieving vaccine receipt by code");
                return new BaseResponse<VaccineReceiptResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã xảy ra lỗi khi lấy phiếu nhập kho theo mã.",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<VaccineReceiptResponseDTO>> GetVaccineReceiptByReceiptDateAsync(DateTime receiptDate, CancellationToken cancellationToken)
        {
            try
            {
                var vaccineReceipts = await _vaccineReceiptRepository.GetAllVaccineReceiptsAsync(cancellationToken);
                var filteredReceipts = vaccineReceipts
                    .Where(vr => vr.ReceiptDate.Date == receiptDate.Date && (vr.isDeleted == null || !vr.isDeleted.Value))
                    .ToList();
                if (!filteredReceipts.Any())
                {
                    _logger.LogError("GetVaccineReceiptByReceiptDateAsync: No vaccine receipts found for the given date");
                    return new BaseResponse<VaccineReceiptResponseDTO>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Không tìm thấy phiếu nhập kho nào cho ngày đã cho.",
                        Data = null
                    };
                }
                var responseDTO = _mapper.Map<VaccineReceiptResponseDTO>(filteredReceipts.First());
                return new BaseResponse<VaccineReceiptResponseDTO>
                {
                    Code = 200,
                    Success = true,
                    Message = "Phiếu nhập kho đã được lấy thành công.",
                    Data = responseDTO
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetVaccineReceiptByReceiptDateAsync: An error occurred while retrieving vaccine receipt by date");
                return new BaseResponse<VaccineReceiptResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã xảy ra lỗi khi lấy phiếu nhập kho theo ngày.",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<VaccineReceiptResponseDTO>> UpdateVaccineReceiptAsync(int receiptId, UpdateVaccineReceiptDTO updateVaccineReceiptDTO, CancellationToken cancellationToken)
        {
            if (updateVaccineReceiptDTO == null || receiptId <= 0)
            {
                _logger.LogError("UpdateVaccineReceiptAsync: Invalid updateVaccineReceiptDTO");
                return new BaseResponse<VaccineReceiptResponseDTO>
                {
                    Code = 400,
                    Success = false,
                    Message = "Dữ liệu để cập nhật phiếu nhập kho không hợp lệ.",
                    Data = null
                };
            }
            try
            {
                var existingReceipt = await _vaccineReceiptRepository.GetVaccineReceiptByIdAsync(receiptId, cancellationToken);
                if (existingReceipt == null || existingReceipt.isDeleted == true)
                {
                    _logger.LogError("UpdateVaccineReceiptAsync: Vaccine receipt not found or deleted");
                    return new BaseResponse<VaccineReceiptResponseDTO>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Phiếu nhập kho không tồn tại hoặc đã bị xóa.",
                        Data = null
                    };
                }
                // Map the updated fields
                existingReceipt.ReceiptDate = updateVaccineReceiptDTO.ReceiptDate ?? existingReceipt.ReceiptDate;
                existingReceipt.ModifiedAt = DateTimeHelper.Now();
                existingReceipt.ModifiedBy = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "System";
                int updatedId = await _vaccineReceiptRepository.UpdateVaccineReceiptAsync(existingReceipt, cancellationToken);
                if (updatedId <= 0)
                {
                    _logger.LogError("UpdateVaccineReceiptAsync: Failed to update vaccine receipt");
                    return new BaseResponse<VaccineReceiptResponseDTO>
                    {
                        Code = 500,
                        Success = false,
                        Message = "Không thể cập nhật phiếu nhập kho.",
                        Data = null
                    };
                }
                var responseDTO = _mapper.Map<VaccineReceiptResponseDTO>(existingReceipt);
                return new BaseResponse<VaccineReceiptResponseDTO>
                {
                    Code = 200,
                    Success = true,
                    Message = "Phiếu nhập kho đã được cập nhật thành công.",
                    Data = responseDTO
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "UpdateVaccineReceiptAsync: An error occurred while updating vaccine receipt");
                return new BaseResponse<VaccineReceiptResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã xảy ra lỗi khi cập nhật phiếu nhập kho.",
                    Data = null
                };
            }
        }
    }
}
