﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PetVax.BusinessObjects.Models
{
    [Table("Customer")]
    public class Customer
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CustomerId { get; set; }
        public int AccountId { get; set; }
        public int? MembershipId { get; set; }
        public string? CustomerCode { get; set; } // e.g., "CUST123456", unique identifier for the customer
        public string? FullName { get; set; }
        public string? UserName { get; set; }
        public string? Image { get; set; } // URL or path to the customer's image
        public string? PhoneNumber { get; set; }
        public string? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public string? Address { get; set; }
        public int? CurrentPoints { get; set; }
        public int? RedeemablePoints { get; set; }
        public decimal? TotalSpent { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string? ModifiedBy { get; set; }
        public bool? isDeleted { get; set; } = false;

        // Navigation properties
        public virtual Account Account { get; set; }
        public virtual Membership Membership { get; set; }
        public virtual ICollection<Pet> Pets { get; set; }
        public virtual ICollection<CustomerVoucher> CustomerVouchers { get; set; }

        public virtual ICollection<ServiceHistory> ServiceHistories { get; set; } // Navigation to ServiceHistory table

    }
}
