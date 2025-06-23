using PetVax.BusinessObjects.DTO.AppointmentDTO;
using PetVax.BusinessObjects.DTO.DiseaseDTO;
using PetVax.BusinessObjects.DTO.MicrochipItemDTO;
using PetVax.BusinessObjects.DTO.VaccineBatchDTO;
using PetVax.BusinessObjects.DTO.VetDTO;
using PetVax.BusinessObjects.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetVax.BusinessObjects.DTO.AppointmentDetailDTO
{
    public class AppointmentDetailResponseDTO
    {
        public int AppointmentDetailId { get; set; }
        public int AppointmentId { get; set; }
        public int VetId { get; set; }
        public EnumList.ServiceType ServiceType { get; set; }
        public int? MicrochipItemId { get; set; }
        public int? PassportId { get; set; }
        public int? HealthConditionId { get; set; }
        public int? VaccineBatchId { get; set; }
        public int? DiseaseId { get; set; }
        public string AppointmentDetailCode { get; set; } // e.g., "AD123456", unique identifier for the appointment detail
        public string? Dose { get; set; } // e.g., "1st Dose", "2nd Dose"
        public string? Reaction { get; set; } // e.g., "None", "Mild", "Severe"
        public string? NextVaccinationInfo { get; set; } // e.g., "Next vaccination due on 2023-12-01"
        public DateTime AppointmentDate { get; set; } // Date of the appointment
        public EnumList.AppointmentStatus AppointmentStatus { get; set; }
        public string? Temperature { get; set; } // For vaccination
        public string? HeartRate { get; set; } // For vaccination
        public string? GeneralCondition { get; set; } // General condition of the pet during the appointment
        public string? Others { get; set; } // Any other notes or observations during the appointment
        public string? Notes { get; set; } // Additional notes for the appointment detail
        public DateTime CreatedAt { get; set; } // Date when the record was created
        public string CreatedBy { get; set; } // User who created the record
        public DateTime? ModifiedAt { get; set; } // Date when the record was last modified
        public string? ModifiedBy { get; set; } // User who last modified the record
        public bool? isDeleted { get; set; } = false; // Soft delete flag

        public VetResponseDTO Vet { get; set; } // Navigation to Vet table
        public MicrochipItemResponseDTO MicrochipItem { get; set; } // Navigation to MicrochipItem table
        public PetPassportResponseDTO PetPassport { get; set; } // Navigation to PetPassport table
        public HealthConditionResponseDTO HealthCondition { get; set; } // Navigation to HealthCondition table
        public VaccineBatchResponseDTO VaccineBatch { get; set; } // Navigation to VaccineBatch table
        public DiseaseResponseDTO Disease { get; set; } // Navigation to Disease table, if applicable (for vaccinations)
    }

    public class AppointmentVaccinationDetailResponseDTO
    {
        public int AppointmentDetailId { get; set; }
        public int AppointmentId { get; set; }
        public string AppointmentDetailCode { get; set; }
        public int VetId { get; set; }
        public EnumList.ServiceType ServiceType { get; set; }
        public int? VaccineBatchId { get; set; }
        public int? DiseaseId { get; set; }
        public string? Dose { get; set; } // e.g., "1st Dose", "2nd Dose"
        public string? Reaction { get; set; } // e.g., "None", "Mild", "Severe"
        public string? NextVaccinationInfo { get; set; } // e.g., "Next vaccination due on 2023-12-01"
        public DateTime AppointmentDate { get; set; } // Date of the appointment
        public EnumList.AppointmentStatus AppointmentStatus { get; set; }
        public string? Temperature { get; set; } // For vaccination
        public string? HeartRate { get; set; } // For vaccination
        public string? GeneralCondition { get; set; } // General condition of the pet during the appointment
        public string? Others { get; set; } // Any other notes or observations during the appointment
        public string? Notes { get; set; } // Additional notes for the appointment detail
        public DateTime CreatedAt { get; set; } // Date when the record was created
        public string CreatedBy { get; set; } // User who created the record
        public DateTime? ModifiedAt { get; set; } // Date when the record was last modified
        public string? ModifiedBy { get; set; } // User who last modified the record

        public VetResponseDTO Vet { get; set; } // Navigation to Vet table
        public VaccineBatchResponseDTO VaccineBatch { get; set; } // Navigation to VaccineBatch table
        public DiseaseResponseDTO Disease { get; set; } // Navigation to Disease table, if applicable (for vaccinations)
        public AppointmentResponseDTO Appointment { get; set; } // Navigation to Appointment table
    }

    public class AppointmentMicrochipResponseDTO
    {
        public int AppointmentDetailId { get; set; }
        public int AppointmentId { get; set; }
        public string AppointmentDetailCode { get; set; }
        public int VetId { get; set; }
        public EnumList.ServiceType ServiceType { get; set; }
        public int? MicrochipItemId { get; set; }
        public DateTime AppointmentDate { get; set; } // Date of the appointment
        public EnumList.AppointmentStatus AppointmentStatus { get; set; }
        public DateTime CreatedAt { get; set; } // Date when the record was created
        public string CreatedBy { get; set; } // User who created the record
        public DateTime? ModifiedAt { get; set; } // Date when the record was last modified
        public string? ModifiedBy { get; set; }
        public VetResponseDTO Vet { get; set; }// User who last modified the record
        public BaseMicrochipItemResponse MicrochipItem { get; set; } // Navigation to MicrochipItem table
    }

    public class AppointmentHealthCertificateResponseDTO
    {
        public int AppointmentDetailId { get; set; }
        public int AppointmentId { get; set; }
        public string AppointmentDetailCode { get; set; }
        public int VetId { get; set; }
        public EnumList.ServiceType ServiceType { get; set; }
        public int? HealthConditionId { get; set; }
        public DateTime AppointmentDate { get; set; } // Date of the appointment
        public EnumList.AppointmentStatus AppointmentStatus { get; set; }
        public DateTime CreatedAt { get; set; } // Date when the record was created
        public string CreatedBy { get; set; } // User who created the record
        public DateTime? ModifiedAt { get; set; } // Date when the record was last modified
        public string? ModifiedBy { get; set; } // User who last modified the record
        public VetResponseDTO Vet { get; set; } // Navigation to Vet table
        public HealthConditionResponseDTO HealthCondition { get; set; } // Navigation to HealthCondition table
    }
}
