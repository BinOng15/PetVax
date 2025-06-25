using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PediVax.BusinessObjects.DBContext;
using PetVax.BusinessObjects.DTO.PetDTO;
using PetVax.BusinessObjects.Models;
using PetVax.Services.IService;

namespace PediVax.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PetsController : ControllerBase
    {
        private readonly IPetService _petService;

        public PetsController(IPetService petService)
        {
            _petService = petService;
        }

        [HttpGet("get-all-pets")]
        [Authorize(Roles = "Admin, Staff, Vet")]
        public async Task<IActionResult> GetAllPets([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string? keyWord = null, CancellationToken cancellationToken = default)
        {
            var request = new GetAllPetsRequestDTO
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                KeyWord = keyWord
            };
            var response = await _petService.GetAllPetsAsync(request, cancellationToken);
            return StatusCode(response.Code, response);
        }

        [HttpPut("update-pet")]
        public async Task<IActionResult> UpdatePet([FromBody] UpdatePetRequestDTO updatePetRequest, CancellationToken cancellationToken = default)
        {
            var response = await _petService.UpdatePetAsync(updatePetRequest, cancellationToken);
            return StatusCode(response.Code, response);
        }

        [HttpGet("get-pet-by-id/{petId}")]
        public async Task<IActionResult> GetPetById(int petId, CancellationToken cancellationToken = default)
        {
            var response = await _petService.GetPetByIdAsync(petId, cancellationToken);
            return StatusCode(response.Code, response);
        }

        [HttpPost("create-pet")]
        public async Task<IActionResult> CreatePet([FromForm] CreatePetRequestDTO createPetRequest, CancellationToken cancellationToken = default)
        {
            var response = await _petService.CreatePetAsync(createPetRequest, cancellationToken);
            return StatusCode(response.Code, response);
        }

        [HttpGet("get-pets-by-account-id/{accountId}")]
        public async Task<IActionResult> GetPetsByAccountId(int accountId, CancellationToken cancellationToken = default)
        {
            var response = await _petService.GetPetsByCustomerIdAsync(accountId, cancellationToken);
            return StatusCode(response.FirstOrDefault()?.Code ?? 200, response);
        }

        [HttpDelete("delete-pet/{petId}")]
        public async Task<IActionResult> DeletePet(int petId, CancellationToken cancellationToken = default)
        {
            var response = await _petService.DeletePetById(petId, cancellationToken);
            return StatusCode(response.Code, response);
        }
    }
 }
