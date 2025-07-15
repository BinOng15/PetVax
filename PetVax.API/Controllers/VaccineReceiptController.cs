using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetVax.BusinessObjects.DTO;
using PetVax.BusinessObjects.DTO.VaccineReceiptDTO;
using PetVax.Services.IService;

namespace PediVax.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VaccineReceiptController : ControllerBase
    {
        private readonly IVaccineReceiptService _vaccineReceiptService;

        public VaccineReceiptController(IVaccineReceiptService vaccineReceiptService)
        {
            _vaccineReceiptService = vaccineReceiptService;
        }
        [HttpGet("get-all-vaccine-receipts")]
        public async Task<IActionResult> GetAllVaccineReceipts([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string? keyWord = null, [FromQuery] bool? status = true, CancellationToken cancellationToken = default)
        {
            var getAllItemsDTO = new GetAllItemsDTO
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                KeyWord = keyWord,
                Status = status
            };
            var response = await _vaccineReceiptService.GetAllVaccineReceiptsAsync(getAllItemsDTO, cancellationToken);
            return StatusCode(response.Code, response);
        }
        [HttpGet("get-vaccine-receipt-by-id/{vaccineReceiptId}")]
        public async Task<IActionResult> GetVaccineReceiptById(int vaccineReceiptId, CancellationToken cancellationToken = default)
        {
            var response = await _vaccineReceiptService.GetVaccineReceiptByIdAsync(vaccineReceiptId, cancellationToken);
            return StatusCode(response.Code, response);
        }
        [HttpGet("get-vaccine-receipt-by-receipt-code/{receiptCode}")]
        public async Task<IActionResult> GetVaccineReceiptByReceiptCode(string receiptCode, CancellationToken cancellationToken = default)
        {
            var response = await _vaccineReceiptService.GetVaccineReceiptByReceiptCodeAsync(receiptCode, cancellationToken);
            return StatusCode(response.Code, response);
        }
        [HttpPost("create-vaccine-receipt")]
        public async Task<IActionResult> CreateVaccineReceipt([FromBody] CreateVaccineReceiptDTO createVaccineReceiptDTO, CancellationToken cancellationToken = default)
        {
            var response = await _vaccineReceiptService.CreateVaccineReceiptAsync(createVaccineReceiptDTO, cancellationToken);
            return StatusCode(response.Code, response);
        }
        [HttpPut("update-vaccine-receipt/{receiptId}")]
        public async Task<IActionResult> UpdateVaccineReceipt(int receiptId, [FromBody] UpdateVaccineReceiptDTO updateVaccineReceiptDTO, CancellationToken cancellationToken = default)
        {
            var response = await _vaccineReceiptService.UpdateVaccineReceiptAsync(receiptId, updateVaccineReceiptDTO, cancellationToken);
            return StatusCode(response.Code, response);
        }
        [HttpDelete("delete-vaccine-receipt/{vaccineReceiptId}")]
        public async Task<IActionResult> DeleteVaccineReceipt(int vaccineReceiptId, CancellationToken cancellationToken = default)
        {
            var response = await _vaccineReceiptService.DeleteVaccineReceiptAsync(vaccineReceiptId, cancellationToken);
            return StatusCode(response.Code, response);
        }
    }
}
