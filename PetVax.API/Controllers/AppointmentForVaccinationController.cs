using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetVax.BusinessObjects.DTO.AppointmentDetailDTO;
using PetVax.BusinessObjects.DTO.AppointmentDTO;
using PetVax.Services.IService;

namespace PediVax.Controllers
{
    [Route("api/AppointmentForVaccination")]
    [ApiController]
    public class AppointmentForVaccinationController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;
        private readonly ILogger<AppointmentForVaccinationController> _logger;

        public AppointmentForVaccinationController(IAppointmentService appointmentService, ILogger<AppointmentForVaccinationController> logger)
        {
            _appointmentService = appointmentService;
            _logger = logger;
        }

        [HttpGet("get-appointment-vaccination-by-id/{appointmentId}")]
        public async Task<IActionResult> GetAppointmentVaccinationById(int appointmentId, CancellationToken cancellationToken = default)
        {
            var response = await _appointmentService.GetAppointmentVaccinationByIdAsync(appointmentId, cancellationToken);
            return StatusCode(response.Code, response);
        }
        [HttpPost("create-appointment-vaccination")]
        public async Task<IActionResult> CreateAppointmentVaccination([FromBody] CreateAppointmentVaccinationDTO createAppointmentVaccinationDTO, CancellationToken cancellationToken = default)
        {
            var response = await _appointmentService.CreateAppointmentVaccinationAsync(createAppointmentVaccinationDTO, cancellationToken);
            return StatusCode(response.Code, response);
        }
        [HttpPut("update-appointment-vaccination/{appointmentId}")]
        public async Task<IActionResult> UpdateAppointmentVaccination(int appointmentId, [FromForm] UpdateAppointmentVaccinationDTO updateAppointmentVaccinationDTO, CancellationToken cancellationToken = default)
        {
            var response = await _appointmentService.UpdateAppointmentVaccination(appointmentId, updateAppointmentVaccinationDTO, cancellationToken);
            return StatusCode(response.Code, response);
        }
    }
}
