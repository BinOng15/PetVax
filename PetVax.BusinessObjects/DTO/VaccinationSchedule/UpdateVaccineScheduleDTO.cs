using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetVax.BusinessObjects.DTO.VaccinationSchedule
{
    public class UpdateVaccineScheduleDTO
    {
        public int? DiseaseId { get; set; } // Disease ID to be updated
        public string? Species { get; set; } // Species to be updated
        public int? DoseNumber { get; set; } // Dose number to be updated
        public int? AgeInterval { get; set; } // Age interval to be updated
    }
}
