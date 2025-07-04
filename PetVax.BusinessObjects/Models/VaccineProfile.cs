using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetVax.BusinessObjects.Models
{
    [Table("VaccineProfile")]
    public class VaccineProfile
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int VaccineProfileId { get; set; }
        public int PetId { get; set; }
        public int? AppointmentDetailId { get; set; }
        public int? VaccinationScheduleId { get; set; }
        public int? DiseaseId { get; set; }
        public DateTime? PreferedDate { get; set; }
        public DateTime? VaccinationDate { get; set; }
        public int? Dose { get; set; }
        public string? Reaction { get; set; }
        public string? NextVaccinationInfo { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsCompleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string? ModifiedBy { get; set; }
        public bool? isDeleted { get; set; } = false;

        // Navigation properties
        public virtual Pet Pet { get; set; }
        [ForeignKey("DiseaseId")]
        public virtual Disease Disease { get; set; }
        public virtual AppointmentDetail AppointmentDetail { get; set; }
        public virtual VaccinationSchedule VaccinationSchedule { get; set; }
    }
}
