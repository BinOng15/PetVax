using PetVax.BusinessObjects.DTO;
using PetVax.BusinessObjects.DTO.VaccineExportDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PetVax.BusinessObjects.DTO.ResponseModel;

namespace PetVax.Services.IService
{
    public interface IVaccineExportService
    {
        Task<DynamicResponse<VaccineExportResponseDTO>> GetAllVaccineExportsAsync(GetAllItemsDTO getAllItemsDTO, CancellationToken cancellationToken);
        Task<BaseResponse<VaccineExportResponseDTO>> GetVaccineExportByIdAsync(int vaccineExportId, CancellationToken cancellationToken);
        Task<BaseResponse<VaccineExportResponseDTO>> GetVaccineExportByExportCodeAsync(string exportCode, CancellationToken cancellationToken);
        Task<BaseResponse<VaccineExportResponseDTO>> GetVaccineExportByExportDateAsync(DateTime exportDate, CancellationToken cancellationToken);
        Task<BaseResponse<VaccineExportResponseDTO>> CreateVaccineExportAsync(CreateVaccineExportDTO createVaccineExportDTO, CancellationToken cancellationToken);
        Task<BaseResponse<VaccineExportResponseDTO>> UpdateVaccineExportAsync(int exportId, UpdateVaccineExportDTO updateVaccineExportDTO, CancellationToken cancellationToken);
        Task<BaseResponse<bool>> DeleteVaccineExportAsync(int vaccineExportId, CancellationToken cancellationToken);
    }
}
