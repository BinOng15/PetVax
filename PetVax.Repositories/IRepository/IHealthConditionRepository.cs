using PetVax.BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetVax.Repositories.IRepository
{
    public interface IHealthConditionRepository
    {
        Task<List<HealthCondition>> GetAllHealthConditionsAsync(CancellationToken cancellationToken);
        Task<HealthCondition> GetHealthConditionByIdAsync(int? id, CancellationToken cancellationToken);
        Task<HealthCondition> AddHealthConditionAsync(HealthCondition healthCondition, CancellationToken cancellationToken);
        Task<HealthCondition> UpdateHealthConditionAsync(HealthCondition healthCondition, CancellationToken cancellationToken);
        Task<bool> DeleteHealthConditionAsync(int id, CancellationToken cancellationToken);
        Task<List<HealthCondition>> GetHealthConditionsByPetIdAsync(int petId, CancellationToken cancellationToken);
        Task<List<HealthCondition>> GetHealthConditionsByPetIdAndStatusAsync(int petId, string status, CancellationToken cancellationToken);
        Task<int> GetTotalHealthConditionsAsync(CancellationToken cancellationToken);
    }
}
