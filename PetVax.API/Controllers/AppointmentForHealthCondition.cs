using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetVax.BusinessObjects.DTO.AppointmentDetailDTO;
using PetVax.BusinessObjects.DTO.AppointmentDTO;
using PetVax.BusinessObjects.DTO.HealthConditionDTO;
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
        private readonly IHealthConditionService _healthConditionService;

       public AppointmentForHealthCondition(IAppointmentDetailService appointmentDetailService, IAppointmentService appointmentService, IHealthConditionService healthConditionService)
        {
            _appointmentDetailService = appointmentDetailService;
            _appointmentService = appointmentService;
            _healthConditionService = healthConditionService;
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

        [HttpPost("Create-healthcondion-by-vet")]
        public async Task<IActionResult> CreateHealthConditionByVetAsync([FromForm] CreateHealthConditionDTO createHealthConditionDTO, CancellationToken cancellationToken)
        {
            var result = await _healthConditionService.CreateHealthConditionAsync(createHealthConditionDTO, cancellationToken);
            return StatusCode(result.Code, result);
        }

        [HttpGet("Get-Appointment-Detail-HealthCondition-By-PetId/{petId}")]
        public async Task<IActionResult> GetAppointmentDetailHealthConditionByPetIdAsync(int petId, CancellationToken cancellationToken)
        {
            var result = await _appointmentDetailService.GetAppointmentDetailHealthConditionByPetIdAsync(petId, cancellationToken);
            return Ok(result);
        }
    }
}
