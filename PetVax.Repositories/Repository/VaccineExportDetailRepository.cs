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
            return await CreateAsync(vaccineExportDetail, cancellationToken);
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
                .ToListAsync(cancellationToken);
        }
        public async Task<VaccineExportDetail> GetVaccineExportDetailByIdAsync(int vaccineExportDetailId, CancellationToken cancellationToken)
        {
            return await _context.VaccineExportDetails
                .Include(ved => ved.VaccineBatch)
                    .ThenInclude(vb => vb.Vaccine)
                .Include(ved => ved.VaccineExport)
                .FirstOrDefaultAsync(ved => ved.VaccineExportDetailId == vaccineExportDetailId, cancellationToken);
        }
        public async Task<List<VaccineExportDetail>> GetVaccineExportDetailsByVaccineExportIdAsync(int vaccineExportId, CancellationToken cancellationToken)
        {
            return await _context.VaccineExportDetails
                .Include(ved => ved.VaccineBatch)
                    .ThenInclude(vb => vb.Vaccine)
                .Include(ved => ved.VaccineExport)
                .Where(ved => ved.VaccineExportId == vaccineExportId && !ved.isDeleted.HasValue || !ved.isDeleted.Value)
                .ToListAsync(cancellationToken);
        }
        public async Task<VaccineExportDetail> GetVaccineExportDetailByVaccineBatchIdAsync(int vaccineBatchId, CancellationToken cancellationToken)
        {
            return await _context.VaccineExportDetails
                .Include(ved => ved.VaccineBatch)
                    .ThenInclude(vb => vb.Vaccine)
                .Include(ved => ved.VaccineExport)
                .FirstOrDefaultAsync(ved => ved.VaccineBatchId == vaccineBatchId && !ved.isDeleted.HasValue || !ved.isDeleted.Value, cancellationToken);
        }
        public async Task<int> UpdateVaccineExportDetailAsync(VaccineExportDetail vaccineExportDetail, CancellationToken cancellationToken)
        {
            return await UpdateAsync(vaccineExportDetail, cancellationToken);
        }
    }
}
