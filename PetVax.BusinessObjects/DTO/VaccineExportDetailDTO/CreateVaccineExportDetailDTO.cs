using PetVax.BusinessObjects.DTO.ColdChainLogDTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetVax.BusinessObjects.DTO.VaccineExportDetailDTO
{
    public class CreateVaccineExportDetailDTO
    {
        [Required(ErrorMessage = "Vui lòng chọn phiếu xuất để tạo chi tiết phiếu xuất.")]
        [Range(1, int.MaxValue, ErrorMessage = "Vui lòng chọn phiếu xuất hợp lệ.")]
        public int VaccineExportId { get; set; }
        [Required(ErrorMessage = "Vui lòng chọn lô vắc xin để tạo chi tiết phiếu xuất.")]
        [Range(1, int.MaxValue, ErrorMessage = "Vui lòng chọn lô vắc xin hợp lệ.")]
        public int VaccineBatchId { get; set; }
        //public int? AppointmentDetailId { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập số lượng vắc xin để tạo chi tiết phiếu xuất.")]
        [Range(1, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn 0.")]
        public int Quantity { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập mục đích để tạo chi tiết phiếu xuất.")]
        public string Purpose { get; set; }
        [StringLength(500, ErrorMessage = "Ghi chú không được vượt quá 500 ký tự.")]
        public string Notes { get; set; } = string.Empty;

        public CreateColdChainLogDTO ColdChainLog { get; set; }
    }

    public class CreateFullVaccineExportDTO
    {
        [Required(ErrorMessage = "Vui lòng chọn ngày xuất kho.")]
        public DateTime ExportDate { get; set; }
        
        [Required(ErrorMessage = "Vui lòng chọn lô vắc xin để tạo chi tiết phiếu xuất.")]
        [Range(1, int.MaxValue, ErrorMessage = "Vui lòng chọn lô vắc xin hợp lệ.")]
        public int VaccineBatchId { get; set; }
        //public int? AppointmentDetailId { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập số lượng vắc xin để tạo chi tiết phiếu xuất.")]
        [Range(1, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn 0.")]
        public int Quantity { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập mục đích để tạo chi tiết phiếu xuất.")]
        public string Purpose { get; set; }
        [StringLength(500, ErrorMessage = "Ghi chú không được vượt quá 500 ký tự.")]
        public string Notes { get; set; } = string.Empty;
        public CreateColdChainLogDTO ColdChainLog { get; set; }
    }
}
