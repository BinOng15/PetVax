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
            var result = await _authService.LoginAsync(loginRequest, cancellationToken);
            return StatusCode(result.Code, result);
        }

        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtp([FromBody] OtpVerificationRequestDTO otpRequest, CancellationToken cancellationToken)
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

        [HttpPost("system-login")]
        public async Task<IActionResult> LoginSimple([FromBody] LoginRequestDTO loginRequest, CancellationToken cancellationToken)
        {
            var result = await _authService.LoginSimple(loginRequest, cancellationToken);
            return StatusCode(result.Code, result);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisRequestDTO regisRequest, CancellationToken cancellationToken)
        {
            var result = await _authService.Register(regisRequest, cancellationToken);
            return StatusCode(result.Code, result);
        }

        [HttpPost("verify-email")]
        public async Task<IActionResult> VerifyEmail([FromBody] OtpVerificationRequestDTO otpRequest, CancellationToken cancellationToken)
        {
            var response = await _authService.VerifyEmail(otpRequest.Email, otpRequest.Otp, cancellationToken);
            return StatusCode(response.Code, response);

        }
        [HttpPost("google-login")]
        public async Task<IActionResult> GoogleLogin([FromBody] GoogleLoginRequestDTO request, CancellationToken cancellationToken)
        {
            var result = await _authService.LoginWithGoogleAsync(request.Email, request.Name, cancellationToken);
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

        [HttpPost("google-verify")]
        public async Task<IActionResult> GoogleVerify([FromBody] GoogleVerifyRequestDTO request, CancellationToken cancellationToken)
        {
            var result = await _authService.VerifyGoogleEmailAsync(request.Email, request.Token, request.Name, cancellationToken);
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

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromForm] ForgetPasswordRequestDTO forgetPasswordRequest, CancellationToken cancellationToken)
        {
            var result = await _authService.ResetPasswordAsync(forgetPasswordRequest, cancellationToken);
            return StatusCode(result.Code, result);
        }
    }
}