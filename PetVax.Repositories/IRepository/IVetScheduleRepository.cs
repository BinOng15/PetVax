﻿using PetVax.BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetVax.Repositories.IRepository
{
    public interface IVetScheduleRepository
    {
        Task<List<VetSchedule>> GetAllVetSchedulesAsync(CancellationToken cancellationToken);
        Task<VetSchedule> GetVetScheduleByIdAsync(int vetScheduleId, CancellationToken cancellationToken);
        Task<int> CreateVetScheduleAsync(VetSchedule vetSchedule, CancellationToken cancellationToken);
        Task<int> UpdateVetScheduleAsync(VetSchedule vetSchedule, CancellationToken cancellationToken);
        Task<bool> DeleteVetScheduleAsync(int vetScheduleId, CancellationToken cancellationToken);

        Task<List<VetSchedule>> GetVetSchedulesByVetIdAsync(int vetId, CancellationToken cancellationToken);
    }
}
