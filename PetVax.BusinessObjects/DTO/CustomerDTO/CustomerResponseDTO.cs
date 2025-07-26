using PetVax.BusinessObjects.DTO.AccountDTO;
using PetVax.BusinessObjects.DTO.MembershipDTO;
using System;
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
        public string? PhoneNumber { get; set; }
        public string? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public string? Address { get; set; }
        public string? CurrentPoints { get; set; }
        public int? RedeemablePoints { get; set; }
        public decimal? TotalSpent { get; set; }
        public DateTime CreatedAt { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public string? ModifiedBy { get; set; }
        public bool? isDeleted { get; set; } = false; // Default to false, indicating the customer is not deleted

        public AccountResponseDTO AccountResponseDTO { get; set; }
        public MembershipResponseDTO MembershipResponseDTO { get; set; }
    }
}
