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
    }
}
