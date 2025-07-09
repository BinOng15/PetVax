using PetVax.BusinessObjects.DTO.DiseaseDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetVax.BusinessObjects.DTO.VaccinationSchedule
{
    public class VaccinationScheduleResponseDTO
    {
        public int VaccinationScheduleId { get; set; }
        public int DiseaseId { get; set; }
        public string Species { get; set; } // e.g., Dog, Cat
        public int DoseNumber { get; set; } // e.g., 1st Dose, 2nd Dose
        public int AgeInterval { get; set; } // e.g., 6 months, 1 year
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string? ModifiedBy { get; set; }

        public virtual DiseaseForVaccinationScheduleResponseDTO Disease { get; set; }
    }

    public class VaccinationScheduleByDiseaseResponseDTO
    {
        public int DiseaseId { get; set; }
        public string DiseaseName { get; set; }
        public List<VaccinationScheduleResponseDTO> Schedules { get; set; }
    }

    public class DiseaseForVaccinationScheduleResponseDTO
    {
        public int DiseaseId { get; set; }
        public string Name { get; set; }
        public string Species { get; set; } // e.g., Dog, Cat
        public string Description { get; set; }
    }

}
