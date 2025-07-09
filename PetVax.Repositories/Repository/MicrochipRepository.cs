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
    public class MicrochipRepository : GenericRepository<Microchip>, IMicrochipRepository
    {
        public MicrochipRepository() : base()
        {
        }
        public async Task<int> CreateMicrochipAsync(Microchip microchip, CancellationToken cancellationToken)
        {
            _context.Add(microchip);
            await _context.SaveChangesAsync(cancellationToken);
            return microchip.MicrochipId;
        }

        public async Task<bool> DeleteMicrochipAsync(int microchipId, CancellationToken cancellationToken)
        {
            return await DeleteAsync(microchipId, cancellationToken);
        }

        public async Task<List<Microchip>> GetAllMicrochipsAsync(CancellationToken cancellationToken)
        {
            return await GetAllAsync(cancellationToken);
        }


        public async Task<Microchip> GetMicrochipByIdAsync(int microchipId, CancellationToken cancellationToken)
        {
            return await GetByIdAsync(microchipId, cancellationToken);
        }

        public async Task<int> UpdateMicrochipAsync(Microchip microchip, CancellationToken cancellationToken)
        {
            return await UpdateAsync(microchip, cancellationToken);
        }
    }
}
