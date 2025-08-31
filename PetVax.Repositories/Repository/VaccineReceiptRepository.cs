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
    public class VaccineReceiptRepository : GenericRepository<VaccineReceipt>, IVaccineReceiptRepository
    {
        public VaccineReceiptRepository() : base()
        {
        }

        public async Task<int> CreateVaccineReceiptAsync(VaccineReceipt vaccineReceipt, CancellationToken cancellationToken)
        {
            await _context.VaccineReceipts.AddAsync(vaccineReceipt, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return vaccineReceipt.VaccineReceiptId;
        }

        public async Task<bool> DeleteVaccineReceiptAsync(int vaccineReceiptId, CancellationToken cancellationToken)
        {
            return await DeleteAsync(vaccineReceiptId, cancellationToken);
        }

        public async Task<List<VaccineReceipt>> GetAllVaccineReceiptsAsync(CancellationToken cancellationToken)
        {
            return await _context.VaccineReceipts
                .OrderByDescending(v => v.CreatedAt)
                .ToListAsync(cancellationToken);
        }

        public async Task<int> GetTotalVaccineReceiptsAsync(CancellationToken cancellationToken)
        {
            return await _context.VaccineReceipts.Where(vr => vr.isDeleted == false)
                .CountAsync(cancellationToken);
        }

        public async Task<VaccineReceipt> GetVaccineReceiptByIdAsync(int vaccineReceiptId, CancellationToken cancellationToken)
        {
            return await _context.VaccineReceipts
                .FirstOrDefaultAsync(vr => vr.VaccineReceiptId == vaccineReceiptId, cancellationToken);
        }

        public async Task<VaccineReceipt> GetVaccineReceiptByReceiptCodeAsync(string receiptCode, CancellationToken cancellationToken)
        {
            return await _context.VaccineReceipts
                .FirstOrDefaultAsync(vr => vr.ReceiptCode == receiptCode, cancellationToken);
        }

        public async Task<int> UpdateVaccineReceiptAsync(VaccineReceipt vaccineReceipt, CancellationToken cancellationToken)
        {
            return await UpdateAsync(vaccineReceipt, cancellationToken);
        }
    }
}
