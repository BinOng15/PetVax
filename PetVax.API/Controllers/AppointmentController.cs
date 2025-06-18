using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetVax.BusinessObjects.DTO;
using PetVax.BusinessObjects.DTO.AppointmentDetailDTO;
using PetVax.BusinessObjects.DTO.AppointmentDTO;
using PetVax.BusinessObjects.DTO.CustomerDTO;
using PetVax.Services.IService;
using static PetVax.BusinessObjects.DTO.AppointmentDTO.CreateAppointmentDTO;

namespace PetVax.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;
        private readonly ILogger<AppointmentController> _logger;

        public AppointmentController(IAppointmentService appointmentService, ILogger<AppointmentController> logger)
        {
            _appointmentService = appointmentService;
            _logger = logger;
        }

        [HttpGet("get-all-appointments")]
        public async Task<IActionResult> GetAllAppointments([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string? keyWord = null, CancellationToken cancellationToken = default)
        {
            var request = new GetAllItemsDTO
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                KeyWord = keyWord
            };
            var response = await _appointmentService.GetAllAppointmentAsync(request, cancellationToken);
            return StatusCode(response.Code, response);
        }
        [HttpGet("get-appointment-by-id/{appointmentId}")]
        public async Task<IActionResult> GetAppointmentById(int appointmentId, CancellationToken cancellationToken = default)
        {
            var response = await _appointmentService.GetAppointmentByIdAsync(appointmentId, cancellationToken);
            return StatusCode(response.Code, response);
        }
        [HttpGet("get-appointment-by-pet-id/{petId}")]
        public async Task<IActionResult> GetAppointmentByPetId(int petId, CancellationToken cancellationToken = default)
        {
            var response = await _appointmentService.GetAppointmentByPetIdAsync(petId, cancellationToken);
            return StatusCode(response.Code, response);
        }
        //[HttpPost("create-appointment")]
        //public async Task<IActionResult> CreateAppointment(CreateFullAppointmentDTO createFullAppointmentDTO,
        //    CancellationToken cancellationToken = default)
        //{
        //    var response = await _appointmentService.CreateFullAppointmentAsync(createFullAppointmentDTO, cancellationToken);
        //    return StatusCode(response.Code, response);
        //}
        //[HttpPut("update-appointment/{appointmentId}")]
        //public async Task<IActionResult> UpdateAppointment(int appointmentId, [FromBody] UpdateAppointmentDTO updateAppointmentDTO, CancellationToken cancellationToken = default)
        //{
        //    var response = await _appointmentService.UpdateAppointmentAsync(appointmentId, updateAppointmentDTO, cancellationToken);
        //    return StatusCode(response.Code, response);
        //}
        [HttpDelete("delete-appointment/{appointmentId}")]
        public async Task<IActionResult> DeleteAppointment(int appointmentId, CancellationToken cancellationToken = default)
        {
            var response = await _appointmentService.DeleteAppointmentAsync(appointmentId, cancellationToken);
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
