using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetVax.BusinessObjects.DTO;
using PetVax.Services.IService;
using PetVax.Services.Service;

namespace PediVax.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerVoucherController : ControllerBase
    {
        private readonly ICustomerVoucherService _customerVoucherService;

        public CustomerVoucherController(ICustomerVoucherService customerVoucherService)
        {
            _customerVoucherService = customerVoucherService;
        }

        [HttpGet("get-all-customer-vouchers")]
        public async Task<IActionResult> GetAllCustomerVouchersAsync([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string? keyWord = null, [FromQuery] bool? status = true, CancellationToken cancellationToken = default)
        {
            var getAllItemsDTO = new GetAllItemsDTO
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                KeyWord = keyWord,
                Status = status
            };
            var response = await _customerVoucherService.GetAllCustomerVouchersAsync(getAllItemsDTO, cancellationToken);
            return StatusCode(response.Code, response);
        }
        [HttpGet("get-customer-voucher-by-id/{customerVoucherId}")]
        public async Task<IActionResult> GetCustomerVoucherByIdAsync(int customerVoucherId, CancellationToken cancellationToken = default)
        {
            var response = await _customerVoucherService.GetCustomerVoucherByIdAsync(customerVoucherId, cancellationToken);
            return StatusCode(response.Code, response);
        }
        [HttpGet("get-customer-voucher-by-customer-id-and-voucher-id/{customerId}/{voucherId}")]
        public async Task<IActionResult> GetCustomerVoucherByCustomerIdAndVoucherIdAsync(int customerId, int voucherId, CancellationToken cancellationToken = default)
        {
            var response = await _customerVoucherService.GetCustomerVoucherByCustomerIdAndVoucherIdAsync(customerId, voucherId, cancellationToken);
            return StatusCode(response.Code, response);
        }
        [HttpGet("get-customer-vouchers-by-customer-id/{customerId}")]
        public async Task<IActionResult> GetCustomerVouchersByCustomerIdAsync(int customerId, CancellationToken cancellationToken = default)
        {
            var response = await _customerVoucherService.GetCustomerVouchersByCustomerIdAsync(customerId, cancellationToken);
            return StatusCode(response.Code, response);
        }
        [HttpGet("get-customer-voucher-by-voucher-id/{voucherId}")]
        public async Task<IActionResult> GetCustomerVoucherByVoucherIdAsync(int voucherId, CancellationToken cancellationToken = default)
        {
            var response = await _customerVoucherService.GetCustomerVoucherByVoucherIdAsync(voucherId, cancellationToken);
            return StatusCode(response.Code, response);
        }
    }
}
