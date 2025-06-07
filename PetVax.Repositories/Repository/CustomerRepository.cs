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
    public class CustomerRepository : GenericRepository<Customer>, ICustomerRepository
    {
        public async Task<int> CreateCustomerAsync(Customer customer)
        {
            Create(customer);
            return customer.CustomerId;
        }

        public async Task<Customer> GetCustomerByAccountId(int accountId, CancellationToken cancellationToken)
        {
            if (accountId <= 0)
            {
                throw new ArgumentException("Invalid account ID", nameof(accountId));
            }
            var customer = await _context.Set<Customer>()
                .FirstOrDefaultAsync(c => c.AccountId == accountId, cancellationToken);
            if (customer == null)
            {
                throw new KeyNotFoundException($"Customer with AccountId {accountId} not found.");
            }
            return customer;
        }
    }
}
