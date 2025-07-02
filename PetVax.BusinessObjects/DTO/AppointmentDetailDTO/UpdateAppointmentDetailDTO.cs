using PetVax.BusinessObjects.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetVax.BusinessObjects.DTO.AppointmentDetailDTO
{
    public class UpdateAppointmentDetailDTO
    {
        public int? AppointmentId { get; set; }
        public int? VetId { get; set; }
        public int? DiseaseId { get; set; }
        public int? VaccineBatchId { get; set; }
        public int? MicrochipItemId { get; set; }
        public int? PassportId { get; set; }
        public int? HealthConditionId { get; set; }
        public int? Dose { get; set; }
        public string? Reaction { get; set; }
        public string? NextVaccinationInfo { get; set; }
        public DateTime? AppointmentDate { get; set; }
        public EnumList.AppointmentStatus? AppointmentStatus { get; set; }
    }

    public class UpdateAppointmentVaccinationDTO
    {
        public int? AppointmentId { get; set; }
        public int? VetId { get; set; }
        public int? DiseaseId { get; set; }
        public int? VaccineBatchId { get; set; }
        public string? Reaction { get; set; }
        public string? NextVaccinationInfo { get; set; }
        public string? Temperature { get; set; }
        public string? HeartRate { get; set; }
        public string? GeneralCondition { get; set; }
        public string? Others { get; set; }
        public string? Notes { get; set; }
        public DateTime? AppointmentDate { get; set; }
        public EnumList.AppointmentStatus? AppointmentStatus { get; set; }
    }

    public class UpdateAppointmentMicrochipDTO
    {
        public int AppointmentId { get; set; }
        public int? VetId { get; set; }
        public int? MicrochipItemId { get; set; }

        public string? Description { get; set; }
        public string? Note { get; set; }
        public EnumList.AppointmentStatus? AppointmentStatus { get; set; }
    }

    public class UpdateAppointmentHealthCertificateDTO
    {
        public int? AppointmentId { get; set; }
        public int? VetId { get; set; }
        public int? PassportId { get; set; }
        public int? HealthConditionId { get; set; }
        public DateTime? AppointmentDate { get; set; }
        public EnumList.AppointmentStatus? AppointmentStatus { get; set; }
    }
}
