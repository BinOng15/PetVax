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
        [ProducesResponseType(typeof(object), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> GetCurrentAccount(CancellationToken cancellationToken)
        {
            var accountId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "AccountId")?.Value;
            if (string.IsNullOrEmpty(accountId))
            {
                return Unauthorized();
            }
            var account = await _accountService.GetAccountByIdAsync(int.Parse(accountId), cancellationToken);
            if (account == null)
            {
                return NotFound(new { Message = "Account not found" });
            }
            return Ok(account);
        }

        [HttpGet("get-all-accounts")]
        [ProducesResponseType(typeof(object), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllAccounts(CancellationToken cancellationToken)
        {
            var accounts = await _accountService.GetAllAccountsAsync(cancellationToken);
            if (accounts == null || !accounts.Any())
            {
                return NotFound(new { Message = "No accounts found" });
            }
            return Ok(accounts);
        }

        [HttpGet("get-account-by-id/{accountId}")]
        [ProducesResponseType(typeof(object), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> GetAccountById(int accountId, CancellationToken cancellationToken)
        {
            if (!User.Identity.IsAuthenticated || !User.Identity.IsAuthenticated)
            {
                return Unauthorized("Unauthorized user!!!.");
            }
            if (accountId <= 0)
            {
                return BadRequest(new { Message = "Invalid account ID" });
            }
            try
            {
                var account = await _accountService.GetAccountByIdAsync(accountId, cancellationToken);
                if (account == null)
                {
                    return NotFound(new { Message = "Account not found" });
                }
                return Ok(account);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching account with ID {id}", accountId);
                return Problem("An unexpected error occurred.");
            }
        }

        [HttpGet("get-account-by-email/{email}")]
        [ProducesResponseType(typeof(object), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> GetAccountByEmail(string email, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(email))
            {
                return BadRequest(new
                {
                    message = "Email cannot be empty!"
                });
            }
            var account = await _accountService.GetAccountByEmailAsync(email, cancellationToken);
            if (account == null)
            {
                return NotFound("No account found with the given email!");
            }
            return Ok(account);
        }
        [HttpPost("create-staff-account")]
        [ProducesResponseType(typeof(object), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateStaffAccount([FromBody] CreateAccountDTO createAccountDTO, CancellationToken cancellationToken)
        {
            if (createAccountDTO == null)
            {
                return BadRequest(new { Message = "Invalid account data" });
            }
            var account = await _accountService.CreateStaffAccountAsync(createAccountDTO, cancellationToken);
            return CreatedAtAction(nameof(GetAccountById), new { accountId = account.AccountId }, account);
        }
        [HttpPost("create-vet-account")]
        [ProducesResponseType(typeof(object), (int)HttpStatusCode.Created)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateVetAccount([FromBody] CreateAccountDTO createAccountDTO, CancellationToken cancellationToken)
        {
            if (createAccountDTO == null)
            {
                return BadRequest(new { Message = "Invalid account data" });
            }
            var account = await _accountService.CreateVetAccountAsync(createAccountDTO, cancellationToken);
            return CreatedAtAction(nameof(GetAccountById), new { accountId = account.AccountId }, account);
        }
        [HttpPut("update-account/{accountId}")]
        [ProducesResponseType(typeof(object), (int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateAccount(int accountId, [FromBody] UpdateAccountDTO updateAccountDTO, CancellationToken cancellationToken)
        {
            if (accountId <= 0 || updateAccountDTO == null)
            {
                return BadRequest(new { Message = "Invalid account ID or data" });
            }
            var result = await _accountService.UpdateAccountAsync(accountId, updateAccountDTO, cancellationToken);
            if (!result)
            {
                return NotFound(new { Message = "Account not found" });
            }
            return Ok(new { Message = "Account updated successfully" });
        }
        [HttpDelete("delete-account/{accountId}")]
        [ProducesResponseType(typeof(object), (int)HttpStatusCode.NoContent)]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteAccount(int accountId, CancellationToken cancellationToken)
        {
            if (accountId <= 0)
            {
                return BadRequest(new { Message = "Invalid account ID" });
            }
            var result = await _accountService.DeleteAccountAsync(accountId, cancellationToken);
            if (!result)
            {
                return NotFound(new { Message = "Account not found" });
            }
            return NoContent();
        }
    }
}
