using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetVax.BusinessObjects.DTO.AccountDTO
{
    public class UpdateAccountDTO
    {
        [EmailAddress(ErrorMessage = "Invalid email format.")]
        public string? Email { get; set; }
        [RegularExpression(@"^(?=.*[A-Z])(?=.*\d).{8,16}$",
            ErrorMessage = "Minimum 8 characters, at least one uppercase letter and one number")]
        public string? Password { get; set; }
        public Enum.EnumList.Role? Role { get; set; }
        public bool? IsVerify { get; set; }
    }
}
