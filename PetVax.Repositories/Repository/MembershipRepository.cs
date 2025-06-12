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
    public class MembershipRepository : GenericRepository<Membership>, IMembershipRepository
    {
        public MembershipRepository() : base()
        {
        }
        public async Task<int> AddMembershipAsync(Membership membership, CancellationToken cancellationToken)
        {
            return await CreateAsync(membership, cancellationToken);
        }

        public async Task<bool> DeleteMembershipAsync(int id, CancellationToken cancellationToken)
        {
            return await DeleteAsync(id, cancellationToken);
        }

        public async Task<List<Membership>> GetAllMembershipsAsync(CancellationToken cancellationToken)
        {
            return await GetAllAsync(cancellationToken);
        }

        public async Task<Membership?> GetMembershipByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await GetByIdAsync(id, cancellationToken);
        }

        public async Task<Membership> GetMembershipsByMembershiCodeAsync(string membershiCode, CancellationToken cancellationToken)
        {
            return await _context.Memberships.FirstOrDefaultAsync(m => m.MembershipCode == membershiCode, cancellationToken);
        }

        public async Task<int> UpdateMembershipAsync(Membership membership, CancellationToken cancellationToken)
        {
            return await UpdateAsync(membership, cancellationToken);
        }
    }
}
