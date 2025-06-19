using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using PetVax.BusinessObjects.DTO.PetDTO;
using PetVax.BusinessObjects.DTO.VetDTO;
using PetVax.BusinessObjects.DTO.VetScheduleDTO;
using PetVax.BusinessObjects.Models;
using PetVax.Repositories.IRepository;
using PetVax.Repositories.Repository;
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
    public class PetService : IPetService
    {
        private readonly IPetRepository _petRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IVaccineProfileRepository _vaccineProfileRepository;
        private readonly ILogger<PetService> _logger;
        private readonly IMapper _mapper;
        private readonly ICloudinariService _cloudinariService;
        private readonly IHttpContextAccessor _httpContextAccessor;
            
        public PetService(IPetRepository petRepository, ICustomerRepository customerRepository, ILogger<PetService> logger, IMapper mapper, ICloudinariService cloudinariService, IHttpContextAccessor httpContextAccessor, IVaccineProfileRepository vaccineProfileRepository)
        {
            _petRepository = petRepository;
            _customerRepository = customerRepository;
            _logger = logger;
            _mapper = mapper;
            _cloudinariService = cloudinariService;
            _httpContextAccessor = httpContextAccessor;
            _vaccineProfileRepository = vaccineProfileRepository;
        }

        public async Task<DynamicResponse<PetResponseDTO>> GetAllPetsAsync(GetAllPetsRequestDTO getAllPetsRequest, CancellationToken cancellationToken)
        {
            try
            {
                var pets = await _petRepository.GetAllPetsAsync(cancellationToken);

                // Filter by keyword if provided
                if (!string.IsNullOrWhiteSpace(getAllPetsRequest?.KeyWord))
                {
                    var keyword = getAllPetsRequest.KeyWord.Trim().ToLower();
                    pets = pets
                        .Where(a => a.Name.ToLower().Contains(keyword))
                        .ToList();
                }

                // Pagination
                int pageNumber = getAllPetsRequest?.PageNumber > 0 ? getAllPetsRequest.PageNumber : 1;
                int pageSize = getAllPetsRequest?.PageSize > 0 ? getAllPetsRequest.PageSize : 10;
                int skip = (pageNumber - 1) * pageSize;
                int totalItem = pets.Count;
                int totalPage = (int)Math.Ceiling((double)totalItem / pageSize);

                var pateVets = pets
                    .Skip(skip)
                    .Take(pageSize)
                    .ToList();

                var responseData = new MegaData<PetResponseDTO>
                {
                    PageInfo = new PagingMetaData
                    {
                        Page = pageNumber,
                        Size = pageSize,
                        TotalItem = totalItem,
                        TotalPage = totalPage,
                        Sort = null,
                        Order = null
                    },
                    SearchInfo = new SearchCondition
                    {
                        keyWord = getAllPetsRequest?.KeyWord,
                    },
                    PageData = pateVets.Select(p => new PetResponseDTO
                    {
                        PetId = p.PetId,
                        PetCode = p.PetCode,
                        CustomerId = p.CustomerId,
                        Name = p.Name,
                        Species = p.Species,
                        Breed = p.Breed,
                        Gender = p.Gender,
                        DateOfBirth = p.DateOfBirth,
                        PlaceToLive = p.PlaceToLive,
                        PlaceOfBirth = p.PlaceOfBirth,
                        Image = p.Image,
                        Weight = p.Weight,
                        Color = p.Color,
                        Nationality = p.Nationality,
                        isSterilized = p.isSterilized
                    }).ToList()
                };

                if (!pateVets.Any())
                {
                    _logger.LogInformation("No pets found for the given criteria");
                    return new DynamicResponse<PetResponseDTO>
                    {
                        Code = 200,
                        Success = false,
                        Message = "No vets found",
                        Data = responseData
                    };
                }

                _logger.LogInformation("Retrieved {Count} pets successfully (Page {PageNumber}, PageSize {PageSize})", pateVets.Count, pageNumber, pageSize);
                return new DynamicResponse<PetResponseDTO>
                {
                    Code = 200,
                    Success = true,
                    Message = "Pets retrieved successfully",
                    Data = responseData
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all vets with pagination");
                return new DynamicResponse<PetResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Error while retrieving vets: " + (ex.InnerException?.Message ?? ex.Message),
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<PetResponseDTO>> UpdatePetAsync(UpdatePetRequestDTO updatePetRequest, CancellationToken cancellationToken)
        {
            try
            {
                var pet = await _petRepository.GetPetByIdAsync(updatePetRequest.PetId, cancellationToken);
                if (pet == null)
                {
                    _logger.LogWarning("Pet with ID {PetId} not found", updatePetRequest.PetId);
                    return new BaseResponse<PetResponseDTO>
                    {
                        Code = 200,
                        Success = false,
                        Message = "Pet not found",
                        Data = null
                    };
                }

                // Update pet properties
                pet.Name = updatePetRequest.Name ?? pet.Name;
                pet.Species = updatePetRequest.Species ?? pet.Species;
                pet.Breed = updatePetRequest.Breed ?? pet.Breed;
                pet.Gender = updatePetRequest?.Gender ?? pet.Gender;
                pet.DateOfBirth = updatePetRequest?.DateOfBirth ?? pet.DateOfBirth;
                pet.PlaceToLive = updatePetRequest?.PlaceToLive ?? pet.PlaceToLive;
                pet.PlaceOfBirth = updatePetRequest?.PlaceOfBirth ?? pet.PlaceOfBirth;

                // Handle image update (support IFormFile like CreatePetAsync)
                if (updatePetRequest.Image != null)
                {
                    pet.Image = await _cloudinariService.UploadImage(updatePetRequest.Image);
                }
                else
                {
                    pet.Image = null;
                }
                // else keep existing image

                pet.Weight = updatePetRequest?.Weight ?? pet.Weight;
                pet.Color = updatePetRequest?.Color ?? pet.Color;
                pet.Nationality = updatePetRequest.Nationality ?? pet.Nationality;
                pet.isSterilized = updatePetRequest.isSterilized;
                pet.ModifiedAt = DateTime.UtcNow;
                pet.ModifiedBy = GetCurrentUserName();

                int update = await _petRepository.UpdatePetAsync(pet, cancellationToken);
                if (update <= 0)
                {
                    _logger.LogWarning("No changes made to pet with ID {PetId}", updatePetRequest.PetId);
                    return new BaseResponse<PetResponseDTO>
                    {
                        Code = 200,
                        Success = false,
                        Message = "No changes made to pet",
                        Data = null
                    };
                }

                _logger.LogInformation("Pet with ID {PetId} updated successfully", updatePetRequest.PetId);
                return new BaseResponse<PetResponseDTO>
                {
                    Code = 200,
                    Success = true,
                    Message = "Pet updated successfully",
                    Data = new PetResponseDTO
                    {
                        PetId = pet.PetId,
                        PetCode = pet.PetCode,
                        CustomerId = pet.CustomerId,
                        Name = pet.Name,
                        Species = pet.Species,
                        Breed = pet.Breed,
                        Gender = pet.Gender,
                        DateOfBirth = pet.DateOfBirth,
                        PlaceToLive = pet.PlaceToLive,
                        PlaceOfBirth = pet.PlaceOfBirth,
                        Image = pet.Image,
                        Weight = pet.Weight,
                        Color = pet.Color,
                        Nationality = pet.Nationality,
                        isSterilized = pet.isSterilized
                    }
                };

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating pet with ID {PetId}", updatePetRequest.PetId);
                return new BaseResponse<PetResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Error while updating pet: " + (ex.InnerException?.Message ?? ex.Message),
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<PetResponseDTO>> GetPetByIdAsync(int petId, CancellationToken cancellationToken)
        {
            try
            {
                var pet = await _petRepository.GetPetByIdAsync(petId, cancellationToken);
                if (pet == null)
                {
                    _logger.LogWarning("Pet with ID {PetId} not found", petId);
                    return new BaseResponse<PetResponseDTO>
                    {
                        Code = 200,
                        Success = false,
                        Message = "Pet not found",
                        Data = null
                    };
                }
                _logger.LogInformation("Retrieved pet with ID {PetId} successfully", petId);
                return new BaseResponse<PetResponseDTO>
                {
                    Code = 200,
                    Success = true,
                    Message = "Pet retrieved successfully",
                    Data = new PetResponseDTO
                    {
                        PetId = pet.PetId,
                        PetCode = pet.PetCode,
                        CustomerId = pet.CustomerId,
                        Name = pet.Name,
                        Species = pet.Species,
                        Breed = pet.Breed,
                        Gender = pet.Gender,
                        DateOfBirth = pet.DateOfBirth,
                        PlaceToLive = pet.PlaceToLive,
                        PlaceOfBirth = pet.PlaceOfBirth,
                        Image = pet.Image,
                        Weight = pet.Weight,
                        Color = pet.Color,
                        Nationality = pet.Nationality,
                        isSterilized = pet.isSterilized
                    }

                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving pet with ID {PetId}", petId);
                return new BaseResponse<PetResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Error while retrieving pet: " + (ex.InnerException?.Message ?? ex.Message),
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<PetResponseDTO>> CreatePetAsync(CreatePetRequestDTO createPetRequest, CancellationToken cancellationToken)
        {
        
            try
            {
                var pet = _mapper.Map<Pet>(createPetRequest);
                if (createPetRequest.Image != null)
                {
                    pet.Image = await _cloudinariService.UploadImage(createPetRequest.Image);
                }
                else
                {
                    pet.Image = null;
                }

                var dob = DateTime.Parse(createPetRequest.DateOfBirth); 
                var today = DateTime.Today;
                int age = today.Year - dob.Year;

                if (dob.Date > today.AddYears(-age))
                {
                    age--; 
                }
                var random = new Random();

                pet.CustomerId = createPetRequest.CustomerId;
                pet.PetCode = "PET" + random.Next(0, 1000000).ToString("D6");
                pet.Name = createPetRequest.Name;
                pet.Species = createPetRequest.Species;
                pet.Breed = createPetRequest.Breed;
                pet.Gender = createPetRequest.Gender;
                pet.DateOfBirth = createPetRequest.DateOfBirth;
                pet.PlaceToLive = createPetRequest.PlaceToLive;
                pet.PlaceOfBirth = createPetRequest.PlaceOfBirth;
                pet.Weight = createPetRequest.Weight;
                pet.Color = createPetRequest.Color;
                pet.Nationality = createPetRequest.Nationality;
                pet.isSterilized = createPetRequest.isSterilized;
                pet.CreatedAt = DateTime.UtcNow;
                pet.CreatedBy = GetCurrentUserName();

                var createdPetId = await _petRepository.CreatePetAsync(pet, cancellationToken);
                if (createdPetId <= 0)
                {
                    return new BaseResponse<PetResponseDTO>
                    {
                        Code = 200,
                        Success = false,
                        Message = "Failed to create pet",
                        Data = null
                    };
                }

                var vaccineProfile = new VaccineProfile
                {
                    PetId = pet.PetId,
                    IsCompleted = false, // Mặc định chưa tiêm đủ
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = GetCurrentUserName()
                };

                var vaccineProfileCreated = await _vaccineProfileRepository.CreateVaccineProfileAsync(vaccineProfile, cancellationToken);
                if (vaccineProfileCreated <= 0)
                {
                    _logger.LogWarning("Failed to create vaccine profile for pet with ID {PetId}", pet.PetId);
                    return new BaseResponse<PetResponseDTO>
                    {
                        Code = 200,
                        Success = false,
                        Message = "Lỗi khi tạo hồ sơ tiêm chủng cho thú cưng",
                        Data = null
                    };
                }
                else
                {
                    _logger.LogInformation("Vaccine profile created successfully for pet with ID {PetId}", pet.PetId);
                }

                var createdPet = await _petRepository.GetPetByIdAsync(pet.PetId, cancellationToken);
                var responseDTO = _mapper.Map<PetResponseDTO>(createdPet);

                return new BaseResponse<PetResponseDTO>
                {
                    Code = 201,
                    Success = true,
                    Message = "Pet created successfully",
                    Data = responseDTO
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating pet");
                return new BaseResponse<PetResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = "Error while creating pet: " + (ex.InnerException?.Message ?? ex.Message),
                    Data = null
                };
            }
        }

        public async Task<List<BaseResponse<PetResponseDTO>>> GetPetsByCustomerIdAsync(int accountId, CancellationToken cancellationToken)
        {
            try
            {
                var customer = await _customerRepository.GetCustomerByAccountId(accountId, cancellationToken);
                if(customer == null)
                {
                    return new List<BaseResponse<PetResponseDTO>>
                    {
                        new BaseResponse<PetResponseDTO>
                        {
                            Code = 200,
                            Success = false,
                            Message = "Customer not found",
                            Data = null
                        }
                    };
                }

                var pets = await _petRepository.GetPetsByCustomerIdAsync(customer.CustomerId, cancellationToken);
                if (pets == null || !pets.Any())
                {
                    return new List<BaseResponse<PetResponseDTO>>
                    {
                        new BaseResponse<PetResponseDTO>
                        {
                            Code = 404,
                            Success = false,
                            Message = "No pets found for this customer",
                            Data = null
                        }
                    };
                }
                var petResponses = pets.Select(p => new PetResponseDTO
                {
                    PetId = p.PetId,
                    PetCode = p.PetCode,
                    CustomerId = p.CustomerId,
                    Name = p.Name,
                    Species = p.Species,
                    Breed = p.Breed,
                    Gender = p.Gender,
                    DateOfBirth = p.DateOfBirth,
                    PlaceToLive = p.PlaceToLive,
                    PlaceOfBirth = p.PlaceOfBirth,
                    Image = p.Image,
                    Weight = p.Weight,
                    Color = p.Color,
                    Nationality = p.Nationality,
                    isSterilized = p.isSterilized
                }).ToList();

                return petResponses.Select(p => new BaseResponse<PetResponseDTO>
                {
                    Code = 200,
                    Success = true,
                    Message = "Pets retrieved successfully",
                    Data = p
                }).ToList();
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Error retrieving pets for customer");
                return new List<BaseResponse<PetResponseDTO>>
                {
                    new BaseResponse<PetResponseDTO>
                    {
                        Code = 500,
                        Success = false,
                        Message = "Error while retrieving pets: " + (ex.InnerException?.Message ?? ex.Message),
                        Data = null
                    }
                };
            }
        }
        private string GetCurrentUserName()
        {
            return _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Name)?.Value ?? "System";
        }
    }
}