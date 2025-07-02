using PetVax.BusinessObjects.DTO;
using PetVax.BusinessObjects.DTO.MicrochipDTO;
using PetVax.BusinessObjects.DTO.MicrochipItemDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PetVax.BusinessObjects.DTO.ResponseModel;

namespace PetVax.Services.IService
{
    public interface IMicrochipService
    {
        Task<BaseResponse<bool>> DeleteMicrochipAsync(int microchipId, CancellationToken cancellationToken);
        Task<BaseResponse<MicrochipResponseDTO>> GetMicrochipByIdAsync(int microchipId, CancellationToken cancellationToken);
        Task<DynamicResponse<MicrochipResponseDTO>> GetAllMicrochipsDynamicAsync(GetAllItemsDTO getAllItemsDTO, CancellationToken cancellationToken);
        //Task<BaseResponse<MicrochipResponseDTO>> CreateMicrochipAsync(MicrochipRequestDTO microchipRequestDTO, CancellationToken cancellationToken);
        Task<BaseResponse<BaseMicrochipItemResponse>> CreateFullMicrochipAsync(MicrochipRequestDTO request, CancellationToken cancellationToken);
        Task<BaseResponse<BaseMicrochipItemResponse>> UpdateMicrochipAsync(int microchipId, MicrochipRequestDTO microchipRequestDTO, CancellationToken cancellationToken);
    }
}
