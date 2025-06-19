using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetVax.BusinessObjects.DTO.VaccineProfileDTO
{
    public class VaccineProfileRequestDTO
    {
        public int PetId { get; set; } // Foreign key to Pet table
        public int DiseaseId { get; set; }
        public int? AppointmentDetailId { get; set; } // Foreign key to AppointmentDetail table, if applicable
        public DateTime? PreferedDate { get; set; } // Preferred date for the vaccination in "yyyy-MM-dd" format
        public DateTime? VaccinationDate { get; set; } // Actual date of vaccination in "yyyy-MM-dd" format
        public string? Dose { get; set; } // e.g., "1st Dose", "2nd Dose"
        public string? Reaction { get; set; } // e.g., "None", "Mild", "Severe"
        public string? NextVaccinationInfo { get; set; } // e.g., "Next vaccination due on 2023-12-01"
    }
}
