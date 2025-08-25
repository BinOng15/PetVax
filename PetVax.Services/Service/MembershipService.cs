using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PetVax.BusinessObjects.DTO;
using PetVax.BusinessObjects.DTO.MembershipDTO;
using PetVax.BusinessObjects.DTO.PetDTO;
using PetVax.BusinessObjects.Helpers;
using PetVax.BusinessObjects.Models;
using PetVax.Repositories.IRepository;
using PetVax.Services.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PetVax.BusinessObjects.DTO.ResponseModel;

namespace PetVax.Services.Service
{
    public class MembershipService : IMembershipService
    {
        private readonly IMembershipRepository _membershipRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly ILogger<MembershipService> _logger;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public MembershipService(
            IMembershipRepository membershipRepository,
            ICustomerRepository customerRepository,
            ILogger<MembershipService> logger,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            _membershipRepository = membershipRepository;
            _customerRepository = customerRepository;
            _logger = logger;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<BaseResponse<CreateUpdateMembershipResponseDTO>> CreateMembershipAsync(CreateMembershipDTO createMembershipDTO, CancellationToken cancellationToken)
        {
            if (createMembershipDTO == null)
            {
                _logger.LogError("CreateMembershipAsync: createMembershipDTO is null");
                return new BaseResponse<CreateUpdateMembershipResponseDTO>
                {
                    Code = 400,
                    Success = false,
                    Message = "Dữ liệu để tạo membership không hợp lệ!",
                    Data = null
                };
            }

            // Validate Rank
            var allowedRanks = new[] { "silver", "gold", "bronze", "platinum", "diamond" };
            if (string.IsNullOrWhiteSpace(createMembershipDTO.Rank) ||
                !allowedRanks.Contains(createMembershipDTO.Rank.Trim().ToLower()))
            {
                _logger.LogError("CreateMembershipAsync: Invalid rank {Rank}", createMembershipDTO.Rank);
                return new BaseResponse<CreateUpdateMembershipResponseDTO>
                {
                    Code = 400,
                    Success = false,
                    Message = "Rank phải là 'bronze', 'silver','gold', 'platinum' hoặc 'diamond'!",
                    Data = null
                };
            }

            try
            {
                var existingMembership = await _membershipRepository.GetMembershipByRankAsync(createMembershipDTO.Rank, cancellationToken);
                if (existingMembership != null && existingMembership != null)
                {
                    _logger.LogWarning("CreateMembershipAsync: Membership with rank {Rank} already exists", createMembershipDTO.Rank);
                    return new BaseResponse<CreateUpdateMembershipResponseDTO>
                    {
                        Code = 409,
                        Success = false,
                        Message = "Membership với rank này đã tồn tại!",
                        Data = null
                    };
                }

                var membership = _mapper.Map<Membership>(createMembershipDTO);
                membership.MembershipCode = "MEM" + new Random().Next(100000, 1000000).ToString();
                membership.Name = createMembershipDTO.Name.Trim();
                membership.Description = createMembershipDTO.Description.Trim();
                membership.MinPoints = createMembershipDTO.MinPoints;
                membership.Benefits = createMembershipDTO.Benefits.Trim();
                membership.Rank = createMembershipDTO.Rank.Trim();
                membership.CreatedBy = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "Unknown";
                membership.CreatedAt = DateTimeHelper.Now();

                var membershipIds = await _membershipRepository.AddMembershipAsync(membership, cancellationToken);
                if (membershipIds < 0)
                {
                    _logger.LogError("CreateMembershipAsync: Failed to create membership");
                    return new BaseResponse<CreateUpdateMembershipResponseDTO>
                    {
                        Code = 500,
                        Success = false,
                        Message = "Không thể tạo membership!",
                        Data = null
                    };
                }

                var createdMembership = await _membershipRepository.GetMembershipByIdAsync(membershipIds, cancellationToken);
                if (createdMembership == null)
                {
                    _logger.LogError("CreateMembershipAsync: Created membership not found after creation");
                    return new BaseResponse<CreateUpdateMembershipResponseDTO>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Membership đã được tạo nhưng không tìm thấy!",
                        Data = null
                    };
                }
                var createdMembershipResponse = _mapper.Map<CreateUpdateMembershipResponseDTO>(createdMembership);
                _logger.LogInformation("CreateMembershipAsync: Membership created successfully with ID {MembershipId}", createdMembership.MembershipId);
                return new BaseResponse<CreateUpdateMembershipResponseDTO>
                {
                    Code = 201,
                    Success = true,
                    Message = "Membership đã được tạo thành công!",
                    Data = createdMembershipResponse
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "CreateMembershipAsync: An error occurred while creating membership");
                return new BaseResponse<CreateUpdateMembershipResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã xảy ra lỗi khi tạo membership!",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<bool>> DeleteMembershipAsync(int membershipId, CancellationToken cancellationToken)
        {
            if (membershipId <= 0)
            {
                _logger.LogError("DeleteMembershipAsync: Invalid membershipId {MembershipId}", membershipId);
                return new BaseResponse<bool>
                {
                    Code = 400,
                    Success = false,
                    Message = "ID membership không hợp lệ!",
                    Data = false
                };
            }
            try
            {
                var existingMembership = await _membershipRepository.GetMembershipByIdAsync(membershipId, cancellationToken);
                if (existingMembership == null)
                {
                    _logger.LogWarning("DeleteMembershipAsync: Membership with ID {MembershipId} not found", membershipId);
                    return new BaseResponse<bool>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Không tìm thấy membership với ID này!",
                        Data = false
                    };
                }
                var isDeleted = await _membershipRepository.DeleteMembershipAsync(membershipId, cancellationToken);
                if (!isDeleted)
                {
                    _logger.LogError("DeleteMembershipAsync: Failed to delete membership with ID {MembershipId}", membershipId);
                    return new BaseResponse<bool>
                    {
                        Code = 500,
                        Success = false,
                        Message = "Không thể xóa membership!",
                        Data = false
                    };
                }
                _logger.LogInformation("DeleteMembershipAsync: Membership with ID {MembershipId} deleted successfully", membershipId);
                return new BaseResponse<bool>
                {
                    Code = 200,
                    Success = true,
                    Message = "Membership đã được xóa thành công!",
                    Data = true
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "DeleteMembershipAsync: An error occurred while deleting membership with ID {MembershipId}", membershipId);
                return new BaseResponse<bool>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã xảy ra lỗi khi xóa membership!",
                    Data = false
                };
            }
        }

