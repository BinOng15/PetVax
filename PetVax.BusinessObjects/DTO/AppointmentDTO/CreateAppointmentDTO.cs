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
        public string ServiceType { get; set; }
        [Required(ErrorMessage = "Vui lòng chọn nơi thực hiện dịch vụ (Tại trung tâm hoặc tại nhà)")]
        public string Location { get; set; }
        [Required(ErrorMessage = "Địa chỉ nhà không được để trống")]
        public string Address { get; set; }

    }
}
