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
        public DiseaseRepository() : base()
        {
        }
        public async Task<int> CreateDiseaseAsync(Disease disease, CancellationToken cancellationToken)
        {
            return await CreateAsync(disease, cancellationToken);
        }
        public async Task<bool> DeleteDiseaseAsync(int diseaseId, CancellationToken cancellationToken)
        {
            return await DeleteAsync(diseaseId, cancellationToken);
        }
        public async Task<List<Disease>> GetAllDiseasesAsync(CancellationToken cancellationToken)
        {
            return await GetAllAsync(cancellationToken);
        }
        public async Task<Disease> GetDiseaseByIdAsync(int diseaseId, CancellationToken cancellationToken)
        {
            return await _context.Diseases
                .FirstOrDefaultAsync(d => d.DiseaseId == diseaseId, cancellationToken);
        }
        public async Task<Disease> GetDiseaseByName(string name, CancellationToken cancellationToken)
        {
            return await _context.Diseases
                .FirstOrDefaultAsync(d => d.Name == name, cancellationToken);
        }
        public async Task<int> UpdateDiseaseAsync(Disease disease, CancellationToken cancellationToken)
        {
            return await UpdateAsync(disease, cancellationToken);
        }
    }
}
