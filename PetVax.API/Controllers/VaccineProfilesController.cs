using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PediVax.BusinessObjects.DBContext;
using PetVax.BusinessObjects.DTO.VaccineProfileDTO;
using PetVax.BusinessObjects.Models;
using PetVax.Services.IService;

namespace PediVax.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VaccineProfilesController : ControllerBase
    {
        private readonly IVaccineProfileService _vaccineProfileService;

        public VaccineProfilesController(IVaccineProfileService vaccineProfileService)
        {
            _vaccineProfileService = vaccineProfileService;
        }

        [HttpGet("GetAllVaccineProfiles")]
        [Authorize(Roles = "Admin, Staff, Vet")]
        public async Task<IActionResult> GetAllVaccineProfiles(CancellationToken cancellationToken)
        {
            var response = await _vaccineProfileService.GetAllVaccineProfilesAsync(cancellationToken);
            return Ok(response);
        }

        [HttpPost("CreateVaccineProfile")]
        public async Task<IActionResult> CreateVaccineProfile([FromBody] VaccineProfileRequestDTO vaccineProfileRequest, CancellationToken cancellationToken)
        {
            if (vaccineProfileRequest == null)
            {
                return BadRequest("Invalid vaccine profile request.");
            }
            var response = await _vaccineProfileService.CreateVaccineProfileAsync(vaccineProfileRequest, cancellationToken);
            return Ok(response);
        }

        //[HttpGet("GetVaccineProfileByPetId/{petId}")]
        //public async Task<IActionResult> GetVaccineProfileByPetId(int petId, CancellationToken cancellationToken)
        //{
        //    if (petId <= 0)
        //    {
        //        return BadRequest("Invalid pet ID.");
        //    }
        //    var response = await _vaccineProfileService.GetVaccineProfileByPetIdAsync(petId, cancellationToken);
        //    return Ok(response);
        //}
        [HttpGet("GetVaccineProfileByPetId/{petId}")]
        [Authorize(Roles = "Admin, Staff, Vet, Customer")]
        public async Task<IActionResult> GetListVaccineProfileByPetId(int petId, CancellationToken cancellationToken)
        {
            if (petId <= 0)
            {
                return BadRequest("Invalid pet ID.");
            }
            var response = await _vaccineProfileService.GetGroupedVaccineProfilesByPetIdAsync(petId, cancellationToken);
            return Ok(response);
        }

        [HttpGet("GetVaccineProfileById/{id}")]
        [Authorize(Roles = "Admin, Staff, Vet, Customer")]
        public async Task<IActionResult> GetVaccineProfileById(int id, CancellationToken cancellationToken)
        {
            if (id <= 0)
            {
                return BadRequest("Invalid vaccine profile ID.");
            }
            var response = await _vaccineProfileService.GetVaccineProfileByIdAsync(id, cancellationToken);
            return Ok(response);
        }
        [HttpPut("UpdateVaccineProfile/{id}")]
        [Authorize(Roles = "Admin, Staff")]
        public async Task<IActionResult> UpdateVaccineProfile(int id, [FromBody] VaccineProfileRequestDTO vaccineProfileRequest, CancellationToken cancellationToken)
        {
            if (id <= 0 || vaccineProfileRequest == null)
            {
                return BadRequest("Invalid vaccine profile ID or request data.");
            }
            // Assuming the service has an update method
            var response = await _vaccineProfileService.UpdateVaccineProfileAsync(id, vaccineProfileRequest, cancellationToken);
            return Ok(response);

        }
        [HttpDelete("DeleteVaccineProfile/{vaccineProfileId}")]
        [Authorize(Roles = "Admin, Staff")]
        public async Task<IActionResult> DeleteVaccineProfile(int vaccineProfileId, CancellationToken cancellationToken)
        {
            if (vaccineProfileId <= 0)
            {
                return BadRequest("Invalid vaccine profile ID.");
            }
            var response = await _vaccineProfileService.DeleteVaccineProfileAsync(vaccineProfileId, cancellationToken);
            return Ok(response);
        }
    }
}
