using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetVax.BusinessObjects.DTO;
using PetVax.BusinessObjects.DTO.SupportCategoryDTO;
using PetVax.Services.IService;

namespace PediVax.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupportCategoryController : ControllerBase
    {
        private readonly ISupportCategoryService _supportCategoryService;

        public SupportCategoryController(ISupportCategoryService supportCategoryService)
        {
            _supportCategoryService = supportCategoryService;
        }

        [HttpGet("get-all-support-categories")]
        public async Task<IActionResult> GetAllSupportCategoriesAsync([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string? keyWord = null, [FromQuery] bool? status = true, CancellationToken cancellationToken = default)
        {
            var request = new GetAllItemsDTO
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                KeyWord = keyWord,
                Status = status
            };
            var response = await _supportCategoryService.GetAllSupportCategoriesAsync(request, cancellationToken);
            return StatusCode(response.Code, response);
        }
        [HttpGet("get-support-category-by-id/{supportCategoryId}")]
        public async Task<IActionResult> GetSupportCategoryByIdAsync(int supportCategoryId, CancellationToken cancellationToken = default)
        {
            var response = await _supportCategoryService.GetSupportCategoryByIdAsync(supportCategoryId, cancellationToken);
            return StatusCode(response.Code, response);
        }
        [HttpPost("create-support-category")]
        public async Task<IActionResult> CreateSupportCategoryAsync([FromBody] CreateSupportCategoryDTO createSupportCategoryDTO, CancellationToken cancellationToken = default)
        {
            var response = await _supportCategoryService.CreateSupportCategoryAsync(createSupportCategoryDTO, cancellationToken);
            return StatusCode(response.Code, response);
        }
        [HttpPut("update-support-category/{supportCategoryId}")]
        public async Task<IActionResult> UpdateSupportCategoryAsync(int supportCategoryId, [FromBody] UpdateSupportCategoryDTO updateSupportCategoryDTO, CancellationToken cancellationToken = default)
        {
            var response = await _supportCategoryService.UpdateSupportCategoryAsync(supportCategoryId, updateSupportCategoryDTO, cancellationToken);
            return StatusCode(response.Code, response);
        }
        [HttpDelete("delete-support-category/{supportCategoryId}")]
        public async Task<IActionResult> DeleteSupportCategoryAsync(int supportCategoryId, CancellationToken cancellationToken = default)
        {
            var response = await _supportCategoryService.DeleteSupportCategoryAsync(supportCategoryId, cancellationToken);
            return StatusCode(response.Code, response);
        }
    }
}
