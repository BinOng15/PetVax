using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetVax.BusinessObjects.Models
{
    [Table("Microchip")]
    public class Microchip
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MicrochipId { get; set; } // Unique identifier for the microchip
        public string MicrochipCode { get; set; } // Unique code for the microchip, e.g., "MC123456789"
        public string Name { get; set; } // e.g., "Microchip A", "Microchip B"
        public string? Description { get; set; } // e.g., "Microchip for pet identification"
        public decimal Price { get; set; } // Price of the microchip
        public string Status { get; set; } // e.g., "Available", "Unvailable"
        public DateTime CreatedAt { get; set; } // Date when the record was created
        public string? CreatedBy { get; set; } // User who created the record
        public string? Notes { get; set; } // Additional notes about the microchip
        bool? isDeleted { get; set; } = false;
    }
}
