using PetVax.BusinessObjects.DTO.ColdChainLogDTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetVax.BusinessObjects.DTO.VaccineExportDetailDTO
{
    public class CreateVaccineExportDetailForVaccinationDTO
    {
        [Required(ErrorMessage = "Vui lòng chọn phiếu xuất để tạo chi tiết phiếu xuất.")]
        [Range(1, int.MaxValue, ErrorMessage = "Vui lòng chọn phiếu xuất hợp lệ.")]
        public int VaccineExportId { get; set; }
        [Required(ErrorMessage = "Vui lòng chọn lô vắc xin để tạo chi tiết phiếu xuất.")]
        [Range(1, int.MaxValue, ErrorMessage = "Vui lòng chọn lô vắc xin hợp lệ.")]
        public int VaccineBatchId { get; set; }
        public int AppointmentDetailId { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập số lượng vắc xin để tạo chi tiết phiếu xuất.")]
        public int Quantity { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập mục đích để tạo chi tiết phiếu xuất.")]
        public string Purpose { get; set; }
        [StringLength(500, ErrorMessage = "Ghi chú không được vượt quá 500 ký tự.")]
        public string Notes { get; set; } = string.Empty;

        public CreateColdChainLogDTOs ColdChainLog { get; set; }
    }

    public class CreateColdChainLogDTOs
    {
        [Required(ErrorMessage = "Vui lòng nhập thời gian ghi nhật ký.")]
        public DateTime LogTime { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập nhiệt độ.")]
        [Range(-100, 100, ErrorMessage = "Nhiệt độ phải nằm trong khoảng từ -100 đến 100 độ C.")]
        public double Temperature { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập độ ẩm.")]
        [Range(0, 100, ErrorMessage = "Độ ẩm phải nằm trong khoảng từ 0% đến 100%.")]
        public double Humidity { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập sự kiện.")]
        [StringLength(200, ErrorMessage = "Sự kiện không được vượt quá 200 ký tự.")]
        public string Event { get; set; }
        [StringLength(500, ErrorMessage = "Ghi chú không được vượt quá 500 ký tự.")]
        public string Notes { get; set; } = string.Empty;
    }
}
