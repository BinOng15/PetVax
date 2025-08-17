using PetVax.BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PetVax.BusinessObjects.Enum.EnumList;

namespace PetVax.Repositories.IRepository
{
    public interface IServiceHistoryRepository
    {
         Task CreateServiceHistoryAsync(CancellationToken cancellationToken);
        Task<bool> DeleteServiceHistoryAsync(int serviceHistoryId, CancellationToken cancellationToken);
        Task<List<ServiceHistory>> GetAllServiceHistoriesAsync(CancellationToken cancellationToken);
        Task<ServiceHistory> GetServiceHistoryByIdAsync(int serviceHistoryId, CancellationToken cancellationToken);
        Task<List<ServiceHistory>> GetServiceHistoriesByCustomerIdAsync(int customerId, CancellationToken cancellationToken);
        Task<ServiceHistory> UpdateServiceHistoryAsync(ServiceHistory serviceHistory, CancellationToken cancellationToken);
        Task UpdateExpiredServiceHistoriesAsync(CancellationToken cancellationToken);
        Task<List<ServiceHistory>> GetServiceHistoriesByServiceTypeAsync(ServiceType serviceType, CancellationToken cancellationToken);
    }
}
