using PetVax.BusinessObjects.DTO;
using PetVax.BusinessObjects.DTO.VaccineDiseaseDTO;
using PetVax.BusinessObjects.DTO.VaccineDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PetVax.BusinessObjects.DTO.ResponseModel;

namespace PetVax.Services.IService
{
    public interface IVaccineDiseaseService
    {
        Task<DynamicResponse<VaccineDiseaseResponseDTO>> GetAllVaccineDiseaseAsync(GetAllItemsDTO getAllItemsDTO, CancellationToken cancellationToken);
        Task<BaseResponse<VaccineDiseaseResponseDTO>> GetVaccineDiseaseByIdAsync(int vaccineDiseaseId, CancellationToken cancellationToken);
        Task<BaseResponse<VaccineDiseaseResponseDTO>> CreateVaccineDiseaseAsync(CreateVaccineDiseaseDTO createVaccineDiseaseDTO, CancellationToken cancellationToken);
        Task<BaseResponse<VaccineDiseaseResponseDTO>> UpdateVaccineDiseaseAsync(int vaccineDiseaseId, UpdateVaccineDiseaseDTO updateVaccineDiseaseDTO, CancellationToken cancellationToken);
        Task<BaseResponse<bool>> DeleteVaccineDiseaseAsync(int vaccineDiseaseId, CancellationToken cancellationToken);
        Task<BaseResponse<List<VaccineDiseaseResponseDTO>>> GetVaccineDiseaseByVaccineIdAsync(int vaccineId, CancellationToken cancellationToken);
        Task<BaseResponse<List<VaccineDiseaseResponseDTO>>> GetVaccineDiseaseByDiseaseIdAsync(int diseaseId, CancellationToken cancellationToken);
    }
}
