using PetVax.BusinessObjects.Enum;
using PetVax.BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PetVax.BusinessObjects.Enum.EnumList;

namespace PetVax.Repositories.IRepository
{
    public interface IAppointmentDetailRepository
    {
        Task<List<AppointmentDetail>> GetAllAppointmentDetailsAsync(CancellationToken cancellationToken);
        Task<AppointmentDetail> GetAppointmentDetailByIdAsync(int id, CancellationToken cancellationToken);
        Task<AppointmentDetail> AddAppointmentDetailAsync(AppointmentDetail appointmentDetail, CancellationToken cancellationToken);
        Task<int> UpdateAppointmentDetailAsync(AppointmentDetail appointmentDetail, CancellationToken cancellationToken);
        //Task<bool> DeleteAppointmentDetailAsync(int id, CancellationToken cancellationToken);
        Task<AppointmentDetail> GetAppointmentDetailsByAppointmentIdAsync(int appointmentId, CancellationToken cancellationToken);
        Task<AppointmentDetail> GetAppointmentDetailsByVetIdAsync(int vetId, CancellationToken cancellationToken);
        Task<AppointmentDetail> GetAppointmentDetailsByMicrochipItemIdAsync(int microchipItemId, CancellationToken cancellationToken);
        Task<AppointmentDetail> GetAppointmentDetailsByPassportIdAsync(int passportId, CancellationToken cancellationToken);
        Task<AppointmentDetail> GetAppointmentDetailsByHealthConditionIdAsync(int healthConditionId, CancellationToken cancellationToken);
        Task<AppointmentDetail> GetAppointmentDetailsByVaccineBatchIdAsync(int vaccineBatchId, CancellationToken cancellationToken);
        Task<AppointmentDetail> GetAppointmentDetailsByDiseaseIdAsync(int diseaseId, CancellationToken cancellationToken);
        Task<AppointmentDetail> GetAppointmentDetailByPetIdAsync(int petId, CancellationToken cancellationToken);
        Task<List<AppointmentDetail>> GetAppointmentDetailsByStatusAsync(EnumList.AppointmentStatus status, CancellationToken cancellationToken);
        Task<List<AppointmentDetail>> GetAppointmentDetailsByServiceTypeAsync(EnumList.ServiceType serviceType, CancellationToken cancellationToken);
        Task<AppointmentDetail> GetAppointmentDetailandServiceTypeByPetIdAsync(int petId, CancellationToken cancellationToken);
        //Task<AppointmentDetail> GetAppointmentDetailWithRelationsAsync(int id, CancellationToken cancellationToken);
        Task<List<AppointmentDetail>> GetAppointmentVaccinationDetailByPetId(int petId, CancellationToken cancellationToken);
        Task<List<AppointmentDetail>> GetAllAppointmentDetailByPetIdAsync(int petId, CancellationToken cancellationToken);

        //Vaccination
        Task<List<AppointmentDetail>> GetAppointmentVaccinationDetailByPetIdAndStatus(int petId, EnumList.AppointmentStatus status, CancellationToken cancellationToken);
        Task<AppointmentDetail> GetAppointmentVaccinationByAppointmentId(int appointmentId, CancellationToken cancellationToken);
        Task<List<AppointmentDetail>> GetAllAppointmentDetailsForVaccinationAsync(CancellationToken cancellationToken);

        //Microchip
        Task<List<AppointmentDetail>> GetAppointmentDetaiMicrochiplsByPetIdAsync(int petId, CancellationToken cancellationToken);
        Task<List<AppointmentDetail>> GetAllAppointmentDetailsMicrochipAsync(CancellationToken cancellationToken);
    }
}
