using PetVax.BusinessObjects.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetVax.BusinessObjects.DTO.AccountDTO
{
    public class AccountResponseDTO
    {
        public int AccountId { get; set; }
        public string Email { get; set; }
        public EnumList.Role Role { get; set; }
    }
}
