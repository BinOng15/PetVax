using PetVax.BusinessObjects.DTO.AccountDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetVax.Services.IService
{
    public interface IAccountService
    {
        Task<AccountResponseDTO> GetAccountByIdAsync(int accountId, CancellationToken cancellationToken);
        Task<AccountResponseDTO> GetAccountByEmailAsync(string email, CancellationToken cancellationToken);
        Task<List<AccountResponseDTO>> GetAllAccountsAsync(CancellationToken cancellationToken);
        Task<AccountResponseDTO> CreateStaffAccountAsync (CreateAccountDTO createAccountDTO, CancellationToken cancellationToken);
        Task<AccountResponseDTO> CreateVetAccountAsync(CreateAccountDTO createAccountDTO, CancellationToken cancellationToken);
        Task<bool> UpdateAccountAsync(int accountId, UpdateAccountDTO updateAccountDTO, CancellationToken cancellationToken);
        Task<bool> DeleteAccountAsync(int accountId, CancellationToken cancellationToken);

    }
}
