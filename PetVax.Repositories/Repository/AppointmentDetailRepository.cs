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
using static PetVax.BusinessObjects.Enum.EnumList;

namespace PetVax.Repositories.Repository
{
    public class AppointmentDetailRepository : GenericRepository<AppointmentDetail>, IAppointmentDetailRepository
    {
        public AppointmentDetailRepository() : base()
        {
        }
        public async Task<AppointmentDetail> AddAppointmentDetailAsync(AppointmentDetail appointmentDetail, CancellationToken cancellationToken)
        {
            _context.Add(appointmentDetail);
            await _context.SaveChangesAsync(cancellationToken);
            return appointmentDetail;
        }

        public async Task<bool> DeleteAppointmentDetailAsync(int id, CancellationToken cancellationToken)
        {
            return await DeleteAsync(id, cancellationToken);
        }

        public async Task<List<AppointmentDetail>> GetAllAppointmentDetailsAsync(CancellationToken cancellationToken)
        {
            return await _context.AppointmentDetails
                .Include(ad => ad.Vet)
                .Include(ad => ad.MicrochipItem).ThenInclude(mc => mc.Microchip)
                .Include(ad => ad.VaccinationCertificate)
                .Include(ad => ad.HealthCondition)
                .Include(ad => ad.VaccineBatch)
                .Include(ad => ad.Disease)
                .Include(ad => ad.Appointment)
                    .ThenInclude(a => a.Customer)
                .Include(ad => ad.Appointment)
                    .ThenInclude(a => a.Pet)
                .Include(ad => ad.Payment)
                .ToListAsync(cancellationToken);
        }

        public async Task<AppointmentDetail> GetAppointmentDetailByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.AppointmentDetails
                .Include(ad => ad.MicrochipItem).ThenInclude(mi => mi.Microchip)
                .Include(ad => ad.Vet).ThenInclude(v => v.Account)
                .Include(ad => ad.Appointment)
                    .ThenInclude(a => a.Customer).ThenInclude(c => c.Account)
                .Include(ad => ad.Appointment)
                    .ThenInclude(a => a.Pet)
                .Include(ad => ad.Disease)
                .Include(ad => ad.Payment)
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
                .Include(ad => ad.Payment)

                .FirstOrDefaultAsync(ad => ad.Appointment.PetId == petId, cancellationToken);
        }

        public async Task<List<AppointmentDetail>> GetAllAppointmentDetailByPetIdAsync(int petId, CancellationToken cancellationToken)
        {
            return await _context.AppointmentDetails
                .Include(a => a.Appointment)
                .Include(a => a.Vet)
                .Include(a => a.Disease)
                .Include(a => a.MicrochipItem).ThenInclude(m => m.Microchip)
                .Include(a => a.VaccineBatch)
                .Include(ad => ad.Payment)

                .Where(ad => ad.Appointment.PetId == petId)
                .ToListAsync(cancellationToken);
        }

        public async Task<AppointmentDetail> GetAppointmentDetailsByAppointmentIdAsync(int appointmentId, CancellationToken cancellationToken)
        {
            return await _context.AppointmentDetails
             .Include(a => a.Appointment)
                 .ThenInclude(a => a.Customer).ThenInclude(c => c.Account)
             .Include(a => a.Appointment)
                 .ThenInclude(a => a.Pet)
             .Include(a => a.Vet)
             .Include(a => a.Disease)
             .Include(a => a.MicrochipItem)
                 .ThenInclude(m => m.Microchip)
             .Include(a => a.VaccineBatch)
                .Include(ad => ad.Payment)

             .FirstOrDefaultAsync(ad => ad.AppointmentId == appointmentId, cancellationToken);

        }

        public async Task<AppointmentDetail> GetAppointmentDetailsByDiseaseIdAsync(int diseaseId, CancellationToken cancellationToken)
        {
            return await _context.AppointmentDetails
                .Include(a => a.Appointment)
                .Include(a => a.Vet)
                .Include(a => a.Disease)
                .Include(a => a.VaccineBatch)
                .Include(ad => ad.Payment)

                .FirstOrDefaultAsync(ad => ad.DiseaseId == diseaseId, cancellationToken);
        }

  

