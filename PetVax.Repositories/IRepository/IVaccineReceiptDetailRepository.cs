using PetVax.BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetVax.Repositories.IRepository
{
    public interface IVaccineReceiptDetailRepository
    {
        Task<List<VaccineReceiptDetail>> GetAllVaccineReceiptDetailsAsync(CancellationToken cancellationToken);
        Task<VaccineReceiptDetail> GetVaccineReceiptDetailByIdAsync(int vaccineReceiptDetailId, CancellationToken cancellationToken);
        Task<int> CreateVaccineReceiptDetailAsync(VaccineReceiptDetail vaccineReceiptDetail, CancellationToken cancellationToken);
        Task<int> UpdateVaccineReceiptDetailAsync(VaccineReceiptDetail vaccineReceiptDetail, CancellationToken cancellationToken);
        Task<bool> DeleteVaccineReceiptDetailAsync(int vaccineReceiptDetailId, CancellationToken cancellationToken);
        Task<List<VaccineReceiptDetail>> GetVaccineReceiptDetailsByVaccineReceiptIdAsync(int vaccineReceiptId, CancellationToken cancellationToken);
        Task<VaccineReceiptDetail> GetVaccineReceiptDetailByVaccineBatchIdAsync(int vaccineBatchId, CancellationToken cancellationToken);
        Task<int> GetTotalVaccineReceiptDetailsAsync(CancellationToken cancellationToken);
    }
}
