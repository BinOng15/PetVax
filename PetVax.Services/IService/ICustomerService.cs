using PetVax.BusinessObjects.DTO.CustomerDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PetVax.BusinessObjects.DTO.ResponseModel;

namespace PetVax.Services.IService
{
    public interface ICustomerService
    {
        Task<BaseResponse<CustomerResponseDTO>> GetCustomerByIdAsync(int customerId, CancellationToken cancellationToken);
        Task<BaseResponse<CustomerResponseDTO>> GetCustomerByAccountIdAsync(int accountId, CancellationToken cancellationToken);
        Task<DynamicResponse<CustomerResponseDTO>> GetAllCustomersAsync(GetAllCustomerRequestDTO getAllCustomerRequestDTO, CancellationToken cancellationToken);
        //Task<BaseResponse<CustomerResponseDTO>> CreateCustomerAsync(CreateCustomerDTO createCustomerDTO, CancellationToken cancellationToken);
        Task<BaseResponse<CustomerResponseDTO>> UpdateCustomerAsync(int customerId, UpdateCustomerDTO updateCustomerDTO, CancellationToken cancellationToken);
        Task<BaseResponse<bool>> DeleteCustomerAsync(int customerId, CancellationToken cancellationToken);

    }
}
