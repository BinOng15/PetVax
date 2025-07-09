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
        [Range(1, int.MaxValue, ErrorMessage = "Id chủ vật nuôi phải lớn hơn 0")]
        public int CustomerId { get; set; }
        [Required(ErrorMessage = "Vật nuôi không được để trống")]
        [Range(1, int.MaxValue, ErrorMessage = "Id vật nuôi phải lớn hơn 0")]
        public int PetId { get; set; }
        [Required(ErrorMessage = "Vui lòng chọn ngày đăng ký dịch vụ")]
        [DataType(DataType.DateTime, ErrorMessage = "Ngày đăng ký dịch vụ không hợp lệ")]
        public DateTime AppointmentDate { get; set; }
        [Required(ErrorMessage = "Vui lòng chọn loại dịch vụ")]
        [Range(1, 5, ErrorMessage = "Loại dịch vụ không hợp lệ")]
        public EnumList.ServiceType ServiceType { get; set; }
        [Required(ErrorMessage = "Vui lòng chọn nơi thực hiện dịch vụ (Tại trung tâm hoặc tại nhà)")]
        [Range(1, 2, ErrorMessage = "Vị trí không hợp lệ")]
        public EnumList.Location Location { get; set; }
        public string Address { get; set; }

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
    }

    public class CreateAppointmentHealthConditionDTO
    {
        public CreateAppointmentDTO Appointment { get; set; }
    }

    public class CreateAppointmentVaccinationCertificateDTO
    {
        public CreateAppointmentDTO AppointmentDTO { get; set; }
        public CreateAppointmentDetailVaccinationDTO AppointmentDetailVaccination { get; set; }

    }

}
