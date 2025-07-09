using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetVax.BusinessObjects.DTO.VaccinationCertificate
{
    public class CreateVaccinationCertificateDTO
    {
        [Required(ErrorMessage = "Id vật nuôi không được để trống")]
        [Range(1, int.MaxValue, ErrorMessage = "Id vật nuôi phải lớn hơn 0")]
        public int PetId { get; set; }
        public int? MicrochipItemId { get; set; }
        [Required(ErrorMessage = "Id bác sĩ không được để trống")]
        [Range(1, int.MaxValue, ErrorMessage = "Id bác sĩ phải lớn hơn 0")]
        public int VetId { get; set; }
        [Required(ErrorMessage = "Id bệnh không được để trống")]
        [Range(1, int.MaxValue, ErrorMessage = "Id bệnh phải lớn hơn 0")]
        public int DiseaseId { get; set; }
        [Required(ErrorMessage = "Tên phòng khám không được để trống")]
        [StringLength(100, ErrorMessage = "Tên phòng khám không được vượt quá 100 ký tự")]
        public string ClinicName { get; set; }
        [Required(ErrorMessage = "Mục đích không được để trống")]
        [StringLength(200, ErrorMessage = "Mục đích không được vượt quá 100 ký tự")]
        public string Purpose { get; set; }
    }
}
