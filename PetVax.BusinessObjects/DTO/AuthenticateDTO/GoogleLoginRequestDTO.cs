using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetVax.BusinessObjects.DTO.AuthenticateDTO
{
    public class GoogleLoginRequestDTO
    {
        public string Email { get; set; }
        public string Name { get; set; }
    }

}
