using PetVax.BusinessObjects.DTO.AppointmentDetailDTO;
using PetVax.BusinessObjects.DTO.PetDTO;
using PetVax.BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetVax.BusinessObjects.DTO.MicrochipItemDTO
{
    public class MicrochipItemResponse
    {
        public int MicrochipId { get; set; } 
        public string Name { get; set; } 
        public string Description { get; set; } 
        public DateTime? InstallationDate { get; set; } 
        public string Status { get; set; }

        public PetMicrochipItemResponse Pet { get; set; } // Navigation to Pet table
    }
}
