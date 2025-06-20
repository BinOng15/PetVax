﻿using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PetVax.BusinessObjects.DTO;
using PetVax.BusinessObjects.DTO.AccountDTO;
using PetVax.BusinessObjects.DTO.AuthenticateDTO;
using PetVax.BusinessObjects.Enum;
using PetVax.BusinessObjects.Helpers;
using PetVax.BusinessObjects.Models;
using PetVax.Repositories.HandleException;
using PetVax.Repositories.IRepository;
using PetVax.Services.IService;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using static PetVax.BusinessObjects.DTO.ResponseModel;

namespace PetVax.Services.Service
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly IAccountRepository _accountRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICustomerRepository _customerRepository;

        // In-memory OTP store: Email -> (OTP, Expiration)
        private static readonly ConcurrentDictionary<string, (string Otp, DateTime Expiration)> _otpStore = new();

        public AuthService(IConfiguration configuration, IAccountRepository accountRepository, IHttpContextAccessor httpContextAccessor,
            ICustomerRepository customerRepository)
        {
            _configuration = configuration;
            _accountRepository = accountRepository;
            _httpContextAccessor = httpContextAccessor;
            _customerRepository = customerRepository;
        }

        public async Task<BaseResponse<AuthResponseDTO>> LoginAsync(LoginRequestDTO loginRequest, CancellationToken cancellationToken)
        {
            try
            {
                var account = await _accountRepository.GetAccountByEmailAsync(loginRequest.Email, cancellationToken);
                if (account == null || !VerifyPassword(loginRequest.Password, account.PasswordHash, account.PasswordSalt))
                {
                    return new BaseResponse<AuthResponseDTO>
                    {
                        Code = 401,
                        Success = false,
                        Message = "Invalid email or password.",
                        Data = null
                    };
                }

                // Generate OTP and send email
                var otp = GenerateOtp();
                var expiration = DateTime.UtcNow.AddMinutes(5);
                _otpStore[loginRequest.Email] = (otp, expiration);
                await SendOtpEmailAsync(loginRequest.Email, otp, cancellationToken);

                // Return response indicating OTP is required
                //var accessToken = GenerateJwtToken(account);
                //var refreshToken = GenerateRefreshToken();
                var response = new AuthResponseDTO
                {
                    AccountId = account.AccountId,
                    Email = account.Email,
                    Role = account.Role,
                    AccessToken = "",
                    RefreshToken = "",
                    AccessTokenExpiration = DateTime.MinValue,
                    RefreshTokenExpiration = DateTime.MinValue
                };
                return new BaseResponse<AuthResponseDTO>
                {
                    Code = 200,
                    Success = true,
                    Message = "OTP sent to email.",
                    Data = response
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<AuthResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<AuthResponseDTO>> VerifyOtpAsync(string email, string otp, CancellationToken cancellationToken)
        {
            try
            {
                if (!_otpStore.TryGetValue(email, out var otpInfo) || otpInfo.Expiration < DateTime.UtcNow || otpInfo.Otp != otp)
                {
                    return new BaseResponse<AuthResponseDTO>
                    {
                        Code = 401,
                        Success = false,
                        Message = "Invalid or expired OTP.",
                        Data = null
                    };
                }

                var account = await _accountRepository.GetAccountByEmailAsync(email, cancellationToken);
                if (account == null)
                {
                    return new BaseResponse<AuthResponseDTO>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Account not found.",
                        Data = null
                    };
                }

                _otpStore.TryRemove(email, out _);

                var accessToken = GenerateJwtToken(account);
                var refreshToken = GenerateRefreshToken();
                var response = new AuthResponseDTO
                {
                    AccountId = account.AccountId,
                    Email = account.Email,
                    Role = account.Role,
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
                    AccessTokenExpiration = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["Jwt:AccessTokenExpiration"])),
                    RefreshTokenExpiration = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["Jwt:RefreshTokenExpiration"]))
                };
                return new BaseResponse<AuthResponseDTO>
                {
                    Code = 200,
                    Success = true,
                    Message = "OTP verified successfully.",
                    Data = response
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<AuthResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        private bool VerifyPassword(string password, string storedHash, string storedSalt)
        {
            var saltBytes = Convert.FromBase64String(storedSalt);
            var hashBytes = new Rfc2898DeriveBytes(password, saltBytes, 10000, HashAlgorithmName.SHA256).GetBytes(32);
            var computedHash = Convert.ToBase64String(hashBytes);

            return Convert.ToBase64String(hashBytes) == storedHash;
        }

        private string GenerateJwtToken(Account account)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                    new Claim(JwtRegisteredClaimNames.Sub, account.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.NameIdentifier, account.AccountId.ToString()),
                    new Claim(ClaimTypes.Email, account.Email),
                    new Claim(ClaimTypes.Role, account.Role.ToString())
                };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["Jwt:AccessTokenExpiration"])),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        private string GenerateOtp()
        {
            var random = new Random();
            return random.Next(100000, 999999).ToString();
        }

        private async Task SendOtpEmailAsync(string toEmail, string otp, CancellationToken cancellationToken)
        {
            var smtpHost = _configuration["Smtp:Host"];
            var smtpPort = int.Parse(_configuration["Smtp:Port"]);
            var smtpUser = _configuration["Smtp:User"];
            var smtpPass = _configuration["Smtp:Pass"];
            var fromEmail = _configuration["Smtp:From"];

            using var client = new SmtpClient(smtpHost, smtpPort)
            {
                Credentials = new NetworkCredential(smtpUser, smtpPass),
                EnableSsl = true
            };

            var mail = new MailMessage(fromEmail, toEmail)
            {
                Subject = "Your OTP Code",
                Body = $@"
                    <html>
                    <body style='font-family: Arial, sans-serif; background-color: #f6f6f6; padding: 30px;'>
                    <div style='max-width: 500px; margin: auto; background: #fff; border-radius: 8px; box-shadow: 0 2px 8px rgba(0,0,0,0.05); padding: 32px;'>
                    <h2 style='color: #2d8cf0; text-align: center;'>PetVax Verification Code</h2>
                    <p style='font-size: 16px; color: #333;'>Dear user,</p>
                    <p style='font-size: 16px; color: #333;'>Your One-Time Password (OTP) for login is:</p>
                    <div style='text-align: center; margin: 24px 0;'>
                    <span style='display: inline-block; font-size: 32px; letter-spacing: 8px; color: #2d8cf0; font-weight: bold; background: #f0f7ff; padding: 16px 32px; border-radius: 6px;'>{otp}</span>
                    </div>
                    <p style='font-size: 15px; color: #555;'>This code is valid for <b>5 minutes</b>. Please do not share this code with anyone.</p>
                    <p style='font-size: 14px; color: #aaa; margin-top: 32px;'>If you did not request this code, please ignore this email.</p>
                    <p style='font-size: 14px; color: #aaa;'>Thank you,<br/>PetVax Team</p>
                    </div>
                    </body>
                    </html>",
                IsBodyHtml = true
            };

            await client.SendMailAsync(mail, cancellationToken);
        }

        public async Task<BaseResponse<AuthResponseDTO>> LoginSimple(LoginRequestDTO loginRequestDTO, CancellationToken cancellationToken)
        {
            try
            {
                var account = await _accountRepository.GetAccountByEmailAsync(loginRequestDTO.Email, cancellationToken);
                if (account == null || !VerifyPassword(loginRequestDTO.Password, account.PasswordHash, account.PasswordSalt))
                {
                    return new BaseResponse<AuthResponseDTO>
                    {
                        Code = 401,
                        Success = false,
                        Message = "Invalid email or password.",
                        Data = null
                    };
                }

                var accessToken = GenerateJwtToken(account);
                var refreshToken = GenerateRefreshToken();

                AuthResponseDTO authResponseDTO = new AuthResponseDTO()
                {
                    AccountId = account.AccountId,
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
                    Email = account.Email,
                    Role = account.Role,
                    AccessTokenExpiration = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["Jwt:AccessTokenExpiration"])),
                    RefreshTokenExpiration = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["Jwt:RefreshTokenExpiration"]))
                };
                return new BaseResponse<AuthResponseDTO>
                {
                    Code = 200,
                    Success = true,
                    Message = "Login successful.",
                    Data = authResponseDTO
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<AuthResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        public async Task<BaseResponse> Register(RegisRequestDTO regisRequestDTO, CancellationToken cancellationToken)
        {
            try
            {
                var account = await _accountRepository.GetAccountByEmailAsync(regisRequestDTO.Email, cancellationToken);
                if (account != null)
                {
                    return new BaseResponse
                    {
                        Code = 409,
                        Success = false,
                        Message = "The account already exists!"
                    };
                }

                // Generate OTP and send email
                var otp = GenerateOtp();
                var expiration = DateTime.UtcNow.AddMinutes(5);
                _otpStore[regisRequestDTO.Email] = (otp, expiration);
                await SendOtpEmailAsync(regisRequestDTO.Email, otp, cancellationToken);

                // password
                string passwordSalt = PasswordHelper.GenerateSalt();
                string passwordHash = PasswordHelper.HashPassword(regisRequestDTO.Password, passwordSalt);

                Account newAccount = new Account
                {
                    Email = regisRequestDTO.Email,
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt,
                    Role = EnumList.Role.Customer,
                    CreatedAt = DateTime.UtcNow,
                    isVerify = false
                };

                await _accountRepository.CreateAccountAsync(newAccount, cancellationToken);

                return new BaseResponse
                {
                    Code = 200,
                    Success = true,
                    Message = "Register successfully. Please check your email to verify OTP!"
                };
            }
            catch (ErrorException ex)
            {
                return new BaseResponse
                {
                    Code = ex.ErrorCode,
                    Success = false,
                    Message = ex.Message
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse
                {
                    Code = 500,
                    Success = false,
                    Message = "Can not save to database: " + ex.Message
                };
            }
        }

        public async Task<BaseResponse> VerifyEmail(string email, string otp, CancellationToken cancellationToken)
        {
            try
            {
                if (!_otpStore.TryGetValue(email, out var otpInfo) || otpInfo.Expiration < DateTime.UtcNow || otpInfo.Otp != otp)
                {
                    return new BaseResponse
                    {
                        Code = 401,
                        Success = false,
                        Message = "Invalid or expired OTP."
                    };
                }

                var account = await _accountRepository.GetAccountByEmailAsync(email, cancellationToken);
                if (account == null)
                {
                    return new BaseResponse
                    {
                        Code = 404,
                        Success = false,
                        Message = "Account not found."
                    };
                }

                account.isVerify = true;
                int update = await _accountRepository.UpdateAccountAsync(account, cancellationToken);

                if (update > 0)
                {
                    // Generate random customer code: C + 6 digits
                    string customerCode = "C" + new Random().Next(100000, 1000000).ToString();
                    Customer customer = new Customer
                    {
                        AccountId = account.AccountId,
                        CreatedAt = DateTime.UtcNow,
                        CustomerCode = customerCode
                    };
                    await _customerRepository.CreateCustomerAsync(customer);
                }

                _otpStore.TryRemove(email, out _);

                return new BaseResponse
                {
                    Code = 200,
                    Success = true,
                    Message = "Email verified successfully!"
                };
            }
            catch (ErrorException ex)
            {
                var errorData = new ErrorResponseModel(ex.ErrorCode, ex.Message);
                return new BaseResponse
                {
                    Code = 500,
                    Success = false,
                    Message = errorData.Message
                };
            }
        }
        public async Task<BaseResponse<AuthResponseDTO>> LoginWithGoogleAsync(string email, string name, CancellationToken cancellationToken)
        {
            try
            {
                // Check if account exists
                var account = await _accountRepository.GetAccountByEmailAsync(email, cancellationToken);

                if (account == null)
                {
                    // Generate email verification token
                    var verificationToken = Guid.NewGuid().ToString();
                    var tokenExpiration = DateTime.UtcNow.AddMinutes(30);

                    // Store token in-memory (for demo; use persistent store in production)
                    _otpStore[email] = (verificationToken, tokenExpiration);

                    // Send verification email
                    await SendGoogleVerificationEmailAsync(email, verificationToken, cancellationToken);

                    // Return response indicating verification required
                    return new BaseResponse<AuthResponseDTO>
                    {
                        Code = 202,
                        Success = true,
                        Message = "Verification email sent.",
                        Data = new AuthResponseDTO
                        {
                            AccountId = 0,
                            Email = email,
                            Role = EnumList.Role.Customer,
                            AccessToken = null,
                            RefreshToken = null,
                            AccessTokenExpiration = DateTime.MinValue,
                            RefreshTokenExpiration = DateTime.MinValue
                        }
                    };
                }

                // Generate access token and refresh token
                var accessToken = GenerateJwtToken(account);
                var refreshToken = GenerateRefreshToken();

                return new BaseResponse<AuthResponseDTO>
                {
                    Code = 200,
                    Success = true,
                    Message = "Login with Google successful.",
                    Data = new AuthResponseDTO
                    {
                        AccountId = account.AccountId,
                        Email = account.Email,
                        Role = account.Role,
                        AccessToken = accessToken,
                        RefreshToken = refreshToken,
                        AccessTokenExpiration = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["Jwt:AccessTokenExpiration"])),
                        RefreshTokenExpiration = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["Jwt:RefreshTokenExpiration"]))
                    }
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<AuthResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        // Call this method from your controller when user clicks the verification link
        public async Task<BaseResponse<AuthResponseDTO>> VerifyGoogleEmailAsync(string email, string token, string name, CancellationToken cancellationToken)
        {
            try
            {
                if (!_otpStore.TryGetValue(email, out var tokenInfo) || tokenInfo.Expiration < DateTime.UtcNow || tokenInfo.Otp != token)
                {
                    return new BaseResponse<AuthResponseDTO>
                    {
                        Code = 401,
                        Success = false,
                        Message = "Invalid or expired verification link.",
                        Data = null
                    };
                }

                var account = await _accountRepository.GetAccountByEmailAsync(email, cancellationToken);
                if (account == null)
                {
                    // Create new account after verification
                    account = new Account
                    {
                        Email = email,
                        Role = EnumList.Role.Customer,
                        CreatedAt = DateTime.UtcNow
                    };
                    await _accountRepository.CreateAccountAsync(account, cancellationToken);
                }

                _otpStore.TryRemove(email, out _);

                var accessToken = GenerateJwtToken(account);
                var refreshToken = GenerateRefreshToken();

                return new BaseResponse<AuthResponseDTO>
                {
                    Code = 200,
                    Success = true,
                    Message = "Email verified and account created.",
                    Data = new AuthResponseDTO
                    {
                        AccountId = account.AccountId,
                        Email = account.Email,
                        Role = account.Role,
                        AccessToken = accessToken,
                        RefreshToken = refreshToken,
                        AccessTokenExpiration = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["Jwt:AccessTokenExpiration"])),
                        RefreshTokenExpiration = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["Jwt:RefreshTokenExpiration"]))
                    }
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<AuthResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = ex.Message,
                    Data = null
                };
            }
        }

        private async Task SendGoogleVerificationEmailAsync(string toEmail, string token, CancellationToken cancellationToken)
        {
            var smtpHost = _configuration["Smtp:Host"];
            var smtpPort = int.Parse(_configuration["Smtp:Port"]);
            var smtpUser = _configuration["Smtp:User"];
            var smtpPass = _configuration["Smtp:Pass"];
            var fromEmail = _configuration["Smtp:From"];
            var verifyUrl = $"{_configuration["App:BaseUrl"]}/api/auth/verify-google?email={Uri.EscapeDataString(toEmail)}&token={Uri.EscapeDataString(token)}";

            using var client = new SmtpClient(smtpHost, smtpPort)
            {
                Credentials = new NetworkCredential(smtpUser, smtpPass),
                EnableSsl = true
            };

            var mail = new MailMessage(fromEmail, toEmail)
            {
                Subject = "PetVax - Verify your email",
                Body = $@"
                    <html>
                    <body style='font-family: Arial, sans-serif; background-color: #f6f6f6; padding: 30px;'>
                    <div style='max-width: 500px; margin: auto; background: #fff; border-radius: 8px; box-shadow: 0 2px 8px rgba(0,0,0,0.05); padding: 32px;'>
                    <h2 style='color: #2d8cf0; text-align: center;'>PetVax Email Verification</h2>
                    <p style='font-size: 16px; color: #333;'>Dear user,</p>
                    <p style='font-size: 16px; color: #333;'>Please verify your email address to complete your Google login:</p>
                    <div style='text-align: center; margin: 24px 0;'>
                    <a href='{verifyUrl}' style='display: inline-block; font-size: 18px; color: #fff; background: #2d8cf0; padding: 14px 32px; border-radius: 6px; text-decoration: none;'>Verify Email</a>
                    </div>
                    <p style='font-size: 15px; color: #555;'>This link is valid for <b>30 minutes</b>. If you did not request this, please ignore this email.</p>
                    <p style='font-size: 14px; color: #aaa; margin-top: 32px;'>Thank you,<br/>PetVax Team</p>
                    </div>
                    </body>
                    </html>",
                IsBodyHtml = true
            };

            await client.SendMailAsync(mail, cancellationToken);
        }

        public async Task<BaseResponse<ForgetPasswordResponseDTO>> ResetPasswordAsync(ForgetPasswordRequestDTO forgetPasswordRequestDTO, CancellationToken cancellationToken)
        {
            try
            {
                // Check if account exists
                var account = await _accountRepository.GetAccountByEmailAsync(forgetPasswordRequestDTO.Email, cancellationToken);
                if (account == null)
                {
                    return new BaseResponse<ForgetPasswordResponseDTO>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Account not found.",
                        Data = null
                    };
                }

                // Verify old password
                if (!VerifyPassword(forgetPasswordRequestDTO.OldPassword, account.PasswordHash, account.PasswordSalt))
                {
                    return new BaseResponse<ForgetPasswordResponseDTO>
                    {
                        Code = 401,
                        Success = false,
                        Message = "Old password is incorrect.",
                        Data = null
                    };
                }

                // Generate new password hash and salt
                string newPasswordSalt = PasswordHelper.GenerateSalt();
                string newPasswordHash = PasswordHelper.HashPassword(forgetPasswordRequestDTO.NewPassword, newPasswordSalt);

                // Update account password
                account.PasswordHash = newPasswordHash;
                account.PasswordSalt = newPasswordSalt;
                await _accountRepository.UpdateAccountAsync(account, cancellationToken);

                return new BaseResponse<ForgetPasswordResponseDTO>
                {
                    Code = 200,
                    Success = true,
                    Message = "Password reset successfully.",
                    Data = new ForgetPasswordResponseDTO
                    {
                        Message = "Password reset successfully.",
                        IsSuccess = true
                    }
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<ForgetPasswordResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = ex.Message,
                    Data = null
                };
            }
        }

    }
}
