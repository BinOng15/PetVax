using PetVax.BusinessObjects.DTO;
using PetVax.BusinessObjects.DTO.AccountDTO;
using PetVax.BusinessObjects.DTO.AuthenticateDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PetVax.BusinessObjects.DTO.ResponseModel;
using System.ComponentModel.Design.Serialization;

namespace PetVax.Services.IService
{
    public interface IAuthService
    {
        Task<BaseResponse<AuthResponseDTO>> LoginAsync(LoginRequestDTO loginRequestDTO, CancellationToken cancellationToken);
        Task<BaseResponse<AuthResponseDTO>> VerifyOtpAsync(string email, string otp, CancellationToken cancellationToken);
        Task<BaseResponse<AuthResponseDTO>> LoginSimple(LoginRequestDTO loginRequestDTO, CancellationToken cancellationToken);
        Task<BaseResponse> Register(RegisRequestDTO regisRequestDTO, CancellationToken cancellationToken);
        Task<BaseResponse<AuthResponseDTO>> LoginWithGoogleAsync(string email, string name, CancellationToken cancellationToken);
        Task<BaseResponse<AuthResponseDTO>> VerifyGoogleEmailAsync(string email, string token, string name, CancellationToken cancellationToken);
        Task<BaseResponse<ForgetPasswordResponseDTO>> ResetPasswordAsync(ForgetPasswordRequestDTO forgetPasswordRequestDTO, CancellationToken cancellationToken);
        Task<BaseResponse> VerifyEmail(string email, string otp, CancellationToken cancellationToken);
    }
}
