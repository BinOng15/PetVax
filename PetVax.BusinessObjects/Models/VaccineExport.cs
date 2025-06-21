using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetVax.BusinessObjects.Models
{
    [Table("VaccineExport")]
    public class VaccineExport
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int VaccineExportId { get; set; } // Unique identifier for the vaccine export record
        public string ExportCode { get; set; }
        public DateTime ExportDate { get; set; } // Date when the vaccine was exported
        public string Reason { get; set; }
        public string Notes { get; set; } // Additional notes about the export
        public DateTime CreatedAt { get; set; } // Date when the record was created
        public string CreatedBy { get; set; } // User who created the record
        public DateTime? ModifiedAt { get; set; } // Date when the record was last modified
        public string? ModifiedBy { get; set; } // User who last modified the record
        public bool? isDeleted { get; set; } = false;

        // Navigation properties
        public virtual ICollection<VaccineExportDetail> VaccineExportDetails { get; set; } // Collection of vaccine export details associated with this export
    }
}
