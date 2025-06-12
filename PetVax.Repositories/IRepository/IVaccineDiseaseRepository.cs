using PetVax.BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetVax.Repositories.IRepository
{
    public interface IVaccineDiseaseRepository
    {
        Task<List<VaccineDisease>> GetAllVaccineDiseaseAsync(CancellationToken cancellationToken);
        Task<VaccineDisease> GetVaccineDiseaseByIdAsync(int vaccineDiseaseId, CancellationToken cancellationToken);
        Task<int> CreateVaccineDiseaseAsync(VaccineDisease vaccineDisease, CancellationToken cancellationToken);
        Task<int> UpdateVaccineDiseaseAsync(VaccineDisease vaccineDisease, CancellationToken cancellationToken);
        Task<bool> DeleteVaccineDiseaseAsync(int vaccineDiseaseId, CancellationToken cancellationToken);
        Task<VaccineDisease> GetVaccineDiseaseByVaccineIdAsync(int vaccineId, CancellationToken cancellationToken);
        Task<VaccineDisease> GetVaccineDiseaseByDiseaseIdAsync(int diseaseId, CancellationToken cancellationToken);
    }
}
