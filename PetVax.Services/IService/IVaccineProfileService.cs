using PetVax.BusinessObjects.DTO.VaccineProfileDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PetVax.BusinessObjects.DTO.ResponseModel;

namespace PetVax.Services.IService
{
    public interface IVaccineProfileService
    {
        Task<List<BaseResponse<VaccineProfileResponseDTO>>> GetAllVaccineProfilesAsync(CancellationToken cancellationToken);
        Task<BaseResponse<VaccineProfileResponseDTO>> CreateVaccineProfileAsync(VaccineProfileRequestDTO vaccineProfileRequest, CancellationToken cancellationToken);
        Task<BaseResponse<VaccineProfileResponseDTO>> GetVaccineProfileByPetIdAsync(int petId, CancellationToken cancellationToken);
        Task<BaseResponse<VaccineProfileResponseDTO>> GetVaccineProfileByIdAsync(int id, CancellationToken cancellationToken);
        Task<BaseResponse<VaccineProfileResponseDTO>> UpdateVaccineProfileAsync(int id, VaccineProfileRequestDTO vaccineProfileRequest, CancellationToken cancellationToken);
        Task<BaseResponse<VaccineProfileResponseDTO>> DeleteVaccineProfileAsync(int vaccineProfileId, CancellationToken cancellationToken);
    }
}
