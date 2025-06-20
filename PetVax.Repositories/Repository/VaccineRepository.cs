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
    public class VaccineRepository : GenericRepository<Vaccine>, IVaccineRepository
    {
        public VaccineRepository() : base()
        {
        }

        public async Task<int> CreateVaccineAsync(Vaccine vaccine, CancellationToken cancellationToken)
        {
            return await CreateAsync(vaccine, cancellationToken);
        }

        public async Task<bool> DeleteVaccineAsync(int vaccineId, CancellationToken cancellationToken)
        {
            return await DeleteAsync(vaccineId, cancellationToken);
        }

        public async Task<List<Vaccine>> GetAllVaccineAsync(CancellationToken cancellationToken)
        {
            return await GetAllAsync(cancellationToken);
        }

        public async Task<List<Vaccine>> GetVaccineByDiseaseId(int diseaseId, CancellationToken cancellationToken)
        {
            return await _context.VaccineDiseases
                .Where(vd => vd.DiseaseId == diseaseId)
                .Include(vd => vd.Vaccine)
                .Select(vd => vd.Vaccine)
                .ToListAsync(cancellationToken);
        }

        public async Task<Vaccine> GetVaccineByIdAsync(int vaccineId, CancellationToken cancellationToken)
        {
            return await _context.Vaccines.FirstOrDefaultAsync(v => v.VaccineId == vaccineId, cancellationToken);
        }

        public async Task<Vaccine> GetVaccineByName(string Name, CancellationToken cancellationToken)
        {
            return await _context.Vaccines.FirstOrDefaultAsync(v => v.Name == Name, cancellationToken);
        }

        public async Task<Vaccine> GetVaccineByVaccineCodeAsync(string vaccineCode, CancellationToken cancellationToken)
        {
            return await _context.Vaccines.FirstOrDefaultAsync(v => v.VaccineCode == vaccineCode, cancellationToken);
        }

        public async Task<int> UpdateVaccineAsync(Vaccine vaccine, CancellationToken cancellationToken)
        {
            return await UpdateAsync(vaccine, cancellationToken);
        }
    }
}
