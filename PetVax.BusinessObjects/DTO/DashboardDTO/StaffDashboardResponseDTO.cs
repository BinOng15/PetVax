using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetVax.BusinessObjects.DTO.DashboardDTO
{
    public class StaffDashboardResponseDTO
    {
        public int TotalCustomers { get; set; }
        public int TotalDiseases { get; set; }
        public int TotalPets { get; set; }
        public int TotalVaccines { get; set; }
        public int TotalVaccineBatches { get; set; }
        public int TotalVets { get; set; }
        public int TotalAppointmentVaccinations { get; set; }
        public int TotalProcessingAppointmentVaccinations { get; set; }
        public int TotalConfirmedAppointmentVaccinations { get; set; }
        public int TotalCheckedInAppointmentVaccinations { get; set; }
        public int TotalProcessedAppointmentVaccinations { get; set; }
        public int TotalPaidAppointmentVaccinations { get; set; }
        public int TotalCompletedAppointmentVaccinations { get; set; }
        public int TotalCancelledAppointmentVaccinations { get; set; }
        public int TotalRejectedAppointmentVaccinations { get; set; }
        public int TotalAppointmentMicrochips { get; set; }
        public int TotalProcessingAppointmentMicrochips { get; set; }
        public int TotalConfirmedAppointmentMicrochips { get; set; }
        public int TotalCheckedInAppointmentMicrochips { get; set; }
        public int TotalProcessedAppointmentMicrochips { get; set; }
        public int TotalPaidAppointmentMicrochips { get; set; }
        public int TotalCompletedAppointmentMicrochips { get; set; }
        public int TotalCancelledAppointmentMicrochips { get; set; }
        public int TotalRejectedAppointmentMicrochips { get; set; }
        public int TotalAppointmentHealthConditions { get; set; }
        public int TotalProcessingAppointmentHealthConditions { get; set; }
        public int TotalConfirmedAppointmentHealthConditions { get; set; }
        public int TotalCheckedInAppointmentHealthConditions { get; set; }
        public int TotalProcessedAppointmentHealthConditions { get; set; }
        public int TotalPaidAppointmentHealthConditions { get; set; }
        public int TotalCompletedAppointmentHealthConditions { get; set; }
        public int TotalCancelledAppointmentHealthConditions { get; set; }
        public int TotalRejectedAppointmentHealthConditions { get; set; }
        public int TotalAppointmentsToday { get; set; }
        public int TotalAppointmentsThisWeek { get; set; }
        public int TotalAppointmentsThisMonth { get; set; }
        public int TotalAppointmentsThisYear { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
