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
        public DateTime CreatedAt { get; set; } // Date when the record was created
        public string CreatedBy { get; set; } // User who created the record
        public DateTime? ModifiedAt { get; set; } // Date when the record was last modified
        public string? ModifiedBy { get; set; } // User who last modified the record
        public bool? isDeleted { get; set; } = false; // Default to false, indicating the microchip is not deleted
    }
}
