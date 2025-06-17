using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetVax.BusinessObjects.DTO.VaccinationSchedule
{
    public class CreateVaccinationScheduleDTO
    {
        [Required(ErrorMessage = "Bệnh không được bỏ trống.")]
        public int DiseaseId { get; set; } // ID of the disease for which the vaccination schedule is created
        [Required(ErrorMessage = "Loài của thú cưng không được bỏ trống.")]
        public string Species { get; set; } // Species for which the vaccination schedule is applicable (e.g., "Dog", "Cat")
        [Required(ErrorMessage = "Mũi tiêm không được bỏ trống.")]
        public int DoseNumber { get; set; } // The number of doses in the vaccination schedule
        [Required(ErrorMessage = "Độ tuổi của thú cưng không được bỏ trống.")]
        public int AgeInterval { get; set; } // Age interval in days for the vaccination schedule
    }
}
