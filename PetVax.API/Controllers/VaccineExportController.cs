using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetVax.BusinessObjects.DTO;
using PetVax.BusinessObjects.DTO.VaccineExportDTO;
using PetVax.Services.IService;

namespace PediVax.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VaccineExportController : ControllerBase
    {
        private readonly IVaccineExportService _vaccineExportService;

        public VaccineExportController(IVaccineExportService vaccineExportService)
        {
            _vaccineExportService = vaccineExportService;
        }

        [HttpGet("get-all-vaccine-exports")]
        [Authorize(Roles = "Admin, Staff")]
        public async Task<IActionResult> GetAllVaccineExports([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string? keyword = null, [FromQuery] bool? status = true, CancellationToken cancellationToken = default)
        {
            var getAllItemsDTO = new GetAllItemsDTO
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                KeyWord = keyword,
                Status = status
            };
            var response = await _vaccineExportService.GetAllVaccineExportsAsync(getAllItemsDTO, cancellationToken);
            return StatusCode(response.Code, response);
        }
        [HttpGet("get-vaccine-export-by-id/{vaccineExportId}")]
        [Authorize(Roles = "Admin, Staff, Vet")]
        public async Task<IActionResult> GetVaccineExportById(int vaccineExportId, CancellationToken cancellationToken)
        {
            var response = await _vaccineExportService.GetVaccineExportByIdAsync(vaccineExportId, cancellationToken);
            return StatusCode(response.Code, response);
        }
        [HttpGet("get-vaccine-export-by-export-code/{exportCode}")]
        [Authorize(Roles = "Admin, Staff, Vet")]
        public async Task<IActionResult> GetVaccineExportByExportCode(string exportCode, CancellationToken cancellationToken)
        {
            var response = await _vaccineExportService.GetVaccineExportByExportCodeAsync(exportCode, cancellationToken);
            return StatusCode(response.Code, response);
        }
        [HttpPost("create-vaccine-export")]
        [Authorize(Roles = "Admin, Staff")]
        public async Task<IActionResult> CreateVaccineExport([FromBody] CreateVaccineExportDTO createVaccineExportDTO, CancellationToken cancellationToken)
        {
            var response = await _vaccineExportService.CreateVaccineExportAsync(createVaccineExportDTO, cancellationToken);
            return StatusCode(response.Code, response);
        }
        [HttpPut("update-vaccine-export/{exportId}")]
        [Authorize(Roles = "Admin, Staff")]
        public async Task<IActionResult> UpdateVaccineExport(int exportId, [FromBody] UpdateVaccineExportDTO updateVaccineExportDTO, CancellationToken cancellationToken)
        {
            var response = await _vaccineExportService.UpdateVaccineExportAsync(exportId, updateVaccineExportDTO, cancellationToken);
            return StatusCode(response.Code, response);
        }
        [HttpDelete("delete-vaccine-export/{vaccineExportId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteVaccineExport(int vaccineExportId, CancellationToken cancellationToken)
        {
            var response = await _vaccineExportService.DeleteVaccineExportAsync(vaccineExportId, cancellationToken);
            return StatusCode(response.Code, response);
        }
    }
}
