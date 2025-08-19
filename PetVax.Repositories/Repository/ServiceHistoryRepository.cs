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
            // Lấy tất cả lịch hẹn đã hoàn tất, bao gồm thông tin Appointment và Payment
            var appointments = await _context.AppointmentDetails
                .Include(a => a.Appointment)
                .Include(a => a.Payment)
                .Where(a => a.AppointmentStatus == AppointmentStatus.Completed)
                .ToListAsync(cancellationToken);

            // Lấy danh sách AppointmentId đã tồn tại trong ServiceHistories (chưa bị xóa)
            var existingIds = await _context.ServiceHistories
                .Select(sh => sh.AppointmentId)
                .ToListAsync(cancellationToken);

            // Tạo danh sách ServiceHistory mới
            var newServiceHistories = appointments
                .Where(a => a.Appointment != null && a.Payment != null && a.Payment.Any()) // Ensure Payment collection is not empty
                .Where(a => !existingIds.Contains(a.AppointmentId)) // tránh trùng
                .Select(a => new ServiceHistory
                {
                    CustomerId = a.Appointment.CustomerId,
                    AppointmentId = a.AppointmentId,
                    PetId = a.Appointment.PetId,
                    ServiceDate = a.AppointmentDate,
                    ServiceType = a.ServiceType,
                    PaymentMethod = a.Payment.FirstOrDefault()?.PaymentMethod, // Access the first Payment's PaymentMethod
                    Amount = a.Payment.FirstOrDefault()?.Amount ?? 0, // Access the first Payment's Amount
                    Status = a.AppointmentStatus.ToString(),
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = "System-Auto",
                    isDeleted = false
                })
                .ToList();

            if (newServiceHistories.Any())
            {
                await _context.ServiceHistories.AddRangeAsync(newServiceHistories, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
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
                .Include(sh => sh.Pet)
                .Include(sh => sh.Customer)
                .ThenInclude(sh => sh.Pets)
                .Include(sh => sh.Appointment)
                .Where(sh => sh.isDeleted != true)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<ServiceHistory>> GetServiceHistoriesByCustomerIdAsync(int customerId, CancellationToken cancellationToken)
        {
            return await _context.ServiceHistories
                .Include(sh => sh.Customer)
                .ThenInclude(sh => sh.Account)
                .Include(sh => sh.Pet)
                .Include(sh => sh.Customer)
                .ThenInclude(sh => sh.Pets)
                .Include(sh => sh.Appointment)
                .Where(sh => sh.CustomerId == customerId && sh.isDeleted != true)
                .ToListAsync(cancellationToken);
        }

        public async Task<ServiceHistory> GetServiceHistoryByIdAsync(int serviceHistoryId, CancellationToken cancellationToken)
        {
            return await _context.ServiceHistories
                .Include(sh => sh.Customer)
                .ThenInclude(sh => sh.Account)
                .Include(sh => sh.Pet)
                .Include(sh => sh.Customer)
                .ThenInclude(sh => sh.Pets)
                .Include(sh => sh.Appointment)   
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

        public async Task<List<ServiceHistory>> GetServiceHistoriesByServiceTypeAsync(ServiceType serviceType, CancellationToken cancellationToken)
        {
            return await _context.ServiceHistories
                .Include(sh => sh.Customer)
                .ThenInclude(sh => sh.Account)
                .Include(sh => sh.Pet)
                .Include(sh => sh.Customer)
                .ThenInclude(sh => sh.Pets)
                .Include(sh => sh.Appointment)
                .Where(sh => sh.ServiceType == serviceType && sh.isDeleted != true)
                .ToListAsync(cancellationToken);
        }
    }
}
