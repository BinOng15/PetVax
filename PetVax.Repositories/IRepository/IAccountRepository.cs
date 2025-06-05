using PetVax.BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetVax.Repositories.IRepository
{
    public interface IAccountRepository
    {
        Task<List<Account>> GetAllAccountsAsync(CancellationToken cancellationToken);
        Task<Account> GetAccountByIdAsync(int accountId, CancellationToken cancellationToken);
        Task<Account> GetAccountByEmailAsync(string email, CancellationToken cancellationToken);
        Task<int> CreateAccountAsync(Account account, CancellationToken cancellationToken);
        Task<int> UpdateAccountAsync(Account account, CancellationToken cancellationToken);
        Task<bool> DeleteAccountAsync(int accountId, CancellationToken cancellationToken);
    }
}
