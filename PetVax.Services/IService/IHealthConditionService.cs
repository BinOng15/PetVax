using PetVax.BusinessObjects.DTO.CertificateForPet;
using PetVax.BusinessObjects.DTO.HealthConditionDTO;
using PetVax.BusinessObjects.DTO.VetDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PetVax.BusinessObjects.DTO.ResponseModel;

namespace PetVax.Services.IService
{
    public interface IHealthConditionService
    {
        Task<BaseResponse<HealthConditionResponse>> CreateHealthConditionAsync(CreateHealthConditionDTO healthConditionDto, CancellationToken cancellationToken);

        Task<DynamicResponse<BaseHealthConditionResponseDTO>> GetAllHealthConditionsAsync(GetAllVetRequestDTO getAllVetRequest, CancellationToken cancellationToken);

        Task<BaseResponse<HealthConditionResponse>> UpdateHealthConditionAsync(int healthConditionId, UpdateHealthCondition healthConditionDto, CancellationToken cancellationToken);
        Task<BaseResponse<HealthConditionResponse>> GetHealthConditionByIdAsync(int healthConditionId, CancellationToken cancellationToken);

        Task<PetVaccinationRecordDTO?> GetPetVaccinationRecordAsync(int petId);

    }
}
