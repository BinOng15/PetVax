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
        private readonly IPetService _petService;

        public AppointmentForHealthCondition(IAppointmentDetailService appointmentDetailService, IAppointmentService appointmentService, IHealthConditionService healthConditionService, IPetService petService)
        {
            _appointmentDetailService = appointmentDetailService;
            _appointmentService = appointmentService;
            _healthConditionService = healthConditionService;
            _petService = petService;
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

        [HttpGet("Get-HealthCondition-By-Id/{healthConditionId}")]
        public async Task<IActionResult> GetHealthConditionByIdAsync(int healthConditionId, CancellationToken cancellationToken)
        {
            var result = await _healthConditionService.GetHealthConditionByIdAsync(healthConditionId, cancellationToken);
            return StatusCode(result.Code, result);
        }

        [HttpPost("Update-HealthCondition-By-Vet/{healthConditionId}")]
        public async Task<IActionResult> UpdateHealthConditionByVetAsync(int healthConditionId, [FromBody] UpdateHealthCondition updateHealthCondition, CancellationToken cancellationToken)
        {
            var result = await _healthConditionService.UpdateHealthConditionAsync(healthConditionId, updateHealthCondition, cancellationToken);
            return StatusCode(result.Code, result);
        }

        [HttpGet("get-certificate-for-pet/{petId}")]
        public async Task<IActionResult> GetVaccinationRecord(int petId)
        {
            var result = await _healthConditionService.GetPetVaccinationRecordAsync(petId);
            if (result == null)
                return NotFound("Pet not found or no certificates.");

            return Ok(result);
        }
        [HttpGet("Get-HealthCondition-By-PetId-And-Status/{petId}/{status}")]
        public async Task<IActionResult> GetHealthConditionByPetIdAndStatusAsync(int petId, string status, CancellationToken cancellationToken)
        {
            var result = await _healthConditionService.GetHealthConditionByPetIdAndStatus(petId, status, cancellationToken);
            return StatusCode(result.Code, result);
        }
    }
}
