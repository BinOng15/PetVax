using PetVax.BusinessObjects.DTO.ColdChainLogDTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetVax.BusinessObjects.DTO.VaccineReceipDetailDTO
{
    public class UpdateVaccineReceiptDetailDTO
    {
        [Range(1, int.MaxValue, ErrorMessage = "Vui lòng chọn phiếu nhập hợp lệ.")]
        public int? VaccineReceiptId { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Vui lòng chọn lô vắc xin hợp lệ.")]
        public int? VaccineBatchId { get; set; }
        public string? Suppiler { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn 0.")]
        public int? Quantity { get; set; }
        [StringLength(200, ErrorMessage = "Trạng thái vắc xin không được vượt quá 200 ký tự.")]
        public string? VaccineStatus { get; set; }
        [StringLength(500, ErrorMessage = "Ghi chú không được vượt quá 500 ký tự.")]
        public string? Notes { get; set; }

        public UpdateColdChainLogDTO? ColdChainLog { get; set; }
    }
}
