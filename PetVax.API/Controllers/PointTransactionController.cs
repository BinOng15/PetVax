using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetVax.BusinessObjects.DTO;
using PetVax.Services.IService;
using PetVax.Services.Service;

namespace PediVax.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PointTransactionController : ControllerBase
    {
        private readonly IPointTransactionService _pointTransactionService;

        public PointTransactionController(IPointTransactionService pointTransactionService)
        {
            _pointTransactionService = pointTransactionService;
        }

        [HttpGet("get-all-point-transactions")]
        public async Task<IActionResult> GetAllPointTransactionsAsync([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string? keyWord = null, [FromQuery] bool? status = true, CancellationToken cancellationToken = default)
        {
            var getAllItemsDTO = new GetAllItemsDTO
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                KeyWord = keyWord,
                Status = status
            };
            var response = await _pointTransactionService.GetAllPointTransactionsAsync(getAllItemsDTO, cancellationToken);
            return StatusCode(response.Code, response);
        }
        [HttpGet("get-point-transaction-by-id/{pointTransactionId}")]
        public async Task<IActionResult> GetPointTransactionByIdAsync(int pointTransactionId, CancellationToken cancellationToken = default)
        {
            var response = await _pointTransactionService.GetPointTransactionByIdAsync(pointTransactionId, cancellationToken);
            return StatusCode(response.Code, response);
        }
        [HttpGet("get-point-transaction-by-customer-id/{customerId}")]
        public async Task<IActionResult> GetPointTransactionByCustomerIdAsync(int customerId, CancellationToken cancellationToken = default)
        {
            var response = await _pointTransactionService.GetPointTransactionByCustomerIdAsync(customerId, cancellationToken);
            return StatusCode(response.Code, response);
        }
    }
}
