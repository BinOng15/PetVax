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
        public CustomerRepository() : base()
        {
        }
        public async Task<int> CreateCustomerAsync(Customer customer)
        {
            Create(customer);
            return customer.CustomerId;
        }

        public async Task<bool> DeleteCustomerAsync(int customerId, CancellationToken cancellationToken)
        {
            return await DeleteAsync(customerId, cancellationToken);
        }

        public async Task<List<Customer>> GetAllCustomersAsync(CancellationToken cancellationToken)
        {
            return await _context.Customers
                .Include(c => c.Account)
                .Include(c => c.Membership)
                .OrderByDescending(v => v.CreatedAt)
                .ToListAsync(cancellationToken);
        }

        public async Task<Customer> GetCustomerByAccountId(int accountId, CancellationToken cancellationToken)
        {
            
            var customer = await _context.Customers
                .Include(c => c.Account)
                .Include(c => c.Membership)
                .FirstOrDefaultAsync(c => c.AccountId == accountId, cancellationToken);           
            return customer;
        }

        public async Task<Customer> GetCustomerByIdAsync(int customerId, CancellationToken cancellationToken)
        {
            return await _context.Customers
                .Include(c => c.Account)
                .Include(c => c.Membership)
                .FirstOrDefaultAsync(c => c.CustomerId == customerId, cancellationToken);
        }

        public async Task<int> GetTotalCustomersAsync(CancellationToken cancellationToken)
        {
            return await _context.Customers.CountAsync(cancellationToken);
        }

        public async Task<int> UpdateCustomerAsync(Customer customer, CancellationToken cancellationToken)
        {
            return await UpdateAsync(customer, cancellationToken);
        }
    }
}
