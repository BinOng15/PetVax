using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PediVax.BusinessObjects.DBContext;
using PetVax.BusinessObjects.DTO;
using PetVax.BusinessObjects.DTO.VetScheduleDTO;
using PetVax.BusinessObjects.Models;
using PetVax.Services.IService;
using PetVax.Services.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        public async Task<ActionResult> GetVetSchedules([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string? keyWord = null, CancellationToken cancellationToken = default)
        {
            var request = new GetAllItemsDTO
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                KeyWord = keyWord
            };
            var response = await _vetScheduleService.GetAllVetSchedulesAsync(request, cancellationToken);
            return StatusCode(response.Code, response);
        }
        

        [HttpGet("Get-schedule-by-id/{vetScheduleId}")]
        [Authorize(Roles = "Admin, Vet, Staff")]
        public async Task<ActionResult> GetVetScheduleById(int vetScheduleId, CancellationToken cancellationToken)
        {
            var response = await _vetScheduleService.GetVetScheduleByIdAsync(vetScheduleId, cancellationToken);
            
            return Ok(response);
        }

        [HttpPost("Create-schedule")]
        //[Authorize(Roles = "Admin, Vet")]
        public async Task<ActionResult> CreateVetSchedule([FromBody] CreateVetScheduleRequestDTO request, CancellationToken cancellationToken)
        {
            var response = await _vetScheduleService.CreateVetScheduleAsync(request, cancellationToken);
  
            return Ok(response);
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

        [HttpGet("Get-schedules-by-date-and-slot")]
        [Authorize(Roles = "Admin, Vet, Staff")]
        public async Task<ActionResult> GetVetSchedulesByDateAndSlot([FromQuery] DateTime? date, [FromQuery] int? slot, CancellationToken cancellationToken)
        {
            var response = await _vetScheduleService.GetAllVetSchedulesByDateAndSlotAsync(date, slot, cancellationToken);
  
            return Ok(response);
        }

        [HttpGet("Get-schedules-from-date-to-date")]
        //[Authorize(Roles = "Admin, Vet, Staff")]
        public async Task<ActionResult> GetVetSchedulesFromDateToDate([FromQuery] DateTime? fromDate, [FromQuery] DateTime? toDate, CancellationToken cancellationToken)
        {
            var response = await _vetScheduleService.GetVetScheduleFromDateToDate(fromDate, toDate, cancellationToken);
  
            return Ok(response);
        }

    }

}
