using PetVax.BusinessObjects.DTO.MicrochipItemDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetVax.BusinessObjects.DTO.MicrochipDTO
{
    public class MicrochipRequestDTO
    {

        public string MicrochipCode { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Notes { get; set; }
        public CreateMicrochipItemRequest createMicrochipItemRequest { get; set; } 
    }
}
