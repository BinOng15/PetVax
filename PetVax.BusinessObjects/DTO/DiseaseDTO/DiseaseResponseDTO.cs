using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetVax.BusinessObjects.DTO.DiseaseDTO
{
    public class DiseaseResponseDTO
    {
        public int DiseaseId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Species { get; set; }
        public string Symptoms { get; set; }
        public string Treatment { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string? ModifiedBy { get; set; }
    }
}
