using PetVax.BusinessObjects.DTO;
using PetVax.BusinessObjects.DTO.CustomerVoucherDTO;
using PetVax.BusinessObjects.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PetVax.BusinessObjects.DTO.ResponseModel;

namespace PetVax.Services.IService
{
    public interface ICustomerVoucherService
    {
        Task<DynamicResponse<CustomerVoucherResponseDTO>> GetAllCustomerVouchersAsync(GetAllItemsDTO getAllItemsDTO, EnumList.VoucherStatus? voucherStatus, CancellationToken cancellationToken);
        Task<BaseResponse<CustomerVoucherResponseDTO>> GetCustomerVoucherByIdAsync(int customerVoucherId, CancellationToken cancellationToken);
        Task<BaseResponse<List<CustomerVoucherResponseDTO>>> GetCustomerVouchersByCustomerIdAsync(int customerId, CancellationToken cancellationToken);
        Task<BaseResponse<CustomerVoucherResponseDTO>> GetCustomerVoucherByCustomerIdAndVoucherIdAsync(int customerId, int voucherId, CancellationToken cancellationToken);
        Task<BaseResponse<List<CustomerVoucherResponseDTO>>> GetCustomerVoucherByVoucherIdAsync(int voucherId, CancellationToken cancellationToken);
    }
}
