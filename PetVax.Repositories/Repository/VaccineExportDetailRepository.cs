using Microsoft.EntityFrameworkCore;
using PetVax.BusinessObjects.Models;
using PetVax.Repositories.IRepository;
using PetVax.Repositories.Repository.BaseResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetVax.Repositories.Repository
{
    public class VaccineExportDetailRepository : GenericRepository<VaccineExportDetail>, IVaccineExportDetailRepository
    {
        public VaccineExportDetailRepository() : base()
        {
        }
        public async Task<int> CreateVaccineExportDetailAsync(VaccineExportDetail vaccineExportDetail, CancellationToken cancellationToken)
        {
            await _context.VaccineExportDetails.AddAsync(vaccineExportDetail, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return vaccineExportDetail.VaccineExportDetailId;
        }
        public async Task<bool> DeleteVaccineExportDetailAsync(int vaccineExportDetailId, CancellationToken cancellationToken)
        {
            return await DeleteAsync(vaccineExportDetailId, cancellationToken);
        }
        public async Task<List<VaccineExportDetail>> GetAllVaccineExportDetailsAsync(CancellationToken cancellationToken)
        {
            return await _context.VaccineExportDetails
                .Include(ved => ved.VaccineBatch)
                    .ThenInclude(vb => vb.Vaccine)
                .Include(ved => ved.VaccineExport)
                .Include(ved => ved.AppointmentDetail)
                    .ThenInclude(ad => ad.Appointment)
                .OrderByDescending(m => m.CreatedAt)
                .ToListAsync(cancellationToken);
        }
        public async Task<VaccineExportDetail> GetVaccineExportDetailByIdAsync(int vaccineExportDetailId, CancellationToken cancellationToken)
        {
            return await _context.VaccineExportDetails
                .Include(ved => ved.VaccineBatch)
                    .ThenInclude(vb => vb.Vaccine)
                .Include(ved => ved.VaccineExport)
                .Include(ved => ved.AppointmentDetail)
                    .ThenInclude(ad => ad.Appointment)
                .FirstOrDefaultAsync(ved => ved.VaccineExportDetailId == vaccineExportDetailId, cancellationToken);
        }
        public async Task<List<VaccineExportDetail>> GetVaccineExportDetailsByVaccineExportIdAsync(int vaccineExportId, CancellationToken cancellationToken)
        {
            return await _context.VaccineExportDetails
                .Include(ved => ved.VaccineBatch)
                    .ThenInclude(vb => vb.Vaccine)
                .Include(ved => ved.VaccineExport)
                .Include(ved => ved.AppointmentDetail)
                    .ThenInclude(ad => ad.Appointment)
                .Where(ved => ved.VaccineExportId == vaccineExportId)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<VaccineExportDetail>> GetVaccineExportDetailByVaccineBatchIdAsync(int vaccineBatchId, CancellationToken cancellationToken)
        {
            return await _context.VaccineExportDetails
                .Include(ved => ved.VaccineBatch)
                    .ThenInclude(vb => vb.Vaccine)
                .Include(ved => ved.VaccineExport)
                .Include(ved => ved.AppointmentDetail)
                    .ThenInclude(ad => ad.Appointment)
                .Where(ved => ved.VaccineBatchId == vaccineBatchId 
                    && (!ved.isDeleted.HasValue || !ved.isDeleted.Value))
                .ToListAsync(cancellationToken);
        }
        public async Task<int> UpdateVaccineExportDetailAsync(VaccineExportDetail vaccineExportDetail, CancellationToken cancellationToken)
        {
            return await UpdateAsync(vaccineExportDetail, cancellationToken);
        }

        public async Task<VaccineExportDetail> GetVaccineExportDetailByAppointmentDetailIdAsync(int appointmentDetailId, CancellationToken cancellationToken)
        {
            return await _context.VaccineExportDetails
                .Include(ved => ved.VaccineBatch)
                    .ThenInclude(vb => vb.Vaccine)
                .Include(ved => ved.VaccineExport)
                .Include(ved => ved.AppointmentDetail)
                    .ThenInclude(ad => ad.Appointment)
                .FirstOrDefaultAsync(
                    ved => ved.AppointmentDetailId == appointmentDetailId
                        && (!ved.isDeleted.HasValue || !ved.isDeleted.Value),
                    cancellationToken);
        }

        public async Task<int> GetTotalVaccineExportDetailsAsync(CancellationToken cancellationToken)
        {
            return await _context.VaccineExportDetails
                .Where(ved => ved.isDeleted == false)
                .CountAsync(cancellationToken);
        }

        public async Task<List<VaccineExportDetail>> GetListVaccineExportDetailByVaccineBatchIdAsync(int vaccineBatchId, CancellationToken cancellationToken)
        {
            return await _context.VaccineExportDetails
                .Include(ved => ved.VaccineBatch)
                    .ThenInclude(vb => vb.Vaccine)
                .Include(ved => ved.VaccineExport)
                .Include(ved => ved.AppointmentDetail)
                    .ThenInclude(ad => ad.Appointment)
                .Where(ved => ved.VaccineBatchId == vaccineBatchId
                    && (!ved.isDeleted.HasValue || !ved.isDeleted.Value))
                .ToListAsync(cancellationToken);
        }
    }
}
