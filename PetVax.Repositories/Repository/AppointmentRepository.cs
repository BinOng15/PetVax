using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using PetVax.BusinessObjects.Enum;
using PetVax.BusinessObjects.Models;
using PetVax.Repositories.IRepository;
using PetVax.Repositories.Repository.BaseResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static PetVax.BusinessObjects.Enum.EnumList;

namespace PetVax.Repositories.Repository
{
    public class AppointmentRepository : GenericRepository<Appointment>, IAppointmentRepository
    {
        public AppointmentRepository() : base()
        {
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            if (_context.Database.CurrentTransaction != null)
            {
                return _context.Database.CurrentTransaction;
            }
            return await _context.Database.BeginTransactionAsync();
        }
        public async Task<Appointment> CreateAppointmentAsync(Appointment appointment, CancellationToken cancellationToken)
        {
            _context.Add(appointment);
            await _context.SaveChangesAsync(cancellationToken);
            return appointment;
        }
        public async Task<bool> DeleteAppointmentAsync(int appointmentId, CancellationToken cancellationToken)
        {
            var appointment = await _context.Appointments
                .FirstOrDefaultAsync(a => a.AppointmentId == appointmentId, cancellationToken);

            if (appointment == null)
                return false;

            appointment.isDeleted = true;
            _context.Update(appointment);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<List<Appointment>> GetAllAppointmentsAsync(CancellationToken cancellationToken)
        {
            return await _context.Appointments
                .Include(a => a.Customer) // Load Customer
                    .ThenInclude(c => c.Account) // Load Account của Customer
                .Include(a => a.Pet) // Load Pet
                .ToListAsync(cancellationToken);
        }
        public async Task<Appointment> GetAppointmentByIdAsync(int appointmentId, CancellationToken cancellationToken)
        {
            return await _context.Appointments
                .Include(a => a.Customer)
                    .ThenInclude(c => c.Account)
                .Include(a => a.Pet)
                .FirstOrDefaultAsync(a => a.AppointmentId == appointmentId, cancellationToken);
        }
        public async Task<List<Appointment>> GetAppointmentByPetIdAndStatusAsync(int petId, EnumList.AppointmentStatus status, CancellationToken cancellationToken)
        {
            return await _context.Set<Appointment>()
                .Include(a => a.Customer)
                    .ThenInclude(c => c.Account)
                .Include(a => a.Pet)
                .Where(a => a.PetId == petId && a.AppointmentStatus == status && a.isDeleted == false)
                .ToListAsync(cancellationToken);
        }
        public async Task<List<Appointment>> GetAppointmentsByCustomerIdAsync(int customerId, CancellationToken cancellationToken)
        {
            return await _context.Set<Appointment>()
                .Include(a => a.Customer)
                .ThenInclude(c => c.Account)
                .Include(a => a.Pet)
                .Where(a => a.CustomerId == customerId && a.isDeleted == false)
                .ToListAsync(cancellationToken);
        }
        public async Task<List<Appointment>> GetAppointmentsByPetIdAsync(int petId, CancellationToken cancellationToken)
        {
            return await _context.Set<Appointment>()
                .Include(a => a.Customer)
                .ThenInclude(c => c.Account)
                .Include(a => a.Pet)
                .Where(a => a.PetId == petId)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<Appointment>> GetAppointmentsByStatusAsync(EnumList.AppointmentStatus status, CancellationToken cancellationToken)
        {
            return await _context.Set<Appointment>()
                .Include(a => a.Customer)
                    .ThenInclude(c => c.Account)
                .Include(a => a.Pet)
                .Where(a => a.AppointmentStatus == status && a.isDeleted == false)
                .ToListAsync(cancellationToken);
        }

        public async Task<Appointment> UpdateAppointmentAsync(Appointment appointment, CancellationToken cancellationToken)
        {
            _context.Update(appointment);
            await _context.SaveChangesAsync(cancellationToken);
            return appointment;
        }

        public async Task<List<Appointment>> GetAppointmentsByCustomerIdAndStatusAsync(int customerId, AppointmentStatus status, CancellationToken cancellationToken)
        {
            return await _context.Set<Appointment>()
                .Include(a => a.Customer)
                .ThenInclude(c => c.Account)
                .Include(a => a.Pet)
                .Where(a => a.CustomerId == customerId && a.AppointmentStatus == status)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<Appointment>> GetAppointmentsByDateRangeAsync(DateTime from, DateTime to, CancellationToken cancellationToken)
        {
            return await _context.Set<Appointment>()
                .Include(a => a.Customer)
                    .ThenInclude(c => c.Account)
                .Include(a => a.Pet)
                .Where(a => a.AppointmentDate >= from && a.AppointmentDate <= to)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<Appointment>> GetPastAppointmentsByCustomerIdAsync(DateTime now, int customerId, CancellationToken cancellationToken)
        {
            return await _context.Set<Appointment>()
                .Include(a => a.Customer)
                    .ThenInclude(c => c.Account)
                .Include(a => a.Pet)
                .Where(a => a.AppointmentDate < now && a.CustomerId == customerId && a.isDeleted == false)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<Appointment>> GetTodayAppointmentsByCustomerIdAsync(DateTime today, int customerId, CancellationToken cancellationToken)
        {
            return await _context.Set<Appointment>()
                .Include(a => a.Customer)
                    .ThenInclude(c => c.Account)
                .Include(a => a.Pet)
                .Where(a => a.AppointmentDate.Date == today.Date && a.CustomerId == customerId && a.isDeleted == false)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<Appointment>> GetFutureAppointmentsByCustomerIdAsync(DateTime now, int customerId, CancellationToken cancellationToken)
        {
            return await _context.Set<Appointment>()
                .Include(a => a.Customer)
                    .ThenInclude(c => c.Account)
                .Include(a => a.Pet)
                .Where(a => a.AppointmentDate > now && a.CustomerId == customerId && a.isDeleted == false)
                .ToListAsync(cancellationToken);
        }

        public async Task UpdateExpiredAppointmentsAsync(CancellationToken cancellationToken)
        {
            var currentDateTime = DateTime.UtcNow;
            var expiredAppointments = await _context.Appointments
                .Where(a => a.AppointmentDate < currentDateTime && a.AppointmentStatus == AppointmentStatus.Processing && a.AppointmentStatus == AppointmentStatus.Confirmed)
                .ToListAsync(cancellationToken);

            if (expiredAppointments.Count == 0)
                return;

            var expiredAppointmentIds = expiredAppointments.Select(a => a.AppointmentId).ToList();

            var expiredDetails = await _context.AppointmentDetails
                .Where(d => expiredAppointmentIds.Contains(d.AppointmentId) && d.AppointmentStatus == AppointmentStatus.Processing)
                .ToListAsync(cancellationToken);

            foreach (var appointment in expiredAppointments)
            {
                appointment.AppointmentStatus = AppointmentStatus.Cancelled;
                _context.Update(appointment);
            }

            foreach (var detail in expiredDetails)
            {
                detail.AppointmentStatus = AppointmentStatus.Cancelled;
                _context.Update(detail);
            }

            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task SendMailWhenAppointmentCancelledAsync(CancellationToken cancellationToken)
        {
            var cancelledAppointments = await _context.Appointments
                .Where(a => a.AppointmentStatus == AppointmentStatus.Cancelled && a.isDeleted == false)
                .ToListAsync(cancellationToken);
            if (cancelledAppointments.Count == 0)
                return;
            foreach (var appointment in cancelledAppointments)
            {
                // Logic to send email notification about the cancelled appointment
                // This could involve using an email service to send the email
                // For example, you might call an email service method here
            }
        }

        private async Task SendNotificationEmail(string toEmail, CancellationToken cancellationToken)
        {
            // Logic to send notification email
            // This could involve using an email service to send the email
            // For example, you might call an email service method here
            // await _emailService.SendEmailAsync(toEmail, "Appointment Notification", "Your appointment has been updated.", cancellationToken);
        }

        public async Task<int> GetTotalAppointmentVaccinations(CancellationToken cancellationToken)
        {
            return await _context.Appointments
                .CountAsync(a => a.ServiceType == EnumList.ServiceType.Vaccination && a.isDeleted == false, cancellationToken);
        }

        public async Task<int> GetTotalProcessingAppointmentVaccinations(CancellationToken cancellationToken)
        {
            return await _context.Appointments
                .CountAsync(a => a.ServiceType == EnumList.ServiceType.Vaccination && a.AppointmentStatus == EnumList.AppointmentStatus.Processing && a.isDeleted == false, cancellationToken);
        }

        public async Task<int> GetTotalConfirmedAppointmentVaccinations(CancellationToken cancellationToken)
        {
            return await _context.Appointments
                .CountAsync(a => a.ServiceType == EnumList.ServiceType.Vaccination && a.AppointmentStatus == EnumList.AppointmentStatus.Confirmed && a.isDeleted == false, cancellationToken);
        }

        public async Task<int> GetTotalCheckedInAppointmentVaccinations(CancellationToken cancellationToken)
        {
            return await _context.Appointments
                .CountAsync(a => a.ServiceType == EnumList.ServiceType.Vaccination && a.AppointmentStatus == EnumList.AppointmentStatus.CheckedIn && a.isDeleted == false, cancellationToken);
        }

        public async Task<int> GetTotalPaidAppointmentVaccinations(CancellationToken cancellationToken)
        {
            return await _context.Appointments
                .CountAsync(a => a.ServiceType == EnumList.ServiceType.Vaccination && a.AppointmentStatus == EnumList.AppointmentStatus.Paid && a.isDeleted == false, cancellationToken);
        }

        public async Task<int> GetTotalCompletedAppointmentVaccinations(CancellationToken cancellationToken)
        {
            return await _context.Appointments
                .CountAsync(a => a.ServiceType == EnumList.ServiceType.Vaccination && a.AppointmentStatus == EnumList.AppointmentStatus.Completed && a.isDeleted == false, cancellationToken);
        }

        public async Task<int> GetTotalCancelledAppointmentVaccinations(CancellationToken cancellationToken)
        {
            return await _context.Appointments
                .CountAsync(a => a.ServiceType == EnumList.ServiceType.Vaccination && a.AppointmentStatus == EnumList.AppointmentStatus.Cancelled && a.isDeleted == false, cancellationToken);
        }

        public async Task<int> GetTotalRejectedAppointmentVaccinations(CancellationToken cancellationToken)
        {
            return await _context.Appointments
                .CountAsync(a => a.ServiceType == EnumList.ServiceType.Vaccination && a.AppointmentStatus == EnumList.AppointmentStatus.Rejected && a.isDeleted == false, cancellationToken);
        }

        public async Task<int> GetTotalAppointmentMicrochips(CancellationToken cancellationToken)
        {
            return await _context.Appointments
                .CountAsync(a => a.ServiceType == EnumList.ServiceType.Microchip && a.isDeleted == false, cancellationToken);
        }

        public async Task<int> GetTotalProcessingAppointmentMicrochips(CancellationToken cancellationToken)
        {
            return await _context.Appointments
                .CountAsync(a => a.ServiceType == EnumList.ServiceType.Microchip && a.AppointmentStatus == EnumList.AppointmentStatus.Processing && a.isDeleted == false, cancellationToken);
        }

        public async Task<int> GetTotalConfirmedAppointmentMicrochips(CancellationToken cancellationToken)
        {
            return await _context.Appointments
                .CountAsync(a => a.ServiceType == EnumList.ServiceType.Microchip && a.AppointmentStatus == EnumList.AppointmentStatus.Confirmed && a.isDeleted == false, cancellationToken);
        }

        public async Task<int> GetTotalCheckedInAppointmentMicrochips(CancellationToken cancellationToken)
        {
            return await _context.Appointments
                .CountAsync(a => a.ServiceType == EnumList.ServiceType.Microchip && a.AppointmentStatus == EnumList.AppointmentStatus.CheckedIn && a.isDeleted == false, cancellationToken);
        }

        public async Task<int> GetTotalPaidAppointmentMicrochips(CancellationToken cancellationToken)
        {
            return await _context.Appointments
                .CountAsync(a => a.ServiceType == EnumList.ServiceType.Microchip && a.AppointmentStatus == EnumList.AppointmentStatus.Paid && a.isDeleted == false, cancellationToken);
        }

        public async Task<int> GetTotalCompletedAppointmentMicrochips(CancellationToken cancellationToken)
        {
            return await _context.Appointments
                .CountAsync(a => a.ServiceType == EnumList.ServiceType.Microchip && a.AppointmentStatus == EnumList.AppointmentStatus.Completed && a.isDeleted == false, cancellationToken);
        }

        public async Task<int> GetTotalCancelledAppointmentMicrochips(CancellationToken cancellationToken)
        {
            return await _context.Appointments
                .CountAsync(a => a.ServiceType == EnumList.ServiceType.Microchip && a.AppointmentStatus == EnumList.AppointmentStatus.Cancelled && a.isDeleted == false, cancellationToken);
        }

        public async Task<int> GetTotalRejectedAppointmentMicrochips(CancellationToken cancellationToken)
        {
            return await _context.Appointments
                .CountAsync(a => a.ServiceType == EnumList.ServiceType.Microchip && a.AppointmentStatus == EnumList.AppointmentStatus.Rejected && a.isDeleted == false, cancellationToken);
        }

        public async Task<int> GetTotalAppointmentHealthConditions(CancellationToken cancellationToken)
        {
            return await _context.Appointments
                .CountAsync(a => a.ServiceType == EnumList.ServiceType.HealthCondition && a.isDeleted == false, cancellationToken);
        }

        public async Task<int> GetTotalProcessingAppointmentHealthConditions(CancellationToken cancellationToken)
        {
            return await _context.Appointments
                .CountAsync(a => a.ServiceType == EnumList.ServiceType.HealthCondition && a.AppointmentStatus == EnumList.AppointmentStatus.Processing && a.isDeleted == false, cancellationToken);
        }

        public async Task<int> GetTotalConfirmedAppointmentHealthConditions(CancellationToken cancellationToken)
        {
            return await _context.Appointments
                .CountAsync(a => a.ServiceType == EnumList.ServiceType.HealthCondition && a.AppointmentStatus == EnumList.AppointmentStatus.Confirmed && a.isDeleted == false, cancellationToken);
        }

        public async Task<int> GetTotalCheckedInAppointmentHealthConditions(CancellationToken cancellationToken)
        {
            return await _context.Appointments
                .CountAsync(a => a.ServiceType == EnumList.ServiceType.HealthCondition && a.AppointmentStatus == EnumList.AppointmentStatus.CheckedIn && a.isDeleted == false, cancellationToken);
        }

        public async Task<int> GetTotalPaidAppointmentHealthConditions(CancellationToken cancellationToken)
        {
            return await _context.Appointments
                .CountAsync(a => a.ServiceType == EnumList.ServiceType.HealthCondition && a.AppointmentStatus == EnumList.AppointmentStatus.Paid && a.isDeleted == false, cancellationToken);
        }

        public async Task<int> GetTotalCompletedAppointmentHealthConditions(CancellationToken cancellationToken)
        {
            return await _context.Appointments
                .CountAsync(a => a.ServiceType == EnumList.ServiceType.HealthCondition && a.AppointmentStatus == EnumList.AppointmentStatus.Completed && a.isDeleted == false, cancellationToken);
        }

        public async Task<int> GetTotalCancelledAppointmentHealthConditions(CancellationToken cancellationToken)
        {
            return await _context.Appointments
                .CountAsync(a => a.ServiceType == EnumList.ServiceType.HealthCondition && a.AppointmentStatus == EnumList.AppointmentStatus.Cancelled && a.isDeleted == false, cancellationToken);
        }

        public async Task<int> GetTotalRejectedAppointmentHealthConditions(CancellationToken cancellationToken)
        {
            return await _context.Appointments
                .CountAsync(a => a.ServiceType == EnumList.ServiceType.HealthCondition && a.AppointmentStatus == EnumList.AppointmentStatus.Rejected && a.isDeleted == false, cancellationToken);
        }
    }
}
