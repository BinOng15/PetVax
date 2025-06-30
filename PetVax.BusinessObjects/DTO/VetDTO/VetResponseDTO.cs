using PetVax.BusinessObjects.DTO.AccountDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetVax.BusinessObjects.DTO.VetDTO
{
    public class VetResponseDTO
    {
        public int VetId { get; set; } 
        public int AccountId { get; set; } 
        public string VetCode { get; set; } 
        public string? Name { get; set; } 

        public string image { get; set; } // Base64 string representation of the image
        public string? Specialization { get; set; } 
        public DateTime? DateOfBirth { get; set; } 
        public string? PhoneNumber { get; set; }  

        public AccountResponseDTO Account {  get; set; }
    }
}
