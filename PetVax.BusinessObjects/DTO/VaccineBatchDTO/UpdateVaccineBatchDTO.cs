using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetVax.BusinessObjects.DTO.VaccineBatchDTO
{
    public class UpdateVaccineBatchDTO
    {
        [Range(1, int.MaxValue, ErrorMessage = "Vui lòng chọn vắc xin hợp lệ.")]
        public int? VaccineId { get; set; }
        public DateTime? ManufactureDate { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public string? Manufacturer { get; set; }
        public string? Source { get; set; }
        public string? StorageCondition { get; set; }
    }
}
