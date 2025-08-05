using PetVax.BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetVax.Repositories.IRepository
{
    public interface IMicrochipItemRepository
    {
        Task<List<MicrochipItem>> GetAllMicrochipItemsAsync(CancellationToken cancellationToken);
        Task<MicrochipItem> GetMicrochipItemByIdAsync(int?  microchipItemId, CancellationToken cancellationToken);
        Task<MicrochipItem> GetMicrochipItemByMicrochipIdAsync(int microchipId, CancellationToken cancellationToken);
        Task<int> CreateMicrochipItemAsync(MicrochipItem microchipItem, CancellationToken cancellationToken);
        Task<int> UpdateMicrochipItemAsync(MicrochipItem microchipItem, CancellationToken cancellationToken);
        Task<bool> DeleteMicrochipItemAsync(int microchipItemId, CancellationToken cancellationToken);
        Task<MicrochipItem> GetMicrochipItemByMicrochipCodedAsync(string microchipCode, CancellationToken cancellationToken);
        Task<MicrochipItem> GetMicrochipItemByPetIdAsync(int? petId, CancellationToken cancellationToken);
        Task<int> GetTotalMicrochipItemsAsync(CancellationToken cancellationToken);
    }
}
