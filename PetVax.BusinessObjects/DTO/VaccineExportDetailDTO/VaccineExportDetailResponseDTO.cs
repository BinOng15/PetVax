using PetVax.BusinessObjects.DTO.AppointmentDetailDTO;
using PetVax.BusinessObjects.DTO.VaccineBatchDTO;
using PetVax.BusinessObjects.DTO.VaccineExportDTO;
using PetVax.BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetVax.BusinessObjects.DTO.VaccineExportDetailDTO
{
    public class VaccineExportDetailResponseDTO
    {
        public int VaccineExportDetailId { get; set; }
        public int VaccineBatchId { get; set; }
        public int VaccineExportId { get; set; }
        public int Quantity { get; set; }
        public string Purpose { get; set; }
        public string Notes { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string? ModifiedBy { get; set; }
        public bool? isDeleted { get; set; } = false;

        // Navigation properties
        public virtual VaccineBatchResponseDTO VaccineBatch { get; set; }
        public virtual VaccineExportResponseDTO VaccineExport { get; set; }
        //public virtual AppointmentDetailResponseDTO AppointmentDetail { get; set; }
    }
}
