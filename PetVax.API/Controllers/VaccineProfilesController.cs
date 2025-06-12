using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        [HttpGet("GetVaccineProfileByPetId/{petId}")]
        public async Task<IActionResult> GetVaccineProfileByPetId(int petId, CancellationToken cancellationToken)
        {
            if (petId <= 0)
            {
                return BadRequest("Invalid pet ID.");
            }
            var response = await _vaccineProfileService.GetVaccineProfileByPetIdAsync(petId, cancellationToken);
            return Ok(response);
        }

    }
}
