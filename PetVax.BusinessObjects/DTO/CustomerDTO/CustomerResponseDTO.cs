﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetVax.BusinessObjects.DTO.CustomerDTO
{
    public class CustomerResponseDTO
    {
        public int CustomerId { get; set; }
        public int AccountId { get; set; }
        public int? MembershipId { get; set; }
        public string? CustomerCode { get; set; }
        public string? FullName { get; set; }
        public string? UserName { get; set; }
        public string? Image { get; set; } // URL or path to the customer's image
        public string Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public string? Address { get; set; }
        public string? CurrentPoints { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string? ModifiedBy { get; set; }
    }
}
