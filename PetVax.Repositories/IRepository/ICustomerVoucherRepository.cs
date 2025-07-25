using PetVax.BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetVax.Repositories.IRepository
{
    public interface ICustomerVoucherRepository
    {
        Task<List<CustomerVoucher>> GetAllCustomerVouchersAsync(CancellationToken cancellationToken);
        Task<CustomerVoucher> GetCustomerVoucherByIdAsync(int customerVoucherId, CancellationToken cancellationToken);
        Task<List<CustomerVoucher>> GetCustomerVouchersByCustomerIdAsync(int customerId, CancellationToken cancellationToken);
        Task<List<CustomerVoucher>> GetCustomerVoucherByVoucherIdAsync(int voucherId, CancellationToken cancellationToken);
        Task<int> CreateCustomerVoucherAsync(CustomerVoucher customerVoucher, CancellationToken cancellationToken);
        Task<int> UpdateCustomerVoucherAsync(CustomerVoucher customerVoucher, CancellationToken cancellationToken);
        Task<bool> DeleteCustomerVoucherAsync(int customerVoucherId, CancellationToken cancellationToken);
        Task<CustomerVoucher> GetCustomerVoucherByCustomerIdAndVoucherIdAsync(int customerId, int voucherId, CancellationToken cancellationToken);
        Task UpdateExpiredCustomerVoucherAsync(CancellationToken cancellationToken);
    }
}
