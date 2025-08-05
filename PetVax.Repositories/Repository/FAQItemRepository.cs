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
    public class FAQItemRepository : GenericRepository<FAQItem>, IFAQItemRepository
    {
        public FAQItemRepository() : base() { }

        public async Task<int> CreateFAQItemAsync(FAQItem faqItem, CancellationToken cancellationToken)
        {
            await _context.FAQItems.AddAsync(faqItem, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return faqItem.FAQItemId;
        }

        public async Task<bool> DeleteFAQItemAsync(int faqItemId, CancellationToken cancellationToken)
        {
            return await DeleteAsync(faqItemId, cancellationToken);
        }

        public async Task<List<FAQItem>> GetAllFAQItemsAsync(CancellationToken cancellationToken)
        {
            return await GetAllAsync(cancellationToken);
        }

        public async Task<FAQItem> GetFAQItemByIdAsync(int faqItemId, CancellationToken cancellationToken)
        {
            return await GetByIdAsync(faqItemId, cancellationToken);
        }

        public async Task<int> UpdateFAQItemAsync(FAQItem faqItem, CancellationToken cancellationToken)
        {
            return await UpdateAsync(faqItem, cancellationToken);
        }
    }
}
