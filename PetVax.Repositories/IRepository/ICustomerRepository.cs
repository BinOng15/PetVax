using PetVax.BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetVax.Repositories.IRepository
{
    public interface ICustomerRepository
    {
        Task<int> CreateCustomerAsync(Customer customer);
        Task<Customer> GetCustomerByAccountId (int accountId, CancellationToken cancellationToken);
        Task<Customer> GetCustomerByIdAsync(int customerId, CancellationToken cancellationToken);
        Task<List<Customer>> GetAllCustomersAsync(CancellationToken cancellationToken);
        Task<int> UpdateCustomerAsync(Customer customer, CancellationToken cancellationToken);
        Task<bool> DeleteCustomerAsync(int customerId, CancellationToken cancellationToken);
    }
}
