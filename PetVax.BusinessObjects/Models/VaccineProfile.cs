using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetVax.BusinessObjects.Models
{
    [Table("VaccineProfile", Schema = "dbo")]
    public class VaccineProfile
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int VaccineProfileId { get; set; } // Unique identifier for the vaccine profile
        public int PetId { get; set; } // Foreign key to Pet table
        public int? DiseaseId { get; set; }
        public int? AppointmentDetailId { get; set; }
        public int? VaccinationScheduleId { get; set; } // Foreign key to VaccineProfile table, if applicable (for follow-up vaccinations)
        public DateTime? PreferedDate { get; set; } // Preferred date for the vaccination in "yyyy-MM-dd" format
        public DateTime? VaccinationDate { get; set; } // Actual date of vaccination in "yyyy-MM-dd" format
        public string? Dose { get; set; } // e.g., "1st Dose", "2nd Dose"
        public string? Reaction { get; set; } // e.g., "None", "Mild", "Severe"
        public string? NextVaccinationInfo { get; set; } // e.g., "Next vaccination due on 2023-12-01"
        public bool? IsActive { get; set; } // Indicates if the vaccine profile is active
        public bool? IsCompleted { get; set; } // Indicates if the vaccination process is completed
        public DateTime CreatedAt { get; set; } // Date when the record was created
        public string? CreatedBy { get; set; } // User who created the record
        public DateTime? ModifiedAt { get; set; } // Date when the record was last modified
        public string? ModifiedBy { get; set; } // User who last modified the record

        // Navigation properties
        public virtual Pet Pet { get; set; } // Navigation to Pet table
        public virtual Disease Disease { get; set; } // Navigation to Disease table
        public virtual AppointmentDetail AppointmentDetail { get; set; } // Navigation to AppointmentDetail table
    }
}
