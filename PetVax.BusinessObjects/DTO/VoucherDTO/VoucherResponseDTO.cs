using PetVax.BusinessObjects.DTO.PointTransactionDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetVax.BusinessObjects.DTO.VoucherDTO
{
    public class VoucherResponseDTO
    {
        public int VoucherId { get; set; }
        public string VoucherCode { get; set; }
        public int TransactionId { get; set; }
        public string VoucherName { get; set; }
        public int PointsRequired { get; set; }
        public string Description { get; set; }
        public decimal DiscountAmount { get; set; }
        public DateTime ExpirationDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string? ModifiedBy { get; set; }
        public bool? isDeleted { get; set; } = false;

        public PointTransactionResponseDTO PointTransaction { get; set; }
    }

    public class CreateUpdateVoucherResponseDTO
    {
        public int VoucherId { get; set; }
        public string VoucherCode { get; set; }
        public int TransactionId { get; set; }
        public string VoucherName { get; set; }
        public int PointsRequired { get; set; }
        public string Description { get; set; }
        public decimal DiscountAmount { get; set; }
        public DateTime ExpirationDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string? ModifiedBy { get; set; }
        public bool? isDeleted { get; set; } = false;
    }

}
