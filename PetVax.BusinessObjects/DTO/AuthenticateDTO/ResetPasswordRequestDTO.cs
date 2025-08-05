using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetVax.BusinessObjects.DTO.AuthenticateDTO
{
    public class ResetPasswordRequestDTO
    {
        [Required(ErrorMessage = "Vui lòng nhập email.")]
        [EmailAddress(ErrorMessage = "Email không đúng định dạng.")]
        public string Email { get; set; }
        [Required]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*\d).{8,16}$",
            ErrorMessage = "Tối thiểu 8 ký tự, ít nhất một chữ cái viết hoa và một chữ số")]
        public string OldPassword { get; set; }
        [Required]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*\d).{8,16}$",
            ErrorMessage = "Tối thiểu 8 ký tự, ít nhất một chữ cái viết hoa và một chữ số")]
        public string NewPassword { get; set; }
    }

    public class ResetPasswordResponseDTO
    {
        public string Message { get; set; } = string.Empty;
        public bool IsSuccess { get; set; }
    }
    public class ResetPasswordAfterForgetDTO
    {
        [Required(ErrorMessage = "Vui lòng nhập email.")]
        [EmailAddress(ErrorMessage = "Email không đúng định dạng.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập OTP")]
        public string Otp { get; set; }
        [Required]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*\d).{8,16}$",
            ErrorMessage = "Tối thiểu 8 ký tự, ít nhất một chữ cái viết hoa và một chữ số")]
        public string OldPassword { get; set; }
        [Required]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*\d).{8,16}$",
            ErrorMessage = "Tối thiểu 8 ký tự, ít nhất một chữ cái viết hoa và một chữ số")]
        public string NewPassword { get; set; }
    }
}
