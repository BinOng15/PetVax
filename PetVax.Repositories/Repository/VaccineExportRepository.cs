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
            return await CreateAsync(vaccineExport, cancellationToken);
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
