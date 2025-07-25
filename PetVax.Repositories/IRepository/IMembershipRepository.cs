using PetVax.BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetVax.Repositories.IRepository
{
    public interface IMembershipRepository
    {
        
        Task<List<Membership>> GetAllMembershipsAsync(CancellationToken cancellationToken);
        Task<Membership> GetMembershipByIdAsync(int id, CancellationToken cancellationToken);
        Task<int> AddMembershipAsync(Membership membership, CancellationToken cancellationToken);
        Task<int> UpdateMembershipAsync(Membership membership, CancellationToken cancellationToken);
        Task<bool> DeleteMembershipAsync(int id, CancellationToken cancellationToken);
        Task<Membership> GetMembershipsByMembershiCodeAsync(string membershiCode, CancellationToken cancellationToken);
        Task<Membership> GetMembershipByCustomerIdAsync(int customerId, CancellationToken cancellationToken);
        Task<Membership> GetMembershipByRankAsync(string rank, CancellationToken cancellationToken);
    }
}
