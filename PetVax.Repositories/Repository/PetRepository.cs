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
    public class PetRepository : GenericRepository<Pet>, IPetRepository
    {
        public PetRepository() : base()
        {
        }
        public async Task<int> CreatePetAsync(Pet pet, CancellationToken cancellationToken)
        {
            return await CreateAsync(pet, cancellationToken);
        }

        public async Task<bool> DeletePetAsync(int petId, CancellationToken cancellationToken)
        {
            return await DeleteAsync(petId, cancellationToken);
        }

        public async Task<List<Pet>> GetAllPetsAsync(CancellationToken cancellationToken)
        {
            return await _context.Pets
                .Include(p => p.Customer).ThenInclude(c => c.Account)
                .Include(p => p.MicrochipItems)
                .Where(p => p.isDeleted == false)
                .OrderByDescending(m => m.CreatedAt)
                .ToListAsync(cancellationToken);
        }

        public async Task<Pet> GetPetAndAppointmentByIdAsync(int? petId, CancellationToken cancellationToken)
        {
            return await _context.Pets
                .Include(p => p.Appointments)
                .Include(p => p.Customer).ThenInclude(c => c.Account)
                .Include(p => p.MicrochipItems)
                .FirstOrDefaultAsync(p => p.PetId == petId, cancellationToken);
        }

        public async Task<Pet> GetPetByIdAsync(int petId, CancellationToken cancellationToken)
        {
            return await _context.Pets
                .Include(p => p.Customer).ThenInclude(c => c.Account)
                .Include(p => p.MicrochipItems)
                .FirstOrDefaultAsync(p => p.PetId == petId, cancellationToken);
        }

        public async Task<Pet> GetPetByNameAsync(string petName, CancellationToken cancellationToken)
        {
            return await GetPetByNameAsync(petName, cancellationToken);
        }

        public async Task<List<Pet>> GetPetsByCustomerIdAsync(int customerId, CancellationToken cancellationToken)
        {
            return await _context.Pets
                .Include(p => p.Customer).ThenInclude(c => c.Account)
                .Include(p => p.MicrochipItems)
                .Where(p => p.CustomerId == customerId && p.isDeleted == false)
                .ToListAsync(cancellationToken);
        }

        public async Task<int> UpdatePetAsync(Pet pet, CancellationToken cancellationToken)
        {
            return await UpdateAsync(pet, cancellationToken);
        }

        public async Task<Pet?> GetPetWithHealthDataAsync(int petId)
        {
            return await _context.Pets
                .Include(p => p.HealthConditions.Where(h => h.isDeleted == false || h.isDeleted == null))
                .FirstOrDefaultAsync(p => p.PetId == petId && (p.isDeleted == false || p.isDeleted == null));
        }
        public async Task<List<VaccineProfile>> GetVaccineProfilesByPetIdAsync(int petId)
        {
            return await _context.VaccineProfiles
                .Include(vp => vp.Disease)
                .Include(vp => vp.AppointmentDetail)
                    .ThenInclude(ad => ad.VaccineBatch)
                        .ThenInclude(vb => vb.Vaccine)
                .Include(vp => vp.AppointmentDetail)
                    .ThenInclude(ad => ad.Vet)
                .Where(vp => vp.PetId == petId && (vp.isDeleted == false || vp.isDeleted == null))
                .ToListAsync();
        }

        public async Task<int> GetTotalPetsAsync(CancellationToken cancellationToken)
        {
            return await _context.Pets
                .Where(p => p.isDeleted == false)
                .CountAsync(cancellationToken);
        }
    }
}
