using PetVax.BusinessObjects.DTO;
using PetVax.BusinessObjects.DTO.VaccineDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PetVax.BusinessObjects.DTO.ResponseModel;

namespace PetVax.Services.IService
{
    public interface IVaccineService
    {
        Task<BaseResponse<VaccineResponseDTO>> GetVaccineByIdAsync(int vaccineId, CancellationToken cancellationToken);
        Task<DynamicResponse<VaccineResponseDTO>> GetAllVaccineAsync (GetAllItemsDTO getAllItemsDTO, CancellationToken cancellationToken);
        Task<BaseResponse<VaccineResponseDTO>> CreateVaccineAsync (CreateVaccineDTO createVaccineDTO, CancellationToken cancellationToken);
        Task<BaseResponse<VaccineResponseDTO>> UpdateVaccineAsync(int vaccineId, UpdateVaccineDTO updateVaccineDTO, CancellationToken cancellationToken);
        Task<BaseResponse<bool>> DeleteVaccineAsync(int vaccineId, CancellationToken cancellationToken);
        Task<BaseResponse<VaccineResponseDTO>> GetVaccineByVaccineCodeAsync(string vaccineCode, CancellationToken cancellationToken);
        Task<BaseResponse<VaccineResponseDTO>> GetVaccineByNameAsync(string Name, CancellationToken cancellationToken);
        Task<BaseResponse<VaccineResponseDTO>> GetVaccineByDiseaseIdAsync(int diseaseId, CancellationToken cancellationToken);
    }
}
