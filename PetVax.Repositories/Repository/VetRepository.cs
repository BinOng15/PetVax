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
    public class VetRepository : GenericRepository<Vet>, IVetRepository
    {
        public VetRepository() : base() 
        {
        }

        public async Task<int> CreateVetAsync(Vet vet, CancellationToken cancellationToken)
        {
            if (vet == null)
            {
                throw new ArgumentNullException(nameof(vet), "Vet cannot be null");
            }
            // Ensure the vet has a valid AccountId
            if (vet.AccountId <= 0)
            {
                throw new ArgumentException("Invalid AccountId for Vet", nameof(vet.AccountId));
            }
            return await CreateAsync(vet, cancellationToken);
        }
    }
}
