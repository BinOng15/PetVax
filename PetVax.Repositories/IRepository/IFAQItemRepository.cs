using PetVax.BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetVax.Repositories.IRepository
{
    public interface IFAQItemRepository
    {
        Task<List<FAQItem>> GetAllFAQItemsAsync(CancellationToken cancellationToken);
        Task<FAQItem> GetFAQItemByIdAsync(int faqItemId, CancellationToken cancellationToken);
        Task<int> CreateFAQItemAsync(FAQItem faqItem, CancellationToken cancellationToken);
        Task<int> UpdateFAQItemAsync(FAQItem faqItem, CancellationToken cancellationToken);
        Task<bool> DeleteFAQItemAsync(int faqItemId, CancellationToken cancellationToken);
    }
}
