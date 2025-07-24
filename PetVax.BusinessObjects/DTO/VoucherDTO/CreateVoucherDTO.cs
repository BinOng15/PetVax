using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetVax.BusinessObjects.DTO.VoucherDTO
{
    public class CreateVoucherDTO
    {
        [Required(ErrorMessage = "Vui lòng nhập tên của voucher!")]
        [StringLength(100, ErrorMessage = "Tên voucher không được vượt quá 100 ký tự!")]
        public string VoucherName { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập số điểm cần thiết để sử dụng voucher!")]
        [Range(0, int.MaxValue, ErrorMessage = "Số điểm cần thiết phải là một số nguyên không âm!")]
        public int PointsRequired { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập mô tả cho voucher!")]
        [StringLength(200, ErrorMessage = "Mô tả voucher không được vượt quá 200 ký tự!")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập số tiền giảm giá!")]
        [Range(0, double.MaxValue, ErrorMessage = "Số tiền giảm giá phải là một số dương!")]
        public decimal DiscountAmount { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập ngày hết hạn của voucher!")]
        [DataType(DataType.Date, ErrorMessage = "Ngày hết hạn không hợp lệ!")]
        public DateTime ExpirationDate { get; set; }
    }
}
