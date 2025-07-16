using PetVax.BusinessObjects.DTO;
using PetVax.BusinessObjects.DTO.VaccineReceiptDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PetVax.BusinessObjects.DTO.ResponseModel;

namespace PetVax.Services.IService
{
    public interface IVaccineReceiptService
    {
        Task<DynamicResponse<VaccineReceiptResponseDTO>> GetAllVaccineReceiptsAsync(GetAllItemsDTO getAllItemsDTO, CancellationToken cancellationToken);
        Task<BaseResponse<VaccineReceiptResponseDTO>> GetVaccineReceiptByIdAsync(int vaccineReceiptId, CancellationToken cancellationToken);
        Task<BaseResponse<VaccineReceiptResponseDTO>> GetVaccineReceiptByReceiptCodeAsync(string receiptCode, CancellationToken cancellationToken);
        Task<BaseResponse<VaccineReceiptResponseDTO>> GetVaccineReceiptByReceiptDateAsync(DateTime receiptDate, CancellationToken cancellationToken);
        Task<BaseResponse<VaccineReceiptResponseDTO>> CreateVaccineReceiptAsync(CreateVaccineReceiptDTO createVaccineReceiptDTO, CancellationToken cancellationToken);
        Task<BaseResponse<VaccineReceiptResponseDTO>> UpdateVaccineReceiptAsync(int receiptId, UpdateVaccineReceiptDTO updateVaccineReceiptDTO, CancellationToken cancellationToken);
        Task<BaseResponse<bool>> DeleteVaccineReceiptAsync(int vaccineReceiptId, CancellationToken cancellationToken);
    }
}
