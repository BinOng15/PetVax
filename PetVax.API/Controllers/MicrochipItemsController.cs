using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PediVax.BusinessObjects.DBContext;
using PetVax.BusinessObjects.DTO.MicrochipItemDTO;
using PetVax.BusinessObjects.Models;
using PetVax.Services.IService;

namespace PediVax.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MicrochipItemsController : ControllerBase
    {
        private readonly IMicrochipItemService _microchipItemService;

        public MicrochipItemsController(IMicrochipItemService microchipItemService)
        {
            _microchipItemService = microchipItemService;
        }

        [HttpGet("get-information-by-microchip-code")]
        public async Task<IActionResult> GetMicrochipItemsByMicrochipCode(string microchipCode, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(microchipCode))
            {
                return BadRequest("Microchip code cannot be null or empty.");
            }
            var response = await _microchipItemService.GetMicrochipItemByMicrochipCodeAsync(microchipCode, cancellationToken);
            return StatusCode(response.Code, response);
        }

        [HttpPost("create-microchip-item")]
        public async Task<IActionResult> CreateMicrochipItem([FromBody] CreateMicrochipItemRequest microchipItem, CancellationToken cancellationToken = default)
        {

            var response = await _microchipItemService.CreateMicrochipItemAsync(microchipItem, cancellationToken);
            return StatusCode(response.Code, response);
        }

        [HttpPut("update-microchip-item/{id}")]
        public async Task<IActionResult> UpdateMicrochipItem(int id, [FromBody] UpdateMicrochipItemRequest microchipItem, CancellationToken cancellationToken = default)
        {
            var response = await _microchipItemService.UpdateMicrochipItemAsync(id, microchipItem, cancellationToken);
            return StatusCode(response.Code, response);
        }
    }
}
