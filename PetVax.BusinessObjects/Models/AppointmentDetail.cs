using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PetVax.BusinessObjects.Enum;

namespace PetVax.BusinessObjects.Models
{
    [Table("AppointmentDetail")]
    public class AppointmentDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AppointmentDetailId { get; set; } // Unique identifier for the appointment detail
        public int AppointmentId { get; set; } // Foreign key to Appointment table
        public int? VetId { get; set; } // Foreign key to Vet table
        public EnumList.ServiceType ServiceType { get; set; } // e.g., "Vaccination", "Microchip", "Passport", "Health Check"
        public int? MicrochipItemId { get; set; } // Foreign key to MicrochipItem table
        public int? PassportId { get; set; } // Foreign key to PetPassport table
        public int? HealthConditionId { get; set; } // Foreign key to HealthCondition table
        public int? VaccineBatchId { get; set; } // Foreign key to VaccineBatch table
        public int? DiseaseId { get; set; } // Foreign key to Disease table, if applicable (for vaccinations)
        public string AppointmentDetailCode { get; set; } // e.g., "AD123456", unique identifier for the appointment detail
        public string? Dose { get; set; } // e.g., "1st Dose", "2nd Dose"
        public string? Reaction { get; set; } // e.g., "None", "Mild", "Severe"
        public string? NextVaccinationInfo { get; set; } // e.g., "Next vaccination due on 2023-12-01"
        public DateTime AppointmentDate { get; set; } // Date of the appointment
        public Enum.EnumList.AppointmentStatus AppointmentStatus { get; set; }
        public DateTime CreatedAt { get; set; } // Date when the record was created
        public string CreatedBy { get; set; } // User who created the record
        public DateTime? ModifiedAt { get; set; } // Date when the record was last modified
        public string? ModifiedBy { get; set; } // User who last modified the record
        public bool? isDeleted { get; set; } = false;

        // Navigation properties
        public virtual Appointment Appointment { get; set; } // Navigation to Appointment table
        public virtual Vet Vet { get; set; } // Navigation to Vet table
        public virtual MicrochipItem MicrochipItem { get; set; } // Navigation to MicrochipItem table
        public virtual PetPassport PetPassport { get; set; } // Navigation to PetPassport table
        public virtual HealthCondition HealthCondition { get; set; } // Navigation to HealthCondition table
        public virtual VaccineBatch VaccineBatch { get; set; } // Navigation to VaccineBatch table
        public virtual Disease Disease { get; set; } // Navigation to Disease table, if applicable (for vaccinations)
        public virtual ICollection<VaccineProfile> VaccineProfiles { get; set; } // Collection of vaccine profiles associated with this appointment detail
    }
}
