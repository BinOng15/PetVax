using PetVax.BusinessObjects.DTO;
using PetVax.BusinessObjects.DTO.FAQItemDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PetVax.BusinessObjects.DTO.ResponseModel;

namespace PetVax.Services.IService
{
    public interface IFAQItemService
    {
        Task<DynamicResponse<FAQResponseDTO>> GetAllFAQsAsync(GetAllItemsDTO getAllItemsDTO, CancellationToken cancellationToken);
        Task<BaseResponse<FAQResponseDTO>> GetFAQByIdAsync(int faqId, CancellationToken cancellationToken);
        Task<BaseResponse<FAQResponseDTO>> CreateFAQAsync(CreateFAQDTO createFAQDTO, CancellationToken cancellationToken);
        Task<BaseResponse<FAQResponseDTO>> UpdateFAQAsync(int faqId, UpdateFAQDTO updateFAQDTO, CancellationToken cancellationToken);
        Task<BaseResponse<FAQResponseDTO>> DeleteFAQAsync(int faqId, CancellationToken cancellationToken);
    }
}
