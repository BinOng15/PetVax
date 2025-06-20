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
        public async Task<int> CreateAppointmentAsync(Appointment appointment, CancellationToken cancellationToken)
        {
            _context.Add(appointment);
            await _context.SaveChangesAsync(cancellationToken);
            return appointment.AppointmentId;
        }
        public async Task<bool> DeleteAppointmentAsync(int appointmentId, CancellationToken cancellationToken)
        {
            return await DeleteAppointmentAsync(appointmentId, cancellationToken);
        }

        public async Task<List<Appointment>> GetAllAppointmentsAsync(CancellationToken cancellationToken)
        {
            return await GetAllAsync(cancellationToken);
        }
        public async Task<Appointment> GetAppointmentByIdAsync(int appointmentId, CancellationToken cancellationToken)
        {
            return await GetByIdAsync(appointmentId, cancellationToken);
        }
        public async Task<List<Appointment>> GetAppointmentByPetIdAndStatusAsync(int petId, EnumList.AppointmentStatus status, CancellationToken cancellationToken)
        {
            return await _context.Set<Appointment>()
                .Where(a => a.PetId == petId && a.AppointmentStatus == status)
                .ToListAsync(cancellationToken);
        }
        public async Task<List<Appointment>> GetAppointmentsByCustomerIdAsync(int customerId, CancellationToken cancellationToken)
        {
            return await _context.Set<Appointment>()
                .Where(a => a.CustomerId == customerId)
                .ToListAsync(cancellationToken);
        }
        public async Task<List<Appointment>> GetAppointmentsByPetIdAsync(int petId, CancellationToken cancellationToken)
        {
            return await _context.Set<Appointment>()
                .Where(a => a.PetId == petId)
                .ToListAsync(cancellationToken);
        }
        public async Task<int> UpdateAppointmentAsync(Appointment appointment, CancellationToken cancellationToken)
        {
            return await UpdateAsync(appointment, cancellationToken);
        }
    }
}
