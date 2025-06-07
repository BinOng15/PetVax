using PetVax.BusinessObjects.Models;
using PetVax.Repositories.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetVax.Services.Service
{
    public class CustomerService : ICustomerService
    {
        public Task<int> CreateCustomerAsync(Customer customer)
        {
            throw new NotImplementedException();
        }
    }
}
