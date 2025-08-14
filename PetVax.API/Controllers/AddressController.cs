using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetVax.BusinessObjects.DTO;
using PetVax.BusinessObjects.DTO.AddressDTO;
using PetVax.Services.IService;
using PetVax.Services.Service;

namespace PediVax.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        private readonly IAddressService _addressService;

        public AddressController(IAddressService addressService)
        {
            _addressService = addressService;
        }

        [HttpGet("get-all-address")]
        public async Task<IActionResult> GetAllAddresss([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string? keyWord = null, CancellationToken cancellationToken = default)
        {
            var request = new GetAllItemsDTO
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                KeyWord = keyWord
            };
            var response = await _addressService.GetAllAddressAsync(request, cancellationToken);
            return StatusCode(response.Code, response);
        }
        [HttpGet("get-address-by-id/{addressId}")]
        public async Task<IActionResult> GetAddressById(int addressId, CancellationToken cancellationToken)
        {
            var response = await _addressService.GetAddressByIdAsync(addressId, cancellationToken);
            return StatusCode(response.Code, response);
        }
        [HttpPost("create-address")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateAddress([FromForm] CreateAddressDTO createAddressDTO, CancellationToken cancellationToken)
        {
            var response = await _addressService.CreateAddressAsync(createAddressDTO, cancellationToken);
            return StatusCode(response.Code, response);
        }
        [HttpPut("update-address")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateAddress([FromForm] int addressId, [FromForm] UpdateAddressDTO updateAddressDTO, CancellationToken cancellationToken)
        {
            var response = await _addressService.UpdateAddressAsync(addressId, updateAddressDTO, cancellationToken);
            return StatusCode(response.Code, response);
        }
        [HttpDelete("delete-address/{addressId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteAddress(int addressId, CancellationToken cancellationToken)
        {
            var response = await _addressService.DeleteAddressAsync(addressId, cancellationToken);
            return StatusCode(response.Code, response);
        }
    }
}
