using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetVax.BusinessObjects.Models
{
    [Table("VaccinationCertificate")]
    public class VaccinationCertificate
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CertificateId { get; set; }
        public int PetId { get; set; }
        public int? MicrochipItemId { get; set; }
        public int VetId { get; set; }
        public int DiseaseId { get; set; }
        public string CertificateCode { get; set; }
        public int DoseNumber { get; set; }
        public DateTime VaccinationDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public string ClinicName { get; set; }
        public string ClinicAddress { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime? ValidUntil { get; set; }
        public string Purpose { get; set; }
        public decimal Price { get; set; }
        public string Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string? ModifiedBy { get; set; }
        public bool? isDeleted { get; set; } = false;

        // Navigation properties
        public virtual Pet Pet { get; set; }
        public virtual MicrochipItem MicrochipItem { get; set; }
        public virtual Vet Vet { get; set; }
        public virtual Disease Disease { get; set; }
        public virtual ICollection<HealthConditionVaccinationCertificate> HealthConditionVaccinationCertificates { get; set; }
    }
}
