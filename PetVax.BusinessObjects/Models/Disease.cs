using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetVax.BusinessObjects.Models
{
    [Table("Disease")]
    public class Disease
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DiseaseId { get; set; } // Unique identifier for the disease
        public string Name { get; set; } // Name of the disease, e.g., "Canine Parvovirus", "Feline Leukemia"
        public string Description { get; set; } // Description of the disease, e.g., "Highly contagious viral disease in dogs"
        public string Species { get; set; } // Species affected by the disease, e.g., "Dog", "Cat"
        public string Symptoms { get; set; } // Common symptoms of the disease, e.g., "Vomiting, Diarrhea"
        public string Treatment { get; set; } // Treatment options for the disease, e.g., "Supportive care, Vaccination"
        public string Status { get; set; } // Status of the disease, e.g., "Active", "Inactive"
        public DateTime CreatedAt { get; set; } // Date when the record was created
        public string CreatedBy { get; set; } // User who created the record
        public DateTime? ModifiedAt { get; set; } // Date when the record was last modified
        public string? ModifiedBy { get; set; } // User who last modified the record
        bool? isDeleted { get; set; } = false;

        // Navigation properties
        public virtual ICollection<VaccineProfileDisease> VaccineProfileDiseases { get; set; }
        public virtual ICollection<VaccineDisease> VaccineDiseases { get; set; }
        public virtual ICollection<VaccinationSchedule> VaccinationSchedules { get; set; } // Navigation to VaccinationSchedule table

    }
}
