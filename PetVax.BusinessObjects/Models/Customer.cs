using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetVax.BusinessObjects.Models
{
    [Table("Customer", Schema = "dbo")]
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }
        public int AccountId { get; set; }
        public int MembershipId { get; set; }
        public string CustomerCode { get; set; } // e.g., "CUST123456", unique identifier for the customer
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? DateOfBirth { get; set; }
        public string Gender { get; set; }
        public string? Address { get; set; }
        public string CurrentPoints { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string? ModifiedBy { get; set; }

        // Navigation properties
        public virtual Account Account { get; set; }
        public virtual Membership Membership { get; set; }
        public virtual ICollection<Pet> Pets { get; set; }

    }
}
