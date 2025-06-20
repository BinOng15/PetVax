﻿using PetVax.BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetVax.Repositories.IRepository
{
    public interface IAppointmentRepository
    {
        Task<int> CreateAppointmentAsync(Appointment appointment, CancellationToken cancellationToken);
        Task<bool> DeleteAppointmentAsync(int appointmentId, CancellationToken cancellationToken);
        Task<List<Appointment>> GetAllAppointmentsAsync(CancellationToken cancellationToken);
        Task<Appointment> GetAppointmentByIdAsync(int appointmentId, CancellationToken cancellationToken);
        Task<int> UpdateAppointmentAsync(Appointment appointment, CancellationToken cancellationToken);
        Task<List<Appointment>> GetAppointmentsByPetIdAsync(int petId, CancellationToken cancellationToken);
    }
}
