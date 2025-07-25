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
    public class CustomerVoucherBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly TimeSpan _interval = TimeSpan.FromMinutes(60);
        private readonly ILogger<CustomerVoucherBackgroundService> _logger;

        public CustomerVoucherBackgroundService(IServiceProvider serviceProvider, ILogger<CustomerVoucherBackgroundService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using var timer = new PeriodicTimer(_interval);
            _logger.LogInformation("CustomerVoucherBackgroundService started.");

            while (!stoppingToken.IsCancellationRequested && await timer.WaitForNextTickAsync(stoppingToken))
            {
                _logger.LogInformation("CustomerVoucherBackgroundService is running at: {time}", DateTimeHelper.Now());
                using var scope = _serviceProvider.CreateScope();
                var customerVoucherService = scope.ServiceProvider.GetRequiredService<ICustomerVoucherRepository>();

                await customerVoucherService.UpdateExpiredCustomerVoucherAsync(stoppingToken);
                _logger.LogInformation("Customer vouchers updated at: {time}", DateTimeHelper.Now());
            }
        }
    }
}