        public Task<AppointmentDetail> GetAppointmentDetailsByMicrochipItemIdAsync(int? microchipItemId, CancellationToken cancellationToken)
        {
            return _context.AppointmentDetails
                .Include(a => a.Appointment)
                    .ThenInclude(a => a.Customer).ThenInclude(c => c.Account)
                .Include(a => a.Appointment)
                    .ThenInclude(a => a.Pet)
                .Include(a => a.Vet).ThenInclude(v => v.Account)
                .Include(a => a.Vet).ThenInclude(v => v.VetSchedules)
                .Include(a => a.MicrochipItem).ThenInclude(m => m.Microchip)
                .Include(ad => ad.Payment)

                .FirstOrDefaultAsync(ad => ad.MicrochipItemId == microchipItemId, cancellationToken);
        }

        public async Task<List<AppointmentDetail>> GetAppointmentDetaiMicrochiplsByPetIdAsync(int petId, CancellationToken cancellationToken)
        {
            return await _context.AppointmentDetails
                .Include(ad => ad.MicrochipItem).ThenInclude(mi => mi.Microchip)
                .Include(ad => ad.Vet).ThenInclude(v => v.Account)
                .Include(ad => ad.Vet).ThenInclude(v => v.VetSchedules)
                .Include(ad => ad.Appointment)
                    .ThenInclude(a => a.Customer).ThenInclude(c => c.Account)
                .Include(ad => ad.Appointment)
                    .ThenInclude(a => a.Pet)
                .Include(ad => ad.Payment)

                .Where(ad => ad.Appointment.PetId == petId && ad.ServiceType == ServiceType.Microchip && ad.isDeleted == false)
                .ToListAsync(cancellationToken);
        }

        public async Task<AppointmentDetail> GetAppointmentDetailsByPassportIdAsync(int passportId, CancellationToken cancellationToken)
        {
            return await _context.AppointmentDetails
                .FirstOrDefaultAsync(ad => ad.VaccinationCertificateId == passportId, cancellationToken);
        }
        public async Task<List<AppointmentDetail>> GetAllAppointmentDetailsMicrochipAsync(CancellationToken cancellationToken)
        {
            return await _context.AppointmentDetails
           .Include(ad => ad.MicrochipItem).ThenInclude(mi => mi.Microchip)
           .Include(ad => ad.Vet).ThenInclude(v => v.Account)
           .Include(ad => ad.Appointment)
               .ThenInclude(a => a.Customer).ThenInclude(c => c.Account)
           .Include(ad => ad.Appointment)
               .ThenInclude(a => a.Pet)
           .Include(ad => ad.Payment)

           .Where(ad => ad.Appointment.ServiceType == ServiceType.Microchip && ad.isDeleted == false)
           .ToListAsync(cancellationToken);
        }

