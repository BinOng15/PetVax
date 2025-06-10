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
        public async Task<int> AddHealthConditionAsync(HealthCondition healthCondition, CancellationToken cancellationToken)
        {
            return await CreateAsync(healthCondition, cancellationToken);
        }

        public async Task<bool> DeleteHealthConditionAsync(int id, CancellationToken cancellationToken)
        {
            return await DeleteAsync(id, cancellationToken);
        }

        public async Task<List<HealthCondition>> GetAllHealthConditionsAsync(CancellationToken cancellationToken)
        {
            return await GetAllAsync(cancellationToken);
        }

        public async Task<HealthCondition?> GetHealthConditionByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await GetByIdAsync(id, cancellationToken);
        }

        public Task<List<HealthCondition>> GetHealthConditionsByPetIdAsync(int petId, CancellationToken cancellationToken)
        {
            return _context.HealthConditions
                .Where(hc => hc.PetId == petId)
                .ToListAsync(cancellationToken);
        }

        public async Task<int> UpdateHealthConditionAsync(HealthCondition healthCondition, CancellationToken cancellationToken)
        {
            return await UpdateAsync(healthCondition, cancellationToken);
        }
    }
}
