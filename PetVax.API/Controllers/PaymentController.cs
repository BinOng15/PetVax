using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetVax.BusinessObjects.DTO;
using PetVax.BusinessObjects.DTO.PaymentDTO;
using PetVax.Services.IService;
using PetVax.Services.Service;

namespace PediVax.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        [HttpGet("get-all")]
        public async Task<IActionResult> GetAllPaymentsAsync([FromQuery] GetAllItemsDTO getAllItemsRequest, CancellationToken cancellationToken)
        {
            var response = await _paymentService.GetAllPaymentsAsync(getAllItemsRequest, cancellationToken);
            return StatusCode(response.Code, response);
        }
        [HttpGet("get-by-id/{paymentId}")]
        public async Task<IActionResult> GetPaymentByIdAsync(int paymentId, CancellationToken cancellationToken)
        {
            var response = await _paymentService.GetPaymentByIdAsync(paymentId, cancellationToken);
            return StatusCode(response.Code, response);
        }
        [HttpPost("create")]
        public async Task<IActionResult> CreatePaymentAsync([FromBody] CreatePaymentRequestDTO createPaymentRequest, CancellationToken cancellationToken)
        {
            var response = await _paymentService.CreatePaymentAsync(createPaymentRequest, cancellationToken);
            return StatusCode(response.Code, response);
        }
        [HttpPut("update/{paymentId}")]
        public async Task<IActionResult> UpdatePaymentAsync(int paymentId, [FromBody] UpdatePaymentRequestDTO updatePaymentRequest, CancellationToken cancellationToken)
        {
            var response = await _paymentService.UpdatePaymentAsync(paymentId, updatePaymentRequest, cancellationToken);
            return StatusCode(response.Code, response);
        }
        [HttpDelete("delete/{paymentId}")]
        public async Task<IActionResult> DeletePaymentByIdAsync(int paymentId, CancellationToken cancellationToken)
        {
            var response = await _paymentService.DeletePaymentByIdAsync(paymentId, cancellationToken);
            return StatusCode(response.Code, response);
        }
        [HttpGet("get-by-customer-id/{customerId}")]
        public async Task<IActionResult> GetPaymentsByCustomerIdAsync(int customerId, CancellationToken cancellationToken)
        {
            var response = await _paymentService.GetPaymentsByCustomerIdAsync(customerId, cancellationToken);
            return StatusCode(response.Code, response);
        }
        [HttpGet("get-by-appointment-detail-id/{appointmentDetailId}")]
        public async Task<IActionResult> GetPaymentsByAppointmentDetailIdAsync(int appointmentDetailId, CancellationToken cancellationToken)
        {
            var response = await _paymentService.GetPaymentsByAppointmentDetailIdAsync(appointmentDetailId, cancellationToken);
            return StatusCode(response.Code, response);
        }
        [HttpPost("payment-callback")]
        public async Task<IActionResult> HandlePayOsCallBackAsync([FromBody] PaymentCallBackDTO paymentCallBackDTO, CancellationToken cancellationToken)
        {
            var response = await _paymentService.HandlePayOsCallBackAsync(paymentCallBackDTO, cancellationToken);
            return StatusCode(response.Code, response);
        }
    }
}
