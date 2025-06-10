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
    public class PetRepository : GenericRepository<Pet>, IPetRepository
    {
        public async Task<int> CreatePetAsync(Pet pet, CancellationToken cancellationToken)
        {
            return await CreateAsync(pet, cancellationToken);
        }

        public async Task<bool> DeletePetAsync(int petId, CancellationToken cancellationToken)
        {
            return await DeleteAsync(petId, cancellationToken);
        }

        public async Task<List<Pet>> GetAllPetsAsync(CancellationToken cancellationToken)
        {
            return await GetAllAsync(cancellationToken);
        }

        public async Task<Pet> GetPetByIdAsync(int petId, CancellationToken cancellationToken)
        {
            return await GetByIdAsync(petId, cancellationToken);
        }

        public async Task<Pet> GetPetByNameAsync(string petName, CancellationToken cancellationToken)
        {
            return await GetPetByNameAsync(petName, cancellationToken);
        }

        public async Task<List<Pet>> GetPetsByCustomerIdAsync(int customerId, CancellationToken cancellationToken)
        {
            return await _context.Pets
                .Where(p => p.CustomerId == customerId)
                .ToListAsync(cancellationToken);
        }

        public async Task<int> UpdatePetAsync(Pet pet, CancellationToken cancellationToken)
        {
            return await UpdateAsync(pet, cancellationToken);
        }
    }
}
