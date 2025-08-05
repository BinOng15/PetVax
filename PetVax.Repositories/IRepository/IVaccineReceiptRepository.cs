using PetVax.BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetVax.Repositories.IRepository
{
    public interface IVaccineReceiptRepository
    {
        Task<List<VaccineReceipt>> GetAllVaccineReceiptsAsync(CancellationToken cancellationToken);
        Task<VaccineReceipt> GetVaccineReceiptByIdAsync(int vaccineReceiptId, CancellationToken cancellationToken);
        Task<int> CreateVaccineReceiptAsync(VaccineReceipt vaccineReceipt, CancellationToken cancellationToken);
        Task<int> UpdateVaccineReceiptAsync(VaccineReceipt vaccineReceipt, CancellationToken cancellationToken);
        Task<bool> DeleteVaccineReceiptAsync(int vaccineReceiptId, CancellationToken cancellationToken);
        Task<VaccineReceipt> GetVaccineReceiptByReceiptCodeAsync(string receiptCode, CancellationToken cancellationToken);
        Task<int> GetTotalVaccineReceiptsAsync(CancellationToken cancellationToken);
    }
}
