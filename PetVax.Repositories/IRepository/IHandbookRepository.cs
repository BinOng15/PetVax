using PetVax.BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetVax.Repositories.IRepository
{
    public interface IHandbookRepository
    {
        Task<List<Handbook>> GetAllHandbookAsync(CancellationToken cancellationToken);
        Task<Handbook> GetHandbookContentByTitleAsync(string title, CancellationToken cancellationToken);
        Task<Handbook> GetHandbookByIdAsync(int handbookId, CancellationToken cancellationToken);
        Task<int> CreateHandbookAsync(Handbook handbook, CancellationToken cancellationToken);
        Task<int> UpdateHandbookAsync(Handbook handbook, CancellationToken cancellationToken);
        Task<bool> DeleteHandbookAsync(int handbookId, CancellationToken cancellationToken);
    }
}
