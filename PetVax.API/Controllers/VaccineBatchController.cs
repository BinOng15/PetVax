using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetVax.BusinessObjects.DTO;
using PetVax.BusinessObjects.DTO.VaccineBatchDTO;
using PetVax.Services.IService;

namespace PediVax.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VaccineBatchController : ControllerBase
    {
        private readonly IVaccineBatchService _vaccineBatchService;
        private readonly ILogger<VaccineBatchController> _logger;

        public VaccineBatchController(IVaccineBatchService vaccineBatchService, ILogger<VaccineBatchController> logger)
        {
            _vaccineBatchService = vaccineBatchService;
            _logger = logger;
        }
        [HttpGet("get-all-vaccine-batches")]
        [Authorize(Roles = "Admin, Staff, Vet")]
        public async Task<IActionResult> GetAllVaccineBatches([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string? keyWord = null, CancellationToken cancellationToken = default)
        {
            var request = new GetAllItemsDTO
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                KeyWord = keyWord
            };
            var response = await _vaccineBatchService.GetAllVaccineBatchsAsync(request, cancellationToken);
            return StatusCode(response.Code, response);
        }
        [HttpGet("get-vaccine-batch-by-id/{vaccineBatchId}")]
        [Authorize(Roles = "Admin, Staff, Vet")]
        public async Task<IActionResult> GetVaccineBatchById(int vaccineBatchId, CancellationToken cancellationToken = default)
        {
            var response = await _vaccineBatchService.GetVaccineBatchByIdAsync(vaccineBatchId, cancellationToken);
            return StatusCode(response.Code, response);
        }
        [HttpGet("get-vaccine-batch-by-vaccine-code/{vaccineCode}")]
        [Authorize(Roles = "Admin, Staff, Vet")]
        public async Task<IActionResult> GetVaccineBatchByVaccineCode(string vaccineCode, CancellationToken cancellationToken = default)
        {
            var response = await _vaccineBatchService.GetVaccineBatchByVaccineCodeAsync(vaccineCode, cancellationToken);
            return StatusCode(response.Code, response);
        }
        [HttpGet("get-vaccine-batch-by-vaccine-id/{vaccineId}")]
        [Authorize(Roles = "Admin, Staff, Vet")]
        public async Task<IActionResult> GetVaccineBatchByVaccineId(int vaccineId, CancellationToken cancellationToken = default)
        {
            var response = await _vaccineBatchService.GetVaccineBatchByVaccineIdAsync(vaccineId, cancellationToken);
            return StatusCode(response.Code, response);
        }
        [HttpPost("create-vaccine-batch")]
        [Authorize(Roles = "Admin, Staff")]
        public async Task<IActionResult> CreateVaccineBatch([FromBody] CreateVaccineBatchDTO createVaccineBatchDTO, CancellationToken cancellationToken = default)
        {
            var response = await _vaccineBatchService.CreateVaccineBatchAsync(createVaccineBatchDTO, cancellationToken);
            return StatusCode(response.Code, response);
        }
        [HttpPut("update-vaccine-batch/{vaccineBatchId}")]
        [Authorize(Roles = "Admin, Staff")]
        public async Task<IActionResult> UpdateVaccineBatch(int vaccineBatchId, [FromBody] UpdateVaccineBatchDTO updateVaccineBatchDTO, CancellationToken cancellationToken = default)
        {
            var response = await _vaccineBatchService.UpdateVaccineBatchAsync(vaccineBatchId, updateVaccineBatchDTO, cancellationToken);
            return StatusCode(response.Code, response);
        }
        [HttpDelete("delete-vaccine-batch/{vaccineBatchId}")]
        [Authorize(Roles = "Admin, Staff")]
        public async Task<IActionResult> DeleteVaccineBatch(int vaccineBatchId, CancellationToken cancellationToken = default)
        {
            var response = await _vaccineBatchService.DeleteVaccineBatchAsync(vaccineBatchId, cancellationToken);
            return StatusCode(response.Code, response);
        }
    }
}
