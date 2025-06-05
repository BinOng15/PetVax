using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetVax.BusinessObjects.DTO.AuthenticateDTO
{
    public class OtpVerificationRequestDTO
    {
        [Required]
        public string Email { get; set; } // Email of the user
        [Required]
        public string Otp { get; set; } // One-Time Password sent to the user's email
    }
}
