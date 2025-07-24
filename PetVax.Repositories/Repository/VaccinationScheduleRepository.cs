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
    public class VaccinationScheduleRepository : GenericRepository<VaccinationSchedule>, IVaccinationScheduleRepository
    {
        public VaccinationScheduleRepository() : base()
        {
        }

        public async Task<int> CreateVaccinationScheduleAsync(VaccinationSchedule vaccinationSchedule, CancellationToken cancellationToken)
        {
            return await CreateAsync(vaccinationSchedule, cancellationToken);
        }

        public async Task<bool> DeleteVaccinationScheduleAsync(int vaccinationScheduleId, CancellationToken cancellationToken)
        {
            return await DeleteAsync(vaccinationScheduleId, cancellationToken);
        }

        public async Task<List<VaccinationSchedule>> GetAllVaccinationSchedulesAsync(CancellationToken cancellationToken)
        {
            return await _context.VaccinationSchedules
                .Include(vs => vs.Disease)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<VaccinationSchedule>> GetVaccinationScheduleByDiseaseIdAsync(int diseaseId, CancellationToken cancellationToken)
        {
            return await _context.VaccinationSchedules
                .Where(vs => vs.DiseaseId == diseaseId)
                .Include(vs => vs.Disease)
                .ToListAsync(cancellationToken);
        }

        public async Task<VaccinationSchedule> GetVaccinationScheduleByDiseaseIdAsyncs(int diseaseId, CancellationToken cancellationToken)
        {
            return await _context.VaccinationSchedules
                .Where(vs => vs.DiseaseId == diseaseId)
                .Include(vs => vs.Disease)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<VaccinationSchedule> GetVaccinationScheduleByIdAsync(int vaccinationScheduleId, CancellationToken cancellationToken)
        {
            return await _context.VaccinationSchedules
                .Include(vs => vs.Disease)
                .FirstOrDefaultAsync(vs => vs.VaccinationScheduleId == vaccinationScheduleId, cancellationToken);
        }

        public async Task<int> UpdateVaccinationScheduleAsync(VaccinationSchedule vaccinationSchedule, CancellationToken cancellationToken)
        {
            return await UpdateAsync(vaccinationSchedule, cancellationToken);
        }

        public async Task<List<VaccinationSchedule>> GetVaccinationScheduleBySpeciesAsync(string species, CancellationToken cancellationToken)
        {
            return await _context.VaccinationSchedules
                .Where(vs => vs.Species.ToLower() == species.ToLower())
                .Include(vs => vs.Disease)
                .ToListAsync(cancellationToken);
        }
    }
}
