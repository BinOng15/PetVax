using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PediVax.BusinessObjects.DBContext;
using PetVax.BusinessObjects.DTO.VetScheduleDTO;
using PetVax.BusinessObjects.Models;
using PetVax.Services.IService;

namespace PediVax.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VetSchedulesController : ControllerBase
    {
        private readonly IVetScheduleService _vetScheduleService;

        public VetSchedulesController(IVetScheduleService vetScheduleService)
        {
            _vetScheduleService = vetScheduleService;
        }

        [HttpGet("Get-all-schedules")]
        [Authorize(Roles = "Admin, Vet, Staff")]
        public async Task<ActionResult> GetVetSchedules(CancellationToken cancellationToken)
        {
            var response = await _vetScheduleService.GetAllVetSchedulesAsync(cancellationToken);

            return Ok(response);
        }

        [HttpGet("Get-schedule-by-id/{vetScheduleId}")]
        [Authorize(Roles = "Admin, Vet, Staff")]
        public async Task<ActionResult> GetVetScheduleById(int vetScheduleId, CancellationToken cancellationToken)
        {
            var response = await _vetScheduleService.GetVetScheduleByIdAsync(vetScheduleId, cancellationToken);
            
            return Ok(response);
        }

        [HttpPost("Create-schedule")]
        [Authorize(Roles = "Admin, Vet")]
        public async Task<ActionResult> CreateVetSchedule([FromBody] CreateVetScheduleRequestDTO request, CancellationToken cancellationToken)
        {

            var response = await _vetScheduleService.CreateVetScheduleAsync(request, cancellationToken);
  
            return CreatedAtAction(nameof(GetVetScheduleById), new { vetScheduleId = response.Data.VetScheduleId }, response);
        }

        [HttpPut("Update-schedule")]
        [Authorize(Roles = "Admin, Vet")]
        public async Task<ActionResult> UpdateVetSchedule([FromBody] UpdateVetScheduleRequestDTO request, CancellationToken cancellationToken)
        {

            var response = await _vetScheduleService.UpdateVetScheduleAsync(request, cancellationToken);
  
            return Ok(response);

        }
        [HttpGet("Get-schedules-by-vet-id/{vetId}")]
        [Authorize(Roles = "Admin, Vet, Staff")]
        public async Task<ActionResult> GetVetSchedulesByVetId(int vetId, CancellationToken cancellationToken)
        {
            var response = await _vetScheduleService.GetAllVetSchedulesByVetIdAsync(vetId, cancellationToken);
  
            return Ok(response);
        }

        [HttpDelete("Delete-schedule/{vetScheduleId}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteVetSchedule(int vetScheduleId, CancellationToken cancellationToken)
        {
            var response = await _vetScheduleService.DeleteVetScheduleAsync(vetScheduleId, cancellationToken);

            return Ok(response);
        }
    }

}
