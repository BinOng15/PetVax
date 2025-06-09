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
    public class PetPassportRepository : GenericRepository<PetPassport>, IPetPassportRepository
    {
        public async Task<int> AddPetPassportAsync(PetPassport petPassport, CancellationToken cancellationToken)
        {
            return await CreateAsync(petPassport, cancellationToken);
        }

        public async Task<bool> DeletePetPassportAsync(int id, CancellationToken cancellationToken)
        {
            return await DeleteAsync(id, cancellationToken);
        }

        public async Task<List<PetPassport>> GetAllPetPassportsAsync(CancellationToken cancellationToken)
        {
            return await GetAllAsync(cancellationToken);
        }

        public async Task<PetPassport?> GetPetPassportByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await GetByIdAsync(id, cancellationToken);
        }

        public async Task<List<PetPassport>> GetPetPassportByMicrochipItemIdAsync(int mmicrochipItemlId, CancellationToken cancellationToken)
        {
            return await _context.PetPassports
                .Where(pp => pp.MicrochipItemId == mmicrochipItemlId)
                .ToListAsync(cancellationToken);
        }

        public async Task<PetPassport> GetPetPassPortByPassportCodeAsync(string passportCode, CancellationToken cancellationToken)
        {
            return await _context.PetPassports
                .FirstOrDefaultAsync(pp => pp.PassportCode == passportCode, cancellationToken);
        }

        public async Task<List<PetPassport>> GetPetPassportsByPetIdAsync(int petId, CancellationToken cancellationToken)
        {
            return await _context.PetPassports
                .Where(pp => pp.PetId == petId)
                .ToListAsync(cancellationToken);
        }

        public async Task<int> UpdatePetPassportAsync(PetPassport petPassport, CancellationToken cancellationToken)
        {
            return await UpdateAsync(petPassport, cancellationToken);
        }
    }
}
