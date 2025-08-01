﻿using PetVax.BusinessObjects.DTO.AppointmentDetailDTO;
using PetVax.BusinessObjects.DTO.CustomerDTO;
using PetVax.BusinessObjects.DTO.DiseaseDTO;
using PetVax.BusinessObjects.DTO.MicrochipItemDTO;
using PetVax.BusinessObjects.DTO.PetDTO;
using PetVax.BusinessObjects.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public EnumList.ServiceType ServiceType { get; set; } // e.g., "Vaccination", "Microchip", "Passport"
        public EnumList.Location Location { get; set; } // e.g., "Clinic", "Home Visit"
        public string Address { get; set; } // Address for the appointment
        public EnumList.AppointmentStatus AppointmentStatus { get; set; }
        public DateTime CreatedAt { get; set; } // Date when the record was created
        public string CreatedBy { get; set; } // User who created the record
        public DateTime? ModifiedAt { get; set; } // Date when the record was last modified
        public string? ModifiedBy { get; set; } // User who last modified the record
        public bool? isDeleted { get; set; } = false; // Soft delete flag

        public CustomerResponseDTO CustomerResponseDTO { get; set; }
        public PetResponseDTOs PetResponseDTO { get; set; }

    }
    public class AppointmentWithDetailResponseDTO
    {
        public AppointmentResponseDTO Appointment { get; set; }
        public AppointmentDetailResponseDTO AppointmentDetail { get; set; }
    }

    public class AppointmentWithVaccinationResponseDTO
    {
        public AppointmentResponseDTO Appointment { get; set; }
        public AppointmentVaccinationDetailResponseDTO Vaccinations { get; set; }
    }

    public class AppointmentForVaccinationResponseDTO
    {
        public AppointmentResponseDTO Appointment { get; set; }
        public AppointmentHasDiseaseResponseDTO AppointmentHasDiseaseResponseDTO { get; set; }
    }
    public class AppointmentHasDiseaseResponseDTO
    {
        public int DiseaseId { get; set; }
        public int VetId { get; set; }
        public DiseaseResponseDTO Disease { get; set; }
    }

    public class AppointmentWithMicorchipResponseDTO
    {
        public AppointmentResponseDTO Appointment { get; set; }
        public AppointmentMicrochipResponseDTO Microchip { get; set; }
    }

    public class AppointmenWithHealthConditionResponseDTO
    {
        public AppointmentResponseDTO Appointment { get; set; }
        public AppointmentHealthConditionResponseDTO HealthCondition { get; set; }
    }

    public class AppointmenDetialMicorchipResponseDTO
    {
        public AppointmentMicrochipResponseDTO Microchip { get; set; }
    }

    public class AppointmentDetailHealthConditionResponseDTO
    {
        public AppointmentHealthConditionResponseDTO HealthConditionDTO { get; set; }
    }

    public class AppointmentWithVaccinationCertificateResponseDTO
    {
        public AppointmentResponseDTO Appointment { get; set; }
        public AppointmentVaccinationCertificateResponseDTO VaccinationCertificate { get; set; }
    }

}
