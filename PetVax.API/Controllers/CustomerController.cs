using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetVax.BusinessObjects.DTO.CustomerDTO;
using PetVax.Services.IService;

namespace PediVax.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ILogger<CustomerController> _logger;
        private readonly ICustomerService _customerService;

        public CustomerController(ILogger<CustomerController> logger, ICustomerService customerService)
        {
            _logger = logger;
            _customerService = customerService;
        }

        [HttpGet("get-customer-by-id/{customerId}")]
        public async Task<IActionResult> GetCustomerById(int customerId, CancellationToken cancellationToken)
        {
            var response = await _customerService.GetCustomerByIdAsync(customerId, cancellationToken);
            return StatusCode(response.Code, response);
        }
        [HttpGet("get-customer-by-account-id/{accountId}")]
        [Authorize(Roles = "Admin, Customer, Staff")]
        public async Task<IActionResult> GetCustomerByAccountId(int accountId, CancellationToken cancellationToken)
        {
            var response = await _customerService.GetCustomerByAccountIdAsync(accountId, cancellationToken);
            return StatusCode(response.Code, response);
        }
        [HttpGet("get-all-customers")]
        [Authorize(Roles = "Admin, Staff")]
        public async Task<IActionResult> GetAllCustomers([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string? keyWord = null, CancellationToken cancellationToken = default)
        {
            var request = new GetAllCustomerRequestDTO
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                KeyWord = keyWord
            };
            var response = await _customerService.GetAllCustomersAsync(request, cancellationToken);
            return StatusCode(response.Code, response);
        }
        [HttpPut("update-customer/{customerId}")]
        public async Task<IActionResult> UpdateCustomer(int customerId, [FromForm] UpdateCustomerDTO updateCustomerDTO, CancellationToken cancellationToken)
        {
            var response = await _customerService.UpdateCustomerAsync(customerId, updateCustomerDTO, cancellationToken);
            return StatusCode(response.Code, response);
        }
        [HttpDelete("delete-customer/{customerId}")]
        [Authorize(Roles = "Admin, Staff")]
        public async Task<IActionResult> DeleteCustomer(int customerId, CancellationToken cancellationToken)
        {
            var response = await _customerService.DeleteCustomerAsync(customerId, cancellationToken);
            return StatusCode(response.Code, response);

        }
    }
}
