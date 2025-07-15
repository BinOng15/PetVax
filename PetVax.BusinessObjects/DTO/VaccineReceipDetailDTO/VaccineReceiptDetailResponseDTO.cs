using PetVax.BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetVax.BusinessObjects.DTO.VaccineReceipDetailDTO
{
    public class VaccineReceiptDetailResponseDTO
    {
        public int VaccineReceiptDetailId { get; set; }
        public int VaccineReceiptId { get; set; }
        public int VaccineBatchId { get; set; }
        public string Suppiler { get; set; }
        public int Quantity { get; set; }
        public string VaccineStatus { get; set; }
        public string Notes { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string? ModifiedBy { get; set; }
        public bool? isDeleted { get; set; } = false;

        // Navigation properties
        public VaccineReceipt VaccineReceipt { get; set; }
        public VaccineBatch VaccineBatch { get; set; }
    }
}
