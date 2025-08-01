﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetVax.BusinessObjects.DTO;
using PetVax.BusinessObjects.DTO.AppointmentDetailDTO;
using PetVax.BusinessObjects.DTO.AppointmentDTO;
using PetVax.BusinessObjects.Models;
using PetVax.Services.IService;
using static PetVax.BusinessObjects.Enum.EnumList;

namespace PediVax.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentForMicrochip : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IAppointmentDetailService _appointmentDetailService;
        public AppointmentForMicrochip(IAppointmentService appointmentService, IAppointmentDetailService appointmentDetailService)
        {
            _appointmentService = appointmentService;
            _appointmentDetailService = appointmentDetailService;
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
        public async Task<IActionResult> UpdateAppointmentMicrochipById(int appointmentId, [FromBody] UpdateAppointmentDTO updateAppointmentForMicrochipDTO, CancellationToken cancellationToken = default)
        {
            var response = await _appointmentService.UpdateAppointmentMicrochipAsync(appointmentId, updateAppointmentForMicrochipDTO, cancellationToken);
            return StatusCode(response.Code, response);
        }

        [HttpGet("get-appointment-microchip-by-pet-id/{petId}")]
        public async Task<IActionResult> GetAppointmentMicrochipByPetId(int petId, CancellationToken cancellationToken = default)
        {
            var response = await _appointmentDetailService.GetAppointmentMicrochipByPetId(petId, cancellationToken);
            return StatusCode(response.Code, response);
        }

        [HttpGet("get-appoinment-microchip-by-appointment-detail/{appointmentId}")]
        public async Task<IActionResult> GetAppointmentMicrochipByAppoinmentDetailId(int appointmentId, CancellationToken cancellationToken = default)
        {
            var response = await _appointmentDetailService.GetAppointmentMicrochipByAppointmentDetailId(appointmentId, cancellationToken);
            return StatusCode(response.Code, response);
        }

        [HttpGet("get-all-appointment-microchip")]
        public async Task<IActionResult> GetAllAppointmentMicrochip([FromQuery] GetAllItemsDTO getAllItemsDTO, int? vetId, CancellationToken cancellationToken = default)
        {
            var response = await _appointmentDetailService.GetAllAppointmemtMicrochipAsync(getAllItemsDTO, vetId, cancellationToken);
            return StatusCode(response.Code, response);
        }

        [HttpGet("get-appointment-microchip-by-pet-id-and-status/{petId}/{status}")]
        public async Task<IActionResult> GetAppointmentMicrochipByPetIdAndStatus(int petId, AppointmentStatus status, CancellationToken cancellationToken = default)
        {
            var response = await _appointmentDetailService.GetAppointmentMicrochipByPetIdAndStatus(petId, status, cancellationToken);
            return Ok(response);
        }
        [HttpGet("get-appointment-microchip-by-appointment-id/{appointmentId}")]
        public async Task<IActionResult> GetAppointmentMicrochipByAppointmentId(int appointmentId, CancellationToken cancellationToken = default)
        {
            var response = await _appointmentService.GetAppointmentMicrochipByAppointmentId(appointmentId, cancellationToken);
            return StatusCode(response.Code, response);
        }
    }
}
