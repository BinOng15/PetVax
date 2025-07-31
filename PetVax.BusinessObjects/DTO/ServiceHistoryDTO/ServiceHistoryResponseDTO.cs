using PetVax.BusinessObjects.DTO.CustomerDTO;
using PetVax.BusinessObjects.DTO.PetDTO;
using PetVax.BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PetVax.BusinessObjects.Enum.EnumList;

namespace PetVax.BusinessObjects.DTO.ServiceHistoryDTO
{
    public class ServiceHistoryResponseDTO
    {
        public int ServiceHistoryId { get; set; }

        public int AppointmentId { get; set; } 
        public ServiceType ServiceType { get; set; } 
        public DateTime ServiceDate { get; set; } 

        public string PaymentMethod { get; set; } 

        public decimal Amount { get; set; } 
        public string Status { get; set; } 
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; } 
        public DateTime? ModifiedAt { get; set; }
        public string? ModifiedBy { get; set; } 
        public bool? isDeleted { get; set; } = false;

        public CustomerResponseDTO Customer { get; set; } // Navigation to Customer DTO
        public PetResponseDTOs Pet { get; set; } // Navigation to Pet DTO
    }
}
