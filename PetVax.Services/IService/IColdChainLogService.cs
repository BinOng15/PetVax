using PetVax.BusinessObjects.DTO;
using PetVax.BusinessObjects.DTO.ColdChainLogDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PetVax.BusinessObjects.DTO.ResponseModel;

namespace PetVax.Services.IService
{
    public interface IColdChainLogService
    {
        Task<DynamicResponse<ColdChainLogResponseDTO>> GetAllColdChainLogsAsync(GetAllItemsDTO getAllItemsDTO, CancellationToken cancellationToken);
        Task<BaseResponse<ColdChainLogResponseDTO>> GetColdchainLogById(int coldChainLogId, CancellationToken cancellationToken);
        Task<BaseResponse<ColdChainLogResponseDTO>> CreateColdChainLogAsync(CreateColdChainLogDTO createColdChainLogDTO, CancellationToken cancellationToken);
        Task<BaseResponse<ColdChainLogResponseDTO>> UpdateColdChainLogAsync(int coldChainLogId, UpdateColdChainLogDTO updateColdChainLogDTO, CancellationToken cancellationToken);
        Task<BaseResponse<ColdChainLogResponseDTO>> DeleteColdChainLogAsync(int coldChainLogId, CancellationToken cancellationToken);
        Task<BaseResponse<List<ColdChainLogResponseDTO>>> GetColdChainLogsByVaccineBatchIdAsync(int vaccineBatchId, GetAllItemsDTO getAllItemsDTO, CancellationToken cancellationToken);
    }
}
