using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetVax.BusinessObjects.DTO.VaccinationCertificate
{
    public class UpdateVaccinationCertificateDTO
    {
        public int? PetId { get; set; }
        public int? MicrochipItemId { get; set; }
        public int? VetId { get; set; }
        public int? DiseaseId { get; set; }
        public string? ClinicName { get; set; }
        public string? ClinicAddress { get; set; }
        public string? Purpose { get; set; }
    }
}
