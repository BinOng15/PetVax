using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetVax.BusinessObjects.Models
{
    [Table("AppointmentDetail", Schema = "dbo")]
    public class AppointmentDetail
    {
        [Key]
        public int AppointmentDetailId { get; set; } // Unique identifier for the appointment detail
        public int AppointmentId { get; set; } // Foreign key to Appointment table
        public int VetId { get; set; } // Foreign key to Vet table
        public int VaccineProfileId { get; set; } // Foreign key to VaccineProfile table
        public string ServiceType { get; set; } // e.g., "Vaccination", "Microchip", "Passport", "Health Check"
        public int MicrochipItemId { get; set; } // Foreign key to MicrochipItem table
        public int PassportId { get; set; } // Foreign key to PetPassport table
        public int HealthConditionId { get; set; } // Foreign key to HealthCondition table
        public int VaccineBatchId { get; set; } // Foreign key to VaccineBatch table
        public string AppointmentDetailCode { get; set; } // e.g., "AD123456", unique identifier for the appointment detail
        public string Dose { get; set; } // e.g., "1st Dose", "2nd Dose"
        public string Reaction { get; set; } // e.g., "None", "Mild", "Severe"
        public string NextVaccinationInfo { get; set; } // e.g., "Next vaccination due on 2023-12-01"
        public DateTime AppointmentDate { get; set; } // Date of the appointment
        public Enum.EnumList.AppointmentStatus AppointmentStatus { get; set; }
        public DateTime CreatedAt { get; set; } // Date when the record was created
        public string CreatedBy { get; set; } // User who created the record
        public DateTime? ModifiedAt { get; set; } // Date when the record was last modified
        public string? ModifiedBy { get; set; } // User who last modified the record

        // Navigation properties
        public virtual Appointment Appointment { get; set; } // Navigation to Appointment table
        public virtual Vet Vet { get; set; } // Navigation to Vet table
        public virtual MicrochipItem MicrochipItem { get; set; } // Navigation to MicrochipItem table
        public virtual PetPassport PetPassport { get; set; } // Navigation to PetPassport table
        public virtual HealthCondition HealthCondition { get; set; } // Navigation to HealthCondition table
        public virtual VaccineBatch VaccineBatch { get; set; } // Navigation to VaccineBatch table
        public virtual VaccineProfile VaccineProfile { get; set; } // Navigation to VaccineProfile table
    }
}
