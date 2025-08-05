using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetVax.BusinessObjects.DTO;
using PetVax.BusinessObjects.DTO.VaccineReceipDetailDTO;
using PetVax.Services.IService;
using PetVax.Services.Service;

namespace PediVax.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VaccineReceiptDetailController : ControllerBase
    {
        private readonly IVaccineReceiptDetailService _vaccineReceiptDetailService;

        public VaccineReceiptDetailController(IVaccineReceiptDetailService vaccineReceiptDetailService)
        {
            _vaccineReceiptDetailService = vaccineReceiptDetailService;
        }
        [HttpGet("get-all-receipt-details")]
        [Authorize(Roles = "Admin, Staff, Vet")]
        public async Task<IActionResult> GetAllReceiptDetails([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string? keyWord = null, [FromQuery] bool? status = true, CancellationToken cancellationToken = default)
        {
            var getAllItemsDTO = new GetAllItemsDTO
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                KeyWord = keyWord,
                Status = status
            };
            var response = await _vaccineReceiptDetailService.GetAllVaccineReceiptDetailsAsync(getAllItemsDTO, cancellationToken);
            return StatusCode(response.Code, response);
        }
        [HttpGet("get-receipt-detail-by-id/{vaccineReceiptDetailId}")]
        [Authorize(Roles = "Admin, Staff, Vet")]
        public async Task<IActionResult> GetReceiptDetailById(int vaccineReceiptDetailId, CancellationToken cancellationToken = default)
        {
            var response = await _vaccineReceiptDetailService.GetVaccineReceiptDetailByIdAsync(vaccineReceiptDetailId, cancellationToken);
            return StatusCode(response.Code, response);
        }
        [HttpGet("get-receipt-details-by-receipt-id/{vaccineReceiptId}")]
        [Authorize(Roles = "Admin, Staff, Vet")]
        public async Task<IActionResult> GetReceiptDetailsByReceiptId(int vaccineReceiptId, CancellationToken cancellationToken = default)
        {
            var response = await _vaccineReceiptDetailService.GetVaccineReceiptDetailsByVaccineReceiptIdAsync(vaccineReceiptId, cancellationToken);
            return StatusCode(response.Code, response);
        }
        [HttpGet("get-receipt-details-by-vaccine-batch-id/{vaccineBatchId}")]
        [Authorize(Roles = "Admin, Staff, Vet")]
        public async Task<IActionResult> GetReceiptDetailsByVaccineBatchId(int vaccineBatchId, CancellationToken cancellationToken = default)
        {
            var response = await _vaccineReceiptDetailService.GetVaccineReceiptDetailByVaccineBatchId(vaccineBatchId, cancellationToken);
            return StatusCode(response.Code, response);
        }
        [HttpPost("create-receipt-detail")]
        [Authorize(Roles = "Admin, Staff")]
        public async Task<IActionResult> CreateReceiptDetail([FromBody] CreateVaccineReceiptDetailDTO vaccineReceiptDetailDTO, CancellationToken cancellationToken = default)
        {
            var response = await _vaccineReceiptDetailService.CreateVaccineReceiptDetailAsync(vaccineReceiptDetailDTO, cancellationToken);
            return StatusCode(response.Code, response);
        }
        [HttpPut("update-receipt-detail/{vaccineReceiptDetailId}")]
        [Authorize(Roles = "Admin, Staff")]
        public async Task<IActionResult> UpdateReceiptDetail(int vaccineReceiptDetailId, [FromBody] UpdateVaccineReceiptDetailDTO vaccineReceiptDetailDTO, CancellationToken cancellationToken = default)
        {
            var response = await _vaccineReceiptDetailService.UpdateVaccineReceiptDetailAsync(vaccineReceiptDetailId, vaccineReceiptDetailDTO, cancellationToken);
            return StatusCode(response.Code, response);
        }
        [HttpDelete("delete-receipt-detail/{vaccineReceiptDetailId}")]
        [Authorize(Roles = "Admin, Staff")]
        public async Task<IActionResult> DeleteReceiptDetail(int vaccineReceiptDetailId, CancellationToken cancellationToken = default)
        {
            var response = await _vaccineReceiptDetailService.DeleteVaccineReceiptDetailAsync(vaccineReceiptDetailId, cancellationToken);
            return StatusCode(response.Code, response);
        }
    }
}
