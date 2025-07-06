using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetVax.BusinessObjects.DTO.AppointmentDetailDTO;
using PetVax.BusinessObjects.DTO.AppointmentDTO;
using PetVax.Repositories.IRepository;
using PetVax.Services.IService;

namespace PediVax.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentForHealthCondition : ControllerBase
    {
        private readonly IAppointmentDetailService _appointmentDetailService;
        private readonly IAppointmentService _appointmentService;

        public AppointmentForHealthCondition(IAppointmentDetailService appointmentDetailService, IAppointmentService appointmentService)
        {
            _appointmentDetailService = appointmentDetailService;
            _appointmentService = appointmentService;
        }

        [HttpGet("Get-Appointment-Detail-HealthCondition-By/{appointmentDetailId}")]
        public async Task<IActionResult> GetAppointmentDetailHealthConditionByIdAsync(int appointmentDetailId, CancellationToken cancellationToken)
        {
            var result = await _appointmentDetailService.GetAppointmentDetailHealthConditionByAppointmentDetailIdAsync(appointmentDetailId, cancellationToken);

            return Ok(result);

        }

        [HttpPost("Create-Appointment-HealthCondition")]
        public async Task<IActionResult> CreateAppointmentHealthConditionAsync([FromBody] CreateAppointmentHealthConditionDTO createAppointmentHealthConditionDTO, CancellationToken cancellationToken)
        {
            var result = await _appointmentService.CreateAppointmentHealConditionAsync(createAppointmentHealthConditionDTO, cancellationToken);
            return StatusCode(result.Code, result);
        }

        [HttpPut("Update-Appointment-HealthCondition")]
        public async Task<IActionResult> UpdateAppointmentHealthConditionAsync([FromBody] UpdateAppointmentHealthConditionDTO updateAppointmentHealthConditionDTO, CancellationToken cancellationToken)
        {
            var result = await _appointmentService.UpdateAppointmentHealthConditionAsync(updateAppointmentHealthConditionDTO, cancellationToken);
            return StatusCode(result.Code, result);
        }
    }
}
