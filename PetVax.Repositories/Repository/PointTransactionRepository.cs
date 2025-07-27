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
    public class PointTransactionRepository : GenericRepository<PointTransaction>, IPointTransactionRepository
    {
        public PointTransactionRepository() : base()
        {
        }

        public async Task<int> CreatePointTransactionAsync(PointTransaction pointTransaction, CancellationToken cancellationToken)
        {
            return await CreateAsync(pointTransaction, cancellationToken);
        }

        public async Task<bool> DeletePointTransactionAsync(int id, CancellationToken cancellationToken)
        {
            return await DeleteAsync(id, cancellationToken);
        }

        public async Task<List<PointTransaction>> GetAllPointTransactionsAsync(CancellationToken cancellationToken)
        {
            return await _context.PointTransactions
                .Include(pt => pt.Customer)
                    .ThenInclude(c => c.Membership)
                .Include(pt => pt.Voucher)
                .Include(pt => pt.Payment)
                .Where(pt => pt.isDeleted == false)
                .ToListAsync(cancellationToken);
        }

        public async Task<PointTransaction> GetPointTransactionByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.PointTransactions
                .Include(pt => pt.Customer)
                    .ThenInclude(c => c.Membership)
                .Include(pt => pt.Voucher)
                .Include(pt => pt.Payment)
                .FirstOrDefaultAsync(pt => pt.TransactionId == id && pt.isDeleted == false, cancellationToken);
        }

        public async Task<List<PointTransaction>> GetPointTransactionsByCustomerIdAsync(int customerId, CancellationToken cancellationToken)
        {
            return await _context.PointTransactions
                .Include(pt => pt.Customer)
                    .ThenInclude(c => c.Membership)
                .Include(pt => pt.Voucher)
                .Include(pt => pt.Payment)
                .Where(pt => pt.CustomerId == customerId && pt.isDeleted == false)
                .ToListAsync(cancellationToken);
        }

        public async Task<int> UpdatePointTransactionAsync(PointTransaction pointTransaction, CancellationToken cancellationToken)
        {
            return await UpdateAsync(pointTransaction, cancellationToken);
        }
    }
}
