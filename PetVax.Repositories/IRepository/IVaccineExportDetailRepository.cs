using PetVax.BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetVax.Repositories.IRepository
{
    public interface IVaccineExportDetailRepository
    {
        Task<List<VaccineExportDetail>> GetAllVaccineExportDetailsAsync(CancellationToken cancellationToken);
        Task<VaccineExportDetail> GetVaccineExportDetailByIdAsync(int vaccineExportDetailId, CancellationToken cancellationToken);
        Task<int> CreateVaccineExportDetailAsync(VaccineExportDetail vaccineExportDetail, CancellationToken cancellationToken);
        Task<int> UpdateVaccineExportDetailAsync(VaccineExportDetail vaccineExportDetail, CancellationToken cancellationToken);
        Task<bool> DeleteVaccineExportDetailAsync(int vaccineExportDetailId, CancellationToken cancellationToken);
        Task<List<VaccineExportDetail>> GetVaccineExportDetailsByVaccineExportIdAsync(int vaccineExportId, CancellationToken cancellationToken);
        Task<VaccineExportDetail> GetVaccineExportDetailByVaccineBatchIdAsync(int vaccineBatchId, CancellationToken cancellationToken);
        Task<VaccineExportDetail> GetVaccineExportDetailByAppointmentDetailIdAsync(int appointmentDetailId, CancellationToken cancellationToken);
    }
}
