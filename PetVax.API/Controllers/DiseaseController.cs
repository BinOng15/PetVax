using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetVax.BusinessObjects.DTO;
using PetVax.BusinessObjects.DTO.DiseaseDTO;
using PetVax.Services.IService;

namespace PediVax.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiseaseController : ControllerBase
    {
        private readonly IDiseaseService _diseaseService;
        private readonly ILogger<DiseaseController> _logger;

        public DiseaseController(IDiseaseService diseaseService, ILogger<DiseaseController> logger)
        {
            _diseaseService = diseaseService;
            _logger = logger;
        }

        [HttpGet("get-all-diseases")]
        public async Task<IActionResult> GetAllDiseases([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string? keyWord = null, CancellationToken cancellationToken = default)
        {
            var request = new GetAllItemsDTO
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                KeyWord = keyWord
            };
            var response = await _diseaseService.GetAllDiseaseAsync(request, cancellationToken);
            return StatusCode(response.Code, response);
        }
        [HttpGet("get-disease-by-id/{diseaseId}")]
        public async Task<IActionResult> GetDiseaseById(int diseaseId, CancellationToken cancellationToken)
        {
            var response = await _diseaseService.GetDiseaseByIdAsync(diseaseId, cancellationToken);
            return StatusCode(response.Code, response);
        }
        [HttpGet("get-disease-by-name/{name}")]
        public async Task<IActionResult> GetDiseaseByName(string name, CancellationToken cancellationToken)
        {
            var response = await _diseaseService.GetDiseaseByNameAsync(name, cancellationToken);
            return StatusCode(response.Code, response);
        }
        [HttpPost("create-disease")]
        public async Task<IActionResult> CreateDisease([FromForm] CreateDiseaseDTO createDiseaseDTO, CancellationToken cancellationToken)
        {
            var response = await _diseaseService.CreateDiseaseAsync(createDiseaseDTO, cancellationToken);
            return StatusCode(response.Code, response);
        }
        [HttpPut("update-disease/{diseaseId}")]
        public async Task<IActionResult> UpdateDisease([FromForm] UpdateDiseaseDTO updateDiseaseDTO, int diseaseId, CancellationToken cancellationToken)
        {
            var response = await _diseaseService.UpdateDiseaseAsync(diseaseId, updateDiseaseDTO, cancellationToken);
            return StatusCode(response.Code, response);
        }
        [HttpDelete("delete-disease/{diseaseId}")]
        public async Task<IActionResult> DeleteDisease(int diseaseId, CancellationToken cancellationToken)
        {
            var response = await _diseaseService.DeleteDiseaseAsync(diseaseId, cancellationToken);
            return StatusCode(response.Code, response);
        }
        [HttpGet("get-disease-by-vaccine-id/{vaccineId}")]
        public async Task<IActionResult> GetDiseaseByVaccineId(int vaccineId, CancellationToken cancellationToken)
        {
            var response = await _diseaseService.GetDiseaseByVaccineIdAsync(vaccineId, cancellationToken);
            return StatusCode(response.Code, response);
        }
    }
}
