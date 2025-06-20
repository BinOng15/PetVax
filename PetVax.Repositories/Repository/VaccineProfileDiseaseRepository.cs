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
    public class VaccineProfileDiseaseRepository : GenericRepository<VaccineProfileDisease>, IVaccineProfileDiseaseRepository
    {
        public VaccineProfileDiseaseRepository() : base() { }

        public async Task<int> CreateVaccineProfileDiseaseAsync(VaccineProfileDisease vaccineProfileDisease, CancellationToken cancellationToken)
        {
            return await CreateAsync(vaccineProfileDisease, cancellationToken);
        }

        public async Task<bool> DeleteVaccineProfileAsync(int vaccineProfileDiseaseId, CancellationToken cancellationToken)
        {
            return await DeleteAsync(vaccineProfileDiseaseId, cancellationToken);
        }

        public async Task<List<VaccineProfileDisease>> GetAllVaccineProfileDiseasesAsync(CancellationToken cancellationToken)
        {
            return await GetAllAsync(cancellationToken);
        }

        public async Task<VaccineProfileDisease> GetVaccineProfileDiseaseByIdAsync(int vaccineProfileDiseaseId, CancellationToken cancellationToken)
        {
            return await _context.VaccineProfileDiseases.FirstOrDefaultAsync(vpd => vpd.VaccineProfileDiseasesId == vaccineProfileDiseaseId, cancellationToken);
        }

        public async Task<List<VaccineProfileDisease>> GetVaccineProfileDiseasesByDiseaseIdAsync(int diseaseId, CancellationToken cancellationToken)
        {
            return await _context.VaccineProfileDiseases
                .Where(vpd => vpd.DiseaseId == diseaseId)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<VaccineProfileDisease>> GetVaccineProfileDiseasesByVaccineProfileIdAsync(int vaccineProfileId, CancellationToken cancellationToken)
        {
            return await _context.VaccineProfileDiseases
                .Where(vpd => vpd.VaccineProfileId == vaccineProfileId)
                .ToListAsync(cancellationToken);
        }

        public async Task<int> UpdateVaccineProfileDiseaseAsync(VaccineProfileDisease vaccineProfileDisease, CancellationToken cancellationToken)
        {
            return await UpdateAsync(vaccineProfileDisease, cancellationToken);
        }
    }
}
