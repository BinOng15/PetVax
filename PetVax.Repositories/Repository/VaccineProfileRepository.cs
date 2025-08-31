using Microsoft.EntityFrameworkCore;
using PediVax.BusinessObjects.DBContext;
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
    public class VaccineProfileRepository : GenericRepository<VaccineProfile>, IVaccineProfileRepository
    {
        public VaccineProfileRepository() : base()
        {
        }

        public async Task<int> CreateVaccineProfileAsync(VaccineProfile vaccineProfile, CancellationToken cancellationToken)
        {
            return await CreateAsync(vaccineProfile, cancellationToken);
        }

        public async Task<bool> DeleteVaccineProfileAsync(int vaccineProfileId, CancellationToken cancellationToken)
        {
            return await DeleteAsync(vaccineProfileId, cancellationToken);
        }

        public async Task<List<VaccineProfile>> GetAllVaccineProfilesAsync(CancellationToken cancellationToken)
        {
            return await _context.VaccineProfiles
                .Include(vp => vp.Disease)
                .Include(vp => vp.AppointmentDetail)
                    .ThenInclude(ad => ad.Vet)
                .Include(vp => vp.AppointmentDetail)
                    .ThenInclude(ad => ad.VaccineBatch)
                        .ThenInclude(vb => vb.Vaccine)
                .OrderByDescending(m => m.CreatedAt)
                .AsNoTracking()
    .           ToListAsync(cancellationToken);
        }

        public async Task<List<VaccineProfile>> GetListVaccineProfileByPetIdAsync(int petId, CancellationToken cancellationToken)
        {
            return await _context.VaccineProfiles
                .Where(vp => vp.PetId == petId)
                .Include(vp => vp.Disease)
                .Include(vp => vp.AppointmentDetail)
                    .ThenInclude(ad => ad.Vet)
                .Include(vp => vp.AppointmentDetail)
                    .ThenInclude(ad => ad.VaccineBatch)
                        .ThenInclude(vb => vb.Vaccine)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        }

        public async Task<VaccineProfile> GetVaccineProfileByIdAsync(int vaccineProfileId, CancellationToken cancellationToken)
        {
            return await _context.VaccineProfiles
                .Include(vp => vp.Disease)
                .Include(vp => vp.AppointmentDetail)
                    .ThenInclude(ad => ad.Vet)
                .Include(vp => vp.AppointmentDetail)
                    .ThenInclude(ad => ad.VaccineBatch)
                        .ThenInclude(vb => vb.Vaccine)
                .AsNoTracking()
                .FirstOrDefaultAsync(vp => vp.VaccineProfileId == vaccineProfileId, cancellationToken);
        }

        public async Task<VaccineProfile> GetVaccineProfileByPetIdAsync(int petId, CancellationToken cancellationToken)
        {
            return await _context.VaccineProfiles
                .Where(vp => vp.PetId == petId)
                .Include(vp => vp.Disease)
                .AsNoTracking()
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<List<VaccineProfile>> GetVaccineProfilesByDiseaseId(int diseaseId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<int> UpdateVaccineProfileAsync(VaccineProfile vaccineProfile, CancellationToken cancellationToken)
        {
            return await UpdateAsync(vaccineProfile, cancellationToken);
        }
    }

}
