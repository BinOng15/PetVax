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
    public class VaccineDiseaseRepository : GenericRepository<VaccineDisease>, IVaccineDiseaseRepository
    {
        public VaccineDiseaseRepository() : base()
        {
        }
        public async Task<int> CreateVaccineDiseaseAsync(VaccineDisease vaccineDisease, CancellationToken cancellationToken)
        {
            return await CreateAsync(vaccineDisease, cancellationToken);
        }

        public async Task<bool> DeleteVaccineDiseaseAsync(int vaccineDiseaseId, CancellationToken cancellationToken)
        {
            return await DeleteAsync(vaccineDiseaseId, cancellationToken);
        }

        public async Task<List<VaccineDisease>> GetAllVaccineDiseaseAsync(CancellationToken cancellationToken)
        {
            return await _context.VaccineDiseases
                .Include(vd => vd.Disease)
                .Include(vd => vd.Vaccine)
                .Where(vd => vd.Vaccine.isDeleted == false && vd.Disease.isDeleted == false && vd.isDeleted == false)
                .OrderByDescending(m => m.CreatedAt)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<VaccineDisease>> GetVaccineDiseaseByDiseaseIdAsync(int diseaseId, CancellationToken cancellationToken)
        {
            return await _context.VaccineDiseases
                .Include(vd => vd.Disease)
                .Include(vd => vd.Vaccine)
                .Where(vd => vd.DiseaseId == diseaseId)
                .Where(vd => vd.Vaccine.isDeleted == false && vd.Disease.isDeleted == false && vd.isDeleted == false)
                .ToListAsync(cancellationToken);
        }


        public async Task<VaccineDisease> GetVaccineDiseaseByIdAsync(int diseaseId, CancellationToken cancellationToken)
        {
            return await _context.VaccineDiseases
                .Include(vd => vd.Disease)
                .Include(vd => vd.Vaccine)
                .Where(vd => vd.Vaccine.isDeleted == false && vd.Disease.isDeleted == false && vd.isDeleted == false)
                .FirstOrDefaultAsync(vd => vd.VaccineDiseaseId == diseaseId, cancellationToken);
        }

        public async Task<List<VaccineDisease>> GetVaccineDiseaseByVaccineIdAsync(int vaccineId, CancellationToken cancellationToken)
        {
            return await _context.VaccineDiseases
                .Include(vd => vd.Disease)
                .Include(vd => vd.Vaccine)
                .Where(vd => vd.VaccineId == vaccineId)
                .Where(vd => vd.Vaccine.isDeleted == false && vd.Disease.isDeleted == false && vd.isDeleted == false)
                .ToListAsync(cancellationToken);
        }

        public async Task<int> UpdateVaccineDiseaseAsync(VaccineDisease vaccineDisease, CancellationToken cancellationToken)
        {
            return await UpdateAsync(vaccineDisease, cancellationToken);
        }
    }
}
