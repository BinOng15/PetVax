using PetVax.BusinessObjects.DTO.AuthenticateDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetVax.Services.IService
{
    public interface IAuthService
    {
        Task<AuthResponseDTO> LoginAsync(LoginRequestDTO loginRequestDTO, CancellationToken cancellationToken);
        Task<AuthResponseDTO> VerifyOtpAsync(string email, string otp, CancellationToken cancellationToken);
        Task<AuthResponseDTO> LoginSimple(LoginRequestDTO loginRequestDTO, CancellationToken cancellationToken);

    }
}
