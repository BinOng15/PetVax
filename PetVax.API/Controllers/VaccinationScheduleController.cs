using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetVax.BusinessObjects.DTO;
using PetVax.BusinessObjects.DTO.VaccinationSchedule;
using PetVax.Services.IService;

namespace PediVax.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VaccinationScheduleController : ControllerBase
    {
        private readonly IVaccinationScheduleService _vaccinationScheduleService;

        public VaccinationScheduleController(IVaccinationScheduleService vaccinationScheduleService)
        {
            _vaccinationScheduleService = vaccinationScheduleService;
        }

        [HttpGet("get-all-vaccination-schedules")]
        public async Task<IActionResult> GetAllVaccinationSchedules([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string? keyWord = null, [FromQuery] bool? status = true, CancellationToken cancellationToken = default)
        {
            var request = new GetAllItemsDTO
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                KeyWord = keyWord,
                Status = status,
            };
            var response = await _vaccinationScheduleService.GetAllVaccinationScheduleAsync(request, cancellationToken);
            return StatusCode(response.Code, response);
        }
        [HttpGet("get-vaccination-schedule-by-id/{vaccinationScheduleId}")]
        public async Task<IActionResult> GetVaccinationScheduleById(int vaccinationScheduleId, CancellationToken cancellationToken)
        {
            var response = await _vaccinationScheduleService.GetVaccinationScheduleByIdAsync(vaccinationScheduleId, cancellationToken);
            return StatusCode(response.Code, response);
        }
        [HttpGet("get-vaccination-schedule-by-disease-id/{diseaseId}")]
        public async Task<IActionResult> GetVaccinationScheduleByDiseaseId(int diseaseId, CancellationToken cancellationToken)
        {
            var response = await _vaccinationScheduleService.GetVaccinationScheduleByDiseaseIdAsync(diseaseId, cancellationToken);
            return StatusCode(response.Code, response);
        }
        [HttpPost("create-vaccination-schedule")]
        public async Task<IActionResult> CreateVaccinationSchedule([FromBody] CreateVaccinationScheduleDTO createVaccinationScheduleDTO, CancellationToken cancellationToken)
        {
            var response = await _vaccinationScheduleService.CreateVaccinationScheduleAsync(createVaccinationScheduleDTO, cancellationToken);
            return StatusCode(response.Code, response);
        }
        [HttpPut("update-vaccination-schedule/{vaccinationScheduleId}")]
        public async Task<IActionResult> UpdateVaccinationSchedule(int vaccinationScheduleId, [FromBody] UpdateVaccinationScheduleDTO updateVaccinationScheduleDTO, CancellationToken cancellationToken)
        {
            var response = await _vaccinationScheduleService.UpdateVaccinationScheduleAsync(vaccinationScheduleId, updateVaccinationScheduleDTO, cancellationToken);
            return StatusCode(response.Code, response);
        }
        [HttpDelete("delete-vaccination-schedule/{vaccinationScheduleId}")]
        public async Task<IActionResult> DeleteVaccinationSchedule(int vaccinationScheduleId, CancellationToken cancellationToken)
        {
            var response = await _vaccinationScheduleService.DeleteVaccinationScheduleAsync(vaccinationScheduleId, cancellationToken);
            return StatusCode(response.Code, response);
        }
    }
}
