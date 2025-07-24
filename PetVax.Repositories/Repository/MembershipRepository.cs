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
            await _context.AddAsync(membership, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return membership.MembershipId;
        }

        public async Task<bool> DeleteMembershipAsync(int id, CancellationToken cancellationToken)
        {
            return await DeleteAsync(id, cancellationToken);
        }

        public async Task<List<Membership>> GetAllMembershipsAsync(CancellationToken cancellationToken)
        {
            return await _context.Memberships
                .Include(m => m.Customers)
                .ToListAsync(cancellationToken);
        }

        public async Task<Membership> GetMembershipByCustomerIdAsync(int customerId, CancellationToken cancellationToken)
        {
            return await _context.Memberships
                .Include(m => m.Customers)
                .FirstOrDefaultAsync(m => m.Customers.Any(c => c.CustomerId == customerId), cancellationToken);
        }

        public async Task<Membership> GetMembershipByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.Memberships
                .Include(m => m.Customers)
                .FirstOrDefaultAsync(m => m.MembershipId == id, cancellationToken);
        }

        public async Task<Membership> GetMembershipByRankAsync(string rank, CancellationToken cancellationToken)
        {
            return await _context.Memberships
                .Where(m => m.Rank == rank)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<Membership> GetMembershipsByMembershiCodeAsync(string membershiCode, CancellationToken cancellationToken)
        {
            return await _context.Memberships
                .Include(m => m.Customers)
                .FirstOrDefaultAsync(m => m.MembershipCode == membershiCode, cancellationToken);
        }

        public async Task<int> UpdateMembershipAsync(Membership membership, CancellationToken cancellationToken)
        {
            return await UpdateAsync(membership, cancellationToken);
        }
    }
}
