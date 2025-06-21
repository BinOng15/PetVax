using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetVax.BusinessObjects.Models
{
    [Table("VaccineDisease")]
    public class VaccineDisease
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int VaccineDiseaseId { get; set; } // Unique identifier for the vaccine disease record
        public int VaccineId { get; set; } // Foreign key to Vaccine table
        public int DiseaseId { get; set; } // Foreign key to Disease table
        public DateTime CreatedAt { get; set; } // Date when the record was created
        public string CreatedBy { get; set; } // User who created the record
        public DateTime? ModifiedAt { get; set; } // Date when the record was last modified
        public string? ModifiedBy { get; set; } // User who last modified the record
        bool? isDeleted { get; set; } = false;

        // Navigation properties
        public virtual Vaccine Vaccine { get; set; } // Navigation to Vaccine table
        public virtual Disease Disease { get; set; } // Navigation to Disease table
    }
}
