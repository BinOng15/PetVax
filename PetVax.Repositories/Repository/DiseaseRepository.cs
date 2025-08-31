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

        public async Task<List<Disease>> GetAllDiseaseAsync(CancellationToken cancellationToken)
        {
            return await _context.Diseases
                .OrderByDescending(v => v.CreatedAt)
                .ToListAsync(cancellationToken);
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

        public async Task<List<Disease>> GetDiseaseBySpecies(string species, CancellationToken cancellationToken)
        {
            return await _context.Diseases
                .Where(d => d.Species == species)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<Disease>> GetDiseaseByVaccineId(int vaccineId, CancellationToken cancellationToken)
        {
            return await _context.VaccineDiseases
                .Where(vd => vd.VaccineId == vaccineId)
                .Include(vd => vd.Disease)
                .Select(vd => vd.Disease)
                .ToListAsync(cancellationToken);
        }

        public async Task<int> GetTotalDiseasesAsync(CancellationToken cancellationToken)
        {
            return await _context.Diseases.Where(m => m.isDeleted == false).CountAsync(cancellationToken);
        }

        public async Task<int> UpdateDiseaseAsync(Disease disease, CancellationToken cancellationToken)
        {
            return await UpdateAsync(disease, cancellationToken);
        }
    }
}
