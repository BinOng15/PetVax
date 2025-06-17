using PetVax.BusinessObjects.DTO.VetScheduleDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PetVax.BusinessObjects.DTO.ResponseModel;

namespace PetVax.Services.IService
{
    public interface IVetScheduleService
    {
        Task<List<BaseResponse<VetScheduleDTO>>> GetAllVetSchedulesAsync(CancellationToken cancellationToken);
        Task<BaseResponse<VetScheduleDTO>> GetVetScheduleByIdAsync(int vetScheduleId, CancellationToken cancellationToken);
        Task<BaseResponse<VetScheduleDTO>> CreateVetScheduleAsync(CreateVetScheduleRequestDTO request, CancellationToken cancellationToken);
        Task<BaseResponse<VetScheduleDTO>> UpdateVetScheduleAsync(UpdateVetScheduleRequestDTO request, CancellationToken cancellationToken);
        Task<List<BaseResponse<VetScheduleDTO>>> GetAllVetSchedulesByVetIdAsync(int vetId, CancellationToken cancellationToken);
        Task<BaseResponse<VetScheduleDTO>> DeleteVetScheduleAsync(int vetScheduleId, CancellationToken cancellationToken);
    }
}
