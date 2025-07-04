using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetVax.BusinessObjects.DTO.VaccineProfileDTO
{
    public class VaccineProfileGroupByDiseaseResponseDTO
    {
        public int? DiseaseId { get; set; }
        public string? DiseaseName { get; set; }
        public List<VaccineProfileResponseDTO> Doses { get; set; } = new();
    }
}
