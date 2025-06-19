using Microsoft.EntityFrameworkCore;
using PetVax.BusinessObjects.Enum;
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

        public async Task<List<VetSchedule>> GetAllVetSchedulesAsync(CancellationToken cancellationToken)
        {
            return await GetAllAsync(cancellationToken);
        }

        public async Task<VetSchedule> GetVetScheduleByIdAsync(int vetScheduleId, CancellationToken cancellationToken)
        {
            return await _context.VetSchedules.FirstOrDefaultAsync(vs => vs.VetScheduleId == vetScheduleId, cancellationToken);
        }

        public async Task<List<VetSchedule>> GetVetSchedulesByVetIdAsync(int vetId, CancellationToken cancellationToken)
        {
            return await _context.VetSchedules
                .Where(vs => vs.VetId == vetId)
                .ToListAsync(cancellationToken);
        }

        public async Task<int> UpdateVetScheduleAsync(VetSchedule vetSchedule, CancellationToken cancellationToken)
        {
            return await UpdateAsync(vetSchedule, cancellationToken);
        }
        public async Task UpdateExpiredVetScheduleAsync(CancellationToken cancellationToken)
        {
            var currentDateTime = DateTime.UtcNow;
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
                    schedule.ModifiedAt = DateTime.UtcNow;
                    schedule.ModifiedBy = "System-Auto"; // Hoặc tên người dùng hiện tại nếu có
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
    }
}
