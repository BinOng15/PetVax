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
            await _context.AddAsync(voucher, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return voucher.VoucherId;
        }

        public async Task<bool> DeleteVoucherAsync(int id, CancellationToken cancellationToken)
        {
            return await DeleteAsync(id, cancellationToken);
        }

        public async Task<List<Voucher>> GetAllVoucherAsync(CancellationToken cancellationToken)
        {
            return await _context.Vouchers
                .Include(v => v.PointTransaction)
                .Where(v => v.isDeleted == false)
                .ToListAsync(cancellationToken);
        }

        public async Task<int> GetTotalVouchersAsync(CancellationToken cancellationToken)
        {
            return await _context.Vouchers
                .Where(v => v.isDeleted == false)
                .CountAsync(cancellationToken);
        }

        public async Task<Voucher> GetVoucherByCodeAsync(string voucherCode, CancellationToken cancellationToken)
        {
            return await _context.Vouchers
                .Include(v => v.PointTransaction)
                .FirstOrDefaultAsync(v => v.VoucherCode == voucherCode && v.isDeleted == false, cancellationToken);
        }

        public async Task<Voucher> GetVoucherByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.Vouchers
                .Include(v => v.PointTransaction)
                .FirstOrDefaultAsync(v => v.VoucherId == id && v.isDeleted == false, cancellationToken);
        }

        public async Task<Voucher> GetVoucherByPointsRequiredAsync(int pointsRequired, CancellationToken cancellationToken)
        {
            return await _context.Vouchers
                .Include(v => v.PointTransaction)
                .FirstOrDefaultAsync(v => v.PointsRequired == pointsRequired && v.isDeleted == false, cancellationToken);
        }

        public async Task<Voucher> GetVouchersByTransactionIdAsync(int transactionId, CancellationToken cancellationToken)
        {
            return await _context.Vouchers
                .Include(v => v.PointTransaction)
                .Where(v => v.TransactionId == transactionId && v.isDeleted == false)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<int> UpdateVoucherAsync(Voucher voucher, CancellationToken cancellationToken)
        {
            return await UpdateAsync(voucher, cancellationToken);
        }
    }
}
