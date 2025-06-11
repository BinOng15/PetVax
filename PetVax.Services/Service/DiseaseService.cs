using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PetVax.BusinessObjects.DTO;
using PetVax.BusinessObjects.DTO.DiseaseDTO;
using PetVax.BusinessObjects.Models;
using PetVax.Repositories.IRepository;
using PetVax.Services.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static PetVax.BusinessObjects.DTO.ResponseModel;

namespace PetVax.Services.Service
{
    public class DiseaseService : IDiseaseService
    {
        private readonly IDiseaseRepository _diseaseRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<DiseaseService> _logger;

        public DiseaseService(IDiseaseRepository diseaseRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor, ILogger<DiseaseService> logger)
        {
            _diseaseRepository = diseaseRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }
        public async Task<BaseResponse<DiseaseResponseDTO>> CreateDiseaseAsync(CreateDiseaseDTO createDiseaseDTO, CancellationToken cancellationToken)
        {
            if (createDiseaseDTO == null)
            {
                _logger.LogError("CreateDiseaseAsync: createDiseaseDTO is null");
                return new BaseResponse<DiseaseResponseDTO>
                {
                    Code = 400,
                    Message = "Invalid request data",
                    Data = null
                };
            }
            try
            {
                var disease = _mapper.Map<Disease>(createDiseaseDTO);
                disease.CreatedAt = DateTime.UtcNow;
                disease.CreatedBy = GetCurrentUserName();

                var diseaseId = await _diseaseRepository.CreateDiseaseAsync(disease, cancellationToken);
                if (diseaseId <= 0)
                {
                    _logger.LogError("CreateDiseaseAsync: Failed to create disease");
                    return new BaseResponse<DiseaseResponseDTO>
                    {
                        Code = 500,
                        Message = "Failed to create disease",
                        Data = null
                    };
                }
                var diseaseResponse = _mapper.Map<DiseaseResponseDTO>(disease);
                diseaseResponse.DiseaseId = diseaseId;
                _logger.LogInformation($"CreateDiseaseAsync: Disease created successfully with ID {diseaseId} by {GetCurrentUserName()}");
                return new BaseResponse<DiseaseResponseDTO>
                {
                    Code = 201,
                    Message = "Disease created successfully",
                    Data = diseaseResponse
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CreateDiseaseAsync: An error occurred while creating disease");
                return new BaseResponse<DiseaseResponseDTO>
                {
                    Code = 500,
                    Message = "An error occurred while creating disease",
                    Data = null
                };
            }
        }

        public Task<BaseResponse<bool>> DeleteDiseaseAsync(int diseaseId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<DynamicResponse<DiseaseResponseDTO>> GetAllDiseaseAsync(GetAllItemsDTO getAllItemsDTO, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse<DiseaseResponseDTO>> GetDiseaseByIdAsync(int diseaseId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse<DiseaseResponseDTO>> GetDiseaseByNameAsync(string name, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<BaseResponse<bool>> UpdateDiseaseAsync(UpdateDiseaseDTO updateDiseaseDTO, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
        private string GetCurrentUserName()
        {
            return _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value ?? "System";
        }
    }
}
