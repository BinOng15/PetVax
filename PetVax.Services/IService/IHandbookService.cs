using PetVax.BusinessObjects.DTO;
using PetVax.BusinessObjects.DTO.HandbookDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PetVax.BusinessObjects.DTO.ResponseModel;

namespace PetVax.Services.IService
{
    public interface IHandbookService
    {
        Task<DynamicResponse<HandbookResponseDTO>> GetAllHandbooksAsync(GetAllItemsDTO getAllItemsDTO, CancellationToken cancellationToken);
        Task<BaseResponse<HandbookResponseDTO>> GetHandbookByIdAsync(int handbookId, CancellationToken cancellationToken);
        Task<BaseResponse<HandbookResponseDTO>> CreateHandbookAsync(CreateHandbookDTO createHandbookDTO, CancellationToken cancellationToken);
        Task<BaseResponse<HandbookResponseDTO>> UpdateHandbookAsync(int handbookId, UpdateHandbookDTO updateHandbookDTO, CancellationToken cancellationToken);
        Task<BaseResponse<HandbookResponseDTO>> DeleteHandbookAsync(int handbookId, CancellationToken cancellationToken);
    }
}
