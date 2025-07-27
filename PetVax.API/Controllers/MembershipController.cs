using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetVax.BusinessObjects.DTO;
using PetVax.BusinessObjects.DTO.MembershipDTO;
using PetVax.Services.IService;
using PetVax.Services.Service;

namespace PediVax.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MembershipController : ControllerBase
    {
        private readonly IMembershipService _membershipService;

        public MembershipController(IMembershipService membershipService)
        {
            _membershipService = membershipService;
        }

        [HttpGet("get-all-memberships")]
        public async Task<IActionResult> GetAllMembershipAsync([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string? keyWord = null, [FromQuery] bool? status = true, CancellationToken cancellationToken = default)
        {
            var getAllItemsDTO = new GetAllItemsDTO
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                KeyWord = keyWord,
                Status = status
            };
            var response = await _membershipService.GetAllMembershipAsync(getAllItemsDTO, cancellationToken);
            return StatusCode(response.Code, response);
        }
        [HttpGet("get-membership-by-id/{membershipId}")]
        public async Task<IActionResult> GetMembershipByIdAsync(int membershipId, CancellationToken cancellationToken = default)
        {
            var response = await _membershipService.GetMembershipByIdAsync(membershipId, cancellationToken);
            return StatusCode(response.Code, response);
        }
        [HttpGet("get-membership-by-customer-id/{customerId}")]
        public async Task<IActionResult> GetMembershipByCustomerIdAsync(int customerId, CancellationToken cancellationToken = default)
        {
            var response = await _membershipService.GetMembershipByCustomerIdAsync(customerId, cancellationToken);
            return StatusCode(response.Code, response);
        }
        [HttpGet("get-membership-by-membership-code/{membershipCode}")]
        public async Task<IActionResult> GetMembershipByMembershipCodeAsync(string membershipCode, CancellationToken cancellationToken = default)
        {
            var response = await _membershipService.GetMembershipByMembershipCodeAsync(membershipCode, cancellationToken);
            return StatusCode(response.Code, response);
        }
        [HttpGet("get-membership-by-rank/{rank}")]
        public async Task<IActionResult> GetMembershipByRankAsync(string rank, CancellationToken cancellationToken = default)
        {
            var response = await _membershipService.GetMembershipByRankAsync(rank, cancellationToken);
            return StatusCode(response.Code, response);
        }
        [HttpPost("create-membership")]
        public async Task<IActionResult> CreateMembershipAsync([FromBody] CreateMembershipDTO createMembershipDTO, CancellationToken cancellationToken = default)
        {
            var response = await _membershipService.CreateMembershipAsync(createMembershipDTO, cancellationToken);
            return StatusCode(response.Code, response);
        }
        [HttpPut("update-membership/{membershipId}")]
        public async Task<IActionResult> UpdateMembershipAsync(int membershipId, [FromBody] UpdateMembershipDTO updateMembershipDTO, CancellationToken cancellationToken = default)
        {
            var response = await _membershipService.UpdateMembershipAsync(membershipId, updateMembershipDTO, cancellationToken);
            return StatusCode(response.Code, response);
        }
        [HttpDelete("delete-membership/{membershipId}")]
        public async Task<IActionResult> DeleteMembershipAsync(int membershipId, CancellationToken cancellationToken = default)
        {
            var response = await _membershipService.DeleteMembershipAsync(membershipId, cancellationToken);
            return StatusCode(response.Code, response);
        }
        [HttpGet("get-customer-ranking-info/{customerId}")]
        public async Task<IActionResult> GetCustomerRankingInfoAsync(int customerId, CancellationToken cancellationToken = default)
        {
            var response = await _membershipService.GetCustomerRankingInfoAsync(customerId, cancellationToken);
            return StatusCode(response.Code, response);
        }
        [HttpGet("get-membership-status/{customerId}")]
        public async Task<IActionResult> GetMembershipStatusAsync(int customerId, CancellationToken cancellationToken = default)
        {
            var response = await _membershipService.GetMembershipStatusAsync(customerId, cancellationToken);
            return StatusCode(response.Code, response);
        }
    }
}
