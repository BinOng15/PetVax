using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PediVax.BusinessObjects.DBContext;
using PetVax.BusinessObjects.DTO.VetDTO;
using PetVax.BusinessObjects.Models;
using PetVax.Services.IService;

namespace PediVax.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VetsController : ControllerBase
    {
        private readonly IVetService _vetService;

        public VetsController(IVetService vetService)
        {
            _vetService = vetService;
        }

        [HttpGet("get-all-vets")]
        [Authorize(Roles = "Admin, Vet")]
        public async Task<IActionResult> GetAllVets([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string? keyWord = null, CancellationToken cancellationToken = default)
        {
            var request = new GetAllVetRequestDTO
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                KeyWord = keyWord
            };
            var response = await _vetService.GetAllVetsAsync(request, cancellationToken);
            return StatusCode(response.Code, response);
        }

        [HttpPut("update-vet")]
        [Authorize(Roles = "Admin, Vet")]
        public async Task<IActionResult> UpdateVet([FromBody] UpdateVetRequest updateVetRequest, CancellationToken cancellationToken = default)
        {

            var response = await _vetService.UpdateVetsAsync(updateVetRequest, cancellationToken);

            return StatusCode(response.Code, response);
        }


        [HttpGet("get-vet-by-id/{vetId}")]
        [Authorize(Roles = "Admin, Vet, Staff")]
        public async Task<IActionResult> GetVetById(int vetId, CancellationToken cancellationToken = default)
        {
            var response = await _vetService.GetVetByIdAsync(vetId, cancellationToken);

            return StatusCode(response.Code, response);
        }

        [HttpDelete("delete-vet/{vetId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteVet(int vetId, CancellationToken cancellationToken = default)
        {
            var response = await _vetService.DeleteVetAsync(vetId, cancellationToken);

            return StatusCode(response.Code, response);
        }
        [HttpPost("create-vet")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateVet([FromBody] CreateVetDTO createVetDTO, CancellationToken cancellationToken = default)
        {
            var response = await _vetService.CreateVetAsync(createVetDTO, cancellationToken);
            return StatusCode(response.Code, response);
        }
    }
}
