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
    public class VaccineReceiptDetailRepository : GenericRepository<VaccineReceiptDetail>, IVaccineReceiptDetailRepository
    {
        public VaccineReceiptDetailRepository() : base()
        {
        }
        public async Task<int> CreateVaccineReceiptDetailAsync(VaccineReceiptDetail vaccineReceiptDetail, CancellationToken cancellationToken)
        {
            return await CreateAsync(vaccineReceiptDetail, cancellationToken);
        }
        public async Task<bool> DeleteVaccineReceiptDetailAsync(int vaccineReceiptDetailId, CancellationToken cancellationToken)
        {
            return await DeleteAsync(vaccineReceiptDetailId, cancellationToken);
        }
        public async Task<List<VaccineReceiptDetail>> GetAllVaccineReceiptDetailsAsync(CancellationToken cancellationToken)
        {
            return await _context.VaccineReceiptDetails
                .Include(vrd => vrd.VaccineBatch)
                    .ThenInclude(vb => vb.Vaccine)
                .Include(vrd => vrd.VaccineReceipt)
                .ToListAsync(cancellationToken);
        }
        public async Task<VaccineReceiptDetail> GetVaccineReceiptDetailByIdAsync(int vaccineReceiptDetailId, CancellationToken cancellationToken)
        {
            return await _context.VaccineReceiptDetails
                .Include(vrd => vrd.VaccineBatch)
                    .ThenInclude(vb => vb.Vaccine)
                .Include(vrd => vrd.VaccineReceipt)
                .FirstOrDefaultAsync(vrd => vrd.VaccineReceiptDetailId == vaccineReceiptDetailId, cancellationToken);
        }
        public async Task<List<VaccineReceiptDetail>> GetVaccineReceiptDetailsByVaccineReceiptIdAsync(int vaccineReceiptId, CancellationToken cancellationToken)
        {
            return await _context.VaccineReceiptDetails
                .Include(vrd => vrd.VaccineBatch)
                    .ThenInclude(vb => vb.Vaccine)
                .Include(vrd => vrd.VaccineReceipt)
                .Where(vrd => vrd.VaccineReceiptId == vaccineReceiptId && !vrd.isDeleted.HasValue || !vrd.isDeleted.Value)
                .ToListAsync(cancellationToken);
        }
        public async Task<VaccineReceiptDetail> GetVaccineReceiptDetailByVaccineBatchIdAsync(int vaccineBatchId, CancellationToken cancellationToken)
        {
            return await _context.VaccineReceiptDetails
                .Include(vrd => vrd.VaccineBatch)
                    .ThenInclude(vb => vb.Vaccine)
                .Include(vrd => vrd.VaccineReceipt)
                .FirstOrDefaultAsync(vrd => vrd.VaccineBatchId == vaccineBatchId && !vrd.isDeleted.HasValue || !vrd.isDeleted.Value, cancellationToken);
        }
        public async Task<int> UpdateVaccineReceiptDetailAsync(VaccineReceiptDetail vaccineReceiptDetail, CancellationToken cancellationToken)
        {
            return await UpdateAsync(vaccineReceiptDetail, cancellationToken);
        }
    }
}
