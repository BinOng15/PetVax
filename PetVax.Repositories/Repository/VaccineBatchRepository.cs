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
    public class VaccineBatchRepository : GenericRepository<VaccineBatch>, IVaccineBatchRepository
    {
        public VaccineBatchRepository() : base()
        {
        }

        public async Task<int> CreateVaccineBatchAsync(VaccineBatch vaccineBatch, CancellationToken cancellationToken)
        {
            await CreateAsync(vaccineBatch, cancellationToken);
            return vaccineBatch.VaccineBatchId;
        }

        public async Task<bool> DeleteVaccineBatchAsync(int vaccineBatchId, CancellationToken cancellationToken)
        {
            return await DeleteAsync(vaccineBatchId, cancellationToken);
        }

        public async Task<List<VaccineBatch>> GetAllVaccineBatchAsync(CancellationToken cancellationToken)
        {
            return await _context.VaccineBatches
                .Include(vb => vb.Vaccine)
                .ToListAsync(cancellationToken);
        }

        public async Task<int> GetTotalVaccineBatchesAsync(CancellationToken cancellationToken)
        {
            return await _context.VaccineBatches
                .Where(vb => vb.isDeleted == false)
                .CountAsync(cancellationToken);
        }

        public async Task<VaccineBatch> GetVaccineBatchByIdAsync(int vaccineBatchId, CancellationToken cancellationToken)
        {
            return await _context.VaccineBatches
                .Include(vb => vb.Vaccine)
                .FirstOrDefaultAsync(vb => vb.VaccineBatchId == vaccineBatchId, cancellationToken);
        }

        public async Task<VaccineBatch> GetVaccineBatchByVaccineCodeAsync(string vaccineCode, CancellationToken cancellationToken)
        {
            return await _context.VaccineBatches
                .Include(vb => vb.Vaccine)
                .FirstOrDefaultAsync(vb => vb.Vaccine.VaccineCode == vaccineCode, cancellationToken);
        }

        public async Task<VaccineBatch> GetVaccineBatchByVaccineId(int vaccineId, CancellationToken cancellationToken)
        {
            return await _context.VaccineBatches
                .Include(vb => vb.Vaccine)
                .FirstOrDefaultAsync(vb => vb.Vaccine.VaccineId == vaccineId, cancellationToken);
        }

        public async Task<int> UpdateVaccineBatchAsync(VaccineBatch vaccineBatch, CancellationToken cancellationToken)
        {
            return await UpdateAsync(vaccineBatch, cancellationToken);
        }
    }
}
