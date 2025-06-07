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

        /// <summary>
        /// Initiates the login process by verifying credentials and sending an OTP to the user's email.
        /// </summary>
        /// <param name="loginRequest">The login request containing email and password.</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation.</param>
        /// <returns>Initial response with user info and OTP sent status.</returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginRequest, CancellationToken cancellationToken)
        {
            if (loginRequest == null)
            {
                return BadRequest("Invalid login request.");
            }

            try
            {
                var authResponse = await _authService.LoginAsync(loginRequest, cancellationToken);

                // Since OTP is sent, return user info without tokens
                return Ok(new
                {
                    authResponse.AccountId,
                    authResponse.Email,
                    authResponse.Role,
                    Message = "OTP has been sent to your email. Please verify."
                });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// Verifies the OTP and issues access and refresh tokens upon successful verification.
        /// </summary>
        /// <param name="otpRequest">The OTP request containing email and OTP.</param>
        /// <param name="cancellationToken">Cancellation token to cancel the operation.</param>
        /// <returns>Response with user info, access token, and refresh token.</returns>
        [HttpPost("verify-otp")]
        public async Task<IActionResult> VerifyOtp([FromBody] OtpVerificationRequestDTO otpRequest, CancellationToken cancellationToken)
        {
            if (otpRequest == null || string.IsNullOrEmpty(otpRequest.Email) || string.IsNullOrEmpty(otpRequest.Otp))
            {
                return BadRequest("Invalid OTP verification request.");
            }

            try
            {
                var authResponse = await _authService.VerifyOtpAsync(otpRequest.Email, otpRequest.Otp, cancellationToken);

                // Set Access Token as HttpOnly cookie
                Response.Cookies.Append("AccessToken", authResponse.AccessToken, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true, // Use true in production (HTTPS)
                    Expires = authResponse.AccessTokenExpiration,
                    SameSite = SameSiteMode.Strict
                });

                // Set Refresh Token as HttpOnly cookie
                Response.Cookies.Append("RefreshToken", authResponse.RefreshToken, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    Expires = authResponse.RefreshTokenExpiration,
                    SameSite = SameSiteMode.Strict
                });

                // Return user info without tokens in response body
                return Ok(new
                {
                    authResponse.AccountId,
                    authResponse.Email,
                    authResponse.Role,
                    authResponse.AccessToken, // This will be comment when using HttpOnly cookies
                    authResponse.RefreshToken,
                    authResponse.AccessTokenExpiration,
                    authResponse.RefreshTokenExpiration,
                    Message = "Login successful."
                });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpPost("system-login")]
        public async Task<IActionResult> LoginSimple([FromBody] LoginRequestDTO loginRequest, CancellationToken cancellationToken)
        {
            try
            {
                var response = await _authService.LoginSimple(loginRequest, cancellationToken);
                return Ok(response);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.InnerException?.Message);
                return Unauthorized(new { message = ex.Message });
            }
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisRequestDTO regisRequest, CancellationToken cancellationToken)
        {
                var response = await _authService.Register(regisRequest, cancellationToken);
                return Ok(response);
   
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
                var authResponse = await _authService.LoginWithGoogleAsync(request.Email, request.Name, cancellationToken);

                // Set Access Token as HttpOnly cookie
                Response.Cookies.Append("AccessToken", authResponse.AccessToken, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    Expires = authResponse.AccessTokenExpiration,
                    SameSite = SameSiteMode.Strict
                });

                // Set Refresh Token as HttpOnly cookie
                Response.Cookies.Append("RefreshToken", authResponse.RefreshToken, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    Expires = authResponse.RefreshTokenExpiration,
                    SameSite = SameSiteMode.Strict
                });

                // Return user info and tokens in response body
                return Ok(new
                {
                    authResponse.AccountId,
                    authResponse.Email,
                    authResponse.Role,
                    authResponse.AccessToken,
                    authResponse.RefreshToken,
                    authResponse.AccessTokenExpiration,
                    authResponse.RefreshTokenExpiration,
                    Message = "Google login successful."
                });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
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
                var authResponse = await _authService.VerifyGoogleEmailAsync(request.Email, request.Token, request.Name, cancellationToken);

                // Set Access Token as HttpOnly cookie
                Response.Cookies.Append("AccessToken", authResponse.AccessToken, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    Expires = authResponse.AccessTokenExpiration,
                    SameSite = SameSiteMode.Strict
                });

                // Set Refresh Token as HttpOnly cookie
                Response.Cookies.Append("RefreshToken", authResponse.RefreshToken, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    Expires = authResponse.RefreshTokenExpiration,
                    SameSite = SameSiteMode.Strict
                });

                // Return user info and tokens in response body
                return Ok(new
                {
                    authResponse.AccountId,
                    authResponse.Email,
                    authResponse.Role,
                    authResponse.AccessToken,
                    authResponse.RefreshToken,
                    authResponse.AccessTokenExpiration,
                    authResponse.RefreshTokenExpiration,
                    Message = "Google email verification successful."
                });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ForgetPasswordRequestDTO forgetPasswordRequest, string otp, CancellationToken cancellationToken)
        {
            if (forgetPasswordRequest == null || string.IsNullOrEmpty(forgetPasswordRequest.Email) || string.IsNullOrEmpty(forgetPasswordRequest.NewPassword))
            {
                return BadRequest("Invalid reset password request.");
            }
            try
            {
                var response = await _authService.ResetPasswordAsync(forgetPasswordRequest, otp, cancellationToken);
                return Ok(response);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

    }
}