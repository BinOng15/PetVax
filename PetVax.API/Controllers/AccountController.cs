using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetVax.BusinessObjects.DTO.AccountDTO;
using PetVax.BusinessObjects.Enum;
using PetVax.Services.IService;
using System.Net;
using System.Security.Claims;

namespace PediVax.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IAccountService _accountService;
        public AccountController(ILogger<AccountController> logger, IAccountService accountService)
        {
            _logger = logger;
            _accountService = accountService;
        }

        [HttpGet("current-account")]
        [Authorize]
        public IActionResult GetCurrentAccount(CancellationToken cancellationToken)
        {
            var accountIdClaim = User.Claims
                .FirstOrDefault(a => a.Type == ClaimTypes.NameIdentifier && int.TryParse(a.Value, out _))?.Value;

            var email = User.FindFirst(ClaimTypes.Email)?.Value ?? "Unknown";
            var roleClaim = User.FindFirst(ClaimTypes.Role)?.Value ?? "Customer";

            // Chuyển đổi chuỗi role sang enum Role
            if (!Enum.TryParse<EnumList.Role>(roleClaim, true, out var role))
            {
                role = EnumList.Role.Customer; // Giá trị mặc định nếu không parse được
            }

            return Ok(new
            {
                AccountId = accountIdClaim,
                Email = email,
                Role = role // Trả về enum Role
            });
        }

        [HttpGet("get-all-accounts")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllAccounts([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string? keyWord = null, [FromQuery] bool? status = null, CancellationToken cancellationToken = default)
        {
            var request = new GetAllAccountRequestDTO
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                KeyWord = keyWord,
                Status = status
            };
            var response = await _accountService.GetAllAccountsAsync(request, cancellationToken);
            return StatusCode(response.Code, response);
        }

        [HttpGet("get-account-by-id/{accountId}")]
        public async Task<IActionResult> GetAccountById(int accountId, CancellationToken cancellationToken)
        {
            var response = await _accountService.GetAccountByIdAsync(accountId, cancellationToken);
            return StatusCode(response.Code, response);
        }

        [HttpGet("get-account-by-email/{email}")]
        public async Task<IActionResult> GetAccountByEmail(string email, CancellationToken cancellationToken)
        {
            var response = await _accountService.GetAccountByEmailAsync(email, cancellationToken);
            return StatusCode(response.Code, response);
        }

        [HttpPost("create-staff-account")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateStaffAccount([FromBody] CreateAccountDTO createAccountDTO, CancellationToken cancellationToken)
        {

            var response = await _accountService.CreateStaffAccountAsync(createAccountDTO, cancellationToken);
            return StatusCode(response.Code, response);
        }

        [HttpPost("create-vet-account")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateVetAccount([FromBody] CreateAccountDTO createAccountDTO, CancellationToken cancellationToken)
        {
            var response = await _accountService.CreateVetAccountAsync(createAccountDTO, cancellationToken);
            return StatusCode(response.Code, response);
        }

        [HttpPut("update-account/{accountId}")]
        public async Task<IActionResult> UpdateAccount(int accountId, [FromBody] UpdateAccountDTO updateAccountDTO, CancellationToken cancellationToken)
        {
            var response = await _accountService.UpdateAccountAsync(accountId, updateAccountDTO, cancellationToken);
            return StatusCode(response.Code, response);
        }

        [HttpDelete("delete-account/{accountId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteAccount(int accountId, CancellationToken cancellationToken)
        {
            var response = await _accountService.DeleteAccountAsync(accountId, cancellationToken);
            return StatusCode(response.Code, new { Message = "Account deleted successfully" });
        }
    }
}
