using PetVax.BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetVax.Repositories.IRepository
{
    public interface IDiseaseRepository
    {
        Task<List<Disease>> GetAllDiseasesAsync(CancellationToken cancellationToken);
        Task<Disease?> GetDiseaseByIdAsync(int id, CancellationToken cancellationToken);
        Task<int> AddDiseaseAsync(Disease disease, CancellationToken cancellationToken);
        Task<int> UpdateDiseaseAsync(Disease disease, CancellationToken cancellationToken);
        Task<bool> DeleteDiseaseAsync(int id, CancellationToken cancellationToken);
        Task<List<Disease>> GetDiseasesByNameAsync(string name, CancellationToken cancellationToken);
    }
}
