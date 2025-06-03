using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetVax.BusinessObjects.Models
{
    [Table("Membership", Schema = "dbo")]
    public class Membership
    {
        [Key]
        public int MembershipId { get; set; }
        public string MembershipCode { get; set; } // e.g., "MEMB123456", unique identifier for the membership
        public string Name { get; set; }
        public string Description { get; set; }
        public string MinPoints { get; set; }
        public string Benefits { get; set; }
        public string Rank { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string? ModifiedBy { get; set; }

        public virtual Customer Customer { get; set; }

    }
}
