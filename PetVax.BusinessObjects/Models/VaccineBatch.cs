using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetVax.BusinessObjects.Models
{
    [Table("VaccineBatch")]
    public class VaccineBatch
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int VaccineBatchId { get; set; } // Unique identifier for the vaccine batch
        public int VaccineId { get; set; } // Foreign key to Vaccine table
        public string BatchNumber { get; set; } // e.g., "BATCH12345"
        public DateTime ManufactureDate { get; set; } // Manufacture date of the vaccine batch in "yyyy-MM-dd" format
        public DateTime ExpiryDate { get; set; } // Expiry date of the vaccine batch in "yyyy-MM-dd" format
        public string Manufacturer { get; set; } // Manufacturer of the vaccine batch, e.g., "ABC Pharmaceuticals"
        public string Source { get; set; } // Source of the vaccine batch, e.g., "Local Supplier", "International Supplier"
        public string StorageCondition { get; set; } // Storage conditions for the vaccine batch, e.g., "2-8°C", "Room Temperature"
        public int Quantity { get; set; } // Quantity of vaccines in the batch
        public DateTime CreateAt { get; set; } // Date when the record was created
        public string CreatedBy { get; set; } // User who created the record
        public DateTime? ModifiedAt { get; set; } // Date when the record was last modified
        public string? ModifiedBy { get; set; } // User who last modified the record
        public bool? isDeleted { get; set; } = false;

        // Navigation propertiess
        public virtual Vaccine Vaccine { get; set; } // Navigation to Vaccine table
        public virtual ICollection<VaccineReceipt> VaccineReceipts { get; set; } // Collection of vaccine receipts associated with this batch
        public virtual ICollection<VaccineExport> VaccineExports { get; set; } // Collection of vaccine exports associated with this batch
        public virtual ICollection<AppointmentDetail> AppointmentDetails { get; set; }
        public virtual ICollection<ColdChainLog> ColdChainLogs { get; set; } // Collection of cold chain logs associated with this batch
    }
}
