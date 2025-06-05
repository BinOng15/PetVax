using Microsoft.EntityFrameworkCore;
using PetVax.BusinessObjects.Models;
using PetVax.Repositories.IRepository;
using PetVax.Repositories.Repository.BaseResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetVax.Repositories.Repository
{
    public class AccountRepository : GenericRepository<Account>, IAccountRepository
    {
        public AccountRepository() : base() 
        { 
        }

        public async Task<int> CreateAccountAsync(Account account, CancellationToken cancellationToken)
        {
            return await CreateAsync(account, cancellationToken);
        }

        public async Task<bool> DeleteAccountAsync(int accountId, CancellationToken cancellationToken)
        {
            return await DeleteAsync(accountId, cancellationToken);
        }

        public async Task<Account> GetAccountByEmailAsync(string email, CancellationToken cancellationToken)
        {
            return await _context.Accounts.FirstOrDefaultAsync(a => a.Email == email, cancellationToken);
        }

        public async Task<Account> GetAccountByIdAsync(int accountId, CancellationToken cancellationToken)
        {
            return await _context.Accounts.FirstOrDefaultAsync(a => a.AccountId == accountId, cancellationToken);
        }

        public async Task<List<Account>> GetAllAccountsAsync(CancellationToken cancellationToken)
        {
            return await GetAllAsync(cancellationToken);
        }

        public async Task<int> UpdateAccountAsync(Account account, CancellationToken cancellationToken)
        {
            return await UpdateAsync(account, cancellationToken);
        }
    }
}
