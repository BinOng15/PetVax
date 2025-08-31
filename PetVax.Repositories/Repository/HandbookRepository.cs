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
    public class HandbookRepository : GenericRepository<Handbook>, IHandbookRepository
    {
        public HandbookRepository() : base()
        {
        }

        public async Task<int> CreateHandbookAsync(Handbook handbook, CancellationToken cancellationToken)
        {
            await _context.Handbooks.AddAsync(handbook, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return handbook.HandbookId;
        }

        public async Task<bool> DeleteHandbookAsync(int handbookId, CancellationToken cancellationToken)
        {
            return await DeleteAsync(handbookId, cancellationToken);
        }

        public async Task<List<Handbook>> GetAllHandbookAsync(CancellationToken cancellationToken)
        {
            return await _context.Handbooks
                .OrderByDescending(h => h.CreatedAt)
                .ToListAsync(cancellationToken);
        }

        public async Task<Handbook> GetHandbookByIdAsync(int handbookId, CancellationToken cancellationToken)
        {
            return await GetByIdAsync(handbookId, cancellationToken);
        }

        public async Task<Handbook> GetHandbookContentByTitleAsync(string title, CancellationToken cancellationToken)
        {
            return await _context.Handbooks
                .Where(h => h.Title.Equals(title, StringComparison.OrdinalIgnoreCase))
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<int> UpdateHandbookAsync(Handbook handbook, CancellationToken cancellationToken)
        {
            return await UpdateAsync(handbook, cancellationToken);
        }
    }
}
