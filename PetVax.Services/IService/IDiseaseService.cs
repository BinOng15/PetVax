using PetVax.BusinessObjects.DTO;
using PetVax.BusinessObjects.DTO.DiseaseDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PetVax.BusinessObjects.DTO.ResponseModel;

namespace PetVax.Services.IService
{
    public interface IDiseaseService
    {
        Task<BaseResponse<DiseaseResponseDTO>> GetDiseaseByIdAsync(int diseaseId, CancellationToken cancellationToken);
        Task<DynamicResponse<DiseaseResponseDTO>> GetAllDiseaseAsync (GetAllItemsDTO getAllItemsDTO, CancellationToken cancellationToken);
        Task<BaseResponse<DiseaseResponseDTO>> CreateDiseaseAsync(CreateDiseaseDTO createDiseaseDTO, CancellationToken cancellationToken);
        Task<BaseResponse<DiseaseResponseDTO>> UpdateDiseaseAsync(int diseaseId, UpdateDiseaseDTO updateDiseaseDTO, CancellationToken cancellationToken);
        Task<BaseResponse<bool>> DeleteDiseaseAsync(int diseaseId, CancellationToken cancellationToken);
        Task<BaseResponse<DiseaseResponseDTO>> GetDiseaseByNameAsync(string name, CancellationToken cancellationToken);
        Task<BaseResponse<DiseaseResponseDTO>> GetDiseaseByVaccineIdAsync(int vaccineId, CancellationToken cancellationToken);
    }
}
