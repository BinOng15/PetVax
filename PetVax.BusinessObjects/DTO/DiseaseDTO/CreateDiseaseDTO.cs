using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetVax.BusinessObjects.DTO.DiseaseDTO
{
    public class CreateDiseaseDTO
    {
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Species is required")]
        public string Species { get; set; }
        [Required(ErrorMessage = "Symptoms are required")]
        public string Symptoms { get; set; }
        [Required(ErrorMessage = "Treatment is required")]
        public string Treatment { get; set; }
    }
}
