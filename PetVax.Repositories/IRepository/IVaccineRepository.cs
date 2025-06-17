using PetVax.BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetVax.Repositories.IRepository
{
    public interface IVaccineRepository
    {
        Task<List<Vaccine>> GetAllVaccineAsync(CancellationToken cancellationToken);
        Task<Vaccine> GetVaccineByIdAsync(int vaccineId, CancellationToken cancellationToken);
        Task<int> CreateVaccineAsync(Vaccine vaccine, CancellationToken cancellationToken);
        Task<int> UpdateVaccineAsync(Vaccine vaccine, CancellationToken cancellationToken);
        Task<bool> DeleteVaccineAsync(int vaccineId, CancellationToken cancellationToken);
        Task<Vaccine> GetVaccineByVaccineCodeAsync(string vaccineCode, CancellationToken cancellationToken);
        Task<Vaccine> GetVaccineByName(string Name, CancellationToken cancellationToken);

    }
}
