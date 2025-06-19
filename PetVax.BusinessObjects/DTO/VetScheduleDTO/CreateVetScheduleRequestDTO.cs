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
        public List<ScheduleSlotDTO> Schedules { get; set; }

        public string Status { get; set; }
    }

    public class ScheduleSlotDTO
    {
        public DateTime ScheduleDate { get; set; }
        public List<int> SlotNumbers { get; set; }
    }
}
