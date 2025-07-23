using PetVax.BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetVax.Repositories.IRepository
{
    public interface IPointTransactionRepository
    {
        Task<List<PointTransaction>> GetAllPointTransactionsAsync(CancellationToken cancellationToken);
        Task<PointTransaction> GetPointTransactionByIdAsync(int id, CancellationToken cancellationToken);
        Task<List<PointTransaction>> GetPointTransactionsByCustomerIdAsync(int customerId, CancellationToken cancellationToken);
        Task<int> CreatePointTransactionAsync(PointTransaction pointTransaction, CancellationToken cancellationToken);
        Task<int> UpdatePointTransactionAsync(PointTransaction pointTransaction, CancellationToken cancellationToken);
        Task<bool> DeletePointTransactionAsync(int id, CancellationToken cancellationToken);
    }
}
