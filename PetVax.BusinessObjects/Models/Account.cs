using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetVax.BusinessObjects.Models
{
    [Table("Account", Schema = "dbo")]
    public class Account
    {
        [Key]
        public int AccountId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public Enum.EnumList.Role Role { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string ModifiedBy { get; set; }

        // Navigation properties


    }
}
