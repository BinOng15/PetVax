using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PetVax.BusinessObjects.Enum.EnumList;

namespace PetVax.BusinessObjects.Models
{
    [Table("ServiceHistory")]
    public class ServiceHistory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ServiceHistoryId { get; set; } // Unique identifier for the service history record

        public int CustomerId { get; set; } // Foreign key to Customer table
        public ServiceType ServiceType { get; set; } // Type of service provided, e.g., "Vaccination", "Microchip", "Health Check"
        public DateTime ServiceDate { get; set; } // Date when the service was provided
        public string Status { get; set; } // Status of the service, e.g., "Completed", "Pending", "Cancelled"
        public DateTime CreatedAt { get; set; } // Date when the record was created
        public string CreatedBy { get; set; } // User who created the record
        public DateTime? ModifiedAt { get; set; } // Date when the record was last modified
        public string? ModifiedBy { get; set; } // User who last modified the record
        public bool? isDeleted { get; set; } = false;

        // Navigation properties
        public virtual ICollection<AppointmentDetail> AppointmentDetails { get; set; } // Navigation to AppointmentDetail table
        public virtual Customer Customer { get; set; } // Navigation to Customer table

    }
}
