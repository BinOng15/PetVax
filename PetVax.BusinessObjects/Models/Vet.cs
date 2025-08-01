﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetVax.BusinessObjects.Models
{
    [Table("Vet")]
    public class Vet
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int VetId { get; set; } // Unique identifier for the vet
        public int AccountId { get; set; } // Foreign key to Account table
        public string VetCode { get; set; } // e.g., "VET12345", unique identifier for the vet
        public string? Name { get; set; } // e.g., "Dr. John Doe"
        public string? image { get; set; } // URL or path to the vet's image
        public string? Specialization { get; set; } // e.g., "Veterinary Surgeon", "Pet Nutritionist"
        public DateTime? DateOfBirth { get; set; } // Date of birth of the vet
        public string? PhoneNumber { get; set; } 
        public DateTime CreateAt { get; set; } // Date when the record was created
        public string CreatedBy { get; set; } // User who created the record
        public DateTime? ModifiedAt { get; set; } // Date when the record was last modified
        public string? ModifiedBy { get; set; } // User who last modified the record
        public bool? isDeleted { get; set; } = false;

        // Navigation properties
        public virtual Account Account { get; set; } // Navigation to Account table
        public virtual ICollection<VetSchedule> VetSchedules { get; set; } // Navigation to VetSchedule table
        public virtual ICollection<AppointmentDetail> AppointmentDetails { get; set; } // Navigation to AppointmentDetail table
        public virtual ICollection<VaccinationCertificate> VaccinationCertificates { get; set; } // Navigation to VaccinationCertificate table
        public virtual ICollection<HealthCondition> HealthConditions { get; set; } // Navigation to HealthCondition table
    }
}
