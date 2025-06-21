using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetVax.BusinessObjects.Models
{
    [Table("PetPassport")]
    public class PetPassport
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PassportId { get; set; }
        public int PetId { get; set; }
        public int MicrochipItemId { get; set; } // Foreign key to MicrochipItem table
        public int HealthConditionId { get; set; } // Foreign key to HealthCondition table
        public string PassportCode { get; set; } // Unique code for the passport, e.g., "PP123456789"
        public string PassportImage { get; set; } // URL or path to the passport image
        public string VaccinationDetails { get; set; } // e.g., "Rabies, Distemper"
        public string HealthCheckDetails { get; set; } // e.g., "Healthy, No issues"
        public DateTime IssuedDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public bool isRabiesVaccinated { get; set; } // true if vaccinated against rabies, false otherwise
        public decimal Price { get; set; } // Price of the passport
        public string Status { get; set; } // e.g., "Active", "Expired", "Pending Approval"
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ApprovedAt { get; set; } // Date when the passport was approved
        public string ApprovedBy { get; set; } // User who approved the passport
        public DateTime? ModifiedAt { get; set; }
        public string? ModifiedBy { get; set; }
        bool? isDeleted { get; set; } = false;

        // Navigation properties
        public virtual Pet Pet { get; set; }
        public virtual MicrochipItem MicrochipItem { get; set; }
        public virtual HealthCondition HealthCondition { get; set; }
    }
}
