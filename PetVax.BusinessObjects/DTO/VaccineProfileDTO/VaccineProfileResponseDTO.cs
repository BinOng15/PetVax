using PetVax.BusinessObjects.DTO.AppointmentDetailDTO;
using PetVax.BusinessObjects.DTO.DiseaseDTO;
using PetVax.BusinessObjects.DTO.VaccineBatchDTO;
using PetVax.BusinessObjects.DTO.VaccineDTO;
using PetVax.BusinessObjects.DTO.VetDTO;
using PetVax.BusinessObjects.Enum;
using PetVax.BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetVax.BusinessObjects.DTO.VaccineProfileDTO
{
    public class VaccineProfileResponseDTO
    {
        public int VaccineProfileId { get; set; }
        public int PetId { get; set; }
        public int? AppointmentDetailId { get; set; }
        public int? VaccinationScheduleId { get; set; }
        public int? DiseaseId { get; set; }
        public DateTime? PreferedDate { get; set; }
        public DateTime? VaccinationDate { get; set; }
        public int? Dose { get; set; }
        public string? Reaction { get; set; }
        public string? NextVaccinationInfo { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsCompleted { get; set; }
        public DateTime CreatedAt { get; set; }

        public virtual AppointmentVaccinationForProfileResponseDTO? AppointmentDetail { get; set; }
        public virtual DiseaseVaccineProfileResponseDTO? Disease { get; set; }
    }

    public class DiseaseVaccineProfileResponseDTO
    {
        public string DiseaseName { get; set; }
    }

    public class AppointmentVaccinationForProfileResponseDTO
    {
        public string AppointmentDetailCode { get; set; }
        public int VetId { get; set; }
        public EnumList.ServiceType ServiceType { get; set; }
        public int? VaccineBatchId { get; set; }
        public DateTime AppointmentDate { get; set; } // Date of the appointment
        public EnumList.AppointmentStatus AppointmentStatus { get; set; }
        public string? Temperature { get; set; } // For vaccination
        public string? HeartRate { get; set; } // For vaccination
        public string? GeneralCondition { get; set; } // General condition of the pet during the appointment
        public string? Others { get; set; } // Any other notes or observations during the appointment
        public string? Notes { get; set; } // Additional notes for the appointment detail
        public VetVaccineProfileResponseDTO Vet { get; set; } // Navigation to Vet table
        public VaccineBatchVaccineProfileResponseDTO VaccineBatch { get; set; } // Navigation to VaccineBatch table
    }

    public class VetVaccineProfileResponseDTO
    {
        public string VetCode { get; set; }
        public string? Name { get; set; }
        public string? Specialization { get; set; }
    }
    public class VaccineBatchVaccineProfileResponseDTO
    {
        public string BatchNumber { get; set; }
        public DateTime ManufactureDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public int Quantity { get; set; }
        public int VaccineId { get; set; }
        public VaccineForBatchVaccineProfileResponseDTO Vaccine { get; set; }
    }

    public class VaccineForBatchVaccineProfileResponseDTO
    {
        public string VaccineCode { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Image { get; set; }
        public string Notes { get; set; }
    }
}
