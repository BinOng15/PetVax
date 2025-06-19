using PetVax.BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetVax.Repositories.IRepository
{
    public interface IPetRepository
    {
        Task<List<Pet>> GetAllPetsAsync(CancellationToken cancellationToken);
        Task<Pet> GetPetByIdAsync(int petId, CancellationToken cancellationToken);
        Task<Pet> GetPetByNameAsync(string petName, CancellationToken cancellationToken);
        Task<int> CreatePetAsync(Pet pet, CancellationToken cancellationToken);
        Task<int> UpdatePetAsync(Pet pet, CancellationToken cancellationToken);
        Task<bool> DeletePetAsync(int petId, CancellationToken cancellationToken);
        Task<List<Pet>> GetPetsByCustomerIdAsync(int customerId, CancellationToken cancellationToken);

        Task<Pet> GetPetAndAppointmentByIdAsync(int petId, CancellationToken cancellationToken);
    }
}
