using PetVax.BusinessObjects.DTO.CustomerDTO;
using PetVax.BusinessObjects.DTO.VoucherDTO;
using PetVax.BusinessObjects.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetVax.BusinessObjects.DTO.CustomerVoucherDTO
{
    public class CustomerVoucherResponseDTO
    {
        public int CustomerVoucherId { get; set; }
        public int CustomerId { get; set; }
        public int VoucherId { get; set; }
        public DateTime RedeemedAt { get; set; }
        public string RedeemedBy { get; set; }
        public DateTime ExpirationDate { get; set; }
        public EnumList.VoucherStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string? ModifiedBy { get; set; }
        public bool? isDeleted { get; set; } = false;

        // Navigation properties
        public CustomerResponseDTO Customer { get; set; }
        public VoucherResponseDTO Voucher { get; set; }
    }
}
