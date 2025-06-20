using PetVax.BusinessObjects.DTO.DiseaseDTO;
using PetVax.BusinessObjects.DTO.VaccineProfileDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetVax.BusinessObjects.DTO.VaccineProfileDiseaseDTO
{
    public class VaccineProfileDiseaseResponse
    {
        public int VaccineProfileID { get; set; }
        public int DiseaseID { get; set; }

        public VaccineProfileResponseDTO VaccineProfile { get; set; }
        public DiseaseResponseDTO Disease { get; set; }
    }
}
