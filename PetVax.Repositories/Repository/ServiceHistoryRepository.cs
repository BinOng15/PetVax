using Microsoft.EntityFrameworkCore;
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
    public class ServiceHistoryRepository : GenericRepository<ServiceHistory>, IServiceHistoryRepository
    {
        public ServiceHistoryRepository() : base()
        {
        }
        public async Task CreateServiceHistoryAsync(CancellationToken cancellationToken)
        {
            var currentDateTime = DateTime.UtcNow;
            var currrentDate = currentDateTime.Date;

            // Lấy các lịch hẹn đã hoàn tất
            var appointments = await _context.Appointments
                .Where(a => a.AppointmentDate.Date <= currrentDate && a.AppointmentStatus == AppointmentStatus.Completed)
                .ToListAsync(cancellationToken);

            foreach (var appointment in appointments)
            {
                // Kiểm tra xem ServiceHistory đã tồn tại hay chưa
                bool isExist = await _context.ServiceHistories.AnyAsync(sh =>
                    sh.CustomerId == appointment.CustomerId &&
                    sh.ServiceDate == appointment.AppointmentDate &&
                    sh.ServiceType == appointment.ServiceType &&
                    sh.isDeleted == false,
                    cancellationToken);

                if (!isExist)
                {
                    var serviceHistory = new ServiceHistory
                    {
                        CustomerId = appointment.CustomerId,
                        ServiceDate = appointment.AppointmentDate,
                        ServiceType = appointment.ServiceType,
                        Status = appointment.AppointmentStatus.ToString(),
                        CreatedAt = currentDateTime,
                        CreatedBy = "System-Auto",
                        isDeleted = false
                    };

                    await _context.ServiceHistories.AddAsync(serviceHistory, cancellationToken);
                    await _context.SaveChangesAsync(cancellationToken);
                }
            }
        }


        public async Task<bool> DeleteServiceHistoryAsync(int serviceHistoryId, CancellationToken cancellationToken)
        {
            return await DeleteAsync(serviceHistoryId, cancellationToken);
        }

        public async Task<List<ServiceHistory>> GetAllServiceHistoriesAsync(CancellationToken cancellationToken)
        {
            return await _context.ServiceHistories
                .Include(sh => sh.Customer)
                .ThenInclude(sh => sh.Account)
                .Include(sh => sh.Customer)
                .ThenInclude(sh => sh.Pets)
                .Where(sh => sh.isDeleted != true)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<ServiceHistory>> GetServiceHistoriesByCustomerIdAsync(int customerId, CancellationToken cancellationToken)
        {
            return await _context.ServiceHistories
                .Include(sh => sh.Customer)
                .ThenInclude(sh => sh.Account)
                .Include(sh => sh.Customer)
                .ThenInclude(sh => sh.Pets)
                .Where(sh => sh.CustomerId == customerId && sh.isDeleted != true)
                .ToListAsync(cancellationToken);
        }

        public async Task<ServiceHistory> GetServiceHistoryByIdAsync(int serviceHistoryId, CancellationToken cancellationToken)
        {
            return await _context.ServiceHistories
                .Include(sh => sh.Customer)
                .ThenInclude(sh => sh.Pets)
                .FirstOrDefaultAsync(sh => sh.ServiceHistoryId == serviceHistoryId && sh.isDeleted != true, cancellationToken);
        }

        public Task UpdateExpiredServiceHistoriesAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceHistory> UpdateServiceHistoryAsync(ServiceHistory serviceHistory, CancellationToken cancellationToken)
        {
            _context.ServiceHistories.Update(serviceHistory);
            await _context.SaveChangesAsync(cancellationToken);
            return serviceHistory;
        }
    }
}
