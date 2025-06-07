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
        Task<int> CreateVetAsync(Vet vet, CancellationToken cancellationToken);
    }
}
