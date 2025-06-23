using PetVax.BusinessObjects.DTO.AppointmentDetailDTO;
using PetVax.BusinessObjects.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetVax.BusinessObjects.DTO.AppointmentDTO
{
    public class CreateAppointmentDTO
    {
        [Required(ErrorMessage = "Chủ vật nuôi không được để trống")]
        public int CustomerId { get; set; }
        [Required(ErrorMessage = "Vật nuôi không được để trống")]
        public int PetId { get; set; }
        [Required(ErrorMessage = "Vui lòng chọn ngày đăng ký dịch vụ")]
        public DateTime AppointmentDate { get; set; }
        [Required(ErrorMessage = "Vui lòng chọn loại dịch vụ")]
        public EnumList.ServiceType ServiceType { get; set; }
        [Required(ErrorMessage = "Vui lòng chọn nơi thực hiện dịch vụ (Tại trung tâm hoặc tại nhà)")]
        public EnumList.Location Location { get; set; }
        public string Address { get; set; }

        public DateTime GetAppointmentDateInVietnamTime()
        {
            var vietnamTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            return TimeZoneInfo.ConvertTimeFromUtc(AppointmentDate.ToUniversalTime(), vietnamTimeZone);
        }



    }
    public class CreateFullAppointmentDTO
    {
        public CreateAppointmentDTO Appointment { get; set; }
        public CreateAppointmentDetailDTO AppointmentDetail { get; set; }
    }

    public class CreateAppointmentVaccinationDTO
    {
        public CreateAppointmentDTO Appointment { get; set; }
        public CreateAppointmentDetailVaccinationDTO AppointmentDetailVaccination { get; set; }
    }

    public class CreateAppointmentMicrochipDTO
    {
        public CreateAppointmentDTO Appointment { get; set; }
        public CreateAppointmentDetailMicrochipDTO AppointmentDetailMicrochip { get; set; }
    }
}
