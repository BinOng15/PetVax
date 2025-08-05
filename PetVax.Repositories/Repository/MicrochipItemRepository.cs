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
    public class MicrochipItemRepository : GenericRepository<MicrochipItem>, IMicrochipItemRepository
    {

        public MicrochipItemRepository() : base()
        {
        }
        public async Task<int> CreateMicrochipItemAsync(MicrochipItem microchipItem, CancellationToken cancellationToken)
        {
            _context.Add(microchipItem);
            await _context.SaveChangesAsync(cancellationToken);
            return microchipItem.MicrochipItemId;
        }

        public async Task<bool> DeleteMicrochipItemAsync(int microchipItemId, CancellationToken cancellationToken)
        {
            return await DeleteAsync(microchipItemId, cancellationToken);
        }

        public async Task<List<MicrochipItem>> GetAllMicrochipItemsAsync(CancellationToken cancellationToken)
        {
            return await _context.MicrochipItems
                .Include(mc => mc.Microchip)
                .ToListAsync(cancellationToken);
        }

        public async Task<MicrochipItem> GetMicrochipItemByIdAsync(int? microchipItemId, CancellationToken cancellationToken)
        {
            return await _context.MicrochipItems
                .Where(mc => mc.MicrochipItemId == microchipItemId)
                .Include(mc => mc.Microchip)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<MicrochipItem> GetMicrochipItemByMicrochipCodedAsync(string microchipCode, CancellationToken cancellationToken)
        {
            return await _context.MicrochipItems
                .Include(mc => mc.Microchip)
                .FirstOrDefaultAsync(mc => mc.Microchip.MicrochipCode == microchipCode, cancellationToken);
        }

        public async Task<MicrochipItem> GetMicrochipItemByMicrochipIdAsync(int microchipId, CancellationToken cancellationToken)
        {
            return await _context.MicrochipItems.FirstOrDefaultAsync(m => m.MicrochipId == microchipId, cancellationToken);
        }

        public async Task<MicrochipItem> GetMicrochipItemByPetIdAsync(int? petId, CancellationToken cancellationToken)
        {
            return await _context.MicrochipItems.FirstOrDefaultAsync(m => m.PetId == petId, cancellationToken);
        }

        public async Task<int> GetTotalMicrochipItemsAsync(CancellationToken cancellationToken)
        {
            return await _context.MicrochipItems.Where(m => m.isDeleted == false).CountAsync(cancellationToken);
        }

        public async Task<int> UpdateMicrochipItemAsync(MicrochipItem microchipItem, CancellationToken cancellationToken)
        {
            return await UpdateAsync(microchipItem, cancellationToken);
        }


    }
}
