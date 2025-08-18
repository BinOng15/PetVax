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
    public class ServiceHistoryBackGroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        private readonly TimeSpan _period = TimeSpan.FromSeconds(3);
        private readonly ILogger<ServiceHistoryBackGroundService> _logger;

        public ServiceHistoryBackGroundService(
            IServiceProvider serviceProvider,
            ILogger<ServiceHistoryBackGroundService> logger)
        {
            _serviceProvider = serviceProvider;

            _logger = logger;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var timer = new PeriodicTimer(_period);
            _logger.LogInformation("Service History Background Service is starting.");

            while (!stoppingToken.IsCancellationRequested && await timer.WaitForNextTickAsync(stoppingToken))
            {
                _logger.LogInformation("Running Service History create at {time}", DateTimeHelper.Now());
                using var scope = _serviceProvider.CreateScope();
                var vetScheduleService = scope.ServiceProvider.GetRequiredService<IServiceHistoryRepository>();

                await vetScheduleService.CreateServiceHistoryAsync(stoppingToken);
                _logger.LogInformation("Completed service history create");
            }
        }
    }

}
