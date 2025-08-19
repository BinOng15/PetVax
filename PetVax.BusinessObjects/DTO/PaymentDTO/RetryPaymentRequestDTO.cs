using PetVax.BusinessObjects.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetVax.BusinessObjects.DTO.PaymentDTO
{
    public class RetryPaymentRequestDTO
    {
        [Required(ErrorMessage = "AppointmentDetailId là bắt buộc")]
        [Range(1, int.MaxValue, ErrorMessage = "AppointmentDetailId phải lớn hơn 0")]
        public int AppointmentDetailId { get; set; }
        [Required(ErrorMessage = "Người dùng không được để trống!")]
        public int CustomerId { get; set; }
        public int? VaccineBatchId { get; set; }
        public int? MicrochipId { get; set; }
        public int? VaccinationCertificateId { get; set; }
        public int? HealthConditionId { get; set; }
        [Required(ErrorMessage = "PaymentMethod là bắt buộc")]
        public EnumList.PaymentMethod PaymentMethod { get; set; }
        public string? VoucherCode { get; set; }

    }
}
