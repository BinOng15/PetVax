using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetVax.BusinessObjects.Models
{
    [Table("MicrochipItem")]
    public class MicrochipItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MicrochipItemId { get; set; }
        public int MicrochipId { get; set; } // Foreign key to Microchip table
        public int? PetId { get; set; } // Foreign key to Pet table
        public string Name { get; set; } // e.g., "Microchip A", "Microchip B"
        public string Description { get; set; } // e.g., "Microchip for pet identification"
        public DateTime InstallationDate { get; set; } // Date when the microchip was installed
        public string Status { get; set; } // e.g., "Active", "Inactive", "Lost"
        public DateTime CreatedAt { get; set; } // Date when the record was created
        public string? CreatedBy { get; set; } // User who created the record
        public DateTime? ModifiedAt { get; set; } // Date when the record was last modified
        public string? ModifiedBy { get; set; } // User who last modified the record

        // Navigation properties
        public virtual Microchip Microchip { get; set; } // Navigation to Microchip table
    }
}
