using PetVax.BusinessObjects.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetVax.BusinessObjects.DTO.AccountDTO
{
    public class CreateAccountDTO
    {
        [Required(ErrorMessage = "Email is required.")]
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password is required.")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*\d).{8,16}$",
            ErrorMessage = "Minimum 8 characters, at least one uppercase letter and one number")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Role is required.")]
        public EnumList.Role Role { get; set; }
    }
}
