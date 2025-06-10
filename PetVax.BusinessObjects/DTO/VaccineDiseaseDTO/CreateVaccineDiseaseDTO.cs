using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetVax.BusinessObjects.DTO.VaccineDiseaseDTO
{
    public class CreateVaccineDiseaseDTO
    {
        [Required(ErrorMessage = "Vaccine ID is required")]
        public int VaccineId { get; set; }
        [Required(ErrorMessage = "Disease ID is required")]
        public int DiseaseId { get; set; }
    }
}