        public async Task<DynamicResponse<MembershipResponseDTO>> GetAllMembershipAsync(GetAllItemsDTO getAllItemsDTO, CancellationToken cancellationToken)
        {
            try
            {
                var memberships = await _membershipRepository.GetAllMembershipsAsync(cancellationToken);
                if (!string.IsNullOrWhiteSpace(getAllItemsDTO.KeyWord))
                {
                    memberships = memberships
                        .Where(m => m.Name.Contains(getAllItemsDTO.KeyWord, StringComparison.OrdinalIgnoreCase) ||
                                    m.Rank.Contains(getAllItemsDTO.KeyWord, StringComparison.OrdinalIgnoreCase) ||
                                    m.MembershipCode.Contains(getAllItemsDTO.KeyWord, StringComparison.OrdinalIgnoreCase))
                        .ToList();
                }

                int pageNumber = getAllItemsDTO?.PageNumber > 0 ? getAllItemsDTO.PageNumber : 1;
                int pageSize = getAllItemsDTO?.PageSize > 0 ? getAllItemsDTO.PageSize : 10;
                int skip = (pageNumber - 1) * pageSize;
                int totalItem = memberships.Count;
                int totalPage = (int)Math.Ceiling((double)totalItem / pageSize);

                var paginatedMemberships = memberships
                    .Skip(skip)
                    .Take(pageSize)
                    .ToList();

                var response = new MegaData<MembershipResponseDTO>
                {
                    PageInfo = new PagingMetaData
                    {
                        Page = pageNumber,
                        Size = pageSize,
                        TotalItem = totalItem,
                        TotalPage = totalPage
                    },
                    SearchInfo = new SearchCondition
                    {
                        keyWord = getAllItemsDTO?.KeyWord,
                        status = getAllItemsDTO?.Status,
                    },
                    PageData = _mapper.Map<List<MembershipResponseDTO>>(paginatedMemberships)
                };
                if (paginatedMemberships.Any())
                {
                    _logger.LogInformation("GetAllMembershipAsync: Retrieved {Count} memberships successfully", paginatedMemberships.Count);
                    return new DynamicResponse<MembershipResponseDTO>
                    {
                        Code = 200,
                        Success = true,
                        Message = "Lấy danh sách membership thành công!",
                        Data = response
                    };
                }
                else
                {
                    _logger.LogWarning("GetAllMembershipAsync: No memberships found");
                    return new DynamicResponse<MembershipResponseDTO>
                    {
                        Code = 200,
                        Success = true,
                        Message = "Không tìm thấy membership nào!",
                        Data = response
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetAllMembershipAsync: An error occurred while retrieving memberships");
                return new DynamicResponse<MembershipResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã xảy ra lỗi khi lấy danh sách membership!",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<MembershipRankingResponseDTO>> GetCustomerRankingInfoAsync(int customerId, CancellationToken cancellationToken)
        {
            if (customerId <= 0)
            {
                _logger.LogError("GetCustomerRankingInfoAsync: Invalid customerId {CustomerId}", customerId);
                return new BaseResponse<MembershipRankingResponseDTO>
                {
                    Code = 400,
                    Success = false,
                    Message = "ID khách hàng không hợp lệ!",
                    Data = null
                };
            }
            try
            {
                // 1. Get customer by ID
                var customer = await _customerRepository.GetCustomerByIdAsync(customerId, cancellationToken);
                if (customer == null)
                {
                    _logger.LogWarning("GetCustomerRankingInfoAsync: Customer with ID {CustomerId} not found", customerId);
                    return new BaseResponse<MembershipRankingResponseDTO>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Không tìm thấy khách hàng với ID này!",
                        Data = null
                    };
                }
                // 2. Get current points of the customer
                int currentPoints = customer.CurrentPoints ?? 0;
                // 3. Get all memberships and sort them by MinPoints ascending (lowest to highest)
                var memberships = await _membershipRepository.GetAllMembershipsAsync(cancellationToken);
                var sortedMemberships = memberships.OrderBy(m => m.MinPoints).ToList();

                // 4. Find the current membership (highest MinPoints <= currentPoints)
                Membership currentMembership = null;
                foreach (var m in sortedMemberships)
                {
                    if (currentPoints >= m.MinPoints)
                    {
                        currentMembership = m;
                    }
                    else
                    {
                        break;
                    }
                }

                // 5. Find the next membership (lowest MinPoints > currentPoints)
                var nextMembership = sortedMemberships.FirstOrDefault(m => m.MinPoints > currentPoints);

                int? pointsToNextRank = nextMembership != null ? nextMembership.MinPoints - currentPoints : 0;

                int redeemablePoints = customer.RedeemablePoints ?? 0;
                decimal totalSpents = customer.TotalSpent ?? 0.0m;

                int minPointsCurrentRank = currentMembership?.MinPoints ?? 0;
                int maxPointsCurrentRank = nextMembership?.MinPoints ?? int.MaxValue;

                var rankingInfo = new MembershipRankingResponseDTO
                {
                    CustomerId = customer.CustomerId,
                    MembershipId = customer.MembershipId,
                    CustomerCode = customer.CustomerCode,
                    FullName = customer.FullName ?? "Vui lòng cập nhật hồ sơ cá nhân",
                    CurrentRank = currentMembership?.Rank ?? "Chưa có hạng",
                    CurrentPoints = currentPoints,
                    MinPointsCurrentRank = minPointsCurrentRank,
                    MaxPointsCurrentRank = maxPointsCurrentRank,
                    NextRank = nextMembership?.Rank,
                    PointsToNextRank = pointsToNextRank > 0 ? pointsToNextRank : 0,
                    RedeemablePoints = redeemablePoints,
                    TotalSpent = totalSpents
                };

                _logger.LogInformation("GetCustomerRankingInfoAsync: Successfully retrieved ranking info for customer ID {CustomerId}", customerId);
                return new BaseResponse<MembershipRankingResponseDTO>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy thông tin xếp hạng khách hàng thành công!",
                    Data = rankingInfo
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetCustomerRankingInfoAsync: An error occurred while retrieving ranking info for customer ID {CustomerId}", customerId);
                return new BaseResponse<MembershipRankingResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã xảy ra lỗi khi lấy thông tin xếp hạng khách hàng!",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<MembershipResponseDTO>> GetMembershipByCustomerIdAsync(int customerId, CancellationToken cancellationToken)
        {
            if (customerId <= 0)
            {
                _logger.LogError("GetMembershipByCustomerIdAsync: Invalid customerId {CustomerId}", customerId);
                return new BaseResponse<MembershipResponseDTO>
                {
                    Code = 400,
                    Success = false,
                    Message = "ID khách hàng không hợp lệ!",
                    Data = null
                };
            }
            try
            {
                var membership = await _membershipRepository.GetMembershipByCustomerIdAsync(customerId, cancellationToken);
                if (membership == null)
                {
                    _logger.LogWarning("GetMembershipByCustomerIdAsync: Membership for customer ID {CustomerId} not found", customerId);
                    return new BaseResponse<MembershipResponseDTO>
                    {
                        Code = 200,
                        Success = false,
                        Message = "Không tìm thấy membership cho khách hàng này!",
                        Data = null
                    };
                }
                var membershipResponse = _mapper.Map<MembershipResponseDTO>(membership);
                _logger.LogInformation("GetMembershipByCustomerIdAsync: Membership for customer ID {CustomerId} retrieved successfully", customerId);
                return new BaseResponse<MembershipResponseDTO>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy thông tin membership thành công!",
                    Data = membershipResponse
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetMembershipByCustomerIdAsync: An error occurred while retrieving membership for customer ID {CustomerId}", customerId);
                return new BaseResponse<MembershipResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã xảy ra lỗi khi lấy thông tin membership!",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<MembershipResponseDTO>> GetMembershipByIdAsync(int membershipId, CancellationToken cancellationToken)
        {
            if (membershipId <= 0)
            {
                _logger.LogError("GetMembershipByIdAsync: Invalid membershipId {MembershipId}", membershipId);
                return new BaseResponse<MembershipResponseDTO>
                {
                    Code = 400,
                    Success = false,
                    Message = "ID membership không hợp lệ!",
                    Data = null
                };
            }
            try
            {
                var membership = await _membershipRepository.GetMembershipByIdAsync(membershipId, cancellationToken);
                if (membership == null)
                {
                    _logger.LogWarning("GetMembershipByIdAsync: Membership with ID {MembershipId} not found", membershipId);
                    return new BaseResponse<MembershipResponseDTO>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Không tìm thấy membership với ID này!",
                        Data = null
                    };
                }
                var membershipResponse = _mapper.Map<MembershipResponseDTO>(membership);
                _logger.LogInformation("GetMembershipByIdAsync: Membership with ID {MembershipId} retrieved successfully", membershipId);
                return new BaseResponse<MembershipResponseDTO>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy thông tin membership thành công!",
                    Data = membershipResponse
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetMembershipByIdAsync: An error occurred while retrieving membership with ID {MembershipId}", membershipId);
                return new BaseResponse<MembershipResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã xảy ra lỗi khi lấy thông tin membership!",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<MembershipResponseDTO>> GetMembershipByMembershipCodeAsync(string membershipCode, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(membershipCode))
            {
                _logger.LogError("GetMembershipByMembershipCodeAsync: Invalid membershipCode {MembershipCode}", membershipCode);
                return new BaseResponse<MembershipResponseDTO>
                {
                    Code = 400,
                    Success = false,
                    Message = "Mã membership không hợp lệ!",
                    Data = null
                };
            }
            try
            {
                var membership = await _membershipRepository.GetMembershipsByMembershiCodeAsync(membershipCode, cancellationToken);
                if (membership == null)
                {
                    _logger.LogWarning("GetMembershipByMembershipCodeAsync: Membership with code {MembershipCode} not found", membershipCode);
                    return new BaseResponse<MembershipResponseDTO>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Không tìm thấy membership với mã này!",
                        Data = null
                    };
                }
                var membershipResponse = _mapper.Map<MembershipResponseDTO>(membership);
                _logger.LogInformation("GetMembershipByMembershipCodeAsync: Membership with code {MembershipCode} retrieved successfully", membershipCode);
                return new BaseResponse<MembershipResponseDTO>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy thông tin membership thành công!",
                    Data = membershipResponse
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetMembershipByMembershipCodeAsync: An error occurred while retrieving membership with code {MembershipCode}", membershipCode);
                return new BaseResponse<MembershipResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã xảy ra lỗi khi lấy thông tin membership!",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<MembershipResponseDTO>> GetMembershipByRankAsync(string rank, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(rank))
            {
                _logger.LogError("GetMembershipByRankAsync: Invalid rank {Rank}", rank);
                return new BaseResponse<MembershipResponseDTO>
                {
                    Code = 400,
                    Success = false,
                    Message = "Rank không hợp lệ!",
                    Data = null
                };
            }
            try
            {
                var memberships = await _membershipRepository.GetMembershipByRankAsync(rank, cancellationToken);
                if (memberships == null)
                {
                    _logger.LogWarning("GetMembershipByRankAsync: No memberships found for rank {Rank}", rank);
                    return new BaseResponse<MembershipResponseDTO>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Không tìm thấy membership nào với rank này!",
                        Data = null
                    };
                }
                var membershipResponses = _mapper.Map<MembershipResponseDTO>(memberships);
                _logger.LogInformation("GetMembershipByRankAsync: Retrieved memberships for rank {Rank} successfully", rank);
                return new BaseResponse<MembershipResponseDTO>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy danh sách membership theo rank thành công!",
                    Data = membershipResponses
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetMembershipByRankAsync: An error occurred while retrieving memberships for rank {Rank}", rank);
                return new BaseResponse<MembershipResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã xảy ra lỗi khi lấy danh sách membership theo rank!",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<MembershipRankingFullResponseDTO>> GetMembershipStatusAsync(int customerId, CancellationToken cancellationToken)
        {
            if (customerId <= 0)
            {
                _logger.LogError("GetMembershipStatusAsync: Invalid customerId {CustomerId}", customerId);
                return new BaseResponse<MembershipRankingFullResponseDTO>
                {
                    Code = 400,
                    Success = false,
                    Message = "ID khách hàng không hợp lệ!",
                    Data = null
                };
            }
            try
            {
                var customerRankingInfoResponse = await GetCustomerRankingInfoAsync(customerId, cancellationToken);

                var allTiers = await _membershipRepository.GetAllMembershipsAsync(cancellationToken);
                if (allTiers == null || !allTiers.Any())
                {
                    _logger.LogWarning("GetMembershipStatusAsync: No membership tiers found");
                    return new BaseResponse<MembershipRankingFullResponseDTO>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Không tìm thấy bất kỳ hạng membership nào!",
                        Data = null
                    };
                }
                return new BaseResponse<MembershipRankingFullResponseDTO>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy thông tin trạng thái membership thành công!",
                    Data = new MembershipRankingFullResponseDTO
                    {
                        MembershipRankingResponseDTO = customerRankingInfoResponse.Data,
                        Memberships = _mapper.Map<List<MembershipStatusResponseDTO>>(allTiers)
                    }
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetMembershipStatusAsync: An error occurred while retrieving membership status for customer ID {CustomerId}", customerId);
                return new BaseResponse<MembershipRankingFullResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã xảy ra lỗi khi lấy thông tin trạng thái membership!",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<CreateUpdateMembershipResponseDTO>> UpdateMembershipAsync(int membershipId, UpdateMembershipDTO updateMembershipDTO, CancellationToken cancellationToken)
        {
            if (membershipId <= 0 || updateMembershipDTO == null)
            {
                _logger.LogError("UpdateMembershipAsync: Invalid membershipId {MembershipId} or updateMembershipDTO is null", membershipId);
                return new BaseResponse<CreateUpdateMembershipResponseDTO>
                {
                    Code = 400,
                    Success = false,
                    Message = "ID membership hoặc dữ liệu cập nhật không hợp lệ!",
                    Data = null
                };
            }
            try
            {
                var existingMembership = await _membershipRepository.GetMembershipByIdAsync(membershipId, cancellationToken);
                if (existingMembership == null)
                {
                    _logger.LogWarning("UpdateMembershipAsync: Membership with ID {MembershipId} not found", membershipId);
                    return new BaseResponse<CreateUpdateMembershipResponseDTO>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Không tìm thấy membership với ID này!",
                        Data = null
                    };
                }
                existingMembership.Name = updateMembershipDTO.Name?.Trim() ?? existingMembership.Name;
                existingMembership.Description = updateMembershipDTO.Description?.Trim() ?? existingMembership.Description;
                existingMembership.MinPoints = updateMembershipDTO.MinPoints ?? existingMembership.MinPoints;
                existingMembership.Benefits = updateMembershipDTO.Benefits?.Trim() ?? existingMembership.Benefits;
                existingMembership.Rank = updateMembershipDTO.Rank?.Trim() ?? existingMembership.Rank;
                existingMembership.ModifiedBy = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "Unknown";
                existingMembership.ModifiedAt = DateTimeHelper.Now();
                var updatedIds = await _membershipRepository.UpdateMembershipAsync(existingMembership, cancellationToken);
                if (updatedIds < 0)
                {
                    _logger.LogError("UpdateMembershipAsync: Failed to update membership with ID {MembershipId}", membershipId);
                    return new BaseResponse<CreateUpdateMembershipResponseDTO>
                    {
                        Code = 500,
                        Success = false,
                        Message = "Không thể cập nhật thông tin membership!",
                        Data = null
                    };
                }
                var updatedMembership = await _membershipRepository.GetMembershipByIdAsync(membershipId, cancellationToken);
                if (updatedMembership == null)
                {
                    _logger.LogError("UpdateMembershipAsync: Updated membership not found after update with ID {MembershipId}", membershipId);
                    return new BaseResponse<CreateUpdateMembershipResponseDTO>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Membership đã được cập nhật nhưng không tìm thấy!",
                        Data = null
                    };
                };
                var updatedMembershipResponse = _mapper.Map<CreateUpdateMembershipResponseDTO>(updatedMembership);
                _logger.LogInformation("UpdateMembershipAsync: Membership with ID {MembershipId} updated successfully", membershipId);
                return new BaseResponse<CreateUpdateMembershipResponseDTO>
                {
                    Code = 200,
                    Success = true,
                    Message = "Cập nhật thông tin membership thành công!",
                    Data = updatedMembershipResponse
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "UpdateMembershipAsync: An error occurred while updating membership with ID {MembershipId}", membershipId);
                return new BaseResponse<CreateUpdateMembershipResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã xảy ra lỗi khi cập nhật thông tin membership!",
                    Data = null
                };
            }
        }
    }
}
