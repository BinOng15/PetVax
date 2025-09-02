using PetVax.BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetVax.Repositories.IRepository
{
    public interface IVetScheduleRepository
    {
        Task<List<VetSchedule>> GetAllVetSchedulesAsync(int pageNumber, int pageSize, string keyword,CancellationToken cancellationToken);
        Task<VetSchedule> GetVetScheduleByIdAsync(int vetScheduleId, CancellationToken cancellationToken);
        Task<int> CreateVetScheduleAsync(VetSchedule vetSchedule, CancellationToken cancellationToken);
        Task<int> UpdateVetScheduleAsync(VetSchedule vetSchedule, CancellationToken cancellationToken);
        Task<bool> DeleteVetScheduleAsync(int vetScheduleId, CancellationToken cancellationToken);
        Task<List<VetSchedule>> GetVetSchedulesByVetIdAsync(int vetId, CancellationToken cancellationToken);
        Task UpdateExpiredVetScheduleAsync(CancellationToken cancellationToken);
        Task<List<VetSchedule>> GetVetSchedulesByDateAndSlotAsync(DateTime? date, int? slot, CancellationToken cancellationToken);
        Task<List<VetSchedule>> GetVetSchedulesByDateAsync(DateTime? date, CancellationToken cancellationToken);
        Task<List<VetSchedule>> GetVetSchedulesBySlotAsync(int? slot, CancellationToken cancellationToken);
        Task<List<VetSchedule>> GetVetSchedulesFromDateToDateAsync(DateTime? fromDate, DateTime? toDate, CancellationToken cancellationToken);
        Task<int> GetTotalVetSchedulesAsync(CancellationToken cancellationToken);
        Task<int> GetTotalAvailableVetSchedulesAsync(CancellationToken cancellationToken);
        Task<int> GetTotalUnavailableVetSchedulesAsync(CancellationToken cancellationToken);
        Task<int> GetTotalScheduledVetSchedulesAsync(CancellationToken cancellationToken);
    }
}
