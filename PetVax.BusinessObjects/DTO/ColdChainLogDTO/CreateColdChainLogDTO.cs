using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetVax.BusinessObjects.DTO.ColdChainLogDTO
{
    public class CreateColdChainLogDTO
    {
        [Required(ErrorMessage = "Vui lòng chọn lô vắc xin cần ghi nhật ký.")]
        [Range(1, int.MaxValue, ErrorMessage = "Vui lòng chọn lô vắc xin hợp lệ.")]
        public int VaccineBatchId { get; set; }
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
