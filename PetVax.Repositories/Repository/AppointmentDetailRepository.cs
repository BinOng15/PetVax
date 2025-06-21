using Microsoft.EntityFrameworkCore;
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
            _context.Add(appointmentDetail);
            await _context.SaveChangesAsync(cancellationToken);
            return appointmentDetail.AppointmentDetailId;
        }

        public async Task<bool> DeleteAppointmentDetailAsync(int id, CancellationToken cancellationToken)
        {
            return await DeleteAsync(id, cancellationToken);
        }

        public async Task<List<AppointmentDetail>> GetAllAppointmentDetailsAsync(CancellationToken cancellationToken)
        {
            return await _context.AppointmentDetails
                .Include(ad => ad.Vet)
                .Include(ad => ad.MicrochipItem)
                .Include(ad => ad.PetPassport)
                .Include(ad => ad.HealthCondition)
                .Include(ad => ad.VaccineBatch)
                .Include(ad => ad.Disease)
                .Include(ad => ad.Appointment)
                    .ThenInclude(a => a.Customer)
                .Include(ad => ad.Appointment)
                    .ThenInclude(a => a.Pet)
                .ToListAsync(cancellationToken);
        }

        public async Task<AppointmentDetail> GetAppointmentDetailByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.AppointmentDetails
                .Include(a => a.Appointment)
                .Include(a => a.Vet)
                .Include(a => a.Disease)
                .Include(a => a.VaccineBatch)
                .Include(a => a.MicrochipItem)
                .FirstOrDefaultAsync(a => a.AppointmentDetailId == id, cancellationToken);
        }

        public async Task<AppointmentDetail> GetAppointmentDetailByPetIdAsync(int petId, CancellationToken cancellationToken)
        {
            return await _context.AppointmentDetails
                .Include(a => a.Appointment)
                .Include(a => a.Vet)
                .Include(a => a.Disease)
                .Include(a => a.MicrochipItem)
                .Include(a => a.VaccineBatch)
                .FirstOrDefaultAsync(ad => ad.Appointment.PetId == petId, cancellationToken);
        }

        public async Task<AppointmentDetail> GetAppointmentDetailsByAppointmentIdAsync(int appointmentId, CancellationToken cancellationToken)
        {
            return await _context.AppointmentDetails
                .Include(a => a.Appointment)
                .Include(a => a.Vet)
                .Include(a => a.Disease)
                .Include(a => a.MicrochipItem)
                .Include(a => a.VaccineBatch)
                .FirstOrDefaultAsync(ad => ad.AppointmentId == appointmentId, cancellationToken);
        }

        public async Task<AppointmentDetail> GetAppointmentDetailsByDiseaseIdAsync(int diseaseId, CancellationToken cancellationToken)
        {
            return await _context.AppointmentDetails
                .Include(a => a.Appointment)
                .Include(a => a.Vet)
                .Include(a => a.Disease)
                .Include(a => a.VaccineBatch)
                .FirstOrDefaultAsync(ad => ad.DiseaseId == diseaseId, cancellationToken);
        }

        public async Task<AppointmentDetail> GetAppointmentDetailsByHealthConditionIdAsync(int healthConditionId, CancellationToken cancellationToken)
        {
            return await _context.AppointmentDetails
                .Include(a => a.Appointment)
                .Include(a => a.Vet)
                .Include(a => a.Disease)
                .Include(a => a.MicrochipItem)
                .Include(a => a.VaccineBatch)
                .FirstOrDefaultAsync(ad => ad.HealthConditionId == healthConditionId, cancellationToken);
        }

        public Task<AppointmentDetail> GetAppointmentDetailsByMicrochipItemIdAsync(int microchipItemId, CancellationToken cancellationToken)
        {
            return _context.AppointmentDetails
                .Include(a => a.Appointment)
                .Include(a => a.MicrochipItem)
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
                .Include(a => a.Appointment)
                .Include(a => a.Vet)
                .Include(a => a.Disease)
                .Include(a => a.MicrochipItem)
                .Include(a => a.VaccineBatch)
                .Where(ad => ad.ServiceType == serviceType)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<AppointmentDetail>> GetAppointmentDetailsByStatusAsync(EnumList.AppointmentStatus status, CancellationToken cancellationToken)
        {
            return await _context.AppointmentDetails
                .Include(a => a.Appointment)
                .Include(a => a.Vet)
                .Include(a => a.Disease)
                .Include(a => a.MicrochipItem)
                .Include(a => a.VaccineBatch)
                .Where(ad => ad.AppointmentStatus == status)
                .ToListAsync(cancellationToken);
        }

        public async Task<AppointmentDetail> GetAppointmentDetailsByVaccineBatchIdAsync(int vaccineBatchId, CancellationToken cancellationToken)
        {
            return await _context.AppointmentDetails
                .Include(a => a.Appointment)
                .Include(a => a.Vet)
                .Include(a => a.Disease)
                .Include(a => a.VaccineBatch)
                .FirstOrDefaultAsync(ad => ad.VaccineBatchId == vaccineBatchId, cancellationToken);
        }

        public Task<AppointmentDetail> GetAppointmentDetailsByVetIdAsync(int vetId, CancellationToken cancellationToken)
        {
            return _context.AppointmentDetails
                .Include(a => a.Appointment)
                .Include(a => a.Vet)
                .Include(a => a.Disease)
                .Include(a => a.MicrochipItem)
                .Include(a => a.VaccineBatch)
                .FirstOrDefaultAsync(ad => ad.VetId == vetId, cancellationToken);
        }

        public async Task<AppointmentDetail> GetAppointmentDetailWithRelationsAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.AppointmentDetails
                .Include(ad => ad.Vet)
                .Include(ad => ad.VaccineBatch)
                .Include(ad => ad.Disease)
                .FirstOrDefaultAsync(ad => ad.AppointmentDetailId == id, cancellationToken);
        }

        public async Task<List<AppointmentDetail>> GetAppointmentVaccinationDetailByPetId(int petId, CancellationToken cancellationToken)
        {
            return await _context.AppointmentDetails
                .Where(ad => ad.Appointment.PetId == petId &&
                            ad.ServiceType == EnumList.ServiceType.Vaccination)
                .Include(ad => ad.Vet)
                .Include(ad => ad.VaccineBatch)
                    .ThenInclude(vb => vb.Vaccine)
                .Include(ad => ad.Disease)
                .Include(ad => ad.Appointment)
                    .ThenInclude(a => a.Pet)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<AppointmentDetail>> GetAppointmentVaccinationDetailByPetIdAndStatus(int petId, EnumList.AppointmentStatus status, CancellationToken cancellationToken)
        {
            return await _context.AppointmentDetails
                .Where(ad => ad.Appointment.PetId == petId &&
                            ad.ServiceType == EnumList.ServiceType.Vaccination &&
                            ad.AppointmentStatus == status)
                .Include(ad => ad.Vet)
                .Include(ad => ad.VaccineBatch)
                    .ThenInclude(vb => vb.Vaccine)
                .Include(ad => ad.Disease)
                .Include(ad => ad.Appointment)
                    .ThenInclude(a => a.Pet)
                .ToListAsync(cancellationToken);
        }

        public async Task<int> UpdateAppointmentDetailAsync(AppointmentDetail appointmentDetail, CancellationToken cancellationToken)
        {
            return await UpdateAsync(appointmentDetail, cancellationToken);
        }

        public async Task<AppointmentDetail> GetAppointmentDetailandServiceTypeByPetIdAsync(int petId, CancellationToken cancellationToken)
        {
            return await _context.AppointmentDetails
                .Include(ad => ad.Vet)
                .Include(ad => ad.PetPassport)
                .Include(ad => ad.HealthCondition)
                .Include(ad => ad.VaccineBatch)
                .Include(ad => ad.Disease)
                .FirstOrDefaultAsync(ad => ad.Appointment.PetId == petId, cancellationToken);
        }
    }
}
