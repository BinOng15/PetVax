using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetVax.BusinessObjects.Models
{
    [Table("VaccineReceipt")]
    public class VaccineReceipt
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int VaccineReceiptId { get; set; } // Unique identifier for the vaccine receipt
        public string ReceiptCode { get; set; } // e.g., "REC123456", unique identifier for the receipt
        public DateTime ReceiptDate { get; set; } // Date of the receipt in "yyyy-MM-dd" format
        public string Suppiler { get; set; }
        public string Notes { get; set; } // Additional notes or comments regarding the vaccine receipt
        public DateTime CreatedAt { get; set; } // Date when the record was created
        public string CreatedBy { get; set; } // User who created the record
        public DateTime? ModifiedAt { get; set; } // Date when the record was last modified
        public string? ModifiedBy { get; set; } // User who last modified the record
        public bool? isDeleted { get; set; } = false;

        //Navigation properties
        public virtual ICollection<VaccineReceiptDetail> VaccineReceiptDetails { get; set; }
    }
}
