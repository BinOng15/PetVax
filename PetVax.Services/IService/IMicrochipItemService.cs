using PetVax.BusinessObjects.DTO.MicrochipItemDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PetVax.BusinessObjects.DTO.ResponseModel;

namespace PetVax.Services.IService
{
    public interface IMicrochipItemService
    {
        Task<BaseResponse<MicrochipItemResponse>> GetMicrochipItemByMicrochipCodeAsync(string code, CancellationToken cancellationToken);
        Task<BaseResponse<BaseMicrochipItemResponse>> CreateMicrochipItemAsync(CreateMicrochipItemRequest request, CancellationToken cancellationToken);
        Task<BaseResponse<BaseMicrochipItemResponse>> UpdateMicrochipItemAsync(int microchipItemId, UpdateMicrochipItemRequest request, CancellationToken cancellationToken);
    }
}
