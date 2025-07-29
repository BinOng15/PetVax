using PetVax.BusinessObjects.DTO.AccountDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PetVax.BusinessObjects.DTO.ResponseModel;

namespace PetVax.Services.IService
{
    public interface IAccountService
    {
        Task<BaseResponse<AccountResponseDTO>> GetAccountByIdAsync(int accountId, CancellationToken cancellationToken);
        Task<BaseResponse<AccountResponseDTO>> GetAccountByEmailAsync(string email, CancellationToken cancellationToken);
        Task<DynamicResponse<AccountResponseDTO>> GetAllAccountsAsync(GetAllAccountRequestDTO getAllAccountRequestDTO, CancellationToken cancellationToken);
        Task<BaseResponse<AccountResponseDTO>> CreateStaffAccountAsync(CreateAccountDTO createAccountDTO, CancellationToken cancellationToken);
        Task<BaseResponse<AccountResponseDTO>> CreateVetAccountAsync(CreateAccountDTO createAccountDTO, CancellationToken cancellationToken);
        Task<BaseResponse<bool>> UpdateAccountAsync(int accountId, UpdateAccountDTO updateAccountDTO, CancellationToken cancellationToken);
        Task<BaseResponse<bool>> DeleteAccountAsync(int accountId, CancellationToken cancellationToken);
        Task<BaseResponse<AccountResponseDTO>> GetCurrentAccountAsync(CancellationToken cancellationToken);
    }
}
