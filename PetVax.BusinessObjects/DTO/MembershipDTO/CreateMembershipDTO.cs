using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetVax.BusinessObjects.DTO.MembershipDTO
{
    public class CreateMembershipDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public int MinPoints { get; set; }
        public string Benefits { get; set; }
        public string Rank { get; set; }
    }
}
