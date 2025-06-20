using PetVax.BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetVax.Repositories.IRepository
{
    public interface IVaccineProfileRepository
    {
        Task<List<VaccineProfile>> GetAllVaccineProfilesAsync(CancellationToken cancellationToken);
        Task<VaccineProfile> GetVaccineProfileByIdAsync(int vaccineProfileId, CancellationToken cancellationToken);
        Task<VaccineProfile> GetVaccineProfileByPetIdAsync(int petId, CancellationToken cancellationToken);
        Task<int> CreateVaccineProfileAsync(VaccineProfile vaccineProfile, CancellationToken cancellationToken);
        Task<int> UpdateVaccineProfileAsync(VaccineProfile vaccineProfile, CancellationToken cancellationToken);
        Task<bool> DeleteVaccineProfileAsync(int vaccineProfileId, CancellationToken cancellationToken);
        Task<List<VaccineProfile>> GetVaccineProfilesByDiseaseId(int diseaseId, CancellationToken cancellationToken);
    }
}
