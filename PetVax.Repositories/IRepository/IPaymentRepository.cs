using PetVax.BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetVax.Repositories.IRepository
{
    public interface IPaymentRepository
    {
        Task<List<Payment>> GetAllPaymentsAsync(CancellationToken cancellationToken);
        Task<Payment?> GetPaymentByIdAsync(int id, CancellationToken cancellationToken);
        Task<int> AddPaymentAsync(Payment payment, CancellationToken cancellationToken);
        Task<int> UpdatePaymentAsync(Payment payment, CancellationToken cancellationToken);
        Task<bool> DeletePaymentAsync(int id, CancellationToken cancellationToken);
        Task<List<Payment>> GetPaymentsByAccountIdAsync(int customerId, CancellationToken cancellationToken);
        Task<List<Payment>> GetPaymentsByAppointmentIdAsync(int appointmenDetailtId, CancellationToken cancellationToken);
        Task<Payment> GetPaymentByAppointmentDetailIdAsync(int appointmentDetailId, CancellationToken cancellationToken);
    }
}
