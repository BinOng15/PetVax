using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetVax.BusinessObjects.Models
{
    [Table("VaccineProfileDisease")]
    public class VaccineProfileDisease
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int VaccineProfileDiseasesId { get; set; }
        public int? VaccineProfileId { get; set; }
        public int? DiseaseId { get; set; }

        public virtual Disease Disease { get; set; }
        public virtual VaccineProfile VaccineProfile { get; set; }
    }
}
