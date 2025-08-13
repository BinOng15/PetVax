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
        Task SendMailWhenAppointmentCancelledAsync(CancellationToken cancellationToken);
        Task<int> GetTotalAppointmentVaccinations(CancellationToken cancellationToken);
        Task<int> GetTotalProcessingAppointmentVaccinations(CancellationToken cancellationToken);
        Task<int> GetTotalConfirmedAppointmentVaccinations(CancellationToken cancellationToken);
        Task<int> GetTotalCheckedInAppointmentVaccinations(CancellationToken cancellationToken);
        Task<int> GetTotalPaidAppointmentVaccinations(CancellationToken cancellationToken);
        Task<int> GetTotalCompletedAppointmentVaccinations(CancellationToken cancellationToken);
        Task<int> GetTotalCancelledAppointmentVaccinations(CancellationToken cancellationToken);
        Task<int> GetTotalRejectedAppointmentVaccinations(CancellationToken cancellationToken);
        Task<int> GetTotalAppointmentMicrochips(CancellationToken cancellationToken);
        Task<int> GetTotalProcessingAppointmentMicrochips(CancellationToken cancellationToken);
        Task<int> GetTotalConfirmedAppointmentMicrochips(CancellationToken cancellationToken);
        Task<int> GetTotalCheckedInAppointmentMicrochips(CancellationToken cancellationToken);
        Task<int> GetTotalPaidAppointmentMicrochips(CancellationToken cancellationToken);
        Task<int> GetTotalCompletedAppointmentMicrochips(CancellationToken cancellationToken);
        Task<int> GetTotalCancelledAppointmentMicrochips(CancellationToken cancellationToken);
        Task<int> GetTotalRejectedAppointmentMicrochips(CancellationToken cancellationToken);
        Task<int> GetTotalAppointmentHealthConditions(CancellationToken cancellationToken);
        Task<int> GetTotalProcessingAppointmentHealthConditions(CancellationToken cancellationToken);
        Task<int> GetTotalConfirmedAppointmentHealthConditions(CancellationToken cancellationToken);
        Task<int> GetTotalCheckedInAppointmentHealthConditions(CancellationToken cancellationToken);
        Task<int> GetTotalProcessedAppointmentHealthConditions(CancellationToken cancellationToken);
        Task<int> GetTotalPaidAppointmentHealthConditions(CancellationToken cancellationToken);
        Task<int> GetTotalCompletedAppointmentHealthConditions(CancellationToken cancellationToken);
        Task<int> GetTotalCancelledAppointmentHealthConditions(CancellationToken cancellationToken);
        Task<int> GetTotalRejectedAppointmentHealthConditions(CancellationToken cancellationToken);
        Task<int> GetTotalAppointmentsToday(CancellationToken cancellationToken);
        Task<int> GetTotalAppointmentsThisWeek(CancellationToken cancellationToken);
        Task<int> GetTotalAppointmentsThisMonth(CancellationToken cancellationToken);
        Task<int> GetTotalAppointmentsThisYear(CancellationToken cancellationToken);
    }
}
