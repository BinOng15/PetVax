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
            Processed = 4,
            Paid = 5,
            Applied = 6,
            Issued = 7,
            Denied = 8,
            Completed = 9,
            Cancelled = 10,
            Rejected = 11,
        }

        public enum Slot
        {
            Slot_8h= 8,
            Slot_9h = 9,
            Slot_10h = 10,
            Slot_11h = 11,
            Slot_13h = 13,
            Slot_14h = 14,
            Slot_15h = 15,
            Slot_16h = 16,
        }

        public enum ServiceType
        {
            Vaccination = 1,
            Microchip = 2,
            HealthCondition = 3,
            VaccinationCertificate = 4,
            HealthConditionCertificate = 5,
        }

        public enum Location
        {
            Clinic = 1,
            HomeVisit = 2,
        }

        public enum VetScheduleStatus
        {
            Available = 1,
            Unavailable = 2,
            Scheduled = 3,
        }

        public enum PaymentStatus
        {
            Pending = 1,
            Completed = 2,
            Failed = 3,
            Refunded = 4,
        }

        public enum PaymentMethod
        {
            Cash = 1,
            BankTransfer = 2,
        }
    }
}
