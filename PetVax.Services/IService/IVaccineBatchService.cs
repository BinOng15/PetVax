using PetVax.BusinessObjects.DTO;
using PetVax.BusinessObjects.DTO.VaccineBatchDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PetVax.BusinessObjects.DTO.ResponseModel;

namespace PetVax.Services.IService
{
    public interface IVaccineBatchService
    {
        Task<DynamicResponse<VaccineBatchResponseDTO>> GetAllVaccineBatchsAsync(GetAllItemsDTO getAllItemsDTO, CancellationToken cancellationToken);
        Task<BaseResponse<VaccineBatchResponseDTO>> GetVaccineBatchByIdAsync(int vaccineBatchId, CancellationToken cancellationToken);
        Task<BaseResponse<VaccineBatchResponseDTO>> GetVaccineBatchByVaccineCodeAsync(string vaccineCode, CancellationToken cancellationToken);
        Task<BaseResponse<VaccineBatchResponseDTO>> GetVaccineBatchByVaccineIdAsync(int vaccineId, CancellationToken cancellationToken);
        Task<BaseResponse<VaccineBatchResponseDTO>> CreateVaccineBatchAsync(CreateVaccineBatchDTO createVaccineBatchDTO, CancellationToken cancellationToken);
        Task<BaseResponse<VaccineBatchResponseDTO>> UpdateVaccineBatchAsync(int vaccineBatchId, UpdateVaccineBatchDTO updateVaccineBatchDTO, CancellationToken cancellationToken);
        Task<BaseResponse<bool>> DeleteVaccineBatchAsync(int vaccineBatchId, CancellationToken cancellationToken);
    }
}
