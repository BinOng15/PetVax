using PetVax.BusinessObjects.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetVax.BusinessObjects.DTO.AppointmentDetailDTO
{
    public class CreateAppointmentDetailDTO
    {
        public int? VetId { get; set; }
        public int? DiseaseId { get; set; }
        public int? VaccineBathId { get; set; }
        public int? MicrochipItemId { get; set; }
        public int? PassportId { get; set; }
        public int? HealthConditionId { get; set; }
        public string? Dose { get; set; }
    }

    public class CreateAppointmentDetailVaccinationDTO
    {
        [Required(ErrorMessage = "Vui lòng chọn bệnh để tiêm")]
        public int DiseaseId { get; set; }
    }

    public class CreateAppointmentDetailMicrochipDTO
    {
        [Required(ErrorMessage = "Vui lòng chọn mã microchip")]
        public int MicrochipItemId { get; set; }
    }
}
