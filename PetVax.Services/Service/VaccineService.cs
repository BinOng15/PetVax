using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PetVax.BusinessObjects.DTO;
using PetVax.BusinessObjects.DTO.CustomerDTO;
using PetVax.BusinessObjects.DTO.VaccineDTO;
using PetVax.BusinessObjects.Helpers;
using PetVax.BusinessObjects.Models;
using PetVax.Repositories.IRepository;
using PetVax.Services.ExternalService;
using PetVax.Services.IService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static PetVax.BusinessObjects.DTO.ResponseModel;

namespace PetVax.Services.Service
{
    public class VaccineService : IVaccineService
    {
        private readonly IVaccineRepository _vaccineRepository;
        private readonly IVaccineDiseaseRepository _vaccineDiseaseRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<VaccineService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ICloudinariService _cloudinariService;

        public VaccineService(IVaccineRepository vaccineRepository, IVaccineDiseaseRepository vaccineDiseaseRepository, IMapper mapper, ILogger<VaccineService> logger, IHttpContextAccessor httpContextAccessor, ICloudinariService cloudinariService)
        {
            _vaccineRepository = vaccineRepository;
            _vaccineDiseaseRepository = vaccineDiseaseRepository;
            _mapper = mapper;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _cloudinariService = cloudinariService;
        }

        public async Task<BaseResponse<VaccineResponseDTO>> CreateVaccineAsync(CreateVaccineDTO createVaccineDTO, CancellationToken cancellationToken)
        {
            if (createVaccineDTO == null)
            {
                return new BaseResponse<VaccineResponseDTO>
                {
                    Code = 400,
                    Success = false,
                    Message = "Không có dữ liệu vắc xin được cung cấp."
                };
            }
            try
            {
                var existingVaccine = await _vaccineRepository.GetVaccineByName(createVaccineDTO.Name, cancellationToken);
                if (existingVaccine != null)
                {
                    return new BaseResponse<VaccineResponseDTO>
                    {
                        Code = 409,
                        Success = false,
                        Message = $"Vắc xin với tên '{createVaccineDTO.Name}' đã tồn tại trong hệ thống",
                    };
                }

                var vaccine = _mapper.Map<Vaccine>(createVaccineDTO);
                if (createVaccineDTO.Image != null)
                {
                    vaccine.Image = await _cloudinariService.UploadImage(createVaccineDTO.Image);
                }

                vaccine.Name = createVaccineDTO.Name;
                // Generate VaccineCode: "VX" + 6 random digits
                var random = new Random();
                vaccine.VaccineCode = "VX" + random.Next(0, 1000000).ToString("D6");
                vaccine.Image = await _cloudinariService.UploadImage(createVaccineDTO.Image);
                vaccine.Status = "Active";
                vaccine.CreatedAt = DateTimeHelper.Now();
                vaccine.CreatedBy = GetCurrentUserName(); // Or get from context if available

                var createdVaccineId = await _vaccineRepository.CreateVaccineAsync(vaccine, cancellationToken);
                if (createdVaccineId <= 0)
                {
                    return new BaseResponse<VaccineResponseDTO>
                    {
                        Code = 500,
                        Success = false,
                        Message = "Lỗi khi tạo vắc xin mới. Vui lòng thử lại sau."
                    };
                }

                // Get the created vaccine from DB to ensure all fields (like VaccineId) are set
                var createdVaccine = await _vaccineRepository.GetVaccineByIdAsync(vaccine.VaccineId != 0 ? vaccine.VaccineId : createdVaccineId, cancellationToken);
                var responseDTO = _mapper.Map<VaccineResponseDTO>(createdVaccine ?? vaccine);

                return new BaseResponse<VaccineResponseDTO>
                {
                    Code = 201,
                    Success = true,
                    Message = "Tao vắc xin thành công.",
                    Data = responseDTO
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating vaccine");
                return new BaseResponse<VaccineResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Lỗi khi tạo vắc xin mới. Vui lòng thử lại sau."
                };
            }
        }

        public async Task<BaseResponse<bool>> DeleteVaccineAsync(int vaccineId, CancellationToken cancellationToken)
        {
            if (vaccineId <= 0)
            {
                return new BaseResponse<bool>
                {
                    Code = 400,
                    Success = false,
                    Message = "Không có ID vắc xin hợp lệ được cung cấp."
                };
            }
            try
            {
                var isDeleted = await _vaccineRepository.DeleteVaccineAsync(vaccineId, cancellationToken);
                if (!isDeleted)
                {
                    return new BaseResponse<bool>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Không tìm thấy vắc xin với ID đã cung cấp hoặc vắc xin không thể bị xóa.",
                    };
                }
                return new BaseResponse<bool>
                {
                    Code = 200,
                    Success = true,
                    Message = "Xóa vắc xin thành công.",
                    Data = true
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting vaccine");
                return new BaseResponse<bool>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã xảy ra lỗi khi xóa vắc xin. Vui lòng thử lại sau."
                };
            }
        }

