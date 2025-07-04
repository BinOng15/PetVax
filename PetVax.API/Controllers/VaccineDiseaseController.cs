using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetVax.BusinessObjects.DTO;
using PetVax.BusinessObjects.DTO.VaccineDiseaseDTO;
using PetVax.Services.IService;

namespace PediVax.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VaccineDiseaseController : ControllerBase
    {
        private readonly IVaccineDiseaseService _vaccineDiseaseService;
        private readonly ILogger<VaccineDiseaseController> _logger;

        public VaccineDiseaseController(IVaccineDiseaseService vaccineDiseaseService, ILogger<VaccineDiseaseController> logger)
        {
            _vaccineDiseaseService = vaccineDiseaseService;
            _logger = logger;
        }

        [HttpGet("get-all-vaccine-diseases")]
        [Authorize(Roles = "Admin, Staff, Vet")]
        public async Task<IActionResult> GetAllVaccineDiseases([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string? keyWord = null, CancellationToken cancellationToken = default)
        {
            var request = new GetAllItemsDTO
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                KeyWord = keyWord
            };
            var response = await _vaccineDiseaseService.GetAllVaccineDiseaseAsync(request, cancellationToken);
            return StatusCode(response.Code, response);
        }
        [HttpGet("get-vaccine-disease-by-id/{vaccineDiseaseId}")]
        [Authorize(Roles = "Admin, Staff, Vet")]
        public async Task<IActionResult> GetVaccineDiseaseById(int vaccineDiseaseId, CancellationToken cancellationToken)
        {
            var response = await _vaccineDiseaseService.GetVaccineDiseaseByIdAsync(vaccineDiseaseId, cancellationToken);
            return StatusCode(response.Code, response);
        }
        [HttpPost("create-vaccine-disease")]
        [Authorize(Roles = "Admin, Staff")]
        public async Task<IActionResult> CreateVaccineDisease([FromForm] CreateVaccineDiseaseDTO createVaccineDiseaseDTO, CancellationToken cancellationToken)
        {
            var response = await _vaccineDiseaseService.CreateVaccineDiseaseAsync(createVaccineDiseaseDTO, cancellationToken);
            return StatusCode(response.Code, response);
        }
        [HttpPut("update-vaccine-disease/{vaccineDiseaseId}")]
        [Authorize(Roles = "Admin, Staff")]
        public async Task<IActionResult> UpdateVaccineDisease([FromForm] UpdateVaccineDiseaseDTO updateVaccineDiseaseDTO, int vaccineDiseaseId, CancellationToken cancellationToken)
        {
            var response = await _vaccineDiseaseService.UpdateVaccineDiseaseAsync(vaccineDiseaseId, updateVaccineDiseaseDTO, cancellationToken);
            return StatusCode(response.Code, response);
        }
        [HttpDelete("delete-vaccine-disease/{vaccineDiseaseId}")]
        [Authorize(Roles = "Admin, Staff")]
        public async Task<IActionResult> DeleteVaccineDisease(int vaccineDiseaseId, CancellationToken cancellationToken)
        {
            var response = await _vaccineDiseaseService.DeleteVaccineDiseaseAsync(vaccineDiseaseId, cancellationToken);
            return StatusCode(response.Code, response);
        }
        [HttpGet("get-vaccine-disease-by-vaccine-id/{vaccineId}")]
        [Authorize(Roles = "Admin, Staff, Vet")]
        public async Task<IActionResult> GetVaccineDiseaseByVaccineId(int vaccineId, CancellationToken cancellationToken)
        {
            var response = await _vaccineDiseaseService.GetVaccineDiseaseByVaccineIdAsync(vaccineId, cancellationToken);
            return StatusCode(response.Code, response);
        }
        [HttpGet("get-vaccine-disease-by-disease-id/{diseaseId}")]
        [Authorize(Roles = "Admin, Staff, Vet")]
        public async Task<IActionResult> GetVaccineDiseaseByDiseaseId(int diseaseId, CancellationToken cancellationToken)
        {
            var response = await _vaccineDiseaseService.GetVaccineDiseaseByDiseaseIdAsync(diseaseId, cancellationToken);
            return StatusCode(response.Code, response);
        }
    }
}
