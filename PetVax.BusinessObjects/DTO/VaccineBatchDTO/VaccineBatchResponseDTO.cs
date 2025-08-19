using PetVax.BusinessObjects.DTO.ColdChainLogDTO;
using PetVax.BusinessObjects.DTO.VaccineDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetVax.BusinessObjects.DTO.VaccineBatchDTO
{
    public class VaccineBatchResponseDTO
    {
        public int VaccineBatchId { get; set; }
        public int VaccineId { get; set; }
        public string BatchNumber { get; set; }
        public DateTime ManufactureDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string Manufacturer { get; set; }
        public string Source { get; set; }
        public string StorageCondition { get; set; }
        public int Quantity { get; set; }
        public DateTime CreateAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string? ModifiedBy { get; set; }

        public VaccineResponseDTO VaccineResponseDTO { get; set; }
    }
}
