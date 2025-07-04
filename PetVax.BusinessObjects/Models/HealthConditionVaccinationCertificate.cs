using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetVax.BusinessObjects.Models
{
    [Table("HealthConditionVaccinationCertificate")]
    public class HealthConditionVaccinationCertificate
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int HealthConditionVaccinationCertificateId { get; set; }
        public int HealthConditionId { get; set; }
        public int VaccinationCertificateId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string? ModifiedBy { get; set; }
        public bool? isDeleted { get; set; } = false;

        // Navigation properties
        public virtual HealthCondition HealthCondition { get; set; }
        public virtual VaccinationCertificate VaccinationCertificate { get; set; }
    }
}
