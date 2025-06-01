using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetVax.BusinessObjects.Models
{
    [Table("VaccineReceiptDetail", Schema = "dbo")]
    public class VaccineReceiptDetail
    {
        [Key]
        public int VaccineReceiptDetailId { get; set; } // Unique identifier for the vaccine receipt detail
        public int VaccineReceiptId { get; set; } // Foreign key to VaccineReceipt table
        public int VaccineBatchId { get; set; } // Foreign key to VaccineBatch table
        public int Quantity { get; set; } // Quantity of vaccines in the receipt detail
        public string Notes { get; set; } // Additional notes for the vaccine receipt detail
        public DateTime CreatedAt { get; set; } // Date when the record was created
        public string CreatedBy { get; set; } // User who created the record
        public DateTime? ModifiedAt { get; set; } // Date when the record was last modified
        public string? ModifiedBy { get; set; } // User who last modified the record

        // Navigation properties
        [ForeignKey("VaccineReceiptId")]
        public virtual VaccineReceipt VaccineReceipt { get; set; } // Navigation to VaccineReceipt table
        [ForeignKey("VaccineBatchId")]
        public virtual VaccineBatch VaccineBatch { get; set; } // Navigation to VaccineBatch table
    }
}
