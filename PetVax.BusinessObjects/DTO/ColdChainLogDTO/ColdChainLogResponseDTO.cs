using PetVax.BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetVax.BusinessObjects.DTO.ColdChainLogDTO
{
    public class ColdChainLogResponseDTO
    {
        public int ColdChainLogId { get; set; }
        public int VaccineBatchId { get; set; }
        public DateTime LogTime { get; set; }
        public double Temperature { get; set; }
        public double Humidity { get; set; }
        public string Event { get; set; }
        public string Notes { get; set; }
        public DateTime RecordedAt { get; set; }
        public string RecordedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string? ModifiedBy { get; set; }
        public bool? isDeleted { get; set; } = false;

        // Navigation properties
        public VaccineBatch VaccineBatch { get; set; }
    }
}
