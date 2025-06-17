using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetVax.BusinessObjects.DTO.VaccineBatchDTO
{
    public class CreateVaccineBatchDTO
    {
        [Required(ErrorMessage = "VaccineId is required")]
        public int VaccineId { get; set; }
        [Required(ErrorMessage = "ManufactureDate is required")]
        public DateTime ManufactureDate { get; set; }
        [Required(ErrorMessage = "ExpiryDate is required")]
        public DateTime ExpiryDate { get; set; }
        [Required(ErrorMessage = "Quantity is required")]
        public int Quantity { get; set; }
    }
}
