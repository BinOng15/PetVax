using PetVax.BusinessObjects.DTO.ColdChainLogDTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetVax.BusinessObjects.DTO.VaccineExportDetailDTO
{
    public class UpdateVaccineExportDetailDTO
    {
        [Range(1, int.MaxValue, ErrorMessage = "Vui lòng chọn phiếu xuất hợp lệ.")]
        public int? VaccineExportId { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Vui lòng chọn lô vắc xin hợp lệ.")]
        public int? VaccineBatchId { get; set; }
        public int? Quantity { get; set; }
        public string? Purpose { get; set; }
        [StringLength(500, ErrorMessage = "Ghi chú không được vượt quá 500 ký tự.")]
        public string? Notes { get; set; } = string.Empty;

        public UpdateColdChainLogDTO? ColdChainLog { get; set; }
    }
}
