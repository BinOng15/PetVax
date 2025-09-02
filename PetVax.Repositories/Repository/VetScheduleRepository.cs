using Microsoft.EntityFrameworkCore;
using PetVax.BusinessObjects.Enum;
using PetVax.BusinessObjects.Helpers;
using PetVax.BusinessObjects.Models;
using PetVax.Repositories.IRepository;
using PetVax.Repositories.Repository.BaseResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PetVax.BusinessObjects.Enum.EnumList;

namespace PetVax.Repositories.Repository
{
    public class VetScheduleRepository : GenericRepository<VetSchedule>, IVetScheduleRepository
    {
        public VetScheduleRepository() : base()
        {
        }
        public async Task<int> CreateVetScheduleAsync(VetSchedule vetSchedule, CancellationToken cancellationToken)
        {
            return await CreateAsync(vetSchedule, cancellationToken);
        }

        public async Task<bool> DeleteVetScheduleAsync(int vetScheduleId, CancellationToken cancellationToken)
        {
            return await DeleteAsync(vetScheduleId, cancellationToken);
        }

        public async Task<List<VetSchedule>> GetAllVetSchedulesAsync(int pageNumber, int pageSize, string keyword, CancellationToken cancellationToken)
        {
            var query = _context.VetSchedules.AsQueryable();
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                query = query.Where(vs => vs.VetId.ToString().Contains(keyword));
            }
            return await query
                .OrderBy(vs => vs.ScheduleDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .OrderByDescending(m => m.CreatedAt)
                .ToListAsync(cancellationToken);
        }

        public async Task<VetSchedule> GetVetScheduleByIdAsync(int vetScheduleId, CancellationToken cancellationToken)
        {
            return await _context.VetSchedules
                .Include(vs => vs.Vet)
                .FirstOrDefaultAsync(vs => vs.VetScheduleId == vetScheduleId, cancellationToken);
        }

        public async Task<List<VetSchedule>> GetVetSchedulesByVetIdAsync(int vetId, CancellationToken cancellationToken)
        {
            return await _context.VetSchedules
                .Include(vs => vs.Vet)
                .Where(vs => vs.VetId == vetId && vs.isDeleted != true)
                .ToListAsync(cancellationToken);
        }

        public async Task<int> UpdateVetScheduleAsync(VetSchedule vetSchedule, CancellationToken cancellationToken)
        {
            return await UpdateAsync(vetSchedule, cancellationToken);
        }
        public async Task UpdateExpiredVetScheduleAsync(CancellationToken cancellationToken)
        {
            var currentDateTime = DateTimeHelper.Now();
            var currentDate = currentDateTime.Date;
            var currentHour = currentDateTime.Hour;

            var activeSchedules = await _context.VetSchedules
                .Where(vs => vs.Status == EnumList.VetScheduleStatus.Available && vs.ScheduleDate <= currentDate)
                .ToListAsync(cancellationToken);

            foreach (var schedule in activeSchedules)
            {
                var slotHour = GetHourFromSlotNumber(schedule.SlotNumber);

                if (schedule.ScheduleDate < currentDate ||
                    (schedule.ScheduleDate.Date == currentDate && slotHour < currentHour))
                {
                    schedule.Status = EnumList.VetScheduleStatus.Unavailable;
                    schedule.ModifiedAt = DateTimeHelper.Now();
                    schedule.ModifiedBy = "System-Auto";
                }
            }
            await _context.SaveChangesAsync(cancellationToken);
        }

        private int GetHourFromSlotNumber(int slotNumber)
        {
            return slotNumber switch
            {
                (int)Slot.Slot_8h => 8,
                (int)Slot.Slot_9h => 9,
                (int)Slot.Slot_10h => 10,
                (int)Slot.Slot_11h => 11,
                (int)Slot.Slot_13h => 13,
                (int)Slot.Slot_14h => 14,
                (int)Slot.Slot_15h => 15,
                (int)Slot.Slot_16h => 16,
                _ => throw new ArgumentException("Slot number không hợp lệ")
            };
        }

        public async Task<List<VetSchedule>> GetVetSchedulesByDateAndSlotAsync(DateTime? date, int? slot, CancellationToken cancellationToken)
        {
            return await _context.VetSchedules
                .Include(vs => vs.Vet)
                .Where(vs => vs.ScheduleDate.Date == date && vs.SlotNumber == slot && vs.isDeleted != true)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<VetSchedule>> GetVetSchedulesByDateAsync(DateTime? date, CancellationToken cancellationToken)
        {
            return await _context.VetSchedules
                .Include(vs => vs.Vet)
                .Where(vs => vs.ScheduleDate.Date == date && vs.isDeleted != true)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<VetSchedule>> GetVetSchedulesBySlotAsync(int? slot, CancellationToken cancellationToken)
        {
            return await _context.VetSchedules
                .Include(vs => vs.Vet)
                .Where(vs => vs.SlotNumber == slot && vs.isDeleted != true)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<VetSchedule>> GetVetSchedulesFromDateToDateAsync(DateTime? fromDate, DateTime? toDate, CancellationToken cancellationToken)
        {
            return await _context.VetSchedules
                .Include(vs => vs.Vet)
                .Where(vs => vs.ScheduleDate.Date >= fromDate && vs.ScheduleDate.Date <= toDate && vs.isDeleted != true)
                .ToListAsync(cancellationToken);
        }

        public async Task<int> GetTotalVetSchedulesAsync(CancellationToken cancellationToken)
        {
            return await _context.VetSchedules
                .Where(vs => vs.isDeleted == false)
                .CountAsync(cancellationToken);
        }

        public async Task<int> GetTotalAvailableVetSchedulesAsync(CancellationToken cancellationToken)
        {
            return await _context.VetSchedules
                .Where(vs => vs.Status == EnumList.VetScheduleStatus.Available && vs.isDeleted == false)
                .CountAsync(cancellationToken);
        }

        public async Task<int> GetTotalUnavailableVetSchedulesAsync(CancellationToken cancellationToken)
        {
            return await _context.VetSchedules
                .Where(vs => vs.Status == EnumList.VetScheduleStatus.Unavailable && vs.isDeleted == false)
                .CountAsync(cancellationToken);
        }

        public async Task<int> GetTotalScheduledVetSchedulesAsync(CancellationToken cancellationToken)
        {
            return await _context.VetSchedules
                .Where(vs => vs.Status == EnumList.VetScheduleStatus.Scheduled && vs.isDeleted == false)
                .CountAsync(cancellationToken);
        }
    }
}
