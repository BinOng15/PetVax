using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PetVax.BusinessObjects.Enum.EnumList;

namespace PetVax.BusinessObjects.Helpers
{
    public static class SlotNumberHelper
    {
        public static int GetSlotNumberFromAppointmentDate(DateTime appointmentDate)
        {
            var hour = appointmentDate.Hour;

            return hour switch
            {
                8 => (int)Slot.Slot_8h,
                9 => (int)Slot.Slot_9h,
                10 => (int)Slot.Slot_10h,
                11 => (int)Slot.Slot_11h,
                13 => (int)Slot.Slot_13h,
                14 => (int)Slot.Slot_14h,
                15 => (int)Slot.Slot_15h,
                16 => (int)Slot.Slot_16h,
                _ => throw new ArgumentException("Khung giờ hẹn không hợp lệ.")
            };
        }
    }
}
