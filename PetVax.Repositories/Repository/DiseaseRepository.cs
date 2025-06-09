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
    public class DiseaseRepository : GenericRepository<Disease>, IDiseaseRepository
    {
        public async Task<int> AddDiseaseAsync(Disease disease, CancellationToken cancellationToken)
        {
            return await CreateAsync(disease, cancellationToken);
        }

        public async Task<bool> DeleteDiseaseAsync(int id, CancellationToken cancellationToken)
        {
            return await DeleteAsync(id, cancellationToken);
        }

        public async Task<List<Disease>> GetAllDiseasesAsync(CancellationToken cancellationToken)
        {
            return await GetAllAsync(cancellationToken);
        }

        public async Task<Disease?> GetDiseaseByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await GetByIdAsync(id, cancellationToken);
        }

        public async Task<List<Disease>> GetDiseasesByNameAsync(string name, CancellationToken cancellationToken)
        {
            return await _context.Diseases
                .Where(d => d.Name.Contains(name, StringComparison.OrdinalIgnoreCase))
                .ToListAsync(cancellationToken);
        }

        public async Task<int> UpdateDiseaseAsync(Disease disease, CancellationToken cancellationToken)
        {
            return await UpdateAsync(disease, cancellationToken);
        }
    }
}
