using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PediVax.BusinessObjects.DBContext;
using PetVax.BusinessObjects.DTO;
using PetVax.BusinessObjects.DTO.MicrochipDTO;
using PetVax.BusinessObjects.Models;
using PetVax.Services.IService;
using PetVax.Services.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PediVax.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MicrochipsController : ControllerBase
    {
        private readonly IMicrochipService _microchipService;

        public MicrochipsController(IMicrochipService microchipService)
        {
            _microchipService = microchipService;
        }

        [HttpGet("GetAllMicrochips")]
        public async Task<IActionResult> GetAllMicrochips([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string? keyWord = null, CancellationToken cancellationToken = default)
        {
            var request = new GetAllItemsDTO
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                KeyWord = keyWord
            };
            var response = await _microchipService.GetAllMicrochipsDynamicAsync(request, cancellationToken);
            return StatusCode(response.Code, response);
        }



        [HttpGet("GetMicrochipById/{microchipId}")]
        public async Task<IActionResult> GetMicrochipById(int microchipId, CancellationToken cancellationToken)
        {
            var response = await _microchipService.GetMicrochipByIdAsync(microchipId, cancellationToken);

            return Ok(response);
        }

        [HttpPost("CreateMicrochip")]
        public async Task<IActionResult> CreateMicrochip([FromBody] MicrochipRequestDTO microchipRequestDTO, CancellationToken cancellationToken)
        {
            var response = await _microchipService.CreateFullMicrochipAsync(microchipRequestDTO, cancellationToken);
            return Ok(response);
        }
        [HttpPut("UpdateMicrochip/{microchipId}")]
        public async Task<IActionResult> UpdateMicrochip(int microchipId, [FromBody] MicrochipRequestDTO microchipRequestDTO, CancellationToken cancellationToken)
        {
            var response = await _microchipService.UpdateMicrochipAsync(microchipId, microchipRequestDTO, cancellationToken);
            return Ok(response);
        }

        [HttpDelete("DeleteMicrochip/{microchipId}")]
        public async Task<IActionResult> DeleteMicrochip(int microchipId, CancellationToken cancellationToken)
        {
            var response = await _microchipService.DeleteMicrochipAsync(microchipId, cancellationToken);

            return NotFound(new { Message = "Microchip not found" });
        }
    }
}
