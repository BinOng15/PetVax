using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetVax.BusinessObjects.DTO.MicrochipDTO
{
    public class MicrochipResponseDTO
    {
        public int MicrochipId { get; set; } // Unique identifier for the microchip
        public string MicrochipCode { get; set; } // Unique code for the microchip, e.g., "MC123456789"
        public string Name { get; set; } // e.g., "Microchip A", "Microchip B"
        public string Description { get; set; } // e.g., "Microchip for pet identification"
        public decimal Price { get; set; } // Price of the microchip
        public string Status { get; set; }
        public string Notes { get; set; }

    }
}
