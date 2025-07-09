using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PetVax.BusinessObjects.Helpers;
using PetVax.Repositories.IRepository;
using PetVax.Services.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetVax.Services.Service
{
    public class VetScheduleBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly TimeSpan _period = TimeSpan.FromMinutes(30);
        private readonly ILogger<VetScheduleBackgroundService> _logger;

        public VetScheduleBackgroundService(IServiceProvider serviceProvider, ILogger<VetScheduleBackgroundService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var timer = new PeriodicTimer(_period);
            _logger.LogInformation("Vet Schedule Background Service is starting.");

            while (!stoppingToken.IsCancellationRequested && await timer.WaitForNextTickAsync(stoppingToken))
            {
                _logger.LogInformation("Running vet schedule update at {time}", DateTimeHelper.Now());
                using var scope = _serviceProvider.CreateScope();
                var vetScheduleService = scope.ServiceProvider.GetRequiredService<IVetScheduleRepository>();

                await vetScheduleService.UpdateExpiredVetScheduleAsync(stoppingToken);
                _logger.LogInformation("Completed vet schedules update");
            }
        }
    }
}
