using PetVax.BusinessObjects.DTO.CustomerDTO;
using PetVax.BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetVax.BusinessObjects.DTO.MembershipDTO
{
    public class MembershipResponseDTO
    {
        public int MembershipId { get; set; }
        public string MembershipCode { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int MinPoints { get; set; }
        public string Benefits { get; set; }
        public string Rank { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string? ModifiedBy { get; set; }
        public bool? isDeleted { get; set; } = false;

        public ICollection<CustomerResponseDTO> Customer { get; set; }
    }

    public class CreateUpdateMembershipResponseDTO
    {
        public int MembershipId { get; set; }
        public string MembershipCode { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int MinPoints { get; set; }
        public string Benefits { get; set; }
        public string Rank { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string? ModifiedBy { get; set; }
        public bool? isDeleted { get; set; } = false;
    }
}
