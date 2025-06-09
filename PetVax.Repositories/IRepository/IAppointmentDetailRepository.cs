using PetVax.BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetVax.Repositories.IRepository
{
    public interface IAppointmentDetailRepository
    {
        Task<List<AppointmentDetail>> GetAllAppointmentDetailsAsync(CancellationToken cancellationToken);
        Task<AppointmentDetail?> GetAppointmentDetailByIdAsync(int id, CancellationToken cancellationToken);
        Task<int> AddAppointmentDetailAsync(AppointmentDetail appointmentDetail, CancellationToken cancellationToken);
        Task<int> UpdateAppointmentDetailAsync(AppointmentDetail appointmentDetail, CancellationToken cancellationToken);
        Task<bool> DeleteAppointmentDetailAsync(int id, CancellationToken cancellationToken);
        Task<AppointmentDetail> GetAppointmentDetailsByAppointmentIdAsync(int appointmentId, CancellationToken cancellationToken);
        Task<AppointmentDetail> GetAppointmentDetailsByVetIdAsync(int vetId, CancellationToken cancellationToken);
        Task<AppointmentDetail> GetAppointmentDetailsByMicrochipItemIdAsync(int microchipItemId, CancellationToken cancellationToken);
    }
}
