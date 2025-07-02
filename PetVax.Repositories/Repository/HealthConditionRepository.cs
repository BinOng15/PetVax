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
    public class HealthConditionRepository : GenericRepository<HealthCondition>, IHealthConditionRepository
    {
        public HealthConditionRepository() : base()
        {
        }
        public async Task<HealthCondition> AddHealthConditionAsync(HealthCondition healthCondition, CancellationToken cancellationToken)
        {
            _context.Add(healthCondition);
            await _context.SaveChangesAsync(cancellationToken);
            return healthCondition;
        }

        public async Task<bool> DeleteHealthConditionAsync(int id, CancellationToken cancellationToken)
        {
            return await DeleteAsync(id, cancellationToken);
        }

        public async Task<List<HealthCondition>> GetAllHealthConditionsAsync(CancellationToken cancellationToken)
        {
            return await _context.HealthConditions
                .Include(hc => hc.Pet)
                .Include(hc => hc.Vet)
                .Include(hc => hc.MicrochipItem)
                .ToListAsync(cancellationToken);
        }

        public async Task<HealthCondition?> GetHealthConditionByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.HealthConditions
                .Include(hc => hc.Pet)
                .Include(hc => hc.Vet)
                .Include(hc => hc.MicrochipItem)
                .FirstOrDefaultAsync(hc => hc.HealthConditionId == id, cancellationToken);
        }

        public Task<List<HealthCondition>> GetHealthConditionsByPetIdAsync(int petId, CancellationToken cancellationToken)
        {
            return _context.HealthConditions
                .Where(hc => hc.PetId == petId)
                .Include(hc => hc.Pet)
                .Include(hc => hc.Vet)
                .Include(hc => hc.MicrochipItem)
                .ToListAsync(cancellationToken);
        }

        public async Task<HealthCondition> UpdateHealthConditionAsync(HealthCondition healthCondition, CancellationToken cancellationToken)
        {
            _context.Update(healthCondition);
            await _context.SaveChangesAsync(cancellationToken);
            return healthCondition;
        }
    }
}
