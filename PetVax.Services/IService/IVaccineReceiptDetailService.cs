using PetVax.BusinessObjects.DTO;
using PetVax.BusinessObjects.DTO.VaccineReceipDetailDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PetVax.BusinessObjects.DTO.ResponseModel;

namespace PetVax.Services.IService
{
    public interface IVaccineReceiptDetailService
    {
        Task<DynamicResponse<VaccineReceiptDetailResponseDTO>> GetAllVaccineReceiptDetailsAsync(GetAllItemsDTO getAllItemsDTO, CancellationToken cancellationToken);
        Task<BaseResponse<VaccineReceiptDetailResponseDTO>> GetVaccineReceiptDetailByIdAsync(int vaccineReceiptDetailId, CancellationToken cancellationToken);
        Task<BaseResponse<VaccineReceiptDetailResponseDTO>> GetVaccineReceiptDetailByVaccineBatchId(int vaccineBatchId, CancellationToken cancellationToken);
        Task<BaseResponse<List<VaccineReceiptDetailResponseDTO>>> GetVaccineReceiptDetailsByVaccineReceiptIdAsync(int vaccineReceiptId, CancellationToken cancellationToken);
        Task<BaseResponse<VaccineReceiptDetailResponseDTO>> CreateVaccineReceiptDetailAsync(CreateVaccineReceiptDetailDTO createVaccineReceiptDetailDTO, CancellationToken cancellationToken);
        Task<BaseResponse<VaccineReceiptDetailResponseDTO>> UpdateVaccineReceiptDetailAsync(int vaccineReceiptDetailId, UpdateVaccineReceiptDetailDTO updateVaccineReceiptDetailDTO, CancellationToken cancellationToken);
        Task<BaseResponse<bool>> DeleteVaccineReceiptDetailAsync(int vaccineReceiptDetailId, CancellationToken cancellationToken);
    }
}
