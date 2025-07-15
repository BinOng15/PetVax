using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetVax.BusinessObjects.Models
{
    [Table("VaccineReceiptDetail")]
    public class VaccineReceiptDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int VaccineReceiptDetailId { get; set; } // Unique identifier for the vaccine receipt detail
        [ForeignKey(nameof(VaccineReceipt))]
        public int VaccineReceiptId { get; set; } // Foreign key to VaccineReceipt table
        [ForeignKey(nameof(VaccineBatch))]
        public int VaccineBatchId { get; set; } // Foreign key to VaccineBatch table
        public string Suppiler { get; set; }
        public int Quantity { get; set; } // Quantity of vaccines in the receipt detail
        public string VaccineStatus { get; set; } // Status of the vaccine, e.g., "damage", "packaging error", "storage temperature"
        public string Notes { get; set; } // Additional notes for the vaccine receipt detail
        public DateTime CreatedAt { get; set; } // Date when the record was created
        public string CreatedBy { get; set; } // User who created the record
        public DateTime? ModifiedAt { get; set; } // Date when the record was last modified
        public string? ModifiedBy { get; set; } // User who last modified the record
        public bool? isDeleted { get; set; } = false;

        // Navigation properties
        public virtual VaccineReceipt VaccineReceipt { get; set; } = null!; // Navigation to VaccineReceipt table
        public virtual VaccineBatch VaccineBatch { get; set; } = null!; // Navigation to VaccineBatch table
    }
}
