using PetVax.BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetVax.Repositories.IRepository
{
    public interface IVaccineExportRepository
    {
        Task<List<VaccineExport>> GetAllVaccineExportsAsync(CancellationToken cancellationToken);
        Task<VaccineExport> GetVaccineExportByIdAsync(int vaccineExportId, CancellationToken cancellationToken);
        Task<int> CreateVaccineExportAsync(VaccineExport vaccineExport, CancellationToken cancellationToken);
        Task<int> UpdateVaccineExportAsync(VaccineExport vaccineExport, CancellationToken cancellationToken);
        Task<bool> DeleteVaccineExportAsync(int vaccineExportId, CancellationToken cancellationToken);
    }
}
