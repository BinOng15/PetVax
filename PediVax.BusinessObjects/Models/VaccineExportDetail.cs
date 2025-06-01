using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetVax.BusinessObjects.Models
{
    [Table("VaccineExportDetail", Schema = "dbo")]
    public class VaccineExportDetail
    {
        [Key]
        public int VaccineExportDetailId { get; set; } // Unique identifier for the vaccine export detail
        public int VaccineBatchId { get; set; } // Foreign key to VaccineBatch table
        public int VaccineExportId { get; set; } // Foreign key to VaccineExport table
        public int AppointmentDetailId { get; set; } // Foreign key to AppointmentDetail table
        public int Quantity { get; set; } // Quantity of vaccines exported
        public string Notes { get; set; } // Additional notes regarding the export
        public DateTime CreatedAt { get; set; } // Date when the record was created
        public string CreatedBy { get; set; } // User who created the record
        public DateTime? ModifiedAt { get; set; } // Date when the record was last modified
        public string? ModifiedBy { get; set; } // User who last modified the record

        // Navigation properties
        [ForeignKey("VaccineBatchId")]
        public virtual VaccineBatch VaccineBatch { get; set; } // Navigation to VaccineBatch table
        [ForeignKey("VaccineExportId")]
        public virtual VaccineExport VaccineExport { get; set; } // Navigation to VaccineExport table
        [ForeignKey("AppointmentDetailId")]
        public virtual AppointmentDetail AppointmentDetail { get; set; } // Navigation to AppointmentDetail table
    }
}
