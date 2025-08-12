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
    public class AddressRepository : GenericRepository<Address>, IAddressRepository
    {
        public AddressRepository() : base()
        {
        }

        public async Task<int> CreateAddressAsync(Address address, CancellationToken cancellationToken)
        {
            await _context.Addresses.AddAsync(address, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return address.AddressId;
        }

        public async Task<bool> DeleteAddressAsync(int addressId, CancellationToken cancellationToken)
        {
            return await DeleteAsync(addressId, cancellationToken);
        }

        public async Task<Address> GetAddressByIdAsync(int addressId, CancellationToken cancellationToken)
        {
            return await _context.Addresses
                .FirstOrDefaultAsync(a => a.AddressId == addressId, cancellationToken);
        }

        public async Task<List<Address>> GetAllAddressesAsync(CancellationToken cancellationToken)
        {
            return await GetAllAsync(cancellationToken);
        }

        public async Task<int> UpdateAddressAsync(Address address, CancellationToken cancellationToken)
        {
            return await UpdateAsync(address, cancellationToken);
        }
    }
}
