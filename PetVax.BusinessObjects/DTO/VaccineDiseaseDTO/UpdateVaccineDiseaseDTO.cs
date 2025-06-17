using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetVax.BusinessObjects.DTO.VaccineDiseaseDTO
{
    public class UpdateVaccineDiseaseDTO
    {
        public int? VaccineId { get; set; }
        public int? DiseaseId { get; set; }
    }
}
