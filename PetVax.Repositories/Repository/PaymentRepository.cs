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
    public class PaymentRepository : GenericRepository<Payment>, IPaymentRepository
    {
        public PaymentRepository() : base()
        {
        }
        public async Task<int> AddPaymentAsync(Payment payment, CancellationToken cancellationToken)
        {
            return await CreateAsync(payment, cancellationToken);
        }

        public async Task<bool> DeletePaymentAsync(int id, CancellationToken cancellationToken)
        {
            return await DeleteAsync(id, cancellationToken);
        }

        public async Task<List<Payment>> GetAllPaymentsAsync(CancellationToken cancellationToken)
        {
            return await GetAllAsync(cancellationToken);
        }

        public async Task<Payment?> GetPaymentByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await GetByIdAsync(id, cancellationToken);
        }

        public async Task<List<Payment>> GetPaymentsByAccountIdAsync(int customerId, CancellationToken cancellationToken)
        {
            return await _context.Payments
                .Where(p => p.CustomerId == customerId)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<Payment>> GetPaymentsByAppointmentIdAsync(int appointmenDetailtId, CancellationToken cancellationToken)
        {
            return await _context.Payments
                .Where(p => p.AppointmentDetailId == appointmenDetailtId)
                .ToListAsync(cancellationToken);
        }

        public async Task<int> UpdatePaymentAsync(Payment payment, CancellationToken cancellationToken)
        {
            return await UpdateAsync(payment, cancellationToken);
        }
    }
}
