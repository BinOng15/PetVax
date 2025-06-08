using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetVax.BusinessObjects.DTO.AccountDTO;
using PetVax.BusinessObjects.Enum;
using PetVax.Services.IService;
using System.Net;

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
        public async Task<IActionResult> GetCurrentAccount(CancellationToken cancellationToken)
        {
            var accountId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "AccountId")?.Value;
            if (string.IsNullOrEmpty(accountId))
            {
                return Unauthorized();
            }
            var response = await _accountService.GetAccountByIdAsync(int.Parse(accountId), cancellationToken);
            if (!response.Success || response.Data == null)
            {
                return StatusCode(response.Code, new { Message = response.Message ?? "Account not found" });
            }
            return StatusCode(response.Code, response);
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
            if (!response.Success || response.Data == null)
            {
                return StatusCode(response.Code, new { Message = response.Message ?? "No accounts found" });
            }
            return StatusCode(response.Code, response);
        }

        [HttpGet("get-account-by-id/{accountId}")]
        public async Task<IActionResult> GetAccountById(int accountId, CancellationToken cancellationToken)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Unauthorized("Unauthorized user!!!.");
            }
            if (accountId <= 0)
            {
                return BadRequest(new { Message = "Invalid account ID" });
            }
            try
            {
                var response = await _accountService.GetAccountByIdAsync(accountId, cancellationToken);
                if (!response.Success || response.Data == null)
                {
                    return StatusCode(response.Code, new { Message = response.Message ?? "Account not found" });
                }
                return StatusCode(response.Code, response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving account by ID: {AccountId}", accountId);
                return StatusCode((int)HttpStatusCode.InternalServerError, new { Message = "An error occurred while retrieving the account." });
            }
        }

        [HttpGet("get-account-by-email/{email}")]
        public async Task<IActionResult> GetAccountByEmail(string email, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(email))
            {
                return BadRequest(new { Message = "Email cannot be empty!" });
            }
            var response = await _accountService.GetAccountByEmailAsync(email, cancellationToken);
            if (!response.Success || response.Data == null)
            {
                return StatusCode(response.Code, new { Message = response.Message ?? "Account not found" });
            }
            return StatusCode(response.Code, response);
        }

        [HttpPost("create-staff-account")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateStaffAccount([FromBody] CreateAccountDTO createAccountDTO, CancellationToken cancellationToken)
        {
            if (createAccountDTO == null)
            {
                return BadRequest(new { Message = "Invalid account data" });
            }
            var response = await _accountService.CreateStaffAccountAsync(createAccountDTO, cancellationToken);
            if (!response.Success || response.Data == null)
            {
                return StatusCode(response.Code, new { Message = response.Message ?? "Failed to create staff account" });
            }
            return StatusCode(response.Code, response);
        }

        [HttpPost("create-vet-account")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateVetAccount([FromBody] CreateAccountDTO createAccountDTO, CancellationToken cancellationToken)
        {
            if (createAccountDTO == null)
            {
                return BadRequest(new { Message = "Invalid account data" });
            }
            var response = await _accountService.CreateVetAccountAsync(createAccountDTO, cancellationToken);
            if (!response.Success || response.Data == null)
            {
                return StatusCode(response.Code, new { Message = response.Message ?? "Failed to create vet account" });
            }
            return StatusCode(response.Code, response);
        }

        [HttpPut("update-account/{accountId}")]
        public async Task<IActionResult> UpdateAccount(int accountId, [FromBody] UpdateAccountDTO updateAccountDTO, CancellationToken cancellationToken)
        {
            if (accountId <= 0 || updateAccountDTO == null)
            {
                return BadRequest(new { Message = "Invalid account ID or data" });
            }
            var response = await _accountService.UpdateAccountAsync(accountId, updateAccountDTO, cancellationToken);
            if (!response.Success || response.Data == false)
            {
                return StatusCode(response.Code, new { Message = response.Message ?? "Failed to update account" });
            }
            return StatusCode(response.Code, response);
        }

        [HttpDelete("delete-account/{accountId}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteAccount(int accountId, CancellationToken cancellationToken)
        {
            if (accountId <= 0)
            {
                return BadRequest(new { Message = "Invalid account ID" });
            }
            var response = await _accountService.DeleteAccountAsync(accountId, cancellationToken);
            if (!response.Success || response.Data == false)
            {
                return StatusCode(response.Code, new { Message = response.Message ?? "Failed to delete account" });
            }
            return StatusCode(response.Code, new { Message = "Account deleted successfully" });
        }
    }
}
