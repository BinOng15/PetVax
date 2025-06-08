using AutoMapper;
using Microsoft.Extensions.Logging;
using PetVax.BusinessObjects.DTO.AccountDTO;
using PetVax.BusinessObjects.DTO.VetDTO;
using PetVax.BusinessObjects.Models;
using PetVax.Repositories.IRepository;
using PetVax.Repositories.Repository;
using PetVax.Services.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PetVax.BusinessObjects.DTO.ResponseModel;

namespace PetVax.Services.Service
{
    public class VetService : IVetService
    {
        private readonly IVetRepository _vetRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<VetService> _logger;

        public VetService(IVetRepository vetRepository, IMapper mapper, ILogger<VetService> logger)
        {
            _vetRepository = vetRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<DynamicResponse<VetResponseDTO>> GetAllVetsAsync(GetAllVetRequestDTO getAllVetRequest, CancellationToken cancellationToken)
        {
            try
            {
                var vets = await _vetRepository.GetAllVetsAsync(cancellationToken);

                // Filter by keyword if provided
                if (!string.IsNullOrWhiteSpace(getAllVetRequest?.KeyWord))
                {
                    var keyword = getAllVetRequest.KeyWord.Trim().ToLower();
                    vets = vets
                        .Where(a => a.VetCode.ToLower().Contains(keyword))
                        .ToList();
                }

                // Pagination
                int pageNumber = getAllVetRequest?.PageNumber > 0 ? getAllVetRequest.PageNumber : 1;
                int pageSize = getAllVetRequest?.PageSize > 0 ? getAllVetRequest.PageSize : 10;
                int skip = (pageNumber - 1) * pageSize;
                int totalItem = vets.Count;
                int totalPage = (int)Math.Ceiling((double)totalItem / pageSize);

                var pateVets = vets
                    .Skip(skip)
                    .Take(pageSize)
                    .ToList();

                var responseData = new MegaData<VetResponseDTO>
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
                        keyWord = getAllVetRequest?.KeyWord,
                    },
                    PageData = pateVets.Select(v => new VetResponseDTO
                    {
                        VetId = v.VetId,
                        AccountId = v.AccountId,
                        VetCode = v.VetCode,
                        Name = v.Name,
                        Specialization = v.Specialization,
                        DateOfBirth = v.DateOfBirth,
                        PhoneNumber = v.PhoneNumber
                    }).ToList()
                };

                if (!pateVets.Any())
                {
                    _logger.LogInformation("No vets found for the given criteria");
                    return new DynamicResponse<VetResponseDTO>
                    {
                        Code = 404,
                        Success = false,
                        Message = "No vets found",
                        Data = responseData
                    };
                }

                _logger.LogInformation("Retrieved {Count} vets successfully (Page {PageNumber}, PageSize {PageSize})", pateVets.Count, pageNumber, pageSize);
                return new DynamicResponse<VetResponseDTO>
                {
                    Code = 200,
                    Success = true,
                    Message = "Vets retrieved successfully",
                    Data = responseData
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all vets with pagination");
                return new DynamicResponse<VetResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Error while retrieving vets: " + (ex.InnerException?.Message ?? ex.Message),
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<VetResponseDTO>> UpdateVetsAsync(UpdateVetRequest updateVetRequest, CancellationToken cancellationToken)
        {
            try
            {
                if (updateVetRequest == null || updateVetRequest.VetId <= 0)
                {
                    _logger.LogWarning("Invalid update request for Vet with ID {VetId}", updateVetRequest?.VetId);
                    return new BaseResponse<VetResponseDTO>
                    {
                        Code = 400,
                        Success = false,
                        Message = "Invalid request data",
                        Data = null
                    };
                }
                var existingVet = await _vetRepository.GetVetByIdAsync(updateVetRequest.VetId, cancellationToken);
                if (existingVet == null)
                {
                    _logger.LogWarning("Vet with ID {VetId} not found", updateVetRequest.VetId);
                    return new BaseResponse<VetResponseDTO>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Vet not found",
                        Data = null
                    };
                }
                // Map updated properties
                existingVet.VetCode = updateVetRequest.VetCode ?? existingVet.VetCode;
                existingVet.Name = updateVetRequest.Name ?? existingVet.Name;
                existingVet.PhoneNumber = updateVetRequest.PhoneNumber ?? existingVet.PhoneNumber;
                existingVet.Specialization = updateVetRequest.Specialization ?? existingVet.Specialization;
                existingVet.DateOfBirth = updateVetRequest.DateOfBirth ?? existingVet.DateOfBirth;
                int result = await _vetRepository.UpdateVetAsync(existingVet, cancellationToken);

                if (result > 0)
                {
                    var responseData = new VetResponseDTO();
                    responseData.VetId = existingVet.VetId;
                    responseData.AccountId = existingVet.AccountId;
                    responseData.VetCode = existingVet.VetCode;
                    responseData.Name = existingVet.Name;
                    responseData.PhoneNumber = existingVet.PhoneNumber;
                    responseData.Specialization = existingVet.Specialization;
                    responseData.DateOfBirth = existingVet.DateOfBirth; ;
                    _logger.LogInformation("Updated Vet with ID {VetId} successfully", updateVetRequest.VetId);
                    return new BaseResponse<VetResponseDTO>
                    {
                        Code = 200,
                        Success = true,
                        Message = "Veterinarian updated successfully",
                        Data = responseData
                    };
                }

                _logger.LogWarning("Failed to update Vet with ID {VetId}", updateVetRequest.VetId);
                return new BaseResponse<VetResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Failed to update veterinarian",
                    Data = null
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating Vet with ID {VetId}", updateVetRequest?.VetId);
                return new BaseResponse<VetResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Error while updating veterinarian: " + (ex.InnerException?.Message ?? ex.Message),
                    Data = null

                };
            }
        }

