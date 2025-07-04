using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetVax.BusinessObjects.DTO;
using PetVax.BusinessObjects.DTO.AppointmentDetailDTO;
using PetVax.BusinessObjects.DTO.AppointmentDTO;
using PetVax.BusinessObjects.Enum;
using PetVax.Services.IService;
using PetVax.Services.Service;

namespace PediVax.Controllers
{
    [Route("api/AppointmentForVaccination")]
    [ApiController]
    public class AppointmentForVaccinationController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IAppointmentDetailService _appointmentDetailService;
        private readonly ILogger<AppointmentForVaccinationController> _logger;

        public AppointmentForVaccinationController(IAppointmentService appointmentService, IAppointmentDetailService appointmentDetailService, ILogger<AppointmentForVaccinationController> logger)
        {
            _appointmentService = appointmentService;
            _appointmentDetailService = appointmentDetailService;
            _logger = logger;
        }

        [HttpGet("get-all-appointment-vaccination")]
        public async Task<IActionResult> GetAllAppointmentVaccination([FromQuery] GetAllItemsDTO getAllItemsDTO, CancellationToken cancellationToken = default)
        {
            var response = await _appointmentService.GetAllAppointmentVaccinationAsync(getAllItemsDTO, cancellationToken);
            return StatusCode(response.Code, response);
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
        [HttpGet("get-appointment-vaccination-by-pet-id/{petId}")]
        public async Task<IActionResult> GetAppointmentVaccinationByPetId(int petId, CancellationToken cancellationToken = default)
        {
            var response = await _appointmentDetailService.GetAppointmentVaccinationByPetId(petId, cancellationToken);
            return StatusCode(response.Code, response);
        }
        [HttpGet("get-appointment-vaccination-by-pet-id-and-status/{petId}/{status}")]
        public async Task<IActionResult> GetAppointmentVaccinationByPetIdAndStatus(int petId, EnumList.AppointmentStatus status, CancellationToken cancellationToken = default)
        {
            var response = await _appointmentDetailService.GetAppointmentVaccinationByPetIdAndStatus(petId, status, cancellationToken);
            return StatusCode(response.Code, response);
        }
        [HttpGet("get-appointment-detail-vaccination-by-appointment-id/{appointmentId}")]
        public async Task<IActionResult> GetAppointmentVaccinationByAppointmentId(int appointmentId, CancellationToken cancellationToken = default)
        {
            var response = await _appointmentDetailService.GetAppointmentVaccinationByAppointmentId(appointmentId, cancellationToken);
            return StatusCode(response.Code, response);
        }
    }
}
