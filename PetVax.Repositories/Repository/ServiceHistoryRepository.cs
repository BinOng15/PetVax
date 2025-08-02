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

            // Lấy các lịch hẹn đã hoàn tất, bao gồm cả thông tin Appointment và Payment
            var appointments = await _context.AppointmentDetails
                .Include(a => a.Appointment)
                .Include(a => a.Payment)
                .Where(a =>
                    a.AppointmentDate.Date <= currrentDate &&
                    a.AppointmentStatus == AppointmentStatus.Completed &&
                    a.Payment != null &&
                    a.Payment.PaymentStatus == PaymentStatus.Completed)
                .ToListAsync(cancellationToken);

            foreach (var appointment in appointments)
            {
                // Bỏ qua nếu Appointment hoặc Payment bị null (an toàn)
                if (appointment.Appointment == null || appointment.Payment == null)
                {
                    continue;
                }

                // Kiểm tra xem ServiceHistory đã tồn tại hay chưa
                bool isExist = await _context.ServiceHistories.AnyAsync(sh =>
                    sh.CustomerId == appointment.Appointment.CustomerId &&
                    sh.ServiceDate == appointment.AppointmentDate &&
                    sh.ServiceType == appointment.ServiceType &&
                    sh.isDeleted == false,
                    cancellationToken);

                if (!isExist)
                {
                    var serviceHistory = new ServiceHistory
                    {
                        CustomerId = appointment.Appointment.CustomerId,
                        AppointmentId = appointment.AppointmentId,
                        PetId = appointment.Appointment.PetId,
                        ServiceDate = appointment.AppointmentDate,
                        ServiceType = appointment.ServiceType,
                        PaymentMethod = appointment.Payment.PaymentMethod,
                        Amount = appointment.Payment.Amount,
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
    }
}
