using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetVax.BusinessObjects.Models
{
    [Table("VetSchedule", Schema = "dbo")]
    public class VetSchedule
    {
        [Key]
        public int VetScheduleId { get; set; } // Unique identifier for the vet schedule
        public int VetId { get; set; } // Foreign key to Vet table
        public DateTime ScheduleDate { get; set; } // Date of the schedule
        public TimeSpan StartTime { get; set; } // Start time of the schedule
        public TimeSpan EndTime { get; set; } // End time of the schedule
        public string Status { get; set; } // e.g., "Active", "Cancelled", "Completed"
        public DateTime CreatedAt { get; set; } // Date when the record was created
        public string CreatedBy { get; set; } // User who created the record
        public DateTime? ModifiedAt { get; set; } // Date when the record was last modified
        public string? ModifiedBy { get; set; } // User who last modified the record

        // Navigation properties
        [ForeignKey("VetId")]
        public virtual Vet Vet { get; set; } // Navigation to Vet table
    }
}
