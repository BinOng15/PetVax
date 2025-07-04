using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetVax.BusinessObjects.DTO;
using PetVax.BusinessObjects.DTO.VaccineDTO;
using PetVax.Services.IService;

namespace PediVax.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VaccineController : ControllerBase
    {
        private readonly IVaccineService _vaccineService;
        private readonly ILogger<VaccineController> _logger;

        public VaccineController(IVaccineService vaccineService, ILogger<VaccineController> logger)
        {
            _vaccineService = vaccineService;
            _logger = logger;
        }

        [HttpGet("get-all-vaccines")]
        [Authorize(Roles = "Admin, Staff, Vet")]
        public async Task<IActionResult> GetAllVaccines([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string? keyWord = null, CancellationToken cancellationToken = default)
        {
            var request = new GetAllItemsDTO
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                KeyWord = keyWord
            };
            var response = await _vaccineService.GetAllVaccineAsync(request, cancellationToken);
            return StatusCode(response.Code, response);
        }
        [HttpGet("get-vaccine-by-id/{vaccineId}")]
        [Authorize(Roles = "Admin, Staff, Vet")]
        public async Task<IActionResult> GetVaccineById(int vaccineId, CancellationToken cancellationToken)
        {
            var response = await _vaccineService.GetVaccineByIdAsync(vaccineId, cancellationToken);
            return StatusCode(response.Code, response);
        }
        [HttpGet("get-vaccine-by-code/{vaccineCode}")]
        [Authorize(Roles = "Admin, Staff, Vet")]
        public async Task<IActionResult> GetVaccineByCode(string vaccineCode, CancellationToken cancellationToken)
        {
            var response = await _vaccineService.GetVaccineByVaccineCodeAsync(vaccineCode, cancellationToken);
            return StatusCode(response.Code, response);
        }
        [HttpGet("get-vaccine-by-name/{name}")]
        [Authorize(Roles = "Admin, Staff, Vet")]
        public async Task<IActionResult> GetVaccineByName(string name, CancellationToken cancellationToken)
        {
            var response = await _vaccineService.GetVaccineByNameAsync(name, cancellationToken);
            return StatusCode(response.Code, response);
        }
        [HttpPost("create-vaccine")]
        [Authorize(Roles = "Admin, Staff")]
        public async Task<IActionResult> CreateVaccine([FromForm] CreateVaccineDTO createVaccineDTO, CancellationToken cancellationToken)
        {
            var response = await _vaccineService.CreateVaccineAsync(createVaccineDTO, cancellationToken);
            return StatusCode(response.Code, response);
        }
        [HttpPut("update-vaccine/{vaccineId}")]
        [Authorize(Roles = "Admin, Staff")]
        public async Task<IActionResult> UpdateVaccine([FromForm] UpdateVaccineDTO updateVaccineDTO, int vaccineId, CancellationToken cancellationToken)
        {
            var response = await _vaccineService.UpdateVaccineAsync(vaccineId, updateVaccineDTO, cancellationToken);
            return StatusCode(response.Code, response);
        }
        [HttpDelete("delete-vaccine/{vaccineId}")]
        [Authorize(Roles = "Admin, Staff")]
        public async Task<IActionResult> DeleteVaccine(int vaccineId, CancellationToken cancellationToken)
        {
            var response = await _vaccineService.DeleteVaccineAsync(vaccineId, cancellationToken);
            return StatusCode(response.Code, response);
        }
        [HttpGet("get-vaccine-by-disease-id/{diseaseId}")]
        [Authorize(Roles = "Admin, Staff, Vet")]
        public async Task<IActionResult> GetVaccineByDiseaseId(int diseaseId, CancellationToken cancellationToken)
        {
            var response = await _vaccineService.GetVaccineByDiseaseIdAsync(diseaseId, cancellationToken);
            return StatusCode(response.Code, response);
        }
    }
}
