using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using PediVax.BusinessObjects.DBContext;
using PetVax.BusinessObjects.DTO.AppointmentDetailDTO;
using PetVax.BusinessObjects.DTO.CertificateForPet;
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
        private readonly IPetRepository _petRepository;
        private readonly PetVaxContext _context;

        public HealthConditionService(
            IHealthConditionRepository healthConditionRepository,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            IPetRepository petRepository,
            PetVaxContext context)
        {
            _healthConditionRepository = healthConditionRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
            _petRepository = petRepository;
            _context = context;
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
                string Conclusion = string.Empty;
                string Status = string.Empty;
                if (healthConditionDto.PetId == null)
                {
                    return new BaseResponse<HealthConditionResponse>
                    {
                        Code = 400,
                        Success = false,
                        Message = "PetId là bắt buộc.",
                        Data = null
                    };
                }

                var pet = await _petRepository.GetPetByIdAsync(healthConditionDto.PetId.Value, cancellationToken);
                if (pet == null)
                {
                    return new BaseResponse<HealthConditionResponse>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Không tìm thấy thú cưng.",
                        Data = null
                    };
                }

                var species = pet.Species?.ToLower(); // "dog" hoặc "cat"
                if (species != "dog" && species != "cat")
                {
                    return new BaseResponse<HealthConditionResponse>
                    {
                        Code = 400,
                        Success = false,
                        Message = "Loài thú cưng không được hỗ trợ. Chỉ hỗ trợ 'cat' và 'dog'.",
                        Data = null
                    };
                }

                var healthIssues = new List<string>();

                // Validate dog
                if (species == "dog")
                {
                    if (!string.IsNullOrEmpty(healthConditionDto.Temperature) &&
                        decimal.TryParse(healthConditionDto.Temperature.Replace("°C", "").Trim(), out decimal tempC))
                    {
                        if (tempC < 37.5m || tempC > 39.2m)
                            healthIssues.Add($"Nhiệt độ bất thường: {tempC} °C");
                    }

                    if (!string.IsNullOrEmpty(healthConditionDto.HeartRate) &&
                        int.TryParse(healthConditionDto.HeartRate.Trim(), out int heartRate))
                    {
                        if (heartRate < 60 || heartRate > 140)
                            healthIssues.Add($"Nhịp tim bất thường: {heartRate} bpm");
                    }

                    if (!string.IsNullOrEmpty(healthConditionDto.BreathingRate) &&
                        int.TryParse(healthConditionDto.BreathingRate.Trim(), out int breathingRate))
                    {
                        if (breathingRate < 10 || breathingRate > 30)
                            healthIssues.Add($"Nhịp thở bất thường: {breathingRate} lần/phút");
                    }
                }

                //  Validate cat
                else if (species == "cat")
                {
                    if (!string.IsNullOrEmpty(healthConditionDto.Temperature) &&
                        decimal.TryParse(healthConditionDto.Temperature.Replace("°C", "").Trim(), out decimal tempC))
                    {
                        if (tempC < 38.0m || tempC > 39.5m)
                            healthIssues.Add($"Nhiệt độ bất thường: {tempC} °C");
                    }

                    if (!string.IsNullOrEmpty(healthConditionDto.HeartRate) &&
                        int.TryParse(healthConditionDto.HeartRate.Trim(), out int heartRate))
                    {
                        if (heartRate < 140 || heartRate > 220)
                            healthIssues.Add($"Nhịp tim bất thường: {heartRate} bpm");
                    }

                    if (!string.IsNullOrEmpty(healthConditionDto.BreathingRate) &&
                        int.TryParse(healthConditionDto.BreathingRate.Trim(), out int breathingRate))
                    {
                        if (breathingRate < 20 || breathingRate > 30)
                            healthIssues.Add($"Nhịp thở bất thường: {breathingRate} lần/phút");
                    }
                }

                // check weight
                if (!string.IsNullOrEmpty(healthConditionDto.Weight) &&
                    !decimal.TryParse(healthConditionDto.Weight.Trim(), out _))
                {
                    healthIssues.Add("Cân nặng không hợp lệ.");
                }

                // 
                if (healthIssues.Any())
                {
                    Conclusion = $"❌ Không đạt: {string.Join("; ", healthIssues)}";
                    Status = "FAIL";
                }
                else
                {
                    Conclusion = "Đạt: Tình trạng sức khỏe trong ngưỡng bình thường.";
                    Status = "PASS";
                }

                // save to database
                var random = new Random();
                var healthCondition = _mapper.Map<HealthCondition>(healthConditionDto);
                healthCondition.ConditionCode = $"HC" + random.Next(0, 1000000).ToString("D6");

                healthCondition.Price = 0;
                healthCondition.CreatedAt = DateTime.UtcNow;
                healthCondition.CreatedBy = _httpContextAccessor.HttpContext?.User?.Identity?.Name;

                var created = await _healthConditionRepository.AddHealthConditionAsync(healthCondition, cancellationToken);
                if (created != null)
                {

                }
                var response = _mapper.Map<HealthConditionResponse>(created);

                return new BaseResponse<HealthConditionResponse>
                {
                    Code = 201,
                    Success = true,
                    Message = "Khám sức khỏe hoàn tất.",
                    Data = response
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<HealthConditionResponse>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã xảy ra lỗi khi tạo điều kiện sức khỏe.",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<HealthConditionResponse>> UpdateHealthConditionAsync(int healthConditionId, UpdateHealthCondition healthConditionDto, CancellationToken cancellationToken)
        {
            try
            {
                string Conclusion = string.Empty;
                string Status = string.Empty;
                var existingHealthCondition = await _healthConditionRepository.GetHealthConditionByIdAsync(healthConditionId, cancellationToken);
                if (existingHealthCondition == null)
                {
                    return new BaseResponse<HealthConditionResponse>
                    {
                        Code = 200,
                        Success = false,
                        Message = "Không tìm thấy điều kiện sức khỏe.",
                        Data = null
                    };
                }

                if (healthConditionDto.PetId == null)
                {
                    return new BaseResponse<HealthConditionResponse>
                    {
                        Code = 400,
                        Success = false,
                        Message = "PetId là bắt buộc.",
                        Data = null
                    };
                }

                var pet = await _petRepository.GetPetByIdAsync(healthConditionDto.PetId.Value, cancellationToken);
                if (pet == null)
                {
                    return new BaseResponse<HealthConditionResponse>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Không tìm thấy thú cưng.",
                        Data = null
                    };
                }

                var species = pet.Species?.ToLower(); // "dog" hoặc "cat"
                if (species != "dog" && species != "cat")
                {
                    return new BaseResponse<HealthConditionResponse>
                    {
                        Code = 400,
                        Success = false,
                        Message = "Loài thú cưng không được hỗ trợ. Chỉ hỗ trợ 'cat' và 'dog'.",
                        Data = null
                    };
                }

                var healthIssues = new List<string>();

                // Validate dog
                if (species == "dog")
                {
                    if (!string.IsNullOrEmpty(healthConditionDto.Temperature) &&
                        decimal.TryParse(healthConditionDto.Temperature.Replace("°C", "").Trim(), out decimal tempC))
                    {
                        if (tempC < 37.5m || tempC > 39.2m)
                            healthIssues.Add($"Nhiệt độ bất thường: {tempC} °C");
                    }

                    if (!string.IsNullOrEmpty(healthConditionDto.HeartRate) &&
                        int.TryParse(healthConditionDto.HeartRate.Trim(), out int heartRate))
                    {
                        if (heartRate < 60 || heartRate > 140)
                            healthIssues.Add($"Nhịp tim bất thường: {heartRate} bpm");
                    }

                    if (!string.IsNullOrEmpty(healthConditionDto.BreathingRate) &&
                        int.TryParse(healthConditionDto.BreathingRate.Trim(), out int breathingRate))
                    {
                        if (breathingRate < 10 || breathingRate > 30)
                            healthIssues.Add($"Nhịp thở bất thường: {breathingRate} lần/phút");
                    }
                }

                //  Validate cat
                else if (species == "cat")
                {
                    if (!string.IsNullOrEmpty(healthConditionDto.Temperature) &&
                        decimal.TryParse(healthConditionDto.Temperature.Replace("°C", "").Trim(), out decimal tempC))
                    {
                        if (tempC < 38.0m || tempC > 39.5m)
                            healthIssues.Add($"Nhiệt độ bất thường: {tempC} °C");
                    }

                    if (!string.IsNullOrEmpty(healthConditionDto.HeartRate) &&
                        int.TryParse(healthConditionDto.HeartRate.Trim(), out int heartRate))
                    {
                        if (heartRate < 140 || heartRate > 220)
                            healthIssues.Add($"Nhịp tim bất thường: {heartRate} bpm");
                    }

                    if (!string.IsNullOrEmpty(healthConditionDto.BreathingRate) &&
                        int.TryParse(healthConditionDto.BreathingRate.Trim(), out int breathingRate))
                    {
                        if (breathingRate < 20 || breathingRate > 30)
                            healthIssues.Add($"Nhịp thở bất thường: {breathingRate} lần/phút");
                    }
                }

                // check weight
                if (!string.IsNullOrEmpty(healthConditionDto.Weight) &&
                    !decimal.TryParse(healthConditionDto.Weight.Trim(), out _))
                {
                    healthIssues.Add("Cân nặng không hợp lệ.");
                }

                // 
                if (healthIssues.Any())
                {
                    Conclusion = $"❌ Không đạt: {string.Join("; ", healthIssues)}";
                    Status = "FAIL";
                }
                else
                {
                    Conclusion = "Đạt: Tình trạng sức khỏe trong ngưỡng bình thường.";
                    Status = "PASS";
                }

                // save to database
                var healthCondition = _mapper.Map<HealthCondition>(healthConditionDto);
                healthCondition.ModifiedAt = DateTime.UtcNow;
                healthCondition.ModifiedBy = _httpContextAccessor.HttpContext?.User?.Identity?.Name;

                var created = await _healthConditionRepository.AddHealthConditionAsync(healthCondition, cancellationToken);
                if (created != null)
                {

                }
                var response = _mapper.Map<HealthConditionResponse>(created);

                return new BaseResponse<HealthConditionResponse>
                {
                    Code = 201,
                    Success = true,
                    Message = "Khám sức khỏe hoàn tất.",
                    Data = response
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<HealthConditionResponse>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã xảy ra lỗi khi tạo điều kiện sức khỏe.",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<HealthConditionResponse>> GetHealthConditionByIdAsync(int healthConditionId, CancellationToken cancellationToken)
        {
            try
            {
                var healthCondition = await _healthConditionRepository.GetHealthConditionByIdAsync(healthConditionId, cancellationToken);
                if (healthCondition == null)
                {
                    return new BaseResponse<HealthConditionResponse>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Không tìm thấy điều kiện sức khỏe.",
                        Data = null
                    };
                }
                var response = _mapper.Map<HealthConditionResponse>(healthCondition);
                return new BaseResponse<HealthConditionResponse>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy điều kiện sức khỏe thành công.",
                    Data = response
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<HealthConditionResponse>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã xảy ra lỗi khi lấy điều kiện sức khỏe.",
                    Data = null
                };
            }
        }

        public async Task<PetVaccinationRecordDTO?> GetPetVaccinationRecordAsync(int petId)
        {
            var pet = await _petRepository.GetPetWithHealthDataAsync(petId);
            if (pet == null)
                return null;

            var vaccineProfiles = await _petRepository.GetVaccineProfilesByPetIdAsync(petId);

            var result = new PetVaccinationRecordDTO
            {
                PetId = pet.PetId,
                PetName = pet.Name,
                Species = pet.Species,
                Breed = pet.Breed,
                Gender = pet.Gender,
                DateOfBirth = pet.DateOfBirth,
                HealthConditions = pet.HealthConditions?
                     .Select(h => _mapper.Map<HealthConditionResponse>(h))
                        .ToList() ?? new(),
                Certificates = new List<VaccinationCertificateWithHealthResponseDTO>()
            };

            foreach (var vp in vaccineProfiles)
            {
                var batch = vp.AppointmentDetail?.VaccineBatch;
                var vaccine = batch?.Vaccine;
                var vet = vp.AppointmentDetail?.Vet;

                result.Certificates.Add(new VaccinationCertificateWithHealthResponseDTO
                {
                    DiseaseId = vp.DiseaseId,
                    DiseaseName = vp.Disease?.Name ?? "Không rõ",
                    Dose = vp.Dose,

                    VaccineId = vaccine?.VaccineId,
                    VaccineName = vaccine?.Name,
                    VaccineCode = vaccine?.VaccineCode ?? "",
                    VaccineImage = vaccine?.Image ?? "",
                    VaccineDescription = vaccine?.Description ?? "",
            


                    BatchId = batch?.VaccineBatchId,
                    BatchNumber = batch?.BatchNumber ?? "",
                    ManufactureDate = batch?.ManufactureDate ?? DateTime.MinValue,
                    ExpiryDate = batch?.ExpiryDate ?? DateTime.MinValue,

                    DoseNumber = vp.Dose ?? 0,
                    VaccinationDate = vp.VaccinationDate ?? DateTime.MinValue,
                    ExpirationDate = null,

                    VetName = vet?.Name ?? "Không rõ",
                    ClinicName = "PetVax Clinic",
                    ClinicAddress = "123 Pet Street",

                    Purpose = vp.NextVaccinationInfo ?? "",
                    Price = vaccine?.Price ?? 0,
                    Status = vp.IsCompleted == true ? "Hoàn tất" : "Chưa hoàn tất",

                    IssueDate = vp.CreatedAt,
                    ValidUntil = null,

                });
            }

            return result;
        }

        public async Task<BaseResponse<List<HealthConditionResponse>>> GetHealthConditionByPetIdAndStatus(int petId, string status, CancellationToken cancellationToken)
        {
            try
            {
                var healthConditions = await _healthConditionRepository.GetHealthConditionsByPetIdAndStatusAsync(petId, status, cancellationToken);
                if (healthConditions == null)
                {
                    return new BaseResponse<List<HealthConditionResponse>>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Không tìm thấy điều kiện sức khỏe cho thú cưng này.",
                        Data = null
                    };
                }
                var response = _mapper.Map<List<HealthConditionResponse>>(healthConditions);
                return new BaseResponse<List<HealthConditionResponse>>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy điều kiện sức khỏe thành công.",
                    Data = response
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<List<HealthConditionResponse>>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã xảy ra lỗi khi lấy điều kiện sức khỏe.",
                    Data = null
                };
            }
        }
    }
}
