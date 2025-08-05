using PetVax.BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetVax.Repositories.IRepository
{
    public interface IVoucherRepository
    {
        Task<List<Voucher>> GetAllVoucherAsync(CancellationToken cancellationToken);
        Task<Voucher> GetVoucherByIdAsync(int id, CancellationToken cancellationToken);
        Task<Voucher> GetVoucherByCodeAsync(string voucherCode, CancellationToken cancellationToken);
        Task<Voucher> GetVouchersByTransactionIdAsync(int transactionId, CancellationToken cancellationToken);
        Task<int> CreateVoucherAsync(Voucher voucher, CancellationToken cancellationToken);
        Task<int> UpdateVoucherAsync(Voucher voucher, CancellationToken cancellationToken);
        Task<bool> DeleteVoucherAsync(int id, CancellationToken cancellationToken);
        Task<Voucher> GetVoucherByPointsRequiredAsync(int pointsRequired, CancellationToken cancellationToken);
        Task<int> GetTotalVouchersAsync(CancellationToken cancellationToken);
    }
}
