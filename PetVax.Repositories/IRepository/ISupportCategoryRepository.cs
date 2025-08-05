using PetVax.BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetVax.Repositories.IRepository
{
    public interface ISupportCategoryRepository
    {
        Task<List<SupportCategory>> GetAllSupportCategoriesAsync(CancellationToken cancellationToken);
        Task<SupportCategory> GetSupportCategoryByIdAsync(int supportCategoryId, CancellationToken cancellationToken);
        Task<int> CreateSupportCategoryAsync(SupportCategory supportCategory, CancellationToken cancellationToken);
        Task<int> UpdateSupportCategoryAsync(SupportCategory supportCategory, CancellationToken cancellationToken);
        Task<bool> DeleteSupportCategoryAsync(int supportCategoryId, CancellationToken cancellationToken);
    }
}
