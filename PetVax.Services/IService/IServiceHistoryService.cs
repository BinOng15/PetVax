using PetVax.BusinessObjects.DTO;
using PetVax.BusinessObjects.DTO.ServiceHistoryDTO;
using PetVax.BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PetVax.BusinessObjects.DTO.ResponseModel;
using static PetVax.BusinessObjects.Enum.EnumList;

namespace PetVax.Services.IService
{
    public interface IServiceHistoryService
    {
        Task<BaseResponse<List<ServiceHistoryResponseDTO>>> GetServiceHistoryByCustomerIdAsync(int customerId, CancellationToken cancellationToken);
        Task<DynamicResponse<ServiceHistoryResponseDTO>> GetAllServiceHistoryAsync(GetAllItemsDTO getAllItemsDTO, CancellationToken cancellationToken);
        Task<BaseResponse<List<ServiceHistoryResponseDTO>>> GetServiceHistoryByServiceTypedAsync(ServiceType serviceType, CancellationToken cancellationToken);


    }
}
