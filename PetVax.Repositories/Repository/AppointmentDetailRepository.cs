﻿using Microsoft.EntityFrameworkCore;
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
    public class AppointmentDetailRepository : GenericRepository<AppointmentDetail>, IAppointmentDetailRepository
    {
        public AppointmentDetailRepository() : base()
        {
        }
        public async Task<int> AddAppointmentDetailAsync(AppointmentDetail appointmentDetail, CancellationToken cancellationToken)
        {
            return await CreateAsync(appointmentDetail, cancellationToken);
        }

        public async Task<bool> DeleteAppointmentDetailAsync(int id, CancellationToken cancellationToken)
        {
            return await DeleteAsync(id, cancellationToken);
        }

        public async Task<List<AppointmentDetail>> GetAllAppointmentDetailsAsync(CancellationToken cancellationToken)
        {
            return await GetAllAsync(cancellationToken);
        }

        public async Task<AppointmentDetail?> GetAppointmentDetailByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await GetByIdAsync(id, cancellationToken);
        }

        public async Task<AppointmentDetail> GetAppointmentDetailByPetIdAsync(int petId, CancellationToken cancellationToken)
        {
            return await _context.AppointmentDetails
                .FirstOrDefaultAsync(ad => ad.Appointment.PetId == petId, cancellationToken);
        }

        public async Task<AppointmentDetail> GetAppointmentDetailsByAppointmentIdAsync(int appointmentId, CancellationToken cancellationToken)
        {
            return await _context.AppointmentDetails
                .FirstOrDefaultAsync(ad => ad.AppointmentId == appointmentId, cancellationToken);
        }

        public async Task<AppointmentDetail> GetAppointmentDetailsByDiseaseIdAsync(int diseaseId, CancellationToken cancellationToken)
        {
            return await _context.AppointmentDetails
                .FirstOrDefaultAsync(ad => ad.DiseaseId == diseaseId, cancellationToken);
        }

        public async Task<AppointmentDetail> GetAppointmentDetailsByHealthConditionIdAsync(int healthConditionId, CancellationToken cancellationToken)
        {
            return await _context.AppointmentDetails
                .FirstOrDefaultAsync(ad => ad.HealthConditionId == healthConditionId, cancellationToken);
        }

        public Task<AppointmentDetail> GetAppointmentDetailsByMicrochipItemIdAsync(int microchipItemId, CancellationToken cancellationToken)
        {
            return _context.AppointmentDetails
                .FirstOrDefaultAsync(ad => ad.MicrochipItemId == microchipItemId, cancellationToken);
        }

        public async Task<AppointmentDetail> GetAppointmentDetailsByPassportIdAsync(int passportId, CancellationToken cancellationToken)
        {
            return await _context.AppointmentDetails
                .FirstOrDefaultAsync(ad => ad.PassportId == passportId, cancellationToken);
        }

        public async Task<List<AppointmentDetail>> GetAppointmentDetailsByServiceTypeAsync(EnumList.ServiceType serviceType, CancellationToken cancellationToken)
        {
            return await _context.AppointmentDetails
                .Where(ad => ad.ServiceType == serviceType)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<AppointmentDetail>> GetAppointmentDetailsByStatusAsync(EnumList.AppointmentStatus status, CancellationToken cancellationToken)
        {
            return await _context.AppointmentDetails
                .Where(ad => ad.AppointmentStatus == status)
                .ToListAsync(cancellationToken);
        }

        public async Task<AppointmentDetail> GetAppointmentDetailsByVaccineBatchIdAsync(int vaccineBatchId, CancellationToken cancellationToken)
        {
            return await _context.AppointmentDetails
                .FirstOrDefaultAsync(ad => ad.VaccineBatchId == vaccineBatchId, cancellationToken);
        }

        public Task<AppointmentDetail> GetAppointmentDetailsByVetIdAsync(int vetId, CancellationToken cancellationToken)
        {
            return _context.AppointmentDetails
                .FirstOrDefaultAsync(ad => ad.VetId == vetId, cancellationToken);
        }

        public async Task<int> UpdateAppointmentDetailAsync(AppointmentDetail appointmentDetail, CancellationToken cancellationToken)
        {
            return await UpdateAsync(appointmentDetail, cancellationToken);
        }
    }
}
