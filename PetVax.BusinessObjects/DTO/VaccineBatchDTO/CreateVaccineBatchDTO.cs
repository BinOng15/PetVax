using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetVax.BusinessObjects.DTO.VaccineBatchDTO
{
    public class CreateVaccineBatchDTO
    {
        [Required(ErrorMessage = "Vui lòng lựa chọn vắc xin cần để tạo lô vắc xin.")]
        [Range(1, int.MaxValue, ErrorMessage = "Vui lòng chọn vắc xin hợp lệ.")]
        public int VaccineId { get; set; }
        [Required(ErrorMessage = "Vui lòng chọn ngày sản xuất của lô vắc xin.")]
        public DateTime ManufactureDate { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập ngày hết hạn của lô vắc xin.")]
        public DateTime ExpiryDate { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập nhà sản xuất.")]
        [StringLength(100, ErrorMessage = "Nhà sản xuất không được vượt quá 100 ký tự.")]
        public string Manufacturer { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập nguồn gốc của lô vắc xin.")]
        [StringLength(100, ErrorMessage = "Nguồn gốc không được vượt quá 100 ký tự.")]
        public string Source { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập điều kiện bảo quản của lô vắc xin.")]
        [StringLength(200, ErrorMessage = "Điều kiện bảo quản không được vượt quá 200 ký tự.")]
        public string StorageCondition { get; set; }
        //[Required(ErrorMessage = "Vui lòng nhập số lượng.")]
        //[Range(1, int.MaxValue, ErrorMessage = "Số lượng phải lớn hơn 0.")]
        //public int Quantity { get; set; }
    }
}
