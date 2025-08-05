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
    public class VaccineExportRepository : GenericRepository<VaccineExport>, IVaccineExportRepository
    {
        public VaccineExportRepository() : base()
        {
        }
        public async Task<int> CreateVaccineExportAsync(VaccineExport vaccineExport, CancellationToken cancellationToken)
        {
            await _context.VaccineExports.AddAsync(vaccineExport, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return vaccineExport.VaccineExportId;
        }
        public async Task<bool> DeleteVaccineExportAsync(int vaccineExportId, CancellationToken cancellationToken)
        {
            return await DeleteAsync(vaccineExportId, cancellationToken);
        }
        public async Task<List<VaccineExport>> GetAllVaccineExportsAsync(CancellationToken cancellationToken)
        {
            return await _context.VaccineExports
                .ToListAsync(cancellationToken);
        }

        public async Task<int> GetTotalVaccineExportsAsync(CancellationToken cancellationToken)
        {
            return await _context.VaccineExports.Where(ve => ve.isDeleted == false)
                .CountAsync(cancellationToken);
        }

        public async Task<VaccineExport> GetVaccineExportByExportCodeAsync(string exportCode, CancellationToken cancellationToken)
        {
            return await _context.VaccineExports
                .FirstOrDefaultAsync(ve => ve.ExportCode == exportCode, cancellationToken);
        }

        public async Task<VaccineExport> GetVaccineExportByIdAsync(int vaccineExportId, CancellationToken cancellationToken)
        {
            return await _context.VaccineExports
                .FirstOrDefaultAsync(ve => ve.VaccineExportId == vaccineExportId, cancellationToken);
        }
        public async Task<int> UpdateVaccineExportAsync(VaccineExport vaccineExport, CancellationToken cancellationToken)
        {
            return await UpdateAsync(vaccineExport, cancellationToken);
        }
    }
}
