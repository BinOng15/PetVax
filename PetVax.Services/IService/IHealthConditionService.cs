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

    }
}
