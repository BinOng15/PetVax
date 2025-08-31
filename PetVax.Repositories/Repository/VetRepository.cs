using Microsoft.EntityFrameworkCore;
using PediVax.BusinessObjects.DBContext;
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
    public class VetRepository : GenericRepository<Vet>, IVetRepository
    {
 
        public VetRepository() : base()
        {
        }

        public async Task<int> CreateVetAsync(Vet vet, CancellationToken cancellationToken)
        {
            return await CreateAsync(vet, cancellationToken);
        }

        public Task<bool> DeleteVetAsync(int vetId, CancellationToken cancellationToken)
        {
            return DeleteAsync(vetId, cancellationToken);
        }

        public Task<List<Vet>> GetAllVetsAsync(CancellationToken cancellationToken)
        {
            return _context.Vets
                .Include(v => v.Account)
                .Include(v => v.VetSchedules)
                .Where(v => v.isDeleted != true)
                .OrderByDescending(m => m.CreateAt)
                .ToListAsync(cancellationToken);
        }

        public async Task<int> GetTotalVetsAsync(CancellationToken cancellationToken)
        {
            return await _context.Vets
                .Where(v => v.isDeleted == false)
                .CountAsync(cancellationToken);
        }

        public async Task<Vet> GetVetByAccountIdAsync(int accountId, CancellationToken cancellationToken)
        {
            return await _context.Vets
                .Include(v => v.Account)
                .Include(v => v.VetSchedules)
                .FirstOrDefaultAsync(v => v.AccountId == accountId && v.isDeleted != true, cancellationToken);
        }

        public async Task<Vet> GetVetByIdAsync(int vetId, CancellationToken cancellationToken)
        {

            return await _context.Vets
                .Include(v => v.Account)
                .Include(v => v.VetSchedules)
                .FirstOrDefaultAsync(v => v.VetId == vetId, cancellationToken);
        }

        public async Task<Vet> GetVetByVetCodeAsync(string vetCode, CancellationToken cancellationToken)
        {
            
            return await _context.Vets
                .Include(v => v.Account)
                .Include(v => v.VetSchedules)
                .FirstOrDefaultAsync(v => v.VetCode == vetCode, cancellationToken);
        }

        public Task<int> UpdateVetAsync(Vet vet, CancellationToken cancellationToken)
        {
            return UpdateAsync(vet, cancellationToken);
        }
    }
}
