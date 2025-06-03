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
        Task<List<Account>> GetAllAccounts(CancellationToken cancellationToken);
        Task<Account> GetAccountById(int accountId, CancellationToken cancellationToken);

    }
}
