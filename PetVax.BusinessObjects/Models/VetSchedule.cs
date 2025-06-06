﻿using System;
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
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int VetScheduleId { get; set; } // Unique identifier for the vet schedule
        public int VetId { get; set; } // Foreign key to Vet table
        public DateTime ScheduleDate { get; set; } // Date of the schedule
        public int SlotNumber { get; set; } // e.g., 1 for first slot, 2 for second slot, etc.
        public string Status { get; set; } // e.g., "Active", "Cancelled", "Completed"
        public DateTime CreatedAt { get; set; } // Date when the record was created
        public string CreatedBy { get; set; } // User who created the record
        public DateTime? ModifiedAt { get; set; } // Date when the record was last modified
        public string? ModifiedBy { get; set; } // User who last modified the record

        // Navigation properties
        public virtual Vet Vet { get; set; } // Navigation to Vet table
    }
}
