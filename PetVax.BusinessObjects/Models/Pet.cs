using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetVax.BusinessObjects.Models
{
    [Table("Pet", Schema = "dbo")]
    public class Pet
    {
        [Key]
        public int PetId { get; set; }
        public int CustomerId { get; set; }
        public string PetCode { get; set; } // e.g., "PET123456", unique identifier for the pet
        public string Name { get; set; }
        public string Species { get; set; } // e.g., Dog, Cat
        public string Breed { get; set; }
        public string Age { get; set; } // e.g., "2 years", "6 months"
        public string Gender { get; set; }
        public string DateOfBirth { get; set; }
        public string Image { get; set; } 
        public string Weight { get; set; } // e.g., "10 kg", "5 lbs"
        public string Color { get; set; } // e.g., "Brown", "Black"
        public string Nationality { get; set; } // e.g., "American", "British"
        public bool isSterilized { get; set; } // true if sterilized, false otherwise
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string? ModifiedBy { get; set; }

        // Navigation properties
        [ForeignKey("CustomerId")]
        public virtual Customer Customer { get; set; } // Navigation to Customer table
        public virtual ICollection<MicrochipItem> MicrochipItems { get; set; } // Navigation to MicrochipItem table
        public virtual ICollection<PetPassport> PetPassports { get; set; } // Navigation to PetPassport table
        public virtual ICollection<Appointment> Appointments { get; set; } // Navigation to Appointment table
    }
}
