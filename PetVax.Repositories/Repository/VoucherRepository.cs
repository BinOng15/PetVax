using Microsoft.EntityFrameworkCore;
using PetVax.BusinessObjects.Models;
using PetVax.Repositories.IRepository;
using PetVax.Repositories.Repository.BaseResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetVax.Repositories.Repository
{
    public class VoucherRepository : GenericRepository<Voucher>, IVoucherRepository
    {
        public VoucherRepository() : base()
        {
        }

        public async Task<int> CreateVoucherAsync(Voucher voucher, CancellationToken cancellationToken)
        {
            return await CreateAsync(voucher, cancellationToken);
        }

        public async Task<bool> DeleteVoucherAsync(int id, CancellationToken cancellationToken)
        {
            return await DeleteAsync(id, cancellationToken);
        }

        public async Task<List<Voucher>> GetAllVoucherAsync(CancellationToken cancellationToken)
        {
            return await _context.Vouchers
                .Where(v => v.isDeleted == false)
                .ToListAsync(cancellationToken);
        }

        public async Task<Voucher> GetVoucherByCodeAsync(string voucherCode, CancellationToken cancellationToken)
        {
            return await _context.Vouchers
                .FirstOrDefaultAsync(v => v.VoucherCode == voucherCode && v.isDeleted == false, cancellationToken);
        }

        public async Task<Voucher> GetVoucherByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.Vouchers
                .FirstOrDefaultAsync(v => v.VoucherId == id && v.isDeleted == false, cancellationToken);
        }

        public async Task<List<Voucher>> GetVouchersByTransactionIdAsync(int transactionId, CancellationToken cancellationToken)
        {
            return await _context.Vouchers
                .Where(v => v.TransactionId == transactionId && v.isDeleted == false)
                .ToListAsync(cancellationToken);
        }

        public async Task<int> UpdateVoucherAsync(Voucher voucher, CancellationToken cancellationToken)
        {
            return await UpdateAsync(voucher, cancellationToken);
        }
    }
}
