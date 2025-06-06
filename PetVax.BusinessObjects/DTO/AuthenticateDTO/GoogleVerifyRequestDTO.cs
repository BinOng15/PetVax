using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetVax.BusinessObjects.DTO.AuthenticateDTO
{
    public class GoogleVerifyRequestDTO
    {
        public string Email { get; set; } // Email of the user
        public string Token { get; set; }
        public string Name { get; set; } // Optional: Name of the user
    }
}
