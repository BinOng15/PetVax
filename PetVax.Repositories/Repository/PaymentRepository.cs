using Microsoft.EntityFrameworkCore;
using PetVax.BusinessObjects.Enum;
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
            await _context.Payments.AddAsync(payment, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return payment.PaymentId;
        }

        public async Task<bool> DeletePaymentAsync(int id, CancellationToken cancellationToken)
        {
            return await DeleteAsync(id, cancellationToken);
        }

        public async Task<List<Payment>> GetAllPaymentsAsync(CancellationToken cancellationToken)
        {
            return await _context.Payments
                .Include(p => p.AppointmentDetail)
                .Include(p => p.Customer)
                .Include(p => p.VaccineBatch)
                .Include(p => p.Microchip)
                .Include(p => p.VaccinationCertificate)
                .Include(p => p.HealthCondition)
                .Where(p => p.isDeleted == false)
                .ToListAsync(cancellationToken);
        }

        public async Task<Payment?> GetLastestFailedPaymentByAppointmentDetailIdAsync(int appointmentDetailId, CancellationToken cancellationToken)
        {
            return await _context.Payments
                .Include(p => p.AppointmentDetail)
                .Include(p => p.Customer)
                .Include(p => p.VaccineBatch)
                .Include(p => p.Microchip)
                .Include(p => p.VaccinationCertificate)
                .Include(p => p.HealthCondition)
                .Where(p => p.AppointmentDetailId == appointmentDetailId &&
                           p.PaymentStatus == EnumList.PaymentStatus.Failed)
                .OrderByDescending(p => p.CreatedAt)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<List<Payment>> GetPaymentByAppointmentDetailIdAsync(int appointmentDetailId, CancellationToken cancellationToken)
        {
            return await _context.Payments
                .Include(p => p.AppointmentDetail)
                .Include(p => p.Customer)
                .Include(p => p.VaccineBatch)
                .Include(p => p.Microchip)
                .Include(p => p.VaccinationCertificate)
                .Include(p => p.HealthCondition)
                .Where(p => p.AppointmentDetailId == appointmentDetailId)
                .ToListAsync(cancellationToken);
        }

        public async Task<Payment?> GetPaymentByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.Payments
                .Include(p => p.AppointmentDetail)
                .Include(p => p.Customer)
                .Include(p => p.VaccineBatch)
                .Include(p => p.Microchip)
                .Include(p => p.VaccinationCertificate)
                .Include(p => p.HealthCondition)
                .FirstOrDefaultAsync(p => p.PaymentId == id && p.isDeleted == false, cancellationToken);
        }

        public async Task<List<Payment>> GetPaymentsByAccountIdAsync(int customerId, CancellationToken cancellationToken)
        {
            return await _context.Payments
                .Include(p => p.AppointmentDetail)
                .Include(p => p.Customer)
                .Include(p => p.VaccineBatch)
                .Include(p => p.Microchip)
                .Include(p => p.VaccinationCertificate)
                .Include(p => p.HealthCondition)
                .Where(p => p.CustomerId == customerId && p.isDeleted == false)
                .ToListAsync(cancellationToken);
        }

        public async Task<List<Payment>> GetPaymentsByAppointmentIdAsync(int appointmenDetailtId, CancellationToken cancellationToken)
        {
            return await _context.Payments
                .Include(p => p.AppointmentDetail)
                .Include(p => p.Customer)
                .Include(p => p.VaccineBatch)
                .Include(p => p.Microchip)
                .Include(p => p.VaccinationCertificate)
                .Include(p => p.HealthCondition)
                .Where(p => p.AppointmentDetailId == appointmenDetailtId && p.isDeleted == false)
                .ToListAsync(cancellationToken);
        }

        public async Task<int> GetTotalPaymentsAsync(CancellationToken cancellationToken)
        {
            return await _context.Payments
                .Where(p => p.isDeleted == false)
                .CountAsync(cancellationToken);
        }

        public async Task<int> UpdatePaymentAsync(Payment payment, CancellationToken cancellationToken)
        {
            return await UpdateAsync(payment, cancellationToken);
        }
    }
}
