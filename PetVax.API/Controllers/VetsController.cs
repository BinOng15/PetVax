using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        public async Task<IActionResult> GetAllVets([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string? keyWord = null, CancellationToken cancellationToken = default)
        {
            var request = new GetAllVetRequestDTO
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                KeyWord = keyWord
            };
            var response = await _vetService.GetAllVetsAsync(request, cancellationToken);
            if (!response.Success || response.Data == null)
            {
                return StatusCode(response.Code, new { Message = response.Message ?? "No vets found" });
            }
            return StatusCode(response.Code, response);
        }

        [HttpPut("update-vet")]
        public async Task<IActionResult> UpdateVet([FromBody] UpdateVetRequest updateVetRequest, CancellationToken cancellationToken = default)
        {

            var response = await _vetService.UpdateVetsAsync(updateVetRequest, cancellationToken);
            if (!response.Success || response.Data == null)
            {
                return StatusCode(response.Code, new { Message = response.Message ?? "Failed to update vet" });
            }
            return StatusCode(response.Code, response);
        }


        [HttpGet("get-vet-by-id/{vetId}")]
        public async Task<IActionResult> GetVetById(int vetId, CancellationToken cancellationToken = default)
        {
            var response = await _vetService.GetVetByIdAsync(vetId, cancellationToken);
            if (!response.Success || response.Data == null)
            {
                return StatusCode(response.Code, new { Message = response.Message ?? "Vet not found" });
            }
            return StatusCode(response.Code, response);
        }

        [HttpDelete("delete-vet/{vetId}")]
        public async Task<IActionResult> DeleteVet(int vetId, CancellationToken cancellationToken = default)
        {
            var response = await _vetService.DeleteVetAsync(vetId, cancellationToken);
            if (!response.Success)
            {
                return StatusCode(response.Code, new { Message = response.Message ?? "Failed to delete vet" });
            }
            return StatusCode(response.Code, response);
        }
    }
}
