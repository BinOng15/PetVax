﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetVax.BusinessObjects.Models
{
    [Table("PointTransaction")]
    public class PointTransaction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TransactionId { get; set; }
        public int CustomerId { get; set; }
        public int? PaymentId { get; set; }
        public int? VoucherId { get; set; }
        public string Change { get; set; }
        public string TransactionType { get; set; } // e.g., "Earned", "Redeemed"
        public string Description { get; set; }
        public DateTime TransactionDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string? ModifiedBy { get; set; }
        public bool? isDeleted { get; set; } = false;

        // Navigation properties
        public virtual Customer Customer { get; set; }
        public virtual Payment Payment { get; set; } // Navigation to Payment table, if applicable
        public virtual Voucher Voucher { get; set; } // Navigation to Voucher table, if applicable
        public virtual ICollection<Voucher> Vouchers { get; set; } // Collection of vouchers associated with this transaction

    }
}
