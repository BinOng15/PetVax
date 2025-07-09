using PetVax.BusinessObjects.DTO.HealthConditionDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetVax.BusinessObjects.DTO.CertificateForPet
{
    public class PetVaccinationRecordDTO
    {
        public int PetId { get; set; }
        public string PetName { get; set; }
        public string Species { get; set; }
        public string Breed { get; set; }
        public string Gender { get; set; }
        public string DateOfBirth { get; set; }

        public List<HealthConditionResponse> HealthConditions { get; set; } = new();
        public List<VaccinationCertificateWithHealthResponseDTO> Certificates { get; set; } = new();
    }


    public class VaccinationCertificateWithHealthResponseDTO
    {
        public int? DiseaseId { get; set; }
        public string DiseaseName { get; set; }
        public int? Dose { get; set; }


        public int? VaccineId { get; set; }
        public string VaccineName { get; set; }
        public string VaccineCode { get; set; }
        public string VaccineImage { get; set; }
        public string VaccineDescription { get; set; }


        public int? BatchId { get; set; }
        public string BatchNumber { get; set; }
        public DateTime ManufactureDate { get; set; }
        public DateTime ExpiryDate { get; set; }

        public int DoseNumber { get; set; }
        public DateTime VaccinationDate { get; set; }
        public DateTime? ExpirationDate { get; set; }

        public string VetName { get; set; }
        public string ClinicName { get; set; }
        public string ClinicAddress { get; set; }

        public string Purpose { get; set; }
        public decimal Price { get; set; }
        public string Status { get; set; }

        public DateTime IssueDate { get; set; }
        public DateTime? ValidUntil { get; set; }

    }
}
