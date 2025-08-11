using PetVax.BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetVax.Repositories.IRepository
{
    public interface IAddressRepository
    {
        Task<int> CreateAddressAsync(Address address, CancellationToken cancellationToken);
        Task<int> UpdateAddressAsync(Address address, CancellationToken cancellationToken);
        Task<bool> DeleteAddressAsync(int addressId, CancellationToken cancellationToken);
        Task<Address> GetAddressByIdAsync(int addressId, CancellationToken cancellationToken);
        Task<List<Address>> GetAllAddressesAsync(CancellationToken cancellationToken);
    }
}
