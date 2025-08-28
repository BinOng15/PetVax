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
                    Message = "Dữ liệu tạo tài khoản cho staff không được để trống"
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
                        Message = "Email này đã có trong hệ thống"
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
                    CreatedAt = DateTimeHelper.Now(),
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
                        Message = "Lỗi khi tạo tài khoản cho staff"
                    };
                }

                _logger.LogInformation("Staff account created successfully: {Email}", account.Email);
                return new BaseResponse<AccountResponseDTO>
                {
                    Code = 201,
                    Success = true,
                    Message = "Tạo tài khoản cho staff thành công",
                    Data = _mapper.Map<AccountResponseDTO>(account)
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi tạo tài khoản cho staff");
                return new BaseResponse<AccountResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Lỗi khi tạo staff: " + ex.InnerException?.Message ?? ex.Message
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
                    Message = "Lỗi khi tạo tài khoản cho Vet: Dữ liệu không được để trống"
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
                        Message = "Email này đã có trong hệ thống"
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
                    CreatedAt = DateTimeHelper.Now(),
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
                        Message = "Lỗi khi tạo tài khoản cho Vet"
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
                        Message = "Lỗi khi lấy thông tin tài khoản đã tạo"
                    };
                }

                var random = new Random();
                var vet = new Vet
                {
                    AccountId = createdAccount.AccountId,
                    VetCode = "V" + random.Next(0, 1000000).ToString("D6"),
                    CreateAt = DateTimeHelper.Now(),
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
                        Message = "Lỗi khi tạo tài khoản cho Vet",
                    };
                }

                _logger.LogInformation("Vet account and Vet entity created successfully for email {Email}", account.Email);
                return new BaseResponse<AccountResponseDTO>
                {
                    Code = 201,
                    Success = true,
                    Message = "Tạo tài khoản cho Vet thành công",
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
                    Message = "Lỗi khi tạo tài khoản cho Vet: " + ex.InnerException?.Message ?? ex.Message
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
                    Message = "Tài khoản phải có id lớn hơn 0",
                    Data = false
                };
            }
            try
            {
                var account = await _accountRepository.GetAccountByIdAsync(accountId, cancellationToken);
                if (account == null)
                {
                    _logger.LogError("Failed to find account with ID {AccountId} for soft delete", accountId);
                    return new BaseResponse<bool>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Không tìm thấy tài khoản với ID " + accountId,
                        Data = false
                    };
                }

                account.isDeleted = true;
                var result = await _accountRepository.UpdateAccountAsync(account, cancellationToken);
                if (result <= 0)
                {
                    _logger.LogError("Failed to soft delete account with ID {AccountId}", accountId);
                    return new BaseResponse<bool>
                    {
                        Code = 500,
                        Success = false,
                        Message = "Lỗi khi xóa mềm tài khoản",
                        Data = false
                    };
                }
                _logger.LogInformation("Account with ID {AccountId} soft deleted successfully", accountId);
                return new BaseResponse<bool>
                {
                    Code = 200,
                    Success = true,
                    Message = "Tài khoản đã được xóa mềm thành công",
                    Data = true
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error soft deleting account with ID {AccountId}", accountId);
                return new BaseResponse<bool>
                {
                    Code = 500,
                    Success = false,
                    Message = "Lỗi khi xóa mềm tài khoản: " + (ex.InnerException?.Message ?? ex.Message),
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
                    Message = "Email không được để trống hoặc chỉ chứa khoảng trắng",
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
                        Code = 200,
                        Success = false,
                        Message = "Không tìm thấy tài khoản với email " + email,
                        Data = null
                    };
                }
                _logger.LogInformation("Account retrieved successfully for email {Email}", email);
                return new BaseResponse<AccountResponseDTO>
                {
                    Code = 200,
                    Success = true,
                    Message = "Tài khoản đã được lấy thành công",
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
                    Message = "Lỗi khi lấy tài khoản: " + ex.InnerException?.Message ?? ex.Message,
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
                    Message = "Tài khoản phải có id lớn hơn 0",
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
                        Code = 200,
                        Success = false,
                        Message = "Không tìm thấy tài khoản với ID " + accountId,
                        Data = null
                    };
                }
                _logger.LogInformation("Account retrieved successfully for ID {AccountId}", accountId);
                return new BaseResponse<AccountResponseDTO>
                {
                    Code = 200,
                    Success = true,
                    Message = "Tài khoản đã được lấy thành công",
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
                    Message = "Lỗi khi lấy tài khoản: " + (ex.InnerException?.Message ?? ex.Message),
                };
            }
        }

        public async Task<DynamicResponse<AccountResponseDTOs>> GetAllAccountsAsync(GetAllAccountRequestDTO getAllAccountRequestDTO, CancellationToken cancellationToken)
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

                var responseData = new MegaData<AccountResponseDTOs>
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
                    PageData = _mapper.Map<List<AccountResponseDTOs>>(pagedAccounts)
                };

                if (!pagedAccounts.Any())
                {
                    _logger.LogInformation("No accounts found for the given criteria");
                    return new DynamicResponse<AccountResponseDTOs>
                    {
                        Code = 200,
                        Success = true,
                        Message = "Không tìm thấy tài khoản nào phù hợp với tiêu chí tìm kiếm",
                        Data = null
                    };
                }

                _logger.LogInformation("Retrieved {Count} accounts successfully (Page {PageNumber}, PageSize {PageSize})", pagedAccounts.Count, pageNumber, pageSize);
                return new DynamicResponse<AccountResponseDTOs>
                {
                    Code = 200,
                    Success = true,
                    Message = "Đã lấy danh sách tài khoản thành công",
                    Data = responseData
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all accounts with pagination");
                return new DynamicResponse<AccountResponseDTOs>
                {
                    Code = 500,
                    Success = false,
                    Message = "Lỗi khi lấy danh sách tài khoản: " + (ex.InnerException?.Message ?? ex.Message),
                    Data = null
                };
            }
        }

        public async Task<DynamicResponse<AccountResponseDTO>> GetAllStaffAccountsAsync(GetAllAccountRequestDTO getAllAccountRequestDTO, CancellationToken cancellationToken)
        {
            try
            {
                var accounts = await _accountRepository.GetAllAccountsAsync(cancellationToken);
                accounts = accounts.Where(a => a.Role == EnumList.Role.Staff).ToList();
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
                    _logger.LogInformation("No staff accounts found for the given criteria");
                    return new DynamicResponse<AccountResponseDTO>
                    {
                        Code = 200,
                        Success = true,
                        Message = "Không tìm thấy tài khoản nhân viên nào phù hợp với tiêu chí tìm kiếm",
                        Data = null
                    };
                }
                _logger.LogInformation("Retrieved {Count} staff accounts successfully (Page {PageNumber}, PageSize {PageSize})", pagedAccounts.Count, pageNumber, pageSize);
                return new DynamicResponse<AccountResponseDTO>
                {
                    Code = 200,
                    Success = true,
                    Message = "Đã lấy danh sách tài khoản nhân viên thành công",
                    Data = responseData
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all staff accounts with pagination");
                return new DynamicResponse<AccountResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Lỗi khi lấy danh sách tài khoản nhân viên: " + (ex.InnerException?.Message ?? ex.Message),
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<AccountResponseDTO>> GetCurrentAccountAsync(CancellationToken cancellationToken)
        {
            try
            {
                var httpContext = _httpContextAccessor.HttpContext;
                if (httpContext == null || httpContext.User == null || !httpContext.User.Identity.IsAuthenticated)
                {
                    return new BaseResponse<AccountResponseDTO>
                    {
                        Code = 401,
                        Success = false,
                        Message = "Không xác thực được người dùng hiện tại."
                    };
                }

                // Try to get accountId from claims
                var accountIdClaim = httpContext.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);
                if (accountIdClaim == null || !int.TryParse(accountIdClaim.Value, out int accountId))
                {
                    return new BaseResponse<AccountResponseDTO>
                    {
                        Code = 401,
                        Success = false,
                        Message = "Không tìm thấy thông tin tài khoản trong token."
                    };
                }

                var account = await _accountRepository.GetAccountByIdAsync(accountId, cancellationToken);
                if (account == null)
                {
                    return new BaseResponse<AccountResponseDTO>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Không tìm thấy tài khoản."
                    };
                }

                // If role is Vet, try to get vetId
                int vetId = 0;
                if (account.Role == EnumList.Role.Vet)
                {
                    var vet = await _vetRepository.GetVetByAccountIdAsync(accountId, cancellationToken);
                    if (vet != null)
                    {
                        vetId = vet.VetId;
                    }
                }

                var dto = _mapper.Map<AccountResponseDTO>(account);
                dto.VetId = vetId;

                return new BaseResponse<AccountResponseDTO>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy thông tin tài khoản hiện tại thành công.",
                    Data = dto
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Lỗi khi lấy thông tin tài khoản hiện tại.");
                return new BaseResponse<AccountResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Lỗi khi lấy thông tin tài khoản hiện tại: " + (ex.InnerException?.Message ?? ex.Message)
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
                    Message = "Tài khoản phải có id lớn hơn 0",
                    Data = false
                };
            }
            if (updateAccountDTO == null)
            {
                var response = new BaseResponse<bool>
                {
                    Code = 400,
                    Success = false,
                    Message = "Dữ liệu cập nhật tài khoản không được để trống",
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
                    Message = "Vai trò không hợp lệ. Chỉ có thể là Staff, Vet hoặc Customer",
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
                        Message = "Không tìm thấy tài khoản với ID " + accountId,
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
                        Message = "Email này đã có trong hệ thống",
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
                        Message = "Lỗi khi cập nhật tài khoản",
                        Data = false
                    };
                }
                _logger.LogInformation("Account with ID {AccountId} updated successfully", accountId);
                return new BaseResponse<bool>
                {
                    Code = 200,
                    Success = true,
                    Message = "Tài khoản đã được cập nhật thành công",
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
                    Message = "Lỗi khi cập nhật tài khoản: " + (ex.InnerException?.Message ?? ex.Message),
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
