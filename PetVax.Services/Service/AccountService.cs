using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
using static PetVax.BusinessObjects.DTO.ResponseModel;

namespace PetVax.Services.Service
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly IVetRepository _vetRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<AccountService> _logger;

        public AccountService(
            IAccountRepository accountRepository,
            IVetRepository vetRepository,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            ILogger<AccountService> logger)
        {
            _accountRepository = accountRepository;
            _vetRepository = vetRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public async Task<BaseResponse<AccountResponseDTO>> CreateStaffAccountAsync(CreateAccountDTO createAccountDTO, CancellationToken cancellationToken)
        {
            if (createAccountDTO == null)
            {
                return new BaseResponse<AccountResponseDTO>
                {
                    Code = 400,
                    Success = false,
                    Message = "CreateAccountDTO cannot be null"
                };
            }
            try
            {
                if (await CheckEmailExist(createAccountDTO.Email, cancellationToken))
                {
                    return new BaseResponse<AccountResponseDTO>
                    {
                        Code = 409,
                        Success = false,
                        Message = "Email already exists"
                    };
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
                    isVerify = true
                };

                var result = await _accountRepository.CreateAccountAsync(account, cancellationToken);
                if (result <= 0)
                {
                    _logger.LogError("Failed to create staff account to database. Result: {Result}", result);
                    return new BaseResponse<AccountResponseDTO>
                    {
                        Code = 500,
                        Success = false,
                        Message = "Failed to create staff account in the database"
                    };
                }

                _logger.LogInformation("Staff account created successfully: {Email}", account.Email);
                return new BaseResponse<AccountResponseDTO>
                {
                    Code = 201,
                    Success = true,
                    Message = "Staff account created successfully",
                    Data = _mapper.Map<AccountResponseDTO>(account)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating staff account");
                return new BaseResponse<AccountResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Error while saving account: " + ex.InnerException?.Message ?? ex.Message
                };
            }
        }

        public async Task<BaseResponse<AccountResponseDTO>> CreateVetAccountAsync(CreateAccountDTO createAccountDTO, CancellationToken cancellationToken)
        {
            if (createAccountDTO == null)
            {
                return new BaseResponse<AccountResponseDTO>
                {
                    Code = 400,
                    Success = false,
                    Message = "CreateAccountDTO cannot be null"
                };
            }
            try
            {
                if (await CheckEmailExist(createAccountDTO.Email, cancellationToken))
                {
                    return new BaseResponse<AccountResponseDTO>
                    {
                        Code = 409,
                        Success = false,
                        Message = "Email already exists"
                    };
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
                    isVerify = true
                };
                var result = await _accountRepository.CreateAccountAsync(account, cancellationToken);
                if (result <= 0)
                {
                    _logger.LogError("Failed to create vet account to database. Result: {Result}", result);
                    return new BaseResponse<AccountResponseDTO>
                    {
                        Code = 500,
                        Success = false,
                        Message = "Failed to create vet account in the database"
                    };
                }

                var createdAccount = await _accountRepository.GetAccountByEmailAsync(account.Email, cancellationToken);
                if (createdAccount == null)
                {
                    _logger.LogError("Failed to retrieve created account for email {Email}", account.Email);
                    return new BaseResponse<AccountResponseDTO>
                    {
                        Code = 500,
                        Success = false,
                        Message = "Failed to retrieve created account after creation"
                    };
                }

                var vet = new Vet
                {
                    AccountId = createdAccount.AccountId,
                    VetCode = "V" + Guid.NewGuid().ToString("N").Substring(0, 7).ToUpper(),
                    CreateAt = DateTime.UtcNow,
                    CreatedBy = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "system",
                };
                var vetResult = await _vetRepository.CreateVetAsync(vet, cancellationToken);
                if (vetResult <= 0)
                {
                    _logger.LogError("Failed to create Vet for account {AccountId}", createdAccount.AccountId);
                    return new BaseResponse<AccountResponseDTO>
                    {
                        Code = 500,
                        Success = false,
                        Message = "Failed to create Vet entity in the database"
                    };
                }

                _logger.LogInformation("Vet account and Vet entity created successfully for email {Email}", account.Email);
                return new BaseResponse<AccountResponseDTO>
                {
                    Code = 201,
                    Success = true,
                    Message = "Vet account created successfully",
                    Data = _mapper.Map<AccountResponseDTO>(createdAccount)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating vet account");
                return new BaseResponse<AccountResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Error while saving account: " + ex.InnerException?.Message ?? ex.Message
                };
            }
        }

        public async Task<BaseResponse<bool>> DeleteAccountAsync(int accountId, CancellationToken cancellationToken)
        {
            if (accountId <= 0)
            {
                return new BaseResponse<bool>
                {
                    Code = 400,
                    Success = false,
                    Message = "Account ID must be greater than zero",
                    Data = false
                };
            }
            try
            {
                var result = await _accountRepository.DeleteAccountAsync(accountId, cancellationToken);
                if (!result)
                {
                    _logger.LogError("Failed to delete account with ID {AccountId}", accountId);
                    return new BaseResponse<bool>
                    {
                        Code = 404,
                        Success = false,
                        Message = "No account found with the provided ID",
                        Data = false
                    };
                }
                _logger.LogInformation("Account with ID {AccountId} deleted successfully", accountId);
                return new BaseResponse<bool>
                {
                    Code = 200,
                    Success = true,
                    Message = "Account deleted successfully",
                    Data = true
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting account with ID {AccountId}", accountId);
                return new BaseResponse<bool>
                {
                    Code = 500,
                    Success = false,
                    Message = "Error while deleting account: " + ex.InnerException?.Message ?? ex.Message,
                    Data = false
                };
            }
        }

        public async Task<BaseResponse<AccountResponseDTO>> GetAccountByEmailAsync(string email, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return new BaseResponse<AccountResponseDTO>
                {
                    Code = 400,
                    Success = false,
                    Message = "Email cannot be empty"
                };
            }
            try
            {
                var account = await _accountRepository.GetAccountByEmailAsync(email, cancellationToken);
                if (account == null)
                {
                    _logger.LogWarning("No account found with email {Email}", email);
                    return new BaseResponse<AccountResponseDTO>
                    {
                        Code = 404,
                        Success = false,
                        Message = "No account found with the provided email"
                    };
                }
                _logger.LogInformation("Account retrieved successfully for email {Email}", email);
                return new BaseResponse<AccountResponseDTO>
                {
                    Code = 200,
                    Success = true,
                    Message = "Account retrieved successfully",
                    Data = _mapper.Map<AccountResponseDTO>(account)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving account by email {Email}", email);
                return new BaseResponse<AccountResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Error while retrieving account: " + ex.InnerException?.Message ?? ex.Message
                };
            }
        }

        public async Task<BaseResponse<AccountResponseDTO>> GetAccountByIdAsync(int accountId, CancellationToken cancellationToken)
        {
            if (accountId <= 0)
            {
                return new BaseResponse<AccountResponseDTO>
                {
                    Code = 400,
                    Success = false,
                    Message = "Account ID must be greater than zero"
                };
            }
            try
            {
                var account = await _accountRepository.GetAccountByIdAsync(accountId, cancellationToken);
                if (account == null)
                {
                    _logger.LogWarning("No account found with ID {AccountId}", accountId);
                    return new BaseResponse<AccountResponseDTO>
                    {
                        Code = 404,
                        Success = false,
                        Message = "No account found with the provided ID"
                    };
                }
                _logger.LogInformation("Account retrieved successfully for ID {AccountId}", accountId);
                return new BaseResponse<AccountResponseDTO>
                {
                    Code = 200,
                    Success = true,
                    Message = "Account retrieved successfully",
                    Data = _mapper.Map<AccountResponseDTO>(account)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving account by ID {AccountId}", accountId);
                return new BaseResponse<AccountResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Error while retrieving account: " + ex.InnerException?.Message ?? ex.Message
                };
            }
        }

        public async Task<DynamicResponse<AccountResponseDTO>> GetAllAccountsAsync(GetAllAccountRequestDTO getAllAccountRequestDTO, CancellationToken cancellationToken)
        {
            try
            {
                var accounts = await _accountRepository.GetAllAccountsAsync(cancellationToken);

                // Filter by keyword if provided
                if (!string.IsNullOrWhiteSpace(getAllAccountRequestDTO?.KeyWord))
                {
                    var keyword = getAllAccountRequestDTO.KeyWord.Trim().ToLower();
                    accounts = accounts
                        .Where(a => a.Email.ToLower().Contains(keyword))
                        .ToList();
                }

                // Filter by status if provided
                if (getAllAccountRequestDTO?.Status.HasValue == true)
                {
                    accounts = accounts
                        .Where(a => a.isVerify == getAllAccountRequestDTO.Status.Value)
                        .ToList();
                }

                // Pagination
                int pageNumber = getAllAccountRequestDTO?.PageNumber > 0 ? getAllAccountRequestDTO.PageNumber : 1;
                int pageSize = getAllAccountRequestDTO?.PageSize > 0 ? getAllAccountRequestDTO.PageSize : 10;
                int skip = (pageNumber - 1) * pageSize;
                int totalItem = accounts.Count;
                int totalPage = (int)Math.Ceiling((double)totalItem / pageSize);

                var pagedAccounts = accounts
                    .Skip(skip)
                    .Take(pageSize)
                    .ToList();

                var responseData = new MegaData<AccountResponseDTO>
                {
                    PageInfo = new PagingMetaData
                    {
                        Page = pageNumber,
                        Size = pageSize,
                        TotalItem = totalItem,
                        TotalPage = totalPage,
                        Sort = null,
                        Order = null
                    },
                    SearchInfo = new SearchCondition
                    {
                        keyWord = getAllAccountRequestDTO?.KeyWord,
                        status = getAllAccountRequestDTO?.Status
                    },
                    PageData = _mapper.Map<List<AccountResponseDTO>>(pagedAccounts)
                };

                if (!pagedAccounts.Any())
                {
                    _logger.LogInformation("No accounts found for the given criteria");
                    return new DynamicResponse<AccountResponseDTO>
                    {
                        Code = 404,
                        Success = false,
                        Message = "No accounts found",
                        Data = responseData
                    };
                }

                _logger.LogInformation("Retrieved {Count} accounts successfully (Page {PageNumber}, PageSize {PageSize})", pagedAccounts.Count, pageNumber, pageSize);
                return new DynamicResponse<AccountResponseDTO>
                {
                    Code = 200,
                    Success = true,
                    Message = "Accounts retrieved successfully",
                    Data = responseData
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all accounts with pagination");
                return new DynamicResponse<AccountResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Error while retrieving accounts: " + (ex.InnerException?.Message ?? ex.Message),
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<bool>> UpdateAccountAsync(int accountId, UpdateAccountDTO updateAccountDTO, CancellationToken cancellationToken)
        {
            if (accountId <= 0)
            {
                return new BaseResponse<bool>
                {
                    Code = 400,
                    Success = false,
                    Message = "Account ID must be greater than zero",
                    Data = false
                };
            }
            if (updateAccountDTO == null)
            {
                var response = new BaseResponse<bool>
                {
                    Code = 400,
                    Success = false,
                    Message = "UpdateAccountDTO cannot be null",
                    Data = false
                };
                return response;
            }
            if (updateAccountDTO.Role != null && updateAccountDTO.Role != EnumList.Role.Staff && updateAccountDTO.Role != EnumList.Role.Vet &&
                updateAccountDTO.Role != EnumList.Role.Customer)
            {
                return new BaseResponse<bool>
                {
                    Code = 400,
                    Success = false,
                    Message = "Invalid role specified",
                    Data = false
                };
            }

            try
            {
                var account = await _accountRepository.GetAccountByIdAsync(accountId, cancellationToken);
                if (account == null)
                {
                    _logger.LogWarning("No account found with ID {AccountId} for update", accountId);
                    return new BaseResponse<bool>
                    {
                        Code = 404,
                        Success = false,
                        Message = "No account found with the provided ID",
                        Data = false
                    };
                }
                if (!string.IsNullOrEmpty(updateAccountDTO.Email) && await CheckEmailExist(updateAccountDTO.Email, cancellationToken))
                {
                    _logger.LogWarning("Email {Email} already exists for update", updateAccountDTO.Email);
                    return new BaseResponse<bool>
                    {
                        Code = 409,
                        Success = false,
                        Message = "Email already exists",
                        Data = false
                    };
                }
                // Update properties
                account.Email = updateAccountDTO.Email ?? account.Email;
                if (updateAccountDTO.Role != null)
                    account.Role = updateAccountDTO.Role.Value;
                if (updateAccountDTO.IsVerify.HasValue)
                    account.isVerify = updateAccountDTO.IsVerify.Value;
                if (!string.IsNullOrEmpty(updateAccountDTO.Password))
                {
                    var salt = PasswordHelper.GenerateSalt();
                    var passwordHash = PasswordHelper.HashPassword(updateAccountDTO.Password, salt);
                    account.PasswordSalt = salt;
                    account.PasswordHash = passwordHash;
                }
                var result = await _accountRepository.UpdateAccountAsync(account, cancellationToken);
                if (result <= 0)
                {
                    _logger.LogError("Failed to update account with ID {AccountId}", accountId);
                    return new BaseResponse<bool>
                    {
                        Code = 500,
                        Success = false,
                        Message = "Failed to update account in the database",
                        Data = false
                    };
                }
                _logger.LogInformation("Account with ID {AccountId} updated successfully", accountId);
                return new BaseResponse<bool>
                {
                    Code = 200,
                    Success = true,
                    Message = "Account updated successfully",
                    Data = true
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating account with ID {AccountId}", accountId);
                return new BaseResponse<bool>
                {
                    Code = 500,
                    Success = false,
                    Message = "Error while updating account: " + ex.InnerException?.Message ?? ex.Message,
                    Data = false
                };
            }
        }

        private async Task<bool> CheckEmailExist(string email, CancellationToken cancellationToken)
        {
            var users = await _accountRepository.GetAllAccountsAsync(cancellationToken);
            return users.Any(u => u.Email == email);
        }
    }
}
