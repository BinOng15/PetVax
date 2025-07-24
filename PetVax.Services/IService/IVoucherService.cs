using PetVax.BusinessObjects.DTO;
using PetVax.BusinessObjects.DTO.CustomerVoucherDTO;
using PetVax.BusinessObjects.DTO.VoucherDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PetVax.BusinessObjects.DTO.ResponseModel;

namespace PetVax.Services.IService
{
    public interface IVoucherService
    {
        Task<DynamicResponse<VoucherResponseDTO>> GetAllVoucherAsync(GetAllItemsDTO getAllItemsDTO, CancellationToken cancellationToken);
        Task<BaseResponse<VoucherResponseDTO>> GetVoucherByIdAsync(int voucherId, CancellationToken cancellationToken);
        Task<BaseResponse<VoucherResponseDTO>> GetVoucherByCodeAsync(string voucherCode, CancellationToken cancellationToken);
        Task<BaseResponse<VoucherResponseDTO>> GetVoucherByTransactionIdAsync(int transactionId, CancellationToken cancellationToken);
        Task<BaseResponse<CreateUpdateVoucherResponseDTO>> CreateVoucherAsync(CreateVoucherDTO createVoucherDTO, CancellationToken cancellationToken);
        Task<BaseResponse<CreateUpdateVoucherResponseDTO>> UpdateVoucherAsync(int voucherId, UpdateVoucherDTO updateVoucherDTO, CancellationToken cancellationToken);
        Task<BaseResponse<bool>> DeleteVoucherAsync(int voucherId, CancellationToken cancellationToken);
        Task<BaseResponse<CustomerVoucherResponseDTO>> RedeemPointsForVoucherAsync(int customedId, int voucherId, CancellationToken cancellationToken);
    }
}
