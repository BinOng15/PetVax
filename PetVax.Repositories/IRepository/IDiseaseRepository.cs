using PetVax.BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetVax.Repositories.IRepository
{
    public interface IDiseaseRepository
    {

        Task<List<Disease>> GetAllDiseaseAsync(CancellationToken cancellationToken);
        Task<Disease> GetDiseaseByIdAsync(int diseaseId, CancellationToken cancellationToken);
        Task<int> CreateDiseaseAsync(Disease disease, CancellationToken cancellationToken);
        Task<int> UpdateDiseaseAsync(Disease disease, CancellationToken cancellationToken);
        Task<bool> DeleteDiseaseAsync(int diseaseId, CancellationToken cancellationToken);
        Task<Disease> GetDiseaseByName(string name, CancellationToken cancellationToken);
        Task<List<Disease>> GetDiseaseBySpecies(string species, CancellationToken cancellationToken);
        Task<List<Disease>> GetDiseaseByVaccineId(int vaccineId, CancellationToken cancellationToken);
        Task<int> GetTotalDiseasesAsync(CancellationToken cancellationToken);
    }
}
