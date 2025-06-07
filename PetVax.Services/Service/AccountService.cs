using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PetVax.BusinessObjects.DTO.AccountDTO;
using PetVax.BusinessObjects.Enum;
using PetVax.BusinessObjects.Helpers;
using PetVax.BusinessObjects.Models;
using PetVax.Repositories.IRepository;
using PetVax.Services.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace PetVax.Services.Service
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<AccountService> _logger;

        public AccountService(
            IAccountRepository accountRepository,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            ILogger<AccountService> logger)
        {
            _accountRepository = accountRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public async Task<AccountResponseDTO> CreateStaffAccountAsync(CreateAccountDTO createAccountDTO, CancellationToken cancellationToken)
        {
            if (createAccountDTO == null)
            {
                throw new ArgumentNullException(nameof(createAccountDTO), "CreateAccountDTO cannot be null");
            }
            try
            {
                if (await CheckEmailExist(createAccountDTO.Email, cancellationToken))
                {
                    throw new InvalidOperationException("Email already exists");
                }

                var salt = PasswordHelper.GenerateSalt();
                var passwordHash = PasswordHelper.HashPassword(createAccountDTO.Password, salt);

                var account = new Account()
                {
                    Email = createAccountDTO.Email,
                    PasswordHash = passwordHash,
                    PasswordSalt = salt,
                    Role = EnumList.Role.Staff,
                    AccessToken = string.Empty,
                    RefereshToken = string.Empty,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "system",
                    isVerify = true // Assuming staff accounts are verified by default
                };

                var result = await _accountRepository.CreateAccountAsync(account, cancellationToken);
                if (result <= 0)
                {
                    _logger.LogError("Failed to create staff account to database. Result: {Result}", result);
                    throw new Exception("Failed to create staff account in the database");
                }

                _logger.LogInformation("Staff account created successfully", account.Email);

                return _mapper.Map<AccountResponseDTO>(account);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating staff account");
                throw new ApplicationException("Error while saving account: " + ex.InnerException?.Message, ex);
            }
        }

        public async Task<AccountResponseDTO> CreateVetAccountAsync(CreateAccountDTO createAccountDTO, CancellationToken cancellationToken)
        {
            if (createAccountDTO == null)
            {
                throw new ArgumentNullException(nameof(createAccountDTO), "CreateAccountDTO cannot be null");
            }
            try
            {
                if (await CheckEmailExist(createAccountDTO.Email, cancellationToken))
                {
                    throw new InvalidOperationException("Email already exists");
                }

                var salt = PasswordHelper.GenerateSalt();
                var passwordHash = PasswordHelper.HashPassword(createAccountDTO.Password, salt);
                var account = new Account()
                {
                    Email = createAccountDTO.Email,
                    PasswordHash = passwordHash,
                    PasswordSalt = salt,
                    Role = EnumList.Role.Vet,
                    AccessToken = string.Empty,
                    RefereshToken = string.Empty,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "system",
                    isVerify = true // Assuming vet accounts are verified by default
                };
                var result = await _accountRepository.CreateAccountAsync(account, cancellationToken);
                if (result <= 0)
                {
                    _logger.LogError("Failed to create vet account to database. Result: {Result}", result);
                    throw new Exception("Failed to create vet account in the database");
                }
                _logger.LogInformation("Vet account created successfully", account.Email);
                return _mapper.Map<AccountResponseDTO>(account);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating vet account");
                throw new ApplicationException("Error while saving account: " + ex.InnerException?.Message, ex);
            }
        }

        public async Task<bool> DeleteAccountAsync(int accountId, CancellationToken cancellationToken)
        {
            if (accountId <= 0)
            {
                throw new ArgumentException("Account ID must be greater than zero", nameof(accountId));
            }
            try
            {
                var result = await _accountRepository.DeleteAccountAsync(accountId, cancellationToken);
                if (!result)
                {
                    _logger.LogError("Failed to delete account with ID {AccountId}", accountId);
                    return false;
                }
                _logger.LogInformation("Account with ID {AccountId} deleted successfully", accountId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting account with ID {AccountId}", accountId);
                throw new ApplicationException("Error while deleting account: " + ex.InnerException?.Message, ex);
            }
        }

        public async Task<AccountResponseDTO> GetAccountByEmailAsync(string email, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentException("Email cannot be null or empty", nameof(email));
            }
            try
            {
                var account = await _accountRepository.GetAccountByEmailAsync(email, cancellationToken);
                if (account == null)
                {
                    _logger.LogWarning("No account found with email {Email}", email);
                    return null;
                }
                return _mapper.Map<AccountResponseDTO>(account);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving account by email {Email}", email);
                throw new ApplicationException("Error while retrieving account: " + ex.InnerException?.Message, ex);
            }
        }

        public async Task<AccountResponseDTO> GetAccountByIdAsync(int accountId, CancellationToken cancellationToken)
        {
            if (accountId <= 0)
            {
                throw new ArgumentException("Account ID must be greater than zero", nameof(accountId));
            }
            try
            {
                var account = await _accountRepository.GetAccountByIdAsync(accountId, cancellationToken);
                if (account == null)
                {
                    _logger.LogWarning("No account found with ID {AccountId}", accountId);
                    return null;
                }
                return _mapper.Map<AccountResponseDTO>(account);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving account by ID {AccountId}", accountId);
                throw new ApplicationException("Error while retrieving account: " + ex.InnerException?.Message, ex);
            }
        }

        public async Task<List<AccountResponseDTO>> GetAllAccountsAsync(CancellationToken cancellationToken)
        {
            try
            {
                var accounts = await _accountRepository.GetAllAccountsAsync(cancellationToken);
                if (accounts == null || !accounts.Any())
                {
                    _logger.LogInformation("No accounts found");
                    return new List<AccountResponseDTO>();
                }
                return _mapper.Map<List<AccountResponseDTO>>(accounts);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all accounts");
                throw new ApplicationException("Error while retrieving accounts: " + ex.InnerException?.Message, ex);
            }
        }

        public async Task<bool> UpdateAccountAsync(int accountId, UpdateAccountDTO updateAccountDTO, CancellationToken cancellationToken)
        {
            if (accountId <= 0)
            {
                throw new ArgumentException("Account ID must be greater than zero", nameof(accountId));
            }
            if (updateAccountDTO == null)
            {
                throw new ArgumentNullException(nameof(updateAccountDTO), "UpdateAccountDTO cannot be null");
            }
            if (updateAccountDTO.Role != null && updateAccountDTO.Role != EnumList.Role.Staff && updateAccountDTO.Role != EnumList.Role.Vet && 
                updateAccountDTO.Role != EnumList.Role.Customer)
            {
                throw new ArgumentException("Invalid role specified", nameof(updateAccountDTO.Role));
            }
            
            try
            {
                var account = await _accountRepository.GetAccountByIdAsync(accountId, cancellationToken);
                if (account == null)
                {
                    _logger.LogWarning("No account found with ID {AccountId} for update", accountId);
                    return false;
                }
                if (await CheckEmailExist(updateAccountDTO.Email, cancellationToken))
                {
                    throw new InvalidOperationException("Email already exists");
                }
                // Update properties
                account.Email = updateAccountDTO.Email ?? account.Email;
                var result = await _accountRepository.UpdateAccountAsync(account, cancellationToken);
                if (result <= 0)
                {
                    _logger.LogError("Failed to update account with ID {AccountId}", accountId);
                    return false;
                }
                _logger.LogInformation("Account with ID {AccountId} updated successfully", accountId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating account with ID {AccountId}", accountId);
                throw new ApplicationException("Error while updating account: " + ex.InnerException?.Message, ex);
            }
        }
        private async Task<bool> CheckEmailExist(string email, CancellationToken cancellationToken)
        {
            var users = await _accountRepository.GetAllAccountsAsync(cancellationToken);
            return users.Any(u => u.Email == email);
        }

    }
}
