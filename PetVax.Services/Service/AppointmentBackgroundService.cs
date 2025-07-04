using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PetVax.BusinessObjects.Helpers;
using PetVax.Repositories.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetVax.Services.Service
{
    public class AppointmentBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly TimeSpan _period = TimeSpan.FromMinutes(30);
        private readonly ILogger<AppointmentBackgroundService> _logger;

        public AppointmentBackgroundService(IServiceProvider serviceProvider, ILogger<AppointmentBackgroundService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var timer = new PeriodicTimer(_period);
            _logger.LogInformation("Appointment Background Service is starting.");
            while (!stoppingToken.IsCancellationRequested && await timer.WaitForNextTickAsync(stoppingToken))
            {
                _logger.LogInformation("Running appointment update at {time}", DateTimeHelper.Now());
                using var scope = _serviceProvider.CreateScope();
                var appointmentService = scope.ServiceProvider.GetRequiredService<IAppointmentRepository>();

                await appointmentService.UpdateExpiredAppointmentsAsync(stoppingToken);
                _logger.LogInformation("Completed appointment update");
            }
        }
    }
}
