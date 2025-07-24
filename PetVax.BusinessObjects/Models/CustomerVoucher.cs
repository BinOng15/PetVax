using PetVax.BusinessObjects.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetVax.BusinessObjects.Models
{
    [Table("CustomerVoucher")]
    public class CustomerVoucher
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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
        public virtual Customer Customer { get; set; }
        public virtual Voucher Voucher { get; set; }
    }
}
