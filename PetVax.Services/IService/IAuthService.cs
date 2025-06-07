using PetVax.BusinessObjects.DTO;
using PetVax.BusinessObjects.DTO.AccountDTO;
using PetVax.BusinessObjects.DTO.AuthenticateDTO;
using System.ComponentModel.Design.Serialization;

namespace PetVax.Services.IService
{
    public interface IAuthService
    {
        Task<AuthResponseDTO> LoginAsync(LoginRequestDTO loginRequestDTO, CancellationToken cancellationToken);
        Task<AuthResponseDTO> VerifyOtpAsync(string email, string otp, CancellationToken cancellationToken);
        Task<AuthResponseDTO> LoginSimple(LoginRequestDTO loginRequestDTO, CancellationToken cancellationToken);
        Task<ResponseModel> Register(RegisRequestDTO regisRequestDTO, CancellationToken cancellationToken);
        Task<AuthResponseDTO> LoginWithGoogleAsync(string email, string name, CancellationToken cancellationToken);
        Task<AuthResponseDTO> VerifyGoogleEmailAsync(string email, string token, string name, CancellationToken cancellationToken);
        Task<ResponseModel> VerifyEmail(string email, string otp, CancellationToken cancellationToken);
    }
}
