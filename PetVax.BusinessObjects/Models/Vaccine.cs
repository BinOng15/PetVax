using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetVax.BusinessObjects.Models
{
    [Table("Vaccine")]
    public class Vaccine
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int VaccineId { get; set; } // Unique identifier for the vaccine
        public string VaccineCode { get; set; } // e.g., "VAC12345", unique identifier for the vaccine
        public string Name { get; set; } // e.g., "Rabies Vaccine", "Distemper Vaccine"
        public string Description { get; set; } // e.g., "A vaccine to prevent rabies in pets"
        public decimal Price { get; set; } // Price of the vaccine
        public string Status { get; set; } // e.g., "Active", "Inactive", "Discontinued"
        public string Image { get; set; } // URL or path to the vaccine image
        public string Notes { get; set; } // Additional notes about the vaccine
        public DateTime CreatedAt { get; set; } // Date when the record was created
        public string CreatedBy { get; set; } // User who created the record
        public DateTime? ModifiedAt { get; set; } // Date when the record was last modified
        public string? ModifiedBy { get; set; } // User who last modified the record
        bool? isDeleted { get; set; } = false;

        // Navigation properties
        public virtual ICollection<VaccineBatch> VaccineBatches { get; set; } // Collection of vaccine batches associated with this vaccine
        public virtual ICollection<VaccineDisease> VaccineDiseases { get; set; } // Collection of diseases associated with this vaccine
        public virtual ICollection<Payment> Payments { get; set; } // Collection of payments associated with this vaccine
    }
}
