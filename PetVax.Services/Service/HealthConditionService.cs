using AutoMapper;
using Microsoft.AspNetCore.Http;
using PetVax.BusinessObjects.DTO.AppointmentDetailDTO;
using PetVax.BusinessObjects.DTO.HealthConditionDTO;
using PetVax.BusinessObjects.DTO.VetDTO;
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
    public class HealthConditionService : IHealthConditionService
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

        public async Task<BaseResponse<HealthConditionResponse>> CreateHealthConditionAsync(CreateHealthConditionDTO healthConditionDto, CancellationToken cancellationToken)
        {
            try
            {
                var healthIssues = new List<string>();

                // Validate temperture (°C)
                if (!string.IsNullOrEmpty(healthConditionDto.Temperature) &&
                    decimal.TryParse(healthConditionDto.Temperature.Replace("°C", "").Trim(), out decimal temperatureC))
                {
                    if (temperatureC < 37.5m || temperatureC > 39.2m)
                        healthIssues.Add($"Nhiệt độ bất thường: {temperatureC} °C");
                }

                // Validate heart rate (bpm)
                if (!string.IsNullOrEmpty(healthConditionDto.HeartRate) &&
                    int.TryParse(healthConditionDto.HeartRate.Trim(), out int heartRate))
                {
                    if (heartRate < 60 || heartRate > 140)
                        healthIssues.Add($"Nhịp tim bất thường: {heartRate} bpm");
                }

                // Validate Breathing rate (breaths/min)
                if (!string.IsNullOrEmpty(healthConditionDto.BreathingRate) &&
                    int.TryParse(healthConditionDto.BreathingRate.Trim(), out int breathingRate))
                {
                    if (breathingRate < 10 || breathingRate > 30)
                        healthIssues.Add($"Nhịp thở bất thường: {breathingRate} lần/phút");
                }

                // Validate weight (kg)
                if (!string.IsNullOrEmpty(healthConditionDto.Weight) &&
                    !decimal.TryParse(healthConditionDto.Weight.Trim(), out _))
                {
                    healthIssues.Add("Cân nặng không hợp lệ.");
                }


                if (healthIssues.Any())
                {
                    healthConditionDto.Conclusion = $"❌ Không đạt: {string.Join("; ", healthIssues)}";
                    healthConditionDto.Status = "FAIL";
                }
                else
                {
                    healthConditionDto.Conclusion = "✅ Đạt: Tình trạng sức khỏe trong ngưỡng bình thường.";
                    healthConditionDto.Status = "PASS";
                }

                // Mapping & lưu DB
                var healthCondition = _mapper.Map<HealthCondition>(healthConditionDto);
                healthCondition.CreatedAt = DateTime.UtcNow;
                healthCondition.CreatedBy = _httpContextAccessor.HttpContext?.User?.Identity?.Name;

                var createdHealthCondition = await _healthConditionRepository.AddHealthConditionAsync(healthCondition, cancellationToken);
                var responseData = _mapper.Map<HealthConditionResponse>(createdHealthCondition);

                return new BaseResponse<HealthConditionResponse>
                {
                    Code = 201,
                    Success = true,
                    Message = "✅ Khám sức khỏe hoàn tất.",
                    Data = responseData
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<HealthConditionResponse>
                {
                    Code = 500,
                    Success = false,
                    Message = "❌ Đã xảy ra lỗi khi tạo điều kiện sức khỏe.",
                    Data = null
                };
            }
        }

    }
}
