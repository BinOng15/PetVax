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
    [Table("Payment")]
    public class Payment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PaymentId { get; set; } // Unique identifier for the payment record
        public int AppointmentDetailId { get; set; } // Foreign key to AppointmentDetail table
        public int CustomerId { get; set; } // Foreign key to Customer table
        public int? VaccineBatchId { get; set; } 
        public int? MicrochipId { get; set; } // Foreign key to Microchip table, if applicable
        public int? VaccinationCertificateId { get; set; } // Foreign key to VaccinationCertificate table, if applicable
        public int? HealthConditionId { get; set; } // Foreign key to HealthCondition table, if applicable
        public string PaymentCode { get; set; } // e.g., "PAY123456", unique identifier for the payment
        public decimal Amount { get; set; } // Amount paid for the service
        public DateTime PaymentDate { get; set; } // Date of the payment in "yyyy-MM-dd" format
        public string CheckoutUrl { get; set; }
        public string QRCode { get; set; } // QR code for payment, if applicable
        public string PaymentMethod { get; set; } // e.g., "Credit Card", "Cash", "Online Transfer"
        public EnumList.PaymentStatus PaymentStatus { get; set; } // e.g., "Pending", "Completed", "Failed"
        public DateTime CreatedAt { get; set; } // Date when the record was created
        public string CreatedBy { get; set; } // User who created the record
        public DateTime? ModifiedAt { get; set; } // Date when the record was last modified
        public string? ModifiedBy { get; set; } // User who last modified the record
        public bool? isDeleted { get; set; } = false;

        // Navigation properties
        public virtual AppointmentDetail AppointmentDetail { get; set; } // Navigation to AppointmentDetail table
        public virtual Customer Customer { get; set; } // Navigation to Customer table
        public virtual VaccineBatch VaccineBatch { get; set; } 
        public virtual Microchip Microchip { get; set; } // Navigation to Microchip table, if applicable
        public virtual VaccinationCertificate VaccinationCertificate { get; set; } // Navigation to VaccinationCertificate table, if applicable
        public virtual HealthCondition HealthCondition { get; set; } // Navigation to HealthCondition table, if applicable
    }
}
