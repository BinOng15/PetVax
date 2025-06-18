using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetVax.BusinessObjects.Models
{
    [Table("Account")]
    public class Account
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AccountId { get; set; }
        public string Email { get; set; }
        public string? PasswordHash { get; set; } //Null when use google account
        public string? PasswordSalt { get; set; } //Null when use google account
        public Enum.EnumList.Role Role { get; set; }
        public string? AccessToken { get; set; }
        public string? RefereshToken { get; set; }
        public bool isVerify { get; set; } = false; // Default to false, indicating the account is not verified
        public DateTime CreatedAt { get; set; }
        public string? CreatedBy { get; set; }


        // Navigation properties


    }
}