        public async Task<BaseResponse<VetResponseDTO>> GetVetByIdAsync(int vetId, CancellationToken cancellationToken)
        {
            try
            {
                if (vetId <= 0)
                {
                    _logger.LogWarning("Invalid Vet ID: {VetId}", vetId);
                    return new BaseResponse<VetResponseDTO>
                    {
                        Code = 400,
                        Success = false,
                        Message = "Invalid Vet ID",
                        Data = null
                    };
                }
                var vet = await _vetRepository.GetVetByIdAsync(vetId, cancellationToken);
                if (vet == null)
                {
                    _logger.LogWarning("Vet with ID {VetId} not found", vetId);
                    return new BaseResponse<VetResponseDTO>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Vet not found",
                        Data = null
                    };
                }
                var responseData = new VetResponseDTO();
                responseData.VetId = vet.VetId;
                responseData.AccountId = vet.AccountId;
                responseData.VetCode = vet.VetCode;
                responseData.Name = vet.Name;
                responseData.PhoneNumber = vet.PhoneNumber;
                responseData.Specialization = vet.Specialization;
                responseData.DateOfBirth = vet.DateOfBirth;
                _logger.LogInformation("Retrieved Vet with ID {VetId} successfully", vetId);
                return new BaseResponse<VetResponseDTO>
                {
                    Code = 200,
                    Success = true,
                    Message = "Vet retrieved successfully",
                    Data = responseData
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving Vet with ID {VetId}", vetId);
                return new BaseResponse<VetResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Error while retrieving veterinarian: " + (ex.InnerException?.Message ?? ex.Message),
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<VetResponseDTO>> DeleteVetAsync(int vetId, CancellationToken cancellationToken)
        {
            try
            {
                if (vetId <= 0)
                {
                    _logger.LogWarning("Invalid Vet ID: {VetId}", vetId);
                    return new BaseResponse<VetResponseDTO>
                    {
                        Code = 400,
                        Success = false,
                        Message = "Invalid Vet ID",
                        Data = null
                    };
                }
                var existingVet = await _vetRepository.GetVetByIdAsync(vetId, cancellationToken);
                if (existingVet == null)
                {
                    _logger.LogWarning("Vet with ID {VetId} not found", vetId);
                    return new BaseResponse<VetResponseDTO>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Vet not found",
                        Data = null
                    };
                }
                bool isDeleted = await _vetRepository.DeleteVetAsync(vetId, cancellationToken);
                if (isDeleted)
                {
                    _logger.LogInformation("Deleted Vet with ID {VetId} successfully", vetId);
                    return new BaseResponse<VetResponseDTO>
                    {
                        Code = 200,
                        Success = true,
                        Message = "Veterinarian deleted successfully",
                        Data = null
                    };
                }
                _logger.LogWarning("Failed to delete Vet with ID {VetId}", vetId);
                return new BaseResponse<VetResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Failed to delete veterinarian",
                    Data = null
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting Vet with ID {VetId}", vetId);
                return new BaseResponse<VetResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Error while deleting veterinarian: " + (ex.InnerException?.Message ?? ex.Message),
                    Data = null
                };
            }
        }
    }
}