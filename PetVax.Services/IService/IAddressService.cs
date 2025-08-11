using PetVax.BusinessObjects.DTO;
using PetVax.BusinessObjects.DTO.AddressDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PetVax.BusinessObjects.DTO.ResponseModel;

namespace PetVax.Services.IService
{
    public interface IAddressService
    {
        Task<DynamicResponse<AddressResponseDTO>> GetAllAddressAsync(GetAllItemsDTO getAllItemsDTO, CancellationToken cancellationToken);
        Task<BaseResponse<AddressResponseDTO>> GetAddressByIdAsync(int addressId, CancellationToken cancellationToken);
        Task<BaseResponse<AddressResponseDTO>> CreateAddressAsync(CreateAddressDTO createAddressDTO, CancellationToken cancellationToken);
        Task<BaseResponse<AddressResponseDTO>> UpdateAddressAsync(int addressId, UpdateAddressDTO updateAddressDTO, CancellationToken cancellationToken);
        Task<BaseResponse<bool>> DeleteAddressAsync(int addressId, CancellationToken cancellationToken);
    }
}
