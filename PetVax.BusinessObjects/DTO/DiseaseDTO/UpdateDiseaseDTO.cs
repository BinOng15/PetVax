using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetVax.BusinessObjects.DTO.DiseaseDTO
{
    public class UpdateDiseaseDTO
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Species { get; set; }
        public string? Symptoms { get; set; }
        public string? Treatment { get; set; }
    }
}
