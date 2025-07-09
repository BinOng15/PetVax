using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetVax.BusinessObjects.DTO.MicrochipItemDTO
{
    public class CreateMicrochipItemRequest
    {
        public int? PetId { get; set; } // Foreign key to Pet table
        public string? Name { get; set; } // e.g., "Microchip A", "Microchip B"
        public string? Description { get; set; } // e.g., "Microchip for pet identification"
        public string? Location { get; set; }
        public DateTime? InstallationDate { get; set; } // Date when the microchip was installed
    }
}
