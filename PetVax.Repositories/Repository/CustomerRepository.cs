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
    }
}
