using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetVax.BusinessObjects.DTO.AuthenticateDTO
{
    public class ForgetPasswordRequestDTO
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }
        [Required]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*\d).{8,16}$",
            ErrorMessage = "Minimum 8 characters, at least one uppercase letter and one number")]
        public string OldPassword { get; set; }
        [Required]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*\d).{8,16}$",
            ErrorMessage = "Minimum 8 characters, at least one uppercase letter and one number")]
        public string NewPassword { get; set; }
    }

    public class ForgetPasswordResponseDTO
    {
        public string Message { get; set; } = string.Empty;
        public bool IsSuccess { get; set; }
    }
}
