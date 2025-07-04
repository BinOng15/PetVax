using PetVax.Services.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetVax.Services.Service
{
    public class EmailService : IEmailService
    {
        public async Task SendEmailAsync(string to, string subject, string body)
        {
            // Here you would implement the logic to send an email.
            // This is a placeholder implementation.
            await Task.Run(() =>
            {
                Console.WriteLine($"Sending email to: {to}");
                Console.WriteLine($"Subject: {subject}");
                Console.WriteLine($"Body: {body}");
            });
        }
        public async Task SendAppointmentCancellationEmailAsync(string to, string customerName, DateTime appointmentDate)
        {
            string subject = "Appointment Cancellation Notice";
            string body = $"Dear {customerName},\n\nWe regret to inform you that your appointment scheduled for {appointmentDate:dddd, MMMM dd, yyyy 'at' HH:mm} has been canceled. Please contact us to reschedule or for further assistance.\n\nThank you for your understanding.\n\nBest regards,\nPetVax Team";

            await SendEmailAsync(to, subject, body);
        }
    }
}
