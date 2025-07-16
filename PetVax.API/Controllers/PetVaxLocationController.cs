using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetVax.Services.Service;

namespace PediVax.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PetVaxLocationController : ControllerBase
    {
        private readonly MapBoxService _mapBoxService;

        public PetVaxLocationController(MapBoxService mapBoxService)
        {
            _mapBoxService = mapBoxService;
        }
        [HttpGet("get-coordinates-by-address")]
        public async Task<IActionResult> GetCoordinates([FromQuery] string address)
        {
            var coords = await _mapBoxService.GetCoordinatesAsync(address);
            if (coords == null)
                return NotFound("Không tìm thấy tọa độ cho địa chỉ này.");

            return Ok(new { Latitude = coords.Value.lat, Longitude = coords.Value.lng, Address = address });
        }
        [HttpGet("get-address-by-coordinates")]
        public async Task<IActionResult> GetAddress([FromQuery] double lat, [FromQuery] double lng)
        {
            var address = await _mapBoxService.GetAddressAsync(lat, lng);
            if (address == null)
                return NotFound("Không tìm thấy địa chỉ cho tọa độ này.");
            return Ok(new { Address = address });
        }
        [HttpGet("get-coordinates-of-vaxpet")]
        public async Task<IActionResult> GetFptHcmCoordinates()
        {
            var coords = await _mapBoxService.GetFptHcmCoordinatesAsync();
            if (coords == null)
                return NotFound("Không tìm thấy tọa độ cho địa chỉ của Vax Pet");
            return Ok(new { Latitude = coords.Value.lat, Longitude = coords.Value.lng, Address = "Vax Pet Veterinary Vaccination Center" });
        }

    }
}
