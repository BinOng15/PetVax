using PetVax.BusinessObjects.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetVax.BusinessObjects.DTO.PaymentDTO
{
    public class CreatePaymentRequestDTO
    {
        [Required(ErrorMessage = "Chi tiết cuộc hẹn không được để trống!")]
        public int AppointmentDetailId { get; set; }
        [Required(ErrorMessage = "Người dùng không được để trống!")]
        public int CustomerId { get; set; }
        public int? VaccineBatchId { get; set; }
        public int? MicrochipId { get; set; }
        public int? VaccinationCertificateId { get; set; }
        public int? HealthConditionId { get; set; }
        [Required(ErrorMessage = "Vui lòng chọn hình thức thanh toán!")]
        public EnumList.PaymentMethod PaymentMethod { get; set; }
    }
}
