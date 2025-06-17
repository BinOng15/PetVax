using PetVax.BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetVax.Repositories.IRepository
{
    public interface IPetPassportRepository
    {
        Task<List<PetPassport>> GetAllPetPassportsAsync(CancellationToken cancellationToken);
        Task<PetPassport?> GetPetPassportByIdAsync(int id, CancellationToken cancellationToken);
        Task<int> AddPetPassportAsync(PetPassport petPassport, CancellationToken cancellationToken);
        Task<int> UpdatePetPassportAsync(PetPassport petPassport, CancellationToken cancellationToken);
        Task<bool> DeletePetPassportAsync(int id, CancellationToken cancellationToken);
        Task<List<PetPassport>> GetPetPassportsByPetIdAsync(int petId, CancellationToken cancellationToken);

        Task<List<PetPassport>> GetPetPassportByMicrochipItemIdAsync(int microchipItemId, CancellationToken cancellationToken);

        Task<PetPassport> GetPetPassPortByPassportCodeAsync(string passportCode, CancellationToken cancellationToken);
    }
}
