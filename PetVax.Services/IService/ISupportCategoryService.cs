using PetVax.BusinessObjects.DTO;
using PetVax.BusinessObjects.DTO.SupportCategoryDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PetVax.BusinessObjects.DTO.ResponseModel;

namespace PetVax.Services.IService
{
    public interface ISupportCategoryService
    {
        Task<DynamicResponse<SupportCategoryResponseDTO>> GetAllSupportCategoriesAsync(GetAllItemsDTO getAllItemsDTO, CancellationToken cancellationToken);
        Task<BaseResponse<SupportCategoryResponseDTO>> GetSupportCategoryByIdAsync(int supportCategoryId, CancellationToken cancellationToken);
        Task<BaseResponse<SupportCategoryResponseDTO>> CreateSupportCategoryAsync(CreateSupportCategoryDTO createSupportCategoryDTO, CancellationToken cancellationToken);
        Task<BaseResponse<SupportCategoryResponseDTO>> UpdateSupportCategoryAsync(int supportCategoryId, UpdateSupportCategoryDTO updateSupportCategoryDTO, CancellationToken cancellationToken);
        Task<BaseResponse<SupportCategoryResponseDTO>> DeleteSupportCategoryAsync(int supportCategoryId, CancellationToken cancellationToken);
    }
}
