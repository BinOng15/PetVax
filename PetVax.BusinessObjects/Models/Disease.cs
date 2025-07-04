using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetVax.BusinessObjects.Models
{
    [Table("Disease")]
    public class Disease
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DiseaseId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Species { get; set; }
        public string Symptoms { get; set; }
        public string Treatment { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string? ModifiedBy { get; set; }
        public bool? isDeleted { get; set; } = false;

        // Navigation properties
        public virtual ICollection<VaccineProfile> VaccineProfiles { get; set; }
        public virtual ICollection<VaccineDisease> VaccineDiseases { get; set; }
        public virtual ICollection<VaccinationSchedule> VaccinationSchedules { get; set; }
        public virtual ICollection<VaccinationCertificate> VaccinationCertificates { get; set; }

    }
}
