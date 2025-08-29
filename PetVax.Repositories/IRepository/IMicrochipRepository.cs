using PetVax.BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetVax.Repositories.IRepository
{
    public interface IMicrochipRepository
    {
        Task<List<Microchip>> GetAllMicrochipsAsync(CancellationToken cancellationToken);
        Task<Microchip> GetMicrochipByIdAsync(int microchipId, CancellationToken cancellationToken);
        Task<int> CreateMicrochipAsync(Microchip microchip, CancellationToken cancellationToken);
        Task<int> UpdateMicrochipAsync(Microchip microchip, CancellationToken cancellationToken);
        Task<bool> DeleteMicrochipAsync(int microchipId, CancellationToken cancellationToken);
        Task<int> GetTotalMicrochipsAsync(CancellationToken cancellationToken);
        Task<Microchip> GetMicrochipByCodeAsync(string microchipCode, CancellationToken cancellationToken);
    }
}
