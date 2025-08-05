using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetVax.BusinessObjects.DTO;
using PetVax.BusinessObjects.DTO.FAQItemDTO;
using PetVax.Services.IService;
using PetVax.Services.Service;

namespace PediVax.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FAQController : ControllerBase
    {
        private readonly IFAQItemService _faqItemService;

        public FAQController(IFAQItemService faqItemService)
        {
            _faqItemService = faqItemService;
        }

        [HttpGet("get-all-faqs")]
        public async Task<IActionResult> GetAllFAQsAsync([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string? keyWord = null, [FromQuery] bool? status = true, CancellationToken cancellationToken = default)
        {
            var request = new GetAllItemsDTO
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                KeyWord = keyWord,
                Status = status
            };
            var response = await _faqItemService.GetAllFAQsAsync(request, cancellationToken);
            return StatusCode(response.Code, response);
        }
        [HttpGet("get-faq-by-id/{faqId}")]
        public async Task<IActionResult> GetFAQByIdAsync(int faqId, CancellationToken cancellationToken = default)
        {
            var response = await _faqItemService.GetFAQByIdAsync(faqId, cancellationToken);
            return StatusCode(response.Code, response);
        }
        [HttpPost("create-faq")]
        public async Task<IActionResult> CreateFAQAsync([FromBody] CreateFAQDTO createFAQDTO, CancellationToken cancellationToken = default)
        {
            var response = await _faqItemService.CreateFAQAsync(createFAQDTO, cancellationToken);
            return StatusCode(response.Code, response);
        }
        [HttpPut("update-faq/{faqId}")]
        public async Task<IActionResult> UpdateFAQAsync(int faqId, [FromBody] UpdateFAQDTO updateFAQDTO, CancellationToken cancellationToken = default)
        {
            var response = await _faqItemService.UpdateFAQAsync(faqId, updateFAQDTO, cancellationToken);
            return StatusCode(response.Code, response);
        }
        [HttpDelete("delete-faq/{faqId}")]
        public async Task<IActionResult> DeleteFAQAsync(int faqId, CancellationToken cancellationToken = default)
        {
            var response = await _faqItemService.DeleteFAQAsync(faqId, cancellationToken);
            return StatusCode(response.Code, response);
        }
    }
}
