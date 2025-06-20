﻿using Microsoft.EntityFrameworkCore;
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
    .           ToListAsync(cancellationToken);
        }

        public async Task<VaccineProfile> GetVaccineProfileByIdAsync(int vaccineProfileId, CancellationToken cancellationToken)
        {
            return await _context.VaccineProfiles
                .Where(vp => vp.VaccineProfileId == vaccineProfileId)
                .Include(vp => vp.Disease)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<VaccineProfile> GetVaccineProfileByPetIdAsync(int petId, CancellationToken cancellationToken)
        {
            return await _context.VaccineProfiles
                .Where(vp => vp.PetId == petId)
                .Include(vp => vp.Disease)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<int> UpdateVaccineProfileAsync(VaccineProfile vaccineProfile, CancellationToken cancellationToken)
        {
            return await UpdateAsync(vaccineProfile, cancellationToken);
        }
    }

}
