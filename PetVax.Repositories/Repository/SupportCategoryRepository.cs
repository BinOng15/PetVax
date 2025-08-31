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
    public class SupportCategoryRepository : GenericRepository<SupportCategory>, ISupportCategoryRepository
    {
        public SupportCategoryRepository() : base()
        {
        }

        public async Task<int> CreateSupportCategoryAsync(SupportCategory supportCategory, CancellationToken cancellationToken)
        {
            await _context.SupportCategories.AddAsync(supportCategory, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return supportCategory.SupportCategoryId;
        }

        public async Task<bool> DeleteSupportCategoryAsync(int supportCategoryId, CancellationToken cancellationToken)
        {
            return await DeleteAsync(supportCategoryId, cancellationToken);
        }

        public async Task<List<SupportCategory>> GetAllSupportCategoriesAsync(CancellationToken cancellationToken)
        {
            return await _context.SupportCategories
                .OrderByDescending(s => s.CreatedAt)
                .ToListAsync(cancellationToken);
        }

        public async Task<SupportCategory> GetSupportCategoryByIdAsync(int supportCategoryId, CancellationToken cancellationToken)
        {
            return await GetByIdAsync(supportCategoryId, cancellationToken);
        }

        public async Task<int> UpdateSupportCategoryAsync(SupportCategory supportCategory, CancellationToken cancellationToken)
        {
            return await UpdateAsync(supportCategory, cancellationToken);
        }
    }
}