        public async Task<DynamicResponse<VaccineResponseDTO>> GetAllVaccineAsync(GetAllItemsDTO getAllItemsDTO, CancellationToken cancellationToken)
        {
            try
            {
                var vaccines = await _vaccineRepository.GetAllVaccineAsync(cancellationToken);
                if (!string.IsNullOrWhiteSpace(getAllItemsDTO?.KeyWord))
                {
                    var keyword = getAllItemsDTO.KeyWord.Trim().ToLower();
                    vaccines = vaccines.Where(v => v.Name.ToLower().Contains(keyword) || v.VaccineCode.ToLower().Contains(keyword)).ToList();
                }

                int pageNumber = getAllItemsDTO?.PageNumber > 0 ? getAllItemsDTO.PageNumber : 1;
                int pageSize = getAllItemsDTO?.PageSize > 0 ? getAllItemsDTO.PageSize : 10;
                int skip = (pageNumber - 1) * pageSize;
                int totalItem = vaccines.Count;
                int totalPage = (int)Math.Ceiling((double)totalItem / pageSize);

                var pagedVaccines = vaccines
                    .Skip(skip)
                    .Take(pageSize)
                    .ToList();

                var responseData = new MegaData<VaccineResponseDTO>
                {
                    PageInfo = new PagingMetaData
                    {
                        Page = pageNumber,
                        Size = pageSize,
                        TotalItem = totalItem,
                        TotalPage = totalPage,
                    },
                    SearchInfo = new SearchCondition
                    {
                        keyWord = getAllItemsDTO?.KeyWord,
                        status = getAllItemsDTO?.Status
                    },
                    PageData = _mapper.Map<List<VaccineResponseDTO>>(pagedVaccines)
                };
                if (!pagedVaccines.Any())
                {
                    return new DynamicResponse<VaccineResponseDTO>
                    {
                        Code = 200,
                        Success = false,
                        Message = "Không tìm thấy vắc xin nào phù hợp với tiêu chí tìm kiếm.",
                        Data = null
                    };
                }
                return new DynamicResponse<VaccineResponseDTO>
                {
                    Code = 200,
                    Success = true,
                    Message = "Vắc xin được lấy thành công.",
                    Data = responseData
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving vaccines");
                return new DynamicResponse<VaccineResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã xảy ra lỗi khi lấy danh sách vắc xin. Vui lòng thử lại sau.",
                };
            }
        }

        public async Task<BaseResponse<List<VaccineResponseDTO>>> GetVaccineByDiseaseIdAsync(int diseaseId, CancellationToken cancellationToken)
        {
            if (diseaseId <= 0)
            {
                return new BaseResponse<List<VaccineResponseDTO>>
                {
                    Code = 400,
                    Success = false,
                    Message = "Không có ID bệnh hợp lệ được cung cấp."
                };
            }
            try
            {
                var vaccine = await _vaccineRepository.GetVaccineByDiseaseId(diseaseId, cancellationToken);
                if (vaccine == null)
                {
                    return new BaseResponse<List<VaccineResponseDTO>>
                    {
                        Code = 200,
                        Success = true,
                        Message = "Vaccine không tồn tại với diseaseId này.",
                        Data = null
                    };
                }
                var response = _mapper.Map<List<VaccineResponseDTO>>(vaccine);
                return new BaseResponse<List<VaccineResponseDTO>>
                {
                    Code = 200,
                    Success = true,
                    Message = "Vắc xin được lấy thành công.",
                    Data = response
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving vaccine by disease ID");
                return new BaseResponse<List<VaccineResponseDTO>>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã xảy ra lỗi khi lấy vắc xin theo ID bệnh. Vui lòng thử lại sau."
                };
            }
        }
        public async Task<BaseResponse<VaccineResponseDTO>> GetVaccineByIdAsync(int vaccineId, CancellationToken cancellationToken)
        {
            if (vaccineId <= 0)
            {
                return new BaseResponse<VaccineResponseDTO>
                {
                    Code = 400,
                    Success = false,
                    Message = "Không có ID vắc xin hợp lệ được cung cấp."
                };
            }
            try
            {
                var vaccine = await _vaccineRepository.GetVaccineByIdAsync(vaccineId, cancellationToken);
                if (vaccine == null)
                {
                    return new BaseResponse<VaccineResponseDTO>
                    {
                        Code = 200,
                        Success = false,
                        Message = "Không tìm thấy vắc xin với ID đã cung cấp.",
                    };
                }
                var responseDTO = _mapper.Map<VaccineResponseDTO>(vaccine);
                return new BaseResponse<VaccineResponseDTO>
                {
                    Code = 200,
                    Success = true,
                    Message = "Vắc xin được lấy thành công.",
                    Data = responseDTO
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving vaccine by ID");
                return new BaseResponse<VaccineResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã xảy ra lỗi khi lấy vắc xin theo ID. Vui lòng thử lại sau."
                };
            }
        }

