using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetVax.BusinessObjects.Models
{
    [Table("VaccinationSchedule", Schema = "dbo")]
    public class VaccinationSchedule
    {
        [Key]
        public int VaccinationScheduleId { get; set; } // Unique identifier for the vaccination schedule

        public int DiseaseId { get; set; } // Foreign key to Disease table

        public string Species { get; set; } // e.g., "Dog", "Cat"
        public int DoseNumber { get; set; }
        public int AgeInterval { get; set; } // Age interval in months for the vaccination schedule
        public DateTime CreatedAt { get; set; } // Date when the record was created
        public string CreatedBy { get; set; } // User who created the record
        public DateTime? ModifiedAt { get; set; } // Date when the record was last modified
        public string? ModifiedBy { get; set; } // User who last modified the record

        // Navigation properties
        public virtual Disease Disease { get; set; } // Navigation to Disease table

        public virtual ICollection<VaccineProfile> VaccineProfiles { get; set; } // Navigation to VaccineProfile table
    }
}
