using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PetVax.BusinessObjects.Helpers;
using PetVax.Repositories.IRepository;
using PetVax.Services.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace PetVax.Services.Service
{
    public class AppointmentReminderBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<AppointmentReminderBackgroundService> _logger;

        public AppointmentReminderBackgroundService(
        IServiceProvider serviceProvider,
        ILogger<AppointmentReminderBackgroundService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var now = DateTimeHelper.Now();
                    var nextDay = now.AddDays(1);

                    var _appointmentRepository = _serviceProvider.GetRequiredService<IAppointmentRepository>();
                    var _petRepository = _serviceProvider.GetRequiredService<IPetRepository>();
                    var _configuration = _serviceProvider.GetRequiredService<IConfiguration>();

                    // Lấy các cuộc hẹn sẽ diễn ra trong 24h tới
                    var upcomingAppointments = await _appointmentRepository.GetAppointmentsByDateRangeAsync(now, nextDay, stoppingToken);
                    _logger.LogInformation("Found {count} upcoming appointments for reminder", upcomingAppointments?.Count ?? 0);

                    foreach (var appointment in upcomingAppointments)
                    {
                        try
                        {
                            var pet = await _petRepository.GetPetByIdAsync(appointment.PetId, stoppingToken);
                            var customerEmail = pet?.Customer?.Account?.Email;
                            var customerName = pet?.Customer?.FullName ?? "Quý khách hàng";
                            var petName = pet?.Name ?? "thú cưng của bạn";

                            if (!string.IsNullOrEmpty(customerEmail))
                            {
                                // Prepare SMTP configuration
                                var smtpHost = _configuration["Smtp:Host"];
                                var smtpPort = int.Parse(_configuration["Smtp:Port"]);
                                var smtpUser = _configuration["Smtp:User"];
                                var smtpPass = _configuration["Smtp:Pass"];
                                var fromEmail = _configuration["Smtp:From"];

                                using var client = new SmtpClient(smtpHost, smtpPort)
                                {
                                    Credentials = new NetworkCredential(smtpUser, smtpPass),
                                    EnableSsl = true
                                };

                                var mail = new MailMessage(fromEmail, customerEmail)
                                {
                                    Subject = "Nhắc lịch hẹn tiêm phòng/thú cưng",
                                    Body = $@"
                                        <html>
                                        <body style='font-family: Arial, sans-serif; background-color: #f6f6f6; padding: 30px;'>
                                        <div style='max-width: 500px; margin: auto; background: #fff; border-radius: 8px; box-shadow: 0 2px 8px rgba(0,0,0,0.05); padding: 32px;'>
                                        <h2 style='color: #2d8cf0; text-align: center;'>Nhắc nhở lịch hẹn PetVax</h2>
                                        <p style='font-size: 16px; color: #333;'>Gửi {customerName},</p>
                                        <p style='font-size: 16px; color: #333;'>Chúng tôi muốn nhắc nhở bạn về cuộc hẹn tiêm chủng sắp tới cho {petName}:</p>
                                        <div style='text-align: center; margin: 24px 0;'>
                                        <span style='display: inline-block; font-size: 24px; color: #2d8cf0; font-weight: bold; background: #f0f7ff; padding: 16px 32px; border-radius: 6px;'>
                                            Mã lịch hẹn: {appointment.AppointmentCode}
                                        </span>
                                        </div>
                                        <p style='font-size: 16px; color: #333;'>
                                            <b>Ngày giờ:</b> {appointment.AppointmentDate:dd/MM/yyyy HH:mm}<br/>
                                            <b>Trạng thái:</b> Đã xác nhận<br/>
                                        </p>
                                        <p style='font-size: 15px; color: #555;'>
                                            Vui lòng đảm bảo {petName} đã sẵn sàng cho cuộc hẹn. Nếu bạn cần sắp xếp lại lịch hẹn, hãy liên hệ với chúng tôi sớm nhất có thể.
                                        </p>
                                        <p style='font-size: 14px; color: #aaa; margin-top: 32px;'>
                                            Nếu có bất kỳ thắc mắc nào, vui lòng liên hệ với chúng tôi qua <a href='mailto:support@petvax.com' style='color: #2d8cf0;'>support@petvax.com</a> hoặc gọi cho chúng tôi theo số 0987654321.
                                        </p>
                                        <p style='font-size: 14px; color: #aaa;'>Xin cảm ơn,<br/>VaxPet</p>
                                        </div>
                                        </body>
                                        </html>",
                                    IsBodyHtml = true
                                };

                                // Send email
                                await client.SendMailAsync(mail, stoppingToken);
                                _logger.LogInformation("Sent reminder email to {email} for appointment {code}", customerEmail, appointment.AppointmentCode);
                            }
                            else
                            {
                                _logger.LogWarning("No valid email found for appointment {code}", appointment.AppointmentCode);
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Failed to send reminder email for appointment {code}", appointment.AppointmentCode);
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Lỗi khi gửi email nhắc lịch hẹn.");
                }

                // Chờ 1 ngày (hoặc 1 giờ nếu muốn kiểm tra thường xuyên hơn)
                await Task.Delay(TimeSpan.FromHours(12), stoppingToken);
            }
        }
    }
}
