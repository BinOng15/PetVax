using Microsoft.EntityFrameworkCore;
using PetVax.BusinessObjects.Enum;
using PetVax.BusinessObjects.Helpers;
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
    public class CustomerVoucherRepository : GenericRepository<CustomerVoucher>, ICustomerVoucherRepository
    {
        public CustomerVoucherRepository() : base()
        {
        }

        public async Task<int> CreateCustomerVoucherAsync(CustomerVoucher customerVoucher, CancellationToken cancellationToken)
        {
            await _context.AddAsync(customerVoucher, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return customerVoucher.CustomerVoucherId;
        }

        public async Task<bool> DeleteCustomerVoucherAsync(int customerVoucherId, CancellationToken cancellationToken)
        {
            return await DeleteAsync(customerVoucherId, cancellationToken);
        }

        public async Task<List<CustomerVoucher>> GetAllCustomerVouchersAsync(CancellationToken cancellationToken)
        {
            return await _context.CustomerVouchers
                .Include(cv => cv.Voucher)
                .Include(cv => cv.Customer)
                    .ThenInclude(c => c.Membership)
                .Include(cv => cv.Customer)
                    .ThenInclude(c => c.Account)
                .ToListAsync(cancellationToken);
        }

        public async Task<CustomerVoucher> GetCustomerVoucherByCustomerIdAndVoucherIdAsync(int customerId, int voucherId, CancellationToken cancellationToken)
        {
            return await _context.CustomerVouchers
                .Include(cv => cv.Voucher)
                .Include(cv => cv.Customer)
                    .ThenInclude(c => c.Membership)
                .Include(cv => cv.Customer)
                    .ThenInclude(c => c.Account)
                .FirstOrDefaultAsync(cv => cv.CustomerId == customerId && cv.VoucherId == voucherId, cancellationToken);
        }

        public async Task<CustomerVoucher> GetCustomerVoucherByIdAsync(int customerVoucherId, CancellationToken cancellationToken)
        {
            return await _context.CustomerVouchers
                .Include(cv => cv.Voucher)
                .Include(cv => cv.Customer)
                    .ThenInclude(c => c.Membership)
                .Include(cv => cv.Customer)
                    .ThenInclude(c => c.Account)
                .FirstOrDefaultAsync(cv => cv.CustomerVoucherId == customerVoucherId, cancellationToken);
        }

        public async Task<List<CustomerVoucher>> GetCustomerVoucherByVoucherIdAsync(int voucherId, CancellationToken cancellationToken)
        {
            return await _context.CustomerVouchers
                .Include(cv => cv.Voucher)
                .Include(cv => cv.Customer)
                    .ThenInclude(c => c.Membership)
                .Include(cv => cv.Customer)
                    .ThenInclude(c => c.Account)
                .Where(cv => cv.VoucherId == voucherId)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<CustomerVoucher>> GetCustomerVouchersByCustomerIdAsync(int customerId, CancellationToken cancellationToken)
        {
            return await _context.CustomerVouchers
                .Include(cv => cv.Voucher)
                .Include(cv => cv.Customer)
                    .ThenInclude(c => c.Membership)
                .Include(cv => cv.Customer)
                    .ThenInclude(c => c.Account)
                .Where(cv => cv.CustomerId == customerId)
                .ToListAsync(cancellationToken);
        }

        public async Task<int> UpdateCustomerVoucherAsync(CustomerVoucher customerVoucher, CancellationToken cancellationToken)
        {
            return await UpdateAsync(customerVoucher, cancellationToken);
        }

        public async Task UpdateExpiredCustomerVoucherAsync(CancellationToken cancellationToken)
        {
            var expiredVouchers = await _context.CustomerVouchers
                .Where(cv => cv.Status != EnumList.VoucherStatus.Expired && cv.ExpirationDate < DateTimeHelper.Now())
                .ToListAsync(cancellationToken);

            if (expiredVouchers.Any())
            {
                foreach (var voucher in expiredVouchers)
                {
                    voucher.Status = EnumList.VoucherStatus.Expired;
                    voucher.ModifiedAt = DateTimeHelper.Now();
                    voucher.ModifiedBy = "System-Auto";
                }
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
