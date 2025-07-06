using AutoMapper;
using Microsoft.AspNetCore.Http;
using PetVax.BusinessObjects.DTO.AppointmentDetailDTO;
using PetVax.BusinessObjects.DTO.HealthConditionDTO;
using PetVax.BusinessObjects.DTO.VetDTO;
using PetVax.Repositories.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PetVax.BusinessObjects.DTO.ResponseModel;

namespace PetVax.Services.Service
{
    public class HealthConditionService
    {
        private readonly IHealthConditionRepository _healthConditionRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public HealthConditionService(IHealthConditionRepository healthConditionRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor)
        {
            _healthConditionRepository = healthConditionRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        // / Get all health conditions with pagination and Response DTO
        public async Task<DynamicResponse<BaseHealthConditionResponseDTO>> GetAllHealthConditionsAsync(GetAllVetRequestDTO getAllVetRequest, CancellationToken cancellationToken)
        {
            try
            {
                var healthConditions = await _healthConditionRepository.GetAllHealthConditionsAsync(cancellationToken);

                if (!string.IsNullOrWhiteSpace(getAllVetRequest.KeyWord))
                {
                    var keyword = getAllVetRequest.KeyWord.ToLower();
                    healthConditions = healthConditions
                        .Where(hc =>
                            (hc.ConditionCode != null && hc.ConditionCode.ToLower().Contains(keyword)) ||
                            (hc.Conclusion != null && hc.Conclusion.ToLower().Contains(keyword))
                        )
                        .ToList();
                }

                int pageNumber = getAllVetRequest?.PageNumber > 0 ? getAllVetRequest.PageNumber : 1;
                int pageSize = getAllVetRequest?.PageSize > 0 ? getAllVetRequest.PageSize : 10;
                int skip = (pageNumber - 1) * pageSize;
                int totalItem = healthConditions.Count;
                int totalPage = (int)Math.Ceiling((double)totalItem / pageSize);

                var pagedHealthConditions = healthConditions
                    .Skip(skip)
                    .Take(pageSize)
                    .ToList();

                var responseData = new MegaData<BaseHealthConditionResponseDTO>
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
                        keyWord = getAllVetRequest?.KeyWord,
                        status = getAllVetRequest?.Status,
                    },
                    PageData = _mapper.Map<List<BaseHealthConditionResponseDTO>>(pagedHealthConditions)
                };

                if (!pagedHealthConditions.Any())
                {
                    return new DynamicResponse<BaseHealthConditionResponseDTO>
                    {
                        Code = 200,
                        Success = false,
                        Message = "Không tìm thấy điều kiện sức khỏe nào.",
                        Data = null
                    };
                }

                return new DynamicResponse<BaseHealthConditionResponseDTO>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy tất cả điều kiện sức khỏe thành công.",
                    Data = responseData
                };
            }
            catch (Exception ex)
            {
                // Log the exception (implement logging as needed)
                return new DynamicResponse<BaseHealthConditionResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã xảy ra lỗi khi lấy tất cả điều kiện sức khỏe.",
                    Data = null
                };
            }
        }
    }
}
