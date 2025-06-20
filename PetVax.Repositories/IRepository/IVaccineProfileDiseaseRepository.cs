using PetVax.BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetVax.Repositories.IRepository
{
    public interface IVaccineProfileDiseaseRepository
    {
        Task<List<VaccineProfileDisease>> GetAllVaccineProfileDiseasesAsync(CancellationToken cancellationToken);
        Task<VaccineProfileDisease> GetVaccineProfileDiseaseByIdAsync(int vaccineProfileDiseaseId, CancellationToken cancellationToken);
        Task<int> CreateVaccineProfileDiseaseAsync(VaccineProfileDisease vaccineProfileDisease, CancellationToken cancellationToken);
        Task<int> UpdateVaccineProfileDiseaseAsync(VaccineProfileDisease vaccineProfileDisease, CancellationToken cancellationToken);
        Task<bool> DeleteVaccineProfileAsync(int vaccineProfileDiseaseId, CancellationToken cancellationToken);
        Task<List<VaccineProfileDisease>> GetVaccineProfileDiseasesByDiseaseIdAsync(int diseaseId, CancellationToken cancellationToken);
        Task<List<VaccineProfileDisease>> GetVaccineProfileDiseasesByVaccineProfileIdAsync(int vaccineProfileId, CancellationToken cancellationToken);
    }
}
