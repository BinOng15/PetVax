using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetVax.BusinessObjects.DTO.DashboardDTO
{
    public class VetDashboardResponseDTO
    {
        public int TotalVaccines { get; set; }
        public int TotalVaccineBatches { get; set; }
        public int TotalCheckedInAppointmentVaccinations { get; set; }
        public int TotalCheckedInAppointmentMicrochips { get; set; }
        public int TotalCheckedInAppointmentHealthConditions { get; set; }
        public int TotalProcessedAppointmentVaccinations { get; set; }
        public int TotalProcessedAppointmentMicrochips { get; set; }
        public int TotalProcessedAppointmentHealthConditions { get; set; }
        public int TotalProcessingAppointments { get; set; }
        public int TotalConfirmedAppointments { get; set; }
        public int TotalCheckedInAppointments { get; set; }
        public int TotalProcessedAppointments { get; set; }
        public int TotalPaidAppointments { get; set; }
        public int TotalCompletedAppointments { get; set; }
        public int TotalCancelledAppointments { get; set; }
        public int TotalRejectedAppointments { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
