using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PetVax.BusinessObjects.DTO.AccountDTO;
using PetVax.BusinessObjects.DTO.AuthenticateDTO;
using PetVax.Services.IService;
using System.Threading;

namespace PetVax.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginRequest, CancellationToken cancellationToken)
        {
            if (loginRequest == null)
            {
                return BadRequest("Invalid login request.");
            }

            try
            {
                var result = await _authService.LoginAsync(loginRequest, cancellationToken);
                if (!result.Success)
                {
                    return Unauthorized(result.Message);
                }

                return StatusCode(result.Code, result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtp([FromBody] OtpVerificationRequestDTO otpRequest, CancellationToken cancellationToken)
        {
            if (otpRequest == null || string.IsNullOrEmpty(otpRequest.Email) || string.IsNullOrEmpty(otpRequest.Otp))
            {
                return BadRequest("Invalid OTP verification request.");
            }

            try
            {
                var result = await _authService.VerifyOtpAsync(otpRequest.Email, otpRequest.Otp, cancellationToken);
                if (!result.Success)
                {
                    return Unauthorized(result.Message);
                }

                var authResponse = result.Data;

                Response.Cookies.Append("AccessToken", authResponse.AccessToken, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    Expires = authResponse.AccessTokenExpiration,
                    SameSite = SameSiteMode.Strict
                });

                Response.Cookies.Append("RefreshToken", authResponse.RefreshToken, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    Expires = authResponse.RefreshTokenExpiration,
                    SameSite = SameSiteMode.Strict
                });

                //return StatusCode(StatusCodes.Status200OK, new
                //{
                //    authResponse.AccountId,
                //    authResponse.Email,
                //    authResponse.Role,
                //    authResponse.AccessToken,
                //    authResponse.RefreshToken,
                //    authResponse.AccessTokenExpiration,
                //    authResponse.RefreshTokenExpiration,
                //    Message = "OTP verification successful."
                //});
                return StatusCode(result.Code, result);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("system-login")]
        public async Task<IActionResult> LoginSimple([FromBody] LoginRequestDTO loginRequest, CancellationToken cancellationToken)
        {
            if (loginRequest == null)
            {
                return BadRequest("Invalid login request.");
            }

            try
            {
                var result = await _authService.LoginSimple(loginRequest, cancellationToken);
                if (!result.Success)
                {
                    return Unauthorized(result.Message);
                }
                return StatusCode(result.Code, result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisRequestDTO regisRequest, CancellationToken cancellationToken)
        {
            if (regisRequest == null)
            {
                return BadRequest("Invalid registration request.");
            }

            try
            {
                var result = await _authService.Register(regisRequest, cancellationToken);
                if (!result.Success)
                {
                    return BadRequest(result.Message);
                }
                return StatusCode(result.Code, result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("verify-email")]
        public async Task<IActionResult> VerifyEmail([FromBody] OtpVerificationRequestDTO otpRequest, CancellationToken cancellationToken)
        {
            var response = await _authService.VerifyEmail(otpRequest.Email, otpRequest.Otp, cancellationToken);
            return Ok(response);

        }
        [HttpPost("google-login")]
        public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginRequestDTO request, CancellationToken cancellationToken)
        {
            if (request == null || string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Name))
            {
                return BadRequest("Invalid Google login request.");
            }

            try
            {
                var result = await _authService.LoginWithGoogleAsync(request.Email, request.Name, cancellationToken);
                if (!result.Success)
                {
                    return Unauthorized(result.Message);
                }

                var authResponse = result.Data;

                Response.Cookies.Append("AccessToken", authResponse.AccessToken, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    Expires = authResponse.AccessTokenExpiration,
                    SameSite = SameSiteMode.Strict
                });

                Response.Cookies.Append("RefreshToken", authResponse.RefreshToken, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    Expires = authResponse.RefreshTokenExpiration,
                    SameSite = SameSiteMode.Strict
                });

                //return StatusCode(StatusCodes.Status200OK, new
                //{
                //    authResponse.AccountId,
                //    authResponse.Email,
                //    authResponse.Role,
                //    authResponse.AccessToken,
                //    authResponse.RefreshToken,
                //    authResponse.AccessTokenExpiration,
                //    authResponse.RefreshTokenExpiration,
                //    Message = "Google login successful."
                //});
                return StatusCode(result.Code, result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("google-verify")]
        public async Task<IActionResult> GoogleVerify([FromBody] GoogleVerifyRequestDTO request, CancellationToken cancellationToken)
        {
            if (request == null || string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Token) || string.IsNullOrEmpty(request.Name))
            {
                return BadRequest("Invalid Google verification request.");
            }

            try
            {
                var result = await _authService.VerifyGoogleEmailAsync(request.Email, request.Token, request.Name, cancellationToken);
                if (!result.Success)
                {
                    return Unauthorized(result.Message);
                }

                var authResponse = result.Data;

                Response.Cookies.Append("AccessToken", authResponse.AccessToken, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    Expires = authResponse.AccessTokenExpiration,
                    SameSite = SameSiteMode.Strict
                });

                Response.Cookies.Append("RefreshToken", authResponse.RefreshToken, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    Expires = authResponse.RefreshTokenExpiration,
                    SameSite = SameSiteMode.Strict
                });

                //return StatusCode(StatusCodes.Status200OK, new
                //{
                //    authResponse.AccountId,
                //    authResponse.Email,
                //    authResponse.Role,
                //    authResponse.AccessToken,
                //    authResponse.RefreshToken,
                //    authResponse.AccessTokenExpiration,
                //    authResponse.RefreshTokenExpiration,
                //    Message = "Google verification successful."
                //});
                return StatusCode(result.Code, result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ForgetPasswordRequestDTO forgetPasswordRequest, [FromQuery] string otp, CancellationToken cancellationToken)
        {
            if (forgetPasswordRequest == null || string.IsNullOrEmpty(forgetPasswordRequest.Email) || string.IsNullOrEmpty(forgetPasswordRequest.NewPassword))
            {
                return BadRequest("Invalid reset password request.");
            }
            try
            {
                var result = await _authService.ResetPasswordAsync(forgetPasswordRequest, otp, cancellationToken);
                if (!result.Success)
                {
                    return Unauthorized(result.Message);
                }
                return StatusCode(result.Code, result);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}