using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetVax.BusinessObjects.DTO.VetScheduleDTO
{
    public class CreateVetScheduleRequestDTO
    {
        public int VetId { get; set; } 
        public DateTime ScheduleDate { get; set; } 
        public int SlotNumber { get; set; } 
        public string Status { get; set; } 
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; } 
    }
}
