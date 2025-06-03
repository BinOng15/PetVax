using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetVax.BusinessObjects.Models
{
    [Table("HealthCondition", Schema = "dbo")]
    public class HealthCondition
    {
        [Key]
        public int HealthConditionId { get; set; }
        public int PetId { get; set; } // Foreign key to Pet table
        public string ConditionCode { get; set; } // e.g., "HC123456", unique identifier for the health condition
        public string HeartRate { get; set; }
        public string BreathingRate { get; set; }
        public string Weight { get; set; } 
        public string Temperature { get; set; } // e.g., "38.5 °C", "101.5 °F"
        public string EHNM { get; set; }
        public string SkinAFur { get; set; } // e.g., "Normal", "Dry", "Itchy"
        public string Digestion { get; set; } // e.g., "Normal", "Vomiting", "Diarrhea"
        public string Respiratory { get; set; } // e.g., "Normal", "Coughing", "Sneezing"
        public string Excrete { get; set; } // e.g., "Normal", "Blood in stool", "Urinary issues"
        public string Behavior { get; set; } // e.g., "Normal", "Aggressive", "Lethargic"
        public string Psycho { get; set; } // e.g., "Normal", "Anxious", "Depressed"
        public string Different { get; set; } // e.g., "Normal", "Abnormal", "Unusual"
        public string Conclusion { get; set; } // e.g., "Healthy", "Needs further investigation", "Requires treatment"
        public DateTime CheckDate { get; set; }
        public string Status { get; set; } // e.g., "Active", "Resolved", "Pending Review"
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string? ModifiedBy { get; set; }

        // Navigation properties
        public virtual Pet Pet { get; set; }
    }
}