        public async Task<BaseResponse<VaccineResponseDTO>> GetVaccineByNameAsync(string Name, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                return new BaseResponse<VaccineResponseDTO>
                {
                    Code = 400,
                    Success = false,
                    Message = "Tên vắc xin không được để trống."
                };
            }
            try
            {
                var vaccine = await _vaccineRepository.GetVaccineByName(Name, cancellationToken);
                if (vaccine == null)
                {
                    return new BaseResponse<VaccineResponseDTO>
                    {
                        Code = 200,
                        Success = false,
                        Message = "Không tìm thấy vắc xin với tên đã cung cấp.",
                    };
                }
                var responseDTO = _mapper.Map<VaccineResponseDTO>(vaccine);
                return new BaseResponse<VaccineResponseDTO>
                {
                    Code = 200,
                    Success = true,
                    Message = "Vắc xin được lấy thành công.",
                    Data = responseDTO
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving vaccine by name");
                return new BaseResponse<VaccineResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã xảy ra lỗi khi lấy vắc xin theo tên. Vui lòng thử lại sau."
                };
            }
        }

        public async Task<BaseResponse<VaccineResponseDTO>> GetVaccineByVaccineCodeAsync(string vaccineCode, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(vaccineCode))
            {
                return new BaseResponse<VaccineResponseDTO>
                {
                    Code = 400,
                    Success = false,
                    Message = "Mã vắc xin không được để trống."
                };
            }
            try
            {
                var vaccine = await _vaccineRepository.GetVaccineByVaccineCodeAsync(vaccineCode, cancellationToken);
                if (vaccine == null)
                {
                    return new BaseResponse<VaccineResponseDTO>
                    {
                        Code = 200,
                        Success = false,
                        Message = "Không tìm thấy vắc xin với mã đã cung cấp.",
                    };
                }
                var responseDTO = _mapper.Map<VaccineResponseDTO>(vaccine);
                return new BaseResponse<VaccineResponseDTO>
                {
                    Code = 200,
                    Success = true,
                    Message = "Vắc xin được lấy thành công.",
                    Data = responseDTO
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving vaccine by code");
                return new BaseResponse<VaccineResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã xảy ra lỗi khi lấy vắc xin theo mã. Vui lòng thử lại sau."
                };
            }
        }

        public async Task<BaseResponse<VaccineResponseDTO>> UpdateVaccineAsync(int vaccineId, UpdateVaccineDTO updateVaccineDTO, CancellationToken cancellationToken)
        {
            if (updateVaccineDTO == null)
            {
                return new BaseResponse<VaccineResponseDTO>
                {
                    Code = 400,
                    Success = false,
                    Message = "Không có dữ liệu vắc xin được cung cấp để cập nhật."
                };
            }
            try
            {
                var existingVaccine = await _vaccineRepository.GetVaccineByIdAsync(vaccineId, cancellationToken);
                if (existingVaccine == null)
                {
                    return new BaseResponse<VaccineResponseDTO>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Không tìm thấy vắc xin với ID đã cung cấp.",
                    };
                }
                if (!string.IsNullOrWhiteSpace(updateVaccineDTO.Name))
                    existingVaccine.Name = updateVaccineDTO.Name;
                if (!string.IsNullOrWhiteSpace(updateVaccineDTO.Description))
                    existingVaccine.Description = updateVaccineDTO.Description;
                if (updateVaccineDTO.Price.HasValue)
                    existingVaccine.Price = updateVaccineDTO.Price.Value;
                if (!string.IsNullOrWhiteSpace(updateVaccineDTO.Notes))
                    existingVaccine.Notes = updateVaccineDTO.Notes;
                if (updateVaccineDTO.Image != null)
                    existingVaccine.Image = await _cloudinariService.UploadImage(updateVaccineDTO.Image);

                existingVaccine.ModifiedAt = DateTimeHelper.Now();
                existingVaccine.ModifiedBy = GetCurrentUserName();

                var updatedVaccine = await _vaccineRepository.UpdateVaccineAsync(existingVaccine, cancellationToken);
                if (updatedVaccine <= 0)
                {
                    return new BaseResponse<VaccineResponseDTO>
                    {
                        Code = 500,
                        Success = false,
                        Message = "Lỗi khi cập nhật vắc xin. Vui lòng thử lại sau."
                    };
                }
                // Get the updated vaccine from DB to ensure all fields (like VaccineId) are set
                var vaccine = await _vaccineRepository.GetVaccineByIdAsync(vaccineId, cancellationToken);
                var responseDTO = _mapper.Map<VaccineResponseDTO>(vaccine ?? existingVaccine);
                return new BaseResponse<VaccineResponseDTO>
                {
                    Code = 200,
                    Success = true,
                    Message = "Cập nhật vắc xin thành công.",
                    Data = responseDTO
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating vaccine");
                return new BaseResponse<VaccineResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Đã xảy ra lỗi khi cập nhật vắc xin. Vui lòng thử lại sau."
                };
            }
        }
        private string GetCurrentUserName()
        {
            return _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value ?? "System";
        }
    }
}
