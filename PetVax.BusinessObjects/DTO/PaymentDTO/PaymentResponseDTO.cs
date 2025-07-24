using PetVax.BusinessObjects.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetVax.BusinessObjects.DTO.PaymentDTO
{
    public class PaymentResponseDTO
    {
        public int PaymentId { get; set; }
        public int AppointmentDetailId { get; set; }
        public int CustomerId { get; set; }
        public int? VaccineBatchId { get; set; }
        public int? MicrochipId { get; set; }
        public int? VaccinationCertificateId { get; set; }
        public int? HealthConditionId { get; set; }
        public string PaymentCode { get; set; }
        public string? VoucherCode { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentMethod { get; set; }
        public EnumList.PaymentStatus PaymentStatus { get; set; }
        public string CheckoutUrl { get; set; }
        public string QRCode { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string? ModifiedBy { get; set; }
        public bool? isDeleted { get; set; }
        public string Url { get; set; }

    }
}
