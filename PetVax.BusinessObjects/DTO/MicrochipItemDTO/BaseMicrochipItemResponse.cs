using PetVax.BusinessObjects.DTO.MicrochipDTO;
using PetVax.BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetVax.BusinessObjects.DTO.MicrochipItemDTO
{
    public class BaseMicrochipItemResponse
    {
        public int MicrochipItemId { get; set; }
        public int? PetId { get; set; } 
        public string Name { get; set; } 
        public string Description { get; set; } 
        public DateTime InstallationDate { get; set; } 
        public string Status { get; set; } 
        public DateTime CreatedAt { get; set; } 
        public string? CreatedBy { get; set; } 

        public virtual MicrochipResponseDTO MicrochipResponse { get; set; } // Navigation to Microchip table
    }
}
