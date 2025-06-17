using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetVax.BusinessObjects.Enum
{
    public class EnumList
    {
        public enum Role
        {
            Admin = 1,
            Staff = 2,
            Vet = 3,
            Customer = 4
        }
        public enum AppointmentStatus
        {
            Processing = 1,
            Confirmed = 2,
            CheckedIn = 3,
            Injected = 4,
            Implanted = 5,
            Paid = 6,
            Applied = 7,
            Done = 8,
            Completed = 9,
            Cancelled = 10,
            Rejected = 11,
        }

        public enum Slot
        {
            Slot_8h= 1,
            Slot_9h = 2,
            Slot_10h = 3,
            Slot_11h = 4,
            Slot_13h = 5,
            Slot_14h = 6,
            Slot_15h = 7,
            Slot_16h = 8,
        }

        public enum ServiceType
        {
            Vaccination = 1,
            Microchip = 2,
            HealthCertificate = 3,
        }

        public enum Location
        {
            Clinic = 1,
            HomeVisit = 2,
        }
    }
}
