using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetVax.BusinessObjects.DTO;
using PetVax.BusinessObjects.DTO.HandbookDTO;
using PetVax.Services.IService;

namespace PediVax.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HandbookController : ControllerBase
    {
        private readonly IHandbookService _handbookService;

        public HandbookController(IHandbookService handbookService)
        {
            _handbookService = handbookService;
        }

        [HttpGet("get-all-handbooks")]
        public async Task<IActionResult> GetAllHandbooksAsync([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string? keyWord = null, [FromQuery] bool? status = true, CancellationToken cancellationToken = default)
        {
            var request = new GetAllItemsDTO
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                KeyWord = keyWord,
                Status = status
            };
            var response = await _handbookService.GetAllHandbooksAsync(request, cancellationToken);
            return StatusCode(response.Code, response);
        }
        [HttpGet("get-handbook-by-id/{handbookId}")]
        public async Task<IActionResult> GetHandbookByIdAsync(int handbookId, CancellationToken cancellationToken = default)
        {
            var response = await _handbookService.GetHandbookByIdAsync(handbookId, cancellationToken);
            return StatusCode(response.Code, response);
        }
        [HttpPost("create-handbook")]
        public async Task<IActionResult> CreateHandbookAsync([FromForm] CreateHandbookDTO createHandbookDTO, CancellationToken cancellationToken = default)
        {
            var response = await _handbookService.CreateHandbookAsync(createHandbookDTO, cancellationToken);
            return StatusCode(response.Code, response);
        }
        [HttpPut("update-handbook/{handbookId}")]
        public async Task<IActionResult> UpdateHandbookAsync(int handbookId, [FromForm] UpdateHandbookDTO updateHandbookDTO, CancellationToken cancellationToken = default)
        {
            var response = await _handbookService.UpdateHandbookAsync(handbookId, updateHandbookDTO, cancellationToken);
            return StatusCode(response.Code, response);
        }
        [HttpDelete("delete-handbook/{handbookId}")]
        public async Task<IActionResult> DeleteHandbookAsync(int handbookId, CancellationToken cancellationToken = default)
        {
            var response = await _handbookService.DeleteHandbookAsync(handbookId, cancellationToken);
            return StatusCode(response.Code, response);
        }
    }
}
