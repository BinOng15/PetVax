using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetVax.BusinessObjects.Models
{
    [Table("Payment", Schema = "dbo")]
    public class Payment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PaymentId { get; set; } // Unique identifier for the payment record
        public int AppointmentDetailId { get; set; } // Foreign key to AppointmentDetail table
        public int CustomerId { get; set; } // Foreign key to Customer table
        public int VaccineId { get; set; } // Foreign key to Vaccine table
        public string PaymentCode { get; set; } // e.g., "PAY123456", unique identifier for the payment
        public decimal Amount { get; set; } // Amount paid for the service
        public DateTime PaymentDate { get; set; } // Date of the payment in "yyyy-MM-dd" format
        public string PaymentMethod { get; set; } // e.g., "Credit Card", "Cash", "Online Transfer"
        public string PaymentStatus { get; set; } // e.g., "Pending", "Completed", "Failed"
        public DateTime CreatedAt { get; set; } // Date when the record was created
        public string CreatedBy { get; set; } // User who created the record
        public DateTime? ModifiedAt { get; set; } // Date when the record was last modified
        public string? ModifiedBy { get; set; } // User who last modified the record

        // Navigation properties
        public virtual AppointmentDetail AppointmentDetail { get; set; } // Navigation to AppointmentDetail table
        public virtual Customer Customer { get; set; } // Navigation to Customer table
        public virtual Vaccine Vaccine { get; set; } // Navigation to Vaccine table
    }
}