        public async Task<List<AppointmentDetail>> GetAllAppointmentDetailsMicrochipByPetIdAndStatusAsync(int petId, AppointmentStatus status, CancellationToken cancellationToken)
        {
            return await _context.AppointmentDetails
           .Include(ad => ad.MicrochipItem).ThenInclude(mi => mi.Microchip)
           .Include(ad => ad.Vet).ThenInclude(v => v.Account)
           .Include(ad => ad.Vet).ThenInclude(v => v.VetSchedules)
           .Include(ad => ad.Appointment)
               .ThenInclude(a => a.Customer).ThenInclude(c => c.Account)
           .Include(ad => ad.Appointment)
               .ThenInclude(a => a.Pet)
           .Include(ad => ad.Payment)

           .Where(ad => ad.Appointment.PetId == petId && ad.AppointmentStatus == status && ad.Appointment.ServiceType == ServiceType.Microchip && ad.isDeleted == false)
           .ToListAsync(cancellationToken);
        }
        public async Task<List<AppointmentDetail>> GetAppointmentDetailsByServiceTypeAsync(EnumList.ServiceType serviceType, CancellationToken cancellationToken)
        {
            return await _context.AppointmentDetails
                .Include(a => a.Appointment)
                .Include(a => a.Vet)
                .Include(a => a.Disease)
                .Include(a => a.MicrochipItem).ThenInclude(m => m.Microchip)
                .Include(a => a.VaccineBatch)
                .Include(ad => ad.Payment)

                .Where(ad => ad.ServiceType == serviceType)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<AppointmentDetail>> GetAppointmentDetailsByStatusAsync(EnumList.AppointmentStatus status, CancellationToken cancellationToken)
        {
            return await _context.AppointmentDetails
                .Include(a => a.Appointment)
                .Include(a => a.Vet)
                .Include(a => a.Disease)
                .Include(a => a.MicrochipItem).ThenInclude(m => m.Microchip)
                .Include(a => a.VaccineBatch)
                .Include(ad => ad.Payment)
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
                .Include(ad => ad.Payment)
                .FirstOrDefaultAsync(ad => ad.VaccineBatchId == vaccineBatchId, cancellationToken);
        }

        public Task<AppointmentDetail> GetAppointmentDetailsByVetIdAsync(int vetId, CancellationToken cancellationToken)
        {
            return _context.AppointmentDetails
                .Include(a => a.Appointment)
                .Include(a => a.Vet)
                .Include(a => a.Disease)
                .Include(a => a.MicrochipItem).ThenInclude(m => m.Microchip)
                .Include(a => a.VaccineBatch)
                .Include(ad => ad.Payment)
                .FirstOrDefaultAsync(ad => ad.VetId == vetId, cancellationToken);
        }

        public async Task<AppointmentDetail> GetAppointmentDetailWithRelationsAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.AppointmentDetails
                .Include(ad => ad.Vet)
                .Include(ad => ad.VaccineBatch)
                .Include(ad => ad.Disease)
                .Include(ad => ad.Payment)

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
                .Include(ad => ad.Payment)

                .ToListAsync(cancellationToken);
        }

        public async Task<List<AppointmentDetail>> GetAppointmentVaccinationDetailByPetIdAndStatus(int petId, EnumList.AppointmentStatus status, CancellationToken cancellationToken)
        {
            return await _context.AppointmentDetails
                .Where(ad => ad.Appointment.PetId == petId &&
                            ad.ServiceType == EnumList.ServiceType.Vaccination &&
                            ad.AppointmentStatus == status)
                .Include(ad => ad.Vet)
                    .ThenInclude(v => v.Account)
                .Include(ad => ad.Vet)
                    .ThenInclude(v => v.VetSchedules)
                .Include(ad => ad.VaccineBatch)
                    .ThenInclude(vb => vb.Vaccine)
                .Include(ad => ad.Disease)
                .Include(ad => ad.Appointment)
                    .ThenInclude(a => a.Pet)
                .Include(ad => ad.Payment)

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
                .Include(ad => ad.VaccinationCertificate)
                .Include(ad => ad.HealthCondition)
                .Include(ad => ad.VaccineBatch)
                .Include(ad => ad.Disease)
                .Include(ad => ad.MicrochipItem).ThenInclude(m => m.Microchip)
                .Include(ad => ad.Payment)

                .FirstOrDefaultAsync(ad => ad.Appointment.PetId == petId, cancellationToken);
        }

        public async Task<AppointmentDetail> GetAppointmentVaccinationByAppointmentId(int appointmentId, CancellationToken cancellationToken)
        {
            return await _context.AppointmentDetails
               .Include(ad => ad.Vet)
                   .ThenInclude(v => v.VetSchedules) 
               .Include(ad => ad.Vet)
                   .ThenInclude(v => v.Account)
               .Include(ad => ad.VaccineBatch)
               .Include(ad => ad.Disease)
               .Include(ad => ad.Appointment)
                   .ThenInclude(a => a.Pet)
                       .ThenInclude(p => p.Customer)
                            .ThenInclude(a => a.Account)
                .Include(ad => ad.Payment)

               .FirstOrDefaultAsync(ad => ad.AppointmentId == appointmentId, cancellationToken);
        }

