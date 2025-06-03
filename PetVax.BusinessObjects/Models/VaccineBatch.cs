using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetVax.BusinessObjects.Models
{
    [Table("VaccineBatch", Schema = "dbo")]
    public class VaccineBatch
    {
        [Key]
        public int VaccineBatchId { get; set; } // Unique identifier for the vaccine batch
        public int VaccineId { get; set; } // Foreign key to Vaccine table
        public string BatchNumber { get; set; } // e.g., "BATCH12345"
        public DateTime ManufactureDate { get; set; } // Manufacture date of the vaccine batch in "yyyy-MM-dd" format
        public DateTime ExpiryDate { get; set; } // Expiry date of the vaccine batch in "yyyy-MM-dd" format
        public int Quantity { get; set; } // Quantity of vaccines in the batch
        public DateTime CreateAt { get; set; } // Date when the record was created
        public string CreatedBy { get; set; } // User who created the record
        public DateTime? ModifiedAt { get; set; } // Date when the record was last modified
        public string? ModifiedBy { get; set; } // User who last modified the record

        // Navigation properties
        public virtual Vaccine Vaccine { get; set; } // Navigation to Vaccine table
        public virtual ICollection<VaccineReceipt> VaccineReceipts { get; set; } // Collection of vaccine receipts associated with this batch
        public virtual ICollection<VaccineExport> VaccineExports { get; set; } // Collection of vaccine exports associated with this batch
    }
}
