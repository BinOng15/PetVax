using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetVax.BusinessObjects.DTO;
using PetVax.BusinessObjects.DTO.AppointmentDTO;
using PetVax.BusinessObjects.DTO.CustomerDTO;
using PetVax.Services.IService;

namespace PediVax.Controllers
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

        [HttpGet("Get-all-appointments")]
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
        [HttpGet("Get-appointment-by-id/{appointmentId}")]
        public async Task<IActionResult> GetAppointmentById(int appointmentId, CancellationToken cancellationToken = default)
        {
            var response = await _appointmentService.GetAppointmentByIdAsync(appointmentId, cancellationToken);
            return StatusCode(response.Code, response);
        }
        [HttpGet("Get-appointment-by-pet-id/{petId}")]
        public async Task<IActionResult> GetAppointmentByPetId(int petId, CancellationToken cancellationToken = default)
        {
            var response = await _appointmentService.GetAppointmentByPetIdAsync(petId, cancellationToken);
            return StatusCode(response.Code, response);
        }
        [HttpPost("Create-appointment")]
        public async Task<IActionResult> CreateAppointment([FromBody] CreateAppointmentDTO createAppointmentDTO, CancellationToken cancellationToken = default)
        {
            var response = await _appointmentService.CreateAppointmentAsync(createAppointmentDTO, cancellationToken);
            return StatusCode(response.Code, response);
        }
        [HttpPut("Update-appointment/{appointmentId}")]
        public async Task<IActionResult> UpdateAppointment(int appointmentId, [FromBody] UpdateAppointmentDTO updateAppointmentDTO, CancellationToken cancellationToken = default)
        {
            var response = await _appointmentService.UpdateAppointmentAsync(appointmentId, updateAppointmentDTO, cancellationToken);
            return StatusCode(response.Code, response);
        }
        [HttpDelete("Delete-appointment/{appointmentId}")]
        public async Task<IActionResult> DeleteAppointment(int appointmentId, CancellationToken cancellationToken = default)
        {
            var response = await _appointmentService.DeleteAppointmentAsync(appointmentId, cancellationToken);
            return StatusCode(response.Code, response);
        }
    }
}
