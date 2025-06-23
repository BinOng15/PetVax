using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
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
            return await DeleteAppointmentAsync(appointmentId, cancellationToken);
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
                .Where(a => a.PetId == petId && a.AppointmentStatus == status)
                .ToListAsync(cancellationToken);
        }
        public async Task<List<Appointment>> GetAppointmentsByCustomerIdAsync(int customerId, CancellationToken cancellationToken)
        {
            return await _context.Set<Appointment>()
                .Include(a => a.Customer)
                .ThenInclude(c => c.Account)
                .Include(a => a.Pet)
                .Where(a => a.CustomerId == customerId)
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
                .Where(a => a.AppointmentStatus == status)
                .ToListAsync(cancellationToken);
        }

        public async Task<int> UpdateAppointmentAsync(Appointment appointment, CancellationToken cancellationToken)
        {
            return await UpdateAsync(appointment, cancellationToken);
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
                .Where(a => a.AppointmentDate < now && a.CustomerId == customerId)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<Appointment>> GetTodayAppointmentsByCustomerIdAsync(DateTime today, int customerId, CancellationToken cancellationToken)
        {
            return await _context.Set<Appointment>()
                .Include(a => a.Customer)
                    .ThenInclude(c => c.Account)
                .Include(a => a.Pet)
                .Where(a => a.AppointmentDate.Date == today.Date && a.CustomerId == customerId)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<Appointment>> GetFutureAppointmentsByCustomerIdAsync(DateTime now, int customerId, CancellationToken cancellationToken)
        {
            return await _context.Set<Appointment>()
                .Include(a => a.Customer)
                    .ThenInclude(c => c.Account)
                .Include(a => a.Pet)
                .Where(a => a.AppointmentDate > now && a.CustomerId == customerId)
                .ToListAsync(cancellationToken);
        }
    }
}
