using PetVax.BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetVax.Repositories.IRepository
{
    public interface IVaccineBatchRepository
    {
        Task<List<VaccineBatch>> GetAllVaccineBatchAsync(CancellationToken cancellationToken);
        Task<VaccineBatch> GetVaccineBatchByIdAsync(int vaccineBatchId, CancellationToken cancellationToken);
        Task<int> CreateVaccineBatchAsync(VaccineBatch vaccineBatch, CancellationToken cancellationToken);
        Task<int> UpdateVaccineBatchAsync(VaccineBatch vaccineBatch, CancellationToken cancellationToken);
        Task<bool> DeleteVaccineBatchAsync(int vaccineBatchId, CancellationToken cancellationToken);
        Task<VaccineBatch> GetVaccineBatchByVaccineCodeAsync(string vaccineCode, CancellationToken cancellationToken);
        Task<VaccineBatch> GetVaccineBatchByVaccineId(int vaccineId, CancellationToken cancellationToken);
        Task<int> GetTotalVaccineBatchesAsync(CancellationToken cancellationToken);
    }
}
