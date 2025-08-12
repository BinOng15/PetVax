using PetVax.BusinessObjects.DTO;
using PetVax.BusinessObjects.DTO.VaccineExportDetailDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PetVax.BusinessObjects.DTO.ResponseModel;

namespace PetVax.Services.IService
{
    public interface IVaccineExportDetailService
    {
        Task<DynamicResponse<VaccineExportDetailResponseDTO>> GetAllVaccineExportDetailsAsync(GetAllItemsDTO getAllItemsDTO, CancellationToken cancellationToken);
        Task<BaseResponse<VaccineExportDetailResponseDTO>> GetVaccineExportDetailByIdAsync(int vaccineExportDetailId, CancellationToken cancellationToken);
        Task<BaseResponse<List<VaccineExportDetailResponseDTO>>> GetVaccineExportDetailByVaccineBatchIdAsync(int vaccineBatchId, CancellationToken cancellationToken);
        Task<BaseResponse<List<VaccineExportDetailResponseDTO>>> GetVaccineExportDetailByVaccineExportIdAsync(int vaccineExportId, CancellationToken cancellationToken);
        Task<BaseResponse<VaccineExportDetailResponseForVaccinationDTO>> GetVaccineExportDetailByAppointmentDetailIdAsync(int appointmentDetailId, CancellationToken cancellationToken);
        Task<BaseResponse<VaccineExportDetailResponseDTO>> CreateVaccineExportDetailAsync(CreateVaccineExportDetailDTO createVaccineExportDetailDTO, CancellationToken cancellationToken);
        Task<BaseResponse<VaccineExportDetailResponseDTO>> UpdateVaccineExportDetailAsync(int vaccineExportDetailId, UpdateVaccineExportDetailDTO updateVaccineExportDetailDTO, CancellationToken cancellationToken);
        Task<BaseResponse<bool>> DeleteVaccineExportDetailAsync(int vaccineExportDetailId, CancellationToken cancellationToken);
        Task<BaseResponse<VaccineExportDetailResponseForVaccinationDTO>> CreateVaccineExportDetailForVaccinationAsync(CreateVaccineExportDetailForVaccinationDTO createVaccineExportDetailForVaccinationDTO, CancellationToken cancellationToken);
        Task<BaseResponse<VaccineExportDetailResponseForVaccinationDTO>> UpdateVaccineExportDetailForVaccinationAsync(int vaccineExportDetailId, UpdateVaccineExportDetailForVaccinationDTO updateVaccineExportDetailForVaccinationDTO, CancellationToken cancellationToken);
        Task<BaseResponse<VaccineExportDetailResponseDTO>> CreateFullVaccineExportAsync(CreateFullVaccineExportDTO createFullVaccineExportDTO, CancellationToken cancellationToken);
    }
}
