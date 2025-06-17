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
    public class VetScheduleRepository : GenericRepository<VetSchedule>, IVetScheduleRepository
    {
        public VetScheduleRepository() : base()
        {
        }
        public async Task<int> CreateVetScheduleAsync(VetSchedule vetSchedule, CancellationToken cancellationToken)
        {
            return await CreateAsync(vetSchedule, cancellationToken);
        }

        public async Task<bool> DeleteVetScheduleAsync(int vetScheduleId, CancellationToken cancellationToken)
        {
            return await DeleteAsync(vetScheduleId, cancellationToken);
        }

        public async Task<List<VetSchedule>> GetAllVetSchedulesAsync(CancellationToken cancellationToken)
        {
            return await GetAllAsync(cancellationToken);
        }

        public async Task<VetSchedule> GetVetScheduleByIdAsync(int vetScheduleId, CancellationToken cancellationToken)
        {
            return await _context.VetSchedules.FirstOrDefaultAsync(vs => vs.VetScheduleId == vetScheduleId, cancellationToken);
        }

        public async Task<List<VetSchedule>> GetVetSchedulesByVetIdAsync(int vetId, CancellationToken cancellationToken)
        {
            return await _context.VetSchedules
                .Where(vs => vs.VetId == vetId)
                .ToListAsync(cancellationToken);
        }

        public async Task<int> UpdateVetScheduleAsync(VetSchedule vetSchedule, CancellationToken cancellationToken)
        {
            return await UpdateAsync(vetSchedule, cancellationToken);
        }
    }
}
