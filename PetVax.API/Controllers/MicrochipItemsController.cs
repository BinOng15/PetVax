using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PediVax.BusinessObjects.DBContext;
using PetVax.BusinessObjects.DTO;
using PetVax.BusinessObjects.DTO.AccountDTO;
using PetVax.BusinessObjects.DTO.MicrochipItemDTO;
using PetVax.BusinessObjects.Models;
using PetVax.Services.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static PetVax.BusinessObjects.Enum.EnumList;

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

        [HttpGet("get-information-of-pet-by-microchip-code")]
        public async Task<IActionResult> GetMicrochipItemsByMicrochipCode(string microchipCode,  CancellationToken cancellationToken = default)
        {
            var response = await _microchipItemService.GetMicrochipItemByMicrochipCodeAsync(microchipCode, cancellationToken);
            return StatusCode(response.Code, response);
        }

        //[HttpPost("create-microchip-item")]
        //public async Task<IActionResult> CreateMicrochipItem([FromBody] CreateMicrochipItemRequest microchipItem, CancellationToken cancellationToken = default)
        //{

        //    var response = await _microchipItemService.CreateMicrochipItemAsync(microchipItem, cancellationToken);
        //    return StatusCode(response.Code, response);
        //}

        [HttpPut("update-microchip-item/{id}")]
        public async Task<IActionResult> UpdateMicrochipItem(int id, [FromBody] UpdateMicrochipItemRequest microchipItem, CancellationToken cancellationToken = default)
        {
            var response = await _microchipItemService.UpdateMicrochipItemAsync(id, microchipItem, cancellationToken);
            return StatusCode(response.Code, response);
        }

        [HttpGet("get-microchip-item-by-/{id}")]
        public async Task<IActionResult> GetMicrochipItemById(int id, CancellationToken cancellationToken = default)
        {
            var response = await _microchipItemService.GetMicrochipItemByIdAsync(id, cancellationToken);
            return StatusCode(response.Code, response);
        }

        [HttpGet("get-all-microchip-items")]
        public async Task<IActionResult> GetAllMicrochipItemsPaging(bool isUsed, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string? keyWord = null, [FromQuery] bool? status = null, CancellationToken cancellationToken = default)
        {
            var request = new GetAllItemsDTO
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                KeyWord = keyWord,
                Status = status
            };
            var response = await _microchipItemService.GetAllMicrochipItemsPagingAsync(isUsed, request, cancellationToken);
            return StatusCode(response.Code, response);
        }

        [HttpDelete("delete-microchip-item/{id}")]
        public async Task<IActionResult> DeleteMicrochipItem(int id, CancellationToken cancellationToken = default)
        {

            var response = await _microchipItemService.DeleteMicrochipItemAsync(id, cancellationToken);
            return StatusCode(response.Code, response);
        }

        [HttpPatch("assign-chip-for-pet/{micorchipItemId}/{petId}")]
        public async Task<IActionResult> AssignChipForPet(int micorchipItemId, int petId, CancellationToken cancellationToken = default)
        {
            var response = await _microchipItemService.AssignChipForPet(micorchipItemId, petId, cancellationToken);
            return StatusCode(response.Code, response);
        }
    }
}
