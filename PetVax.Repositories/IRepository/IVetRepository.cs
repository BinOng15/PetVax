using PetVax.BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetVax.Repositories.IRepository
{
    public interface IVetRepository
    {

        Task<List<Vet>> GetAllVetsAsync(CancellationToken cancellationToken);
        Task<Vet> GetVetByIdAsync(int vetId, CancellationToken cancellationToken);
        Task<Vet> GetVetByVetCodeAsync(string vetCode, CancellationToken cancellationToken);
        Task<int> CreateVetAsync(Vet vet, CancellationToken cancellationToken);
        Task<int> UpdateVetAsync(Vet vet, CancellationToken cancellationToken);
        Task<bool> DeleteVetAsync(int vetId, CancellationToken cancellationToken);
        Task<Vet> GetVetByAccountIdAsync(int accountId, CancellationToken cancellationToken);

    }
}
