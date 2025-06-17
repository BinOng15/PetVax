using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetVax.BusinessObjects.DTO.CustomerDTO
{
    public class UpdateCustomerDTO
    {
        public string? FullName { get; set; }
        public string? UserName { get; set; }
        public IFormFile? Image { get; set; } // URL or path to the customer's image
        public string? PhoneNumber { get; set; }
        public string? DateOfBirth { get; set; }
        public string? Gender { get; set; }
        public string? Address { get; set; }
    }
}
