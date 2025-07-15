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
    public class ColdChainLogRepository : GenericRepository<ColdChainLog>, IColdChainLogRepository
    {
        public ColdChainLogRepository() : base()
        {
        }

        public async Task<int> CreateColdChainLogAsync(ColdChainLog coldChainLog, CancellationToken cancellationToken)
        {
            return await CreateAsync(coldChainLog, cancellationToken);
        }

        public async Task<bool> DeleteColdChainLogAsync(int coldChainLogId, CancellationToken cancellationToken)
        {
            return await DeleteAsync(coldChainLogId, cancellationToken);
        }

        public async Task<List<ColdChainLog>> GetAllColdChainLogsAsync(CancellationToken cancellationToken)
        {
            return await _context.ColdChainLogs
                .Include(ccl => ccl.VaccineBatch)
                    .ThenInclude(vb => vb.Vaccine)
                .ToListAsync(cancellationToken);
        }

        public async Task<ColdChainLog> GetColdChainLogByIdAsync(int coldChainLogId, CancellationToken cancellationToken)
        {
            return await _context.ColdChainLogs
                .Include(ccl => ccl.VaccineBatch)
                    .ThenInclude(vb => vb.Vaccine)
                .FirstOrDefaultAsync(ccl => ccl.ColdChainLogId == coldChainLogId, cancellationToken);
        }

        public async Task<List<ColdChainLog>> GetColdChainLogsByVaccineBatchIdAsync(int vaccineBatchId, CancellationToken cancellationToken)
        {
            return await _context.ColdChainLogs
                .Include(ccl => ccl.VaccineBatch)
                    .ThenInclude(vb => vb.Vaccine)
                .Where(ccl => ccl.VaccineBatchId == vaccineBatchId && !ccl.isDeleted.HasValue || !ccl.isDeleted.Value)
                .ToListAsync(cancellationToken);
        }

        public async Task<int> UpdateColdChainLogAsync(ColdChainLog coldChainLog, CancellationToken cancellationToken)
        {
            return await UpdateAsync(coldChainLog, cancellationToken);
        }
    }
}
