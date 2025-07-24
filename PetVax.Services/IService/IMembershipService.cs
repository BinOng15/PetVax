using PetVax.BusinessObjects.DTO;
using PetVax.BusinessObjects.DTO.MembershipDTO;
using PetVax.BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PetVax.BusinessObjects.DTO.ResponseModel;

namespace PetVax.Services.IService
{
    public interface IMembershipService
    {
        Task<DynamicResponse<MembershipResponseDTO>> GetAllMembershipAsync(GetAllItemsDTO getAllItemsDTO, CancellationToken cancellationToken);
        Task<BaseResponse<MembershipResponseDTO>> GetMembershipByIdAsync(int membershipId, CancellationToken cancellationToken);
        Task<BaseResponse<MembershipResponseDTO>> GetMembershipByCustomerIdAsync(int customerId, CancellationToken cancellationToken);
        Task<BaseResponse<MembershipResponseDTO>> GetMembershipByMembershipCodeAsync(string membershipCode, CancellationToken cancellationToken);
        Task<BaseResponse<MembershipResponseDTO>> GetMembershipByRankAsync(string rank, CancellationToken cancellationToken);
        Task<BaseResponse<CreateUpdateMembershipResponseDTO>> CreateMembershipAsync(CreateMembershipDTO createMembershipDTO, CancellationToken cancellationToken);
        Task<BaseResponse<CreateUpdateMembershipResponseDTO>> UpdateMembershipAsync(int membershipId, UpdateMembershipDTO updateMembershipDTO, CancellationToken cancellationToken);
        Task<BaseResponse<bool>> DeleteMembershipAsync(int membershipId, CancellationToken cancellationToken);
    }
}
