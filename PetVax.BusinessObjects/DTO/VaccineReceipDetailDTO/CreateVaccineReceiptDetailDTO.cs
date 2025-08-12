using PetVax.BusinessObjects.DTO.ColdChainLogDTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetVax.BusinessObjects.DTO.VaccineReceipDetailDTO
{
    public class CreateVaccineReceiptDetailDTO
    {
        [Required(ErrorMessage = "Vui lòng chọn phiếu nhập để tạo chi tiết phiếu nhập.")]
        [Range(1, int.MaxValue, ErrorMessage = "Vui lòng chọn phiếu nhập hợp lệ.")]
        public int VaccineReceiptId { get; set; }
        [Required(ErrorMessage = "Vui lòng chọn lô vắc xin để tạo chi tiết phiếu nhập.")]
        [Range(1, int.MaxValue, ErrorMessage = "Vui lòng chọn lô vắc xin hợp lệ.")]
        public int VaccineBatchId { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập nhà cung cấp để tạo chi tiết phiếu nhập.")]
        public string Suppiler { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập số lượng vắc xin để tạo chi tiết phiếu nhập.")]
        [Range(1, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn 0.")]
        public int Quantity { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập trạng thái vắc xin để tạo chi tiết phiếu nhập.")]
        [StringLength(200, ErrorMessage = "Trạng thái vắc xin không được vượt quá 200 ký tự.")]
        public string VaccineStatus { get; set; }
        [StringLength(500, ErrorMessage = "Ghi chú không được vượt quá 500 ký tự.")]
        public string Notes { get; set; } = string.Empty;

        public CreateColdChainLogDTO ColdChainLog { get; set; }
    }

    public class CreateFullVaccineReceiptDTO
    {
        [Required(ErrorMessage = "Vui lòng chọn ngày nhập kho.")]
        public DateTime ReceiptDate { get; set; }
        [Required(ErrorMessage = "Vui lòng chọn lô vắc xin để tạo chi tiết phiếu nhập.")]
        [Range(1, int.MaxValue, ErrorMessage = "Vui lòng chọn lô vắc xin hợp lệ.")]
        public int VaccineBatchId { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập nhà cung cấp để tạo chi tiết phiếu nhập.")]
        public string Suppiler { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập số lượng vắc xin để tạo chi tiết phiếu nhập.")]
        [Range(1, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn 0.")]
        public int Quantity { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập trạng thái vắc xin để tạo chi tiết phiếu nhập.")]
        [StringLength(200, ErrorMessage = "Trạng thái vắc xin không được vượt quá 200 ký tự.")]
        public string VaccineStatus { get; set; }
        [StringLength(500, ErrorMessage = "Ghi chú không được vượt quá 500 ký tự.")]
        public string Notes { get; set; } = string.Empty;

        public CreateColdChainLogDTO ColdChainLog { get; set; }
    }
}
