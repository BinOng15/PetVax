using PetVax.BusinessObjects.DTO.CustomerDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetVax.BusinessObjects.DTO.AppointmentDTO
{
    public class AppointmentResponseDTO
    {
        public int AppointmentId { get; set; } // Unique identifier for the appointment
        public int CustomerId { get; set; } // Foreign key to Customer table
        public int PetId { get; set; } // Foreign key to Pet table
        public string AppointmentCode { get; set; } // e.g., "APPT123456", unique identifier for the appointment
        public DateTime AppointmentDate { get; set; } // Date of the appointment in "yyyy-MM-dd" format
        public string ServiceType { get; set; } // e.g., "Vaccination", "Microchip", "Passport"
        public string Location { get; set; } // e.g., "Clinic", "Home Visit"
        public string Address { get; set; } // Address for the appointment
        public Enum.EnumList.AppointmentStatus AppointmentStatus { get; set; }
        public DateTime CreatedAt { get; set; } // Date when the record was created
        public string CreatedBy { get; set; } // User who created the record
        public DateTime? ModifiedAt { get; set; } // Date when the record was last modified
        public string? ModifiedBy { get; set; } // User who last modified the record

        public CustomerResponseDTO Customer { get; set; }
    }
}
