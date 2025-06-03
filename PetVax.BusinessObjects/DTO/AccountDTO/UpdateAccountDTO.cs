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
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
}
