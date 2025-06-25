using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetVax.BusinessObjects.DTO.AppointmentDetailDTO;
using PetVax.BusinessObjects.DTO.AppointmentDTO;
using PetVax.Services.IService;

namespace PediVax.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentForMicrochip : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;

        public AppointmentForMicrochip(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }

        [HttpPost("create-appointment-microchip")]
        public async Task<IActionResult> CreateAppointmentMicrochip([FromBody] CreateAppointmentMicrochipDTO createAppointmentMicrochipDTO, CancellationToken cancellationToken = default)
        {
            var response = await _appointmentService.CreateAppointmentMicrochipAsync(createAppointmentMicrochipDTO, cancellationToken);
            return StatusCode(response.Code, response);
        }

        [HttpPut("update-appointment-microchip")]
        public async Task<IActionResult> UpdateAppointmentMicrochip([FromForm] UpdateAppointmentMicrochipDTO updateAppointmentForMicrochipDTO, CancellationToken cancellationToken = default)
        {
            var response = await _appointmentService.UpdateAppointmentMicrochip(updateAppointmentForMicrochipDTO, cancellationToken);
            return StatusCode(response.Code, response);
        }

        [HttpPut("update-appointment-microchip/{appointmentId}")]
        public async Task<IActionResult> UpdateAppointmentMicrochipById(int appointmentId, [FromBody] CreateAppointmentMicrochipDTO updateAppointmentForMicrochipDTO, CancellationToken cancellationToken = default)
        {
            var response = await _appointmentService.UpdateAppointmentMicrochipAsync(appointmentId, updateAppointmentForMicrochipDTO, cancellationToken);
            return StatusCode(response.Code, response);
        }
    }
}
