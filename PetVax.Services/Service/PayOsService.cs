using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Net.payOS;
using Net.payOS.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetVax.Services.Service
{
    public class PayOsService
    {
        private readonly IConfiguration _configuration;
        private readonly IConfigurationSection _payOsConfig;
        private readonly ILogger<PayOsService> _logger;

        public PayOsService(IConfiguration configuration, ILogger<PayOsService> logger)
        {
            _configuration = configuration;
            _logger = logger;
            _payOsConfig = _configuration.GetSection("PayOs");
        }
        public async Task<CreatePaymentResult> CreatePaymentLink(PaymentData paymentData)
        {
            var client_id = _payOsConfig.GetSection("ClientId").Value;
            var api_key = _payOsConfig.GetSection("ApiKey").Value;
            var checkSum_key = _payOsConfig.GetSection("CheckSumKey").Value;

            PayOS payOs = new PayOS(client_id, api_key, checkSum_key);

            var result = await payOs.createPaymentLink(paymentData);
            _logger.LogInformation("Link: {link}", paymentData);
            return result;
        }
        public async Task<PaymentLinkInformation> GetPaymentLinkInformation(int id)
        {
            var client_id = _payOsConfig.GetSection("ClientId").Value;
            var api_key = _payOsConfig.GetSection("ApiKey").Value;
            var checkSum_key = _payOsConfig.GetSection("CheckSumKey").Value;

            PayOS payOS = new PayOS(client_id, api_key, checkSum_key);
            PaymentLinkInformation paymentLinkInformation = await payOS.getPaymentLinkInformation(id);
            return paymentLinkInformation;
        }
        public async Task<PaymentLinkInformation> CancelPaymentLink(int id, string reason)
        {
            var client_id = _payOsConfig.GetSection("ClientId").Value;
            var api_key = _payOsConfig.GetSection("ApiKey").Value;
            var checkSum_key = _payOsConfig.GetSection("CheckSumKey").Value;

            PayOS payOS = new PayOS(client_id, api_key, checkSum_key);

            PaymentLinkInformation cancelledPaymentLinkInfo = await payOS.cancelPaymentLink(id, reason);
            return cancelledPaymentLinkInfo;
        }
        public async Task<string> ConfirmWebhook(string url)
        {
            var client_id = _payOsConfig.GetSection("ClientId").Value;
            var api_key = _payOsConfig.GetSection("ApiKey").Value;
            var checkSum_key = _payOsConfig.GetSection("CheckSumKey").Value;

            PayOS payOS = new PayOS(client_id, api_key, checkSum_key);
            return await payOS.confirmWebhook(url);
        }
        public WebhookData VerifyPaymentWebhookData(WebhookType webhookType)
        {
            var client_id = _payOsConfig.GetSection("ClientId").Value;
            var api_key = _payOsConfig.GetSection("ApiKey").Value;
            var checkSum_key = _payOsConfig.GetSection("CheckSumKey").Value;

            PayOS payOS = new PayOS(client_id, api_key, checkSum_key);
            WebhookData webhookData = payOS.verifyPaymentWebhookData(webhookType);
            return webhookData;
        }
    }
}
