using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PetVax.BusinessObjects.Enum;

namespace PetVax.BusinessObjects.Models
{
    [Table("Appointment", Schema = "dbo")]
    public class Appointment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AppointmentId { get; set; } // Unique identifier for the appointment
        public int CustomerId { get; set; } // Foreign key to Customer table
        public int PetId { get; set; } // Foreign key to Pet table
        public string AppointmentCode { get; set; } // e.g., "APPT123456", unique identifier for the appointment
        public DateTime AppointmentDate { get; set; } // Date of the appointment in "yyyy-MM-dd" format
        public EnumList.ServiceType ServiceType { get; set; }
        public EnumList.Location Location { get; set; }
        public string? Address { get; set; } // Address for the appointment
        public Enum.EnumList.AppointmentStatus AppointmentStatus { get; set; }
        public DateTime CreatedAt { get; set; } // Date when the record was created
        public string CreatedBy { get; set; } // User who created the record
        public DateTime? ModifiedAt { get; set; } // Date when the record was last modified
        public string? ModifiedBy { get; set; } // User who last modified the record

        // Navigation properties
        public virtual Customer Customer { get; set; } // Navigation to Customer table
        public virtual Pet Pet { get; set; } // Navigation to Pet table

    }
}
