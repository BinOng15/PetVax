using Microsoft.EntityFrameworkCore.Storage;
using PetVax.BusinessObjects.DTO.AppointmentDTO;
using PetVax.BusinessObjects.Enum;
using PetVax.BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PetVax.BusinessObjects.DTO.ResponseModel;
using static PetVax.BusinessObjects.Enum.EnumList;

namespace PetVax.Repositories.IRepository
{
    public interface IAppointmentRepository
    {
        Task<Appointment> CreateAppointmentAsync(Appointment appointment, CancellationToken cancellationToken);
        Task<bool> DeleteAppointmentAsync(int appointmentId, CancellationToken cancellationToken);
        Task<List<Appointment>> GetAllAppointmentsAsync(CancellationToken cancellationToken);
        Task<Appointment> GetAppointmentByIdAsync(int appointmentId, CancellationToken cancellationToken);
        Task<Appointment> UpdateAppointmentAsync(Appointment appointment, CancellationToken cancellationToken);
        Task<List<Appointment>> GetAppointmentsByPetIdAsync(int petId, CancellationToken cancellationToken);
        Task<List<Appointment>> GetAppointmentByPetIdAndStatusAsync(int petId, EnumList.AppointmentStatus status, CancellationToken cancellationToken);
        Task<List<Appointment>> GetAppointmentsByCustomerIdAsync(int customerId, CancellationToken cancellationToken);
        Task<IDbContextTransaction> BeginTransactionAsync();
        Task<List<Appointment>> GetAppointmentsByStatusAsync(AppointmentStatus status, CancellationToken cancellationToken);
        Task<List<Appointment>> GetAppointmentsByCustomerIdAndStatusAsync(int customerId, AppointmentStatus status, CancellationToken cancellationToken);
        Task<List<Appointment>> GetAppointmentsByDateRangeAsync(DateTime from, DateTime to, CancellationToken cancellationToken);
        Task<List<Appointment>> GetPastAppointmentsByCustomerIdAsync(DateTime now, int customerId, CancellationToken cancellationToken);
        Task<List<Appointment>> GetTodayAppointmentsByCustomerIdAsync(DateTime today, int customerId, CancellationToken cancellationToken);
        Task<List<Appointment>> GetFutureAppointmentsByCustomerIdAsync(DateTime now, int customerId, CancellationToken cancellationToken);
        Task UpdateExpiredAppointmentsAsync(CancellationToken cancellationToken);
    }
}