        public async Task<List<AppointmentDetail>> GetAllAppointmentDetailsForVaccinationAsync(CancellationToken cancellationToken)
        {
            return await _context.AppointmentDetails
                .Include(ad => ad.Vet)
                .Include(ad => ad.VaccineBatch)
                    .ThenInclude(vb => vb.Vaccine)
                .Include(ad => ad.Disease)
                .Include(ad => ad.Appointment)
                    .ThenInclude(a => a.Customer).ThenInclude(c => c.Account)
                .Include(ad => ad.Appointment)
                    .ThenInclude(a => a.Pet)
                .Include(ad => ad.Payment)
                .Where(ad => ad.ServiceType == ServiceType.Vaccination)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<AppointmentDetail>> GetAppointmentVaccinationCertificateByPetIdAsync(int petId, CancellationToken cancellationToken)
        {
            return await _context.AppointmentDetails
                .Include(ad => ad.Vet)
                .Include(ad => ad.VaccinationCertificate)
                .Include(ad => ad.HealthCondition)
                .Include(ad => ad.VaccineBatch)
                .Include(ad => ad.Disease)
                .Include(ad => ad.Appointment)
                    .ThenInclude(a => a.Pet)
                .Include(ad => ad.Payment)

                .Where(ad => ad.Appointment.PetId == petId && ad.ServiceType == ServiceType.VaccinationCertificate)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<AppointmentDetail>> GetAllAppointmentDetailsVaccinationCertificateAsync(CancellationToken cancellationToken)
        {
            return await _context.AppointmentDetails
                .Include(ad => ad.Vet)
                .Include(ad => ad.VaccinationCertificate)
                .Include(ad => ad.HealthCondition)
                .Include(ad => ad.VaccineBatch)
                .Include(ad => ad.Disease)
                .Include(ad => ad.Appointment)
                    .ThenInclude(a => a.Customer).ThenInclude(c => c.Account)
                .Include(ad => ad.Appointment)
                    .ThenInclude(a => a.Pet)
                .Include(ad => ad.Payment)

                .Where(ad => ad.ServiceType == ServiceType.VaccinationCertificate && ad.isDeleted == false)
                .ToListAsync(cancellationToken);
        }

        public async Task<AppointmentDetail> GetAppointmentVaccinationCertificateByAppointmentId(int appointmentId, CancellationToken cancellationToken)
        {
            return await _context.AppointmentDetails
                .Include(ad => ad.Vet)
                .Include(ad => ad.VaccinationCertificate)
                .Include(ad => ad.HealthCondition)
                .Include(ad => ad.VaccineBatch)
                .Include(ad => ad.Disease)
                .Include(ad => ad.Appointment)
                    .ThenInclude(a => a.Pet)
                .Include(ad => ad.Payment)

                .FirstOrDefaultAsync(ad => ad.AppointmentId == appointmentId && ad.ServiceType == ServiceType.VaccinationCertificate, cancellationToken);
        }

        public async Task<List<AppointmentDetail>> GetAppointmentVaccinationCertificateByPetIdAndStatusAsync(int petId, AppointmentStatus status, CancellationToken cancellationToken)
        {
            return await _context.AppointmentDetails
                .Include(ad => ad.Vet)
                .Include(ad => ad.VaccinationCertificate)
                .Include(ad => ad.HealthCondition)
                .Include(ad => ad.VaccineBatch)
                .Include(ad => ad.Disease)
                .Include(ad => ad.Appointment)
                    .ThenInclude(a => a.Pet)
                .Include(ad => ad.Payment)
                .Where(ad => ad.Appointment.PetId == petId && ad.ServiceType == ServiceType.VaccinationCertificate && ad.AppointmentStatus == status)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<AppointmentDetail>> GetAppointmentDetailHealthConditionByPetIdAsync(int petId, CancellationToken cancellationToken)
        {
            return await _context.AppointmentDetails
                .Include (ad => ad.HealthCondition)
                .Include(ad => ad.Vet).ThenInclude(v => v.Account)
                .Include(ad => ad.Vet).ThenInclude(v => v.VetSchedules)
                .Include(ad => ad.Appointment)
                    .ThenInclude(a => a.Customer).ThenInclude(c => c.Account)
                .Include(ad => ad.Appointment)
                    .ThenInclude(a => a.Pet)
                .Include(ad => ad.Payment)
                .Where(ad => ad.Appointment.PetId == petId && ad.ServiceType == ServiceType.HealthCondition && ad.isDeleted == false)
                .ToListAsync(cancellationToken);
        }

        public async Task<AppointmentDetail> GetAppointmentDetailsByHealthConditionIdAsync(int healthConditionId, CancellationToken cancellationToken)
        {
            return await _context.AppointmentDetails
                .Include(ad => ad.HealthCondition)
                .Include(ad => ad.Vet).ThenInclude(v => v.Account)
                .Include(ad => ad.Vet).ThenInclude(v => v.VetSchedules)
                .Include(ad => ad.Appointment)
                    .ThenInclude(a => a.Customer).ThenInclude(c => c.Account)
                .Include(ad => ad.Appointment)
                    .ThenInclude(a => a.Pet)
                .Include(ad => ad.Payment)

                .FirstOrDefaultAsync(ad => ad.HealthConditionId == healthConditionId, cancellationToken);
        }

        public async Task<AppointmentDetail> GetAppointmentDetailHealthConditionByAppointmentIdAsync(int appointmentId, CancellationToken cancellationToken)
        {
            return await _context.AppointmentDetails
                .Include(ad => ad.HealthCondition)
                .Include(ad => ad.Vet).ThenInclude(v => v.Account)
                .Include(ad => ad.Vet).ThenInclude(v => v.VetSchedules)
                .Include(ad => ad.Appointment)
                    .ThenInclude(a => a.Customer).ThenInclude(c => c.Account)
                .Include(ad => ad.Appointment)
                    .ThenInclude(a => a.Pet)
                .Include(ad => ad.Payment)
                .FirstOrDefaultAsync(ad => ad.AppointmentId == appointmentId, cancellationToken);
        }

        public async Task<AppointmentDetail> GetAppointmentDetailHealthConditionByAppointmentDetailIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.AppointmentDetails
                .Include(ad => ad.HealthCondition)
                .Include(ad => ad.Vet).ThenInclude(v => v.Account)
                .Include(ad => ad.Vet).ThenInclude(v => v.VetSchedules)
                .Include(ad => ad.Appointment)
                    .ThenInclude(a => a.Customer).ThenInclude(c => c.Account)
                .Include(ad => ad.Appointment)
                    .ThenInclude(a => a.Pet)
                .Include(ad => ad.Payment)
                .FirstOrDefaultAsync(ad => ad.AppointmentDetailId == id, cancellationToken);
        }

        public async Task<List<AppointmentDetail>> GetAllAppointmentDetailHealthConditionAsync(CancellationToken cancellationToken)
        {
            return await _context.AppointmentDetails
                   .Include(ad => ad.HealthCondition)
                   .Include(ad => ad.Vet).ThenInclude(v => v.Account)
                     .Include(ad => ad.Vet).ThenInclude(v => v.VetSchedules)
                   .Include(ad => ad.Appointment)
                       .ThenInclude(a => a.Customer).ThenInclude(c => c.Account)
                   .Include(ad => ad.Appointment)
                       .ThenInclude(a => a.Pet)
                    .Include(ad => ad.Payment)
                   .Where(a => a.isDeleted == false && a.Appointment.ServiceType == ServiceType.HealthCondition)
                   .ToListAsync(cancellationToken);
        }

        public async Task<List<AppointmentDetail>> GetAllAppointmentDetailsHealthconditionByPetIdAndStatusAsync(int petId, AppointmentStatus status, CancellationToken cancellationToken)
        {
            return await _context.AppointmentDetails
           .Include(ad => ad.HealthCondition)
           .Include(ad => ad.Vet).ThenInclude(v => v.Account)
           .Include(ad => ad.Vet).ThenInclude(v => v.VetSchedules)
           .Include(ad => ad.Appointment)
               .ThenInclude(a => a.Customer).ThenInclude(c => c.Account)
           .Include(ad => ad.Appointment)
               .ThenInclude(a => a.Pet)
           .Include(ad => ad.Payment)

           .Where(ad => ad.Appointment.PetId == petId && ad.Appointment.AppointmentStatus == status && ad.Appointment.ServiceType == ServiceType.HealthCondition && ad.isDeleted == false)
           .ToListAsync(cancellationToken);
        }
    }
}
