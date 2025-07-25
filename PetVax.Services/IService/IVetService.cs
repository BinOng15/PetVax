using PetVax.BusinessObjects.DTO.VetDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PetVax.BusinessObjects.DTO.ResponseModel;

namespace PetVax.Services.IService
{
    public interface IVetService
    {
        Task<DynamicResponse<VetResponseDTO>> GetAllVetsAsync(GetAllVetRequestDTO getAllVetRequest, CancellationToken cancellationToken);
        Task<BaseResponse<VetResponseDTO>> UpdateVetsAsync(UpdateVetRequest updateVetRequest, CancellationToken cancellationToken);
        Task<BaseResponse<VetResponseDTO>> GetVetByIdAsync(int vetId, CancellationToken cancellationToken);
        Task<BaseResponse<VetResponseDTO>> DeleteVetAsync(int vetId, CancellationToken cancellationToken);
        Task<BaseResponse<VetResponseDTO>> CreateVetAsync(CreateVetDTO createVetDTO, CancellationToken cancellationToken);
        Task<BaseResponse<VetResponseDTO>> GetVetByAccountIdAsync(int accountId, CancellationToken cancellationToken);
    }
}
