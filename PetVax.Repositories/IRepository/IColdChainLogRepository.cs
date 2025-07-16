using PetVax.BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetVax.Repositories.IRepository
{
    public interface IColdChainLogRepository
    {
        Task<List<ColdChainLog>> GetAllColdChainLogsAsync(CancellationToken cancellationToken);
        Task<ColdChainLog> GetColdChainLogByIdAsync(int coldChainLogId, CancellationToken cancellationToken);
        Task<int> CreateColdChainLogAsync(ColdChainLog coldChainLog, CancellationToken cancellationToken);
        Task<int> UpdateColdChainLogAsync(ColdChainLog coldChainLog, CancellationToken cancellationToken);
        Task<bool> DeleteColdChainLogAsync(int coldChainLogId, CancellationToken cancellationToken);
        Task<List<ColdChainLog>> GetColdChainLogsByVaccineBatchIdAsync(int vaccineBatchId, CancellationToken cancellationToken);
    }
}
