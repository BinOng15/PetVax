using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetVax.BusinessObjects.Models
{
    [Table("ColdChainLog")]
    public class ColdChainLog
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ColdChainLogId { get; set; }
        public int VaccineBatchId { get; set; }
        public DateTime LogTime { get; set; }
        public double Temperature { get; set; }
        public double Humidity { get; set; }
        public string Event { get; set; }
        public string Notes { get; set; }
        public DateTime RecordedAt { get; set; }
        public string RecordedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string? ModifiedBy { get; set; }
        public bool? isDeleted { get; set; } = false;

        // Navigation properties
        public virtual VaccineBatch VaccineBatch { get; set; } = null!; // Navigation to VaccineBatch table
    }
}
