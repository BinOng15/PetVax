using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PetVax.BusinessObjects.DTO;
using PetVax.BusinessObjects.DTO.VaccineBatchDTO;
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
    public class VaccineBatchService : IVaccineBatchService
    {
        private readonly IVaccineBatchRepository _vaccineBatchRepository;
        private readonly ILogger<VaccineBatchService> _logger;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public VaccineBatchService(IVaccineBatchRepository vaccineBatchRepository, ILogger<VaccineBatchService> logger, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _vaccineBatchRepository = vaccineBatchRepository;
            _logger = logger;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<BaseResponse<VaccineBatchResponseDTO>> CreateVaccineBatchAsync(CreateVaccineBatchDTO createVaccineBatchDTO, CancellationToken cancellationToken)
        {
            try
            {
                var vaccineBatch = _mapper.Map<VaccineBatch>(createVaccineBatchDTO);
                vaccineBatch.VaccineId = createVaccineBatchDTO.VaccineId;
                vaccineBatch.BatchNumber = "BATCH" + new Random().Next(100000, 1000000).ToString();
                vaccineBatch.Manufacturer = createVaccineBatchDTO.Manufacturer;
                vaccineBatch.ManufactureDate = createVaccineBatchDTO.ManufactureDate;
                vaccineBatch.ExpiryDate = createVaccineBatchDTO.ExpiryDate;
                vaccineBatch.Source = createVaccineBatchDTO.Source;
                vaccineBatch.StorageConditions = createVaccineBatchDTO.StorageCondition;
                vaccineBatch.Quantity = createVaccineBatchDTO.Quantity;
                vaccineBatch.CreateAt = DateTimeHelper.Now();
                vaccineBatch.CreatedBy = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "system";
                int batchId = await _vaccineBatchRepository.CreateVaccineBatchAsync(vaccineBatch, cancellationToken);

                // Get the batch again to ensure navigation properties are loaded
                var createdBatch = await _vaccineBatchRepository.GetVaccineBatchByIdAsync(batchId, cancellationToken);

                var response = _mapper.Map<VaccineBatchResponseDTO>(createdBatch);
                response.VaccineBatchId = batchId;

                return new BaseResponse<VaccineBatchResponseDTO>
                {
                    Code = 201,
                    Success = true,
                    Message = "Tạo lô vaccine thành công",
                    Data = response
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating vaccine batch.");
                return new BaseResponse<VaccineBatchResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Lỗi khi tạo lô vaccine",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<bool>> DeleteVaccineBatchAsync(int vaccineBatchId, CancellationToken cancellationToken)
        {
            try
            {
                var vaccineBatch = await _vaccineBatchRepository.GetVaccineBatchByIdAsync(vaccineBatchId, cancellationToken);
                if (vaccineBatch == null)
                {
                    return new BaseResponse<bool>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Lô vaccine không tồn tại",
                        Data = false
                    };
                }

                vaccineBatch.isDeleted = true;
                vaccineBatch.ModifiedAt = DateTimeHelper.Now();
                vaccineBatch.ModifiedBy = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "system";

                await _vaccineBatchRepository.UpdateVaccineBatchAsync(vaccineBatch, cancellationToken);

                return new BaseResponse<bool>
                {
                    Code = 200,
                    Success = true,
                    Message = "Xóa lô vaccine thành công",
                    Data = true
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while soft deleting vaccine batch.");
                return new BaseResponse<bool>
                {
                    Code = 500,
                    Success = false,
                    Message = "Lỗi khi xóa lô vaccine",
                    Data = false
                };
            }
        }

        public async Task<DynamicResponse<VaccineBatchResponseDTO>> GetAllVaccineBatchsAsync(GetAllItemsDTO getAllItemsDTO, CancellationToken cancellationToken)
        {
            try
            {
                var vaccineBatches = await _vaccineBatchRepository.GetAllVaccineBatchAsync(cancellationToken);
                if (!string.IsNullOrWhiteSpace(getAllItemsDTO.KeyWord))
                {
                    var keyword = getAllItemsDTO.KeyWord.ToLower();
                    vaccineBatches = vaccineBatches
                        .Where(vb => vb.Vaccine.Name.ToLower().Contains(keyword) || vb.Vaccine.VaccineCode.ToLower().Contains(keyword))
                        .ToList();
                }

                int pageNumber = getAllItemsDTO?.PageNumber > 0 ? getAllItemsDTO.PageNumber : 1;
                int pageSize = getAllItemsDTO?.PageSize > 0 ? getAllItemsDTO.PageSize : 10;
                int skip = (pageNumber - 1) * pageSize;
                int totalItem = vaccineBatches.Count;
                int totalPage = (int)Math.Ceiling((double)totalItem / pageSize);

                var pagedBatchs = vaccineBatches
                    .Skip(skip)
                    .Take(pageSize)
                    .ToList();

                var response = new MegaData<VaccineBatchResponseDTO>
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
                    PageData = _mapper.Map<List<VaccineBatchResponseDTO>>(pagedBatchs)
                };

                if (pagedBatchs.Any())
                {
                    return new DynamicResponse<VaccineBatchResponseDTO>
                    {
                        Code = 200,
                        Success = true,
                        Message = "Lấy danh sách lô vaccine thành công",
                        Data = response
                    };
                }
                else
                {
                    return new DynamicResponse<VaccineBatchResponseDTO>
                    {
                        Code = 200,
                        Success = false,
                        Message = "Không tìm thấy lô vaccine nào",
                        Data = null
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting all vaccine batches.");
                return new DynamicResponse<VaccineBatchResponseDTO>
                {
                    Code = 500,
                    Message = "Lỗi khi tất danh sách lô vaccine",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<VaccineBatchResponseDTO>> GetVaccineBatchByIdAsync(int vaccineBatchId, CancellationToken cancellationToken)
        {
            try
            {
                var vaccineBatch = await _vaccineBatchRepository.GetVaccineBatchByIdAsync(vaccineBatchId, cancellationToken);
                if (vaccineBatch == null)
                {
                    return new BaseResponse<VaccineBatchResponseDTO>
                    {
                        Code = 200,
                        Success = false,
                        Message = "Lô vaccine không tồn tại",
                        Data = null
                    };
                }
                var response = _mapper.Map<VaccineBatchResponseDTO>(vaccineBatch);
                return new BaseResponse<VaccineBatchResponseDTO>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy thông tin lô vaccine thành công",
                    Data = response
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting vaccine batch by ID.");
                return new BaseResponse<VaccineBatchResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Lỗi khi lấy thông tin lô vaccine",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<VaccineBatchResponseDTO>> GetVaccineBatchByVaccineCodeAsync(string vaccineCode, CancellationToken cancellationToken)
        {
            try
            {
                var vaccineBatch = await _vaccineBatchRepository.GetVaccineBatchByVaccineCodeAsync(vaccineCode, cancellationToken);
                if (vaccineBatch == null)
                {
                    return new BaseResponse<VaccineBatchResponseDTO>
                    {
                        Code = 200,
                        Success = false,
                        Message = "Lô vaccine không tồn tại",
                        Data = null
                    };
                }
                var response = _mapper.Map<VaccineBatchResponseDTO>(vaccineBatch);
                return new BaseResponse<VaccineBatchResponseDTO>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy thông tin lô vaccine thành công",
                    Data = response
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting vaccine batch by vaccine code.");
                return new BaseResponse<VaccineBatchResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Lỗi khi lấy thông tin lô vaccine",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<VaccineBatchResponseDTO>> GetVaccineBatchByVaccineIdAsync(int vaccineId, CancellationToken cancellationToken)
        {
            try
            {
                var vaccineBatch = await _vaccineBatchRepository.GetVaccineBatchByVaccineId(vaccineId, cancellationToken);
                if (vaccineBatch == null)
                {
                    return new BaseResponse<VaccineBatchResponseDTO>
                    {
                        Code = 200,
                        Success = false,
                        Message = "Lô vaccine không tồn tại",
                        Data = null
                    };
                }
                var response = _mapper.Map<VaccineBatchResponseDTO>(vaccineBatch);
                return new BaseResponse<VaccineBatchResponseDTO>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy thông tin lô vaccine thành công",
                    Data = response
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting vaccine batch by vaccine ID.");
                return new BaseResponse<VaccineBatchResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Lỗi khi lấy thông tin lô vaccine",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<VaccineBatchResponseDTO>> UpdateVaccineBatchAsync(int vaccineBatchId, UpdateVaccineBatchDTO updateVaccineBatchDTO, CancellationToken cancellationToken)
        {
            try
            {
                var existingBatch = await _vaccineBatchRepository.GetVaccineBatchByIdAsync(vaccineBatchId, cancellationToken);
                if (existingBatch == null)
                {
                    return new BaseResponse<VaccineBatchResponseDTO>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Lô vaccine không tồn tại",
                        Data = null
                    };
                }

                // Update properties
                existingBatch.VaccineId = updateVaccineBatchDTO.VaccineId ?? existingBatch.VaccineId;
                existingBatch.ManufactureDate = updateVaccineBatchDTO.ManufactureDate ?? existingBatch.ManufactureDate;
                existingBatch.ExpiryDate = updateVaccineBatchDTO.ExpiryDate ?? existingBatch.ExpiryDate;
                existingBatch.Manufacturer = updateVaccineBatchDTO.Manufacturer ?? existingBatch.Manufacturer;
                existingBatch.Source = updateVaccineBatchDTO.Source ?? existingBatch.Source;
                existingBatch.StorageConditions = updateVaccineBatchDTO.StorageCondition ?? existingBatch.StorageConditions;
                existingBatch.Quantity = updateVaccineBatchDTO.Quantity ?? existingBatch.Quantity;
                existingBatch.ModifiedAt = DateTimeHelper.Now();
                existingBatch.ModifiedBy = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "system";
                int batchId = await _vaccineBatchRepository.UpdateVaccineBatchAsync(existingBatch, cancellationToken);

                var updatedBatch = await _vaccineBatchRepository.GetVaccineBatchByIdAsync(batchId, cancellationToken);

                var response = _mapper.Map<VaccineBatchResponseDTO>(updatedBatch);
                return new BaseResponse<VaccineBatchResponseDTO>
                {
                    Code = 200,
                    Success = true,
                    Message = "Cập nhật lô vaccine thành công",
                    Data = response
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating vaccine batch.");
                return new BaseResponse<VaccineBatchResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Lỗi khi cập nhật lô vaccine",
                    Data = null
                };
            }
        }
    }
}
