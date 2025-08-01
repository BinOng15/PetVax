﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetVax.BusinessObjects.DTO;
using PetVax.BusinessObjects.DTO.AppointmentDetailDTO;
using PetVax.BusinessObjects.DTO.AppointmentDTO;
using PetVax.BusinessObjects.DTO.HealthConditionDTO;
using PetVax.Repositories.IRepository;
using PetVax.Services.IService;
using static PetVax.BusinessObjects.Enum.EnumList;

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

        [HttpGet("Get-Appointment-Detail-HealthCondition-By/{appointmentId}")]
        public async Task<IActionResult> GetAppointmentDetailHealthConditionByIdAsync(int appointmentId, CancellationToken cancellationToken)
        {
            var result = await _appointmentDetailService.GetAppointmentDetailHealthConditionByAppointmentIdAsync(appointmentId, cancellationToken);

            return Ok(result);

        }

        [HttpPost("Create-Appointment-HealthCondition")]
        public async Task<IActionResult> CreateAppointmentHealthConditionAsync([FromBody] CreateAppointmentHealthConditionDTO createAppointmentHealthConditionDTO, CancellationToken cancellationToken)
        {
            var result = await _appointmentService.CreateAppointmentHealConditionAsync(createAppointmentHealthConditionDTO, cancellationToken);
            return StatusCode(result.Code, result);
        }

        [HttpPut("Update-Appointment-HealthCondition-For-Staff")]
        public async Task<IActionResult> UpdateAppointmentHealthConditionAsync([FromQuery] int appointmentId, [FromForm] UpdateAppointmentHealthConditionDTO updateAppointmentHealthConditionDTO, CancellationToken cancellationToken)
        {
            var result = await _appointmentService.UpdateAppointmentHealthConditionAsync( appointmentId, updateAppointmentHealthConditionDTO, cancellationToken);
            return StatusCode(result.Code, result);
        }

        //[HttpPost("Create-healthcondion-by-vet")]
        //public async Task<IActionResult> CreateHealthConditionByVetAsync([FromForm] CreateHealthConditionDTO createHealthConditionDTO, CancellationToken cancellationToken)
        //{
        //    var result = await _healthConditionService.CreateHealthConditionAsync(createHealthConditionDTO, cancellationToken);
        //    return StatusCode(result.Code, result);
        //}

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

        //[HttpPut("Update-HealthCondition-By-Vet/{healthConditionId}")]
        //public async Task<IActionResult> UpdateHealthConditionByVetAsync(int healthConditionId, [FromForm] UpdateHealthCondition updateHealthCondition, CancellationToken cancellationToken)
        //{
        //    var result = await _healthConditionService.UpdateHealthConditionAsync(healthConditionId, updateHealthCondition, cancellationToken);
        //    return StatusCode(result.Code, result);
        //}

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

        [HttpGet("Get-All-Appointment-Detail-HealthConditions")]
        public async Task<IActionResult> GetAllAppointmentDetailHealthConditionAsync([FromQuery] GetAllItemsDTO getAllItemsDTO, int? vetId, CancellationToken cancellationToken)
        {
            var result = await _appointmentDetailService.GetAllAppointmentDetailHealthConditionAsync(getAllItemsDTO, vetId, cancellationToken);
            return Ok(result);
        }

        [HttpPut("Update-appointment-for-customer/{appointmentId}")]
        public async Task<IActionResult> UpdateAppointmentForCustomerAsync(int appointmentId, [FromBody] UpdateAppointmentDTO updateAppointmentHealthConditionDTO, CancellationToken cancellationToken)
        {
            var result = await _appointmentService.UpdateAppointmentHealConditionAsync(appointmentId, updateAppointmentHealthConditionDTO, cancellationToken);
            return StatusCode(result.Code, result);
        }

        [HttpGet("Get-Appointment-Detail-Healthcondition-by-Pet-And-Status/{petId}/{status}")]
            public async Task<IActionResult> GetAppointmentDetailHealthConditionByPetIdAndStatusAsync(int petId, AppointmentStatus status, CancellationToken cancellationToken)
        {
            var result = await _appointmentDetailService.GetAppointmentDetailHealthConditionByPetIdAndStatusAsync(petId, status, cancellationToken);
            return StatusCode(result.Code, result);
        }
    }
}
