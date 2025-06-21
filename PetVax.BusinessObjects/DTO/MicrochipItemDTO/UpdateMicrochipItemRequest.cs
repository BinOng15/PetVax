using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetVax.BusinessObjects.DTO.MicrochipItemDTO
{
    public class UpdateMicrochipItemRequest
    {
        public int MicrochipId { get; set; } 
        public int? PetId { get; set; } 
        public string Name { get; set; } 
        public string Description { get; set; } 
        public DateTime InstallationDate { get; set; } 
        public string Status { get; set; } 

    }
}
