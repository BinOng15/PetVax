﻿using PetVax.BusinessObjects.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetVax.BusinessObjects.DTO.VetScheduleDTO
{
    public class UpdateVetScheduleRequestDTO
    {
        public int vetScheduleId {  get; set; }
        public int VetId { get; set; }
        public DateTime ScheduleDate { get; set; } 
        public int SlotNumber { get; set; } 
        public EnumList.VetScheduleStatus Status { get; set; } 
    }
}
