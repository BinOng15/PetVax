using AutoMapper;
using Microsoft.AspNetCore.Http;
using PetVax.BusinessObjects.DTO;
using PetVax.BusinessObjects.DTO.AppointmentDetailDTO;
using PetVax.BusinessObjects.DTO.CustomerDTO;
using PetVax.BusinessObjects.DTO.MicrochipItemDTO;
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
    public class MicrochipItemService : IMicrochipItemService
    {
        private readonly IMicrochipItemRepository _microchipItemRepository;
        private readonly IMapper _mapper;
        private readonly IPetRepository _petRepository;
        private readonly IMicrochipRepository _microchipRepository;
        private readonly IAppointmentDetailRepository _appointmentDetailRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public MicrochipItemService(
            IMicrochipItemRepository microchipItemRepository,
            IMapper mapper,
            IPetRepository petRepository,
            IMicrochipRepository microchipRepository,
            IAppointmentDetailRepository appointmentDetailRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _microchipItemRepository = microchipItemRepository;
            _mapper = mapper;
            _petRepository = petRepository;
            _microchipRepository = microchipRepository;
            _appointmentDetailRepository = appointmentDetailRepository;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<BaseResponse<MicrochipItemResponse>> GetMicrochipItemByMicrochipCodeAsync(string code, CancellationToken cancellationToken = default)
        {
            try
            {
                //  Get MicrochipItem by code
                var microchipItem = await _microchipItemRepository.GetMicrochipItemByMicrochipCodedAsync(code, cancellationToken);
                if (microchipItem == null)
                {
                    return new BaseResponse<MicrochipItemResponse>
                    {
                        Code = 200,
                        Message = "Không tìm thấy Microchip hoặc Microchip này chưa được cấy vào thú cưng!",
                        Data = null
                    };
                }

                //  Get corresponding Pet
                var pet = await _petRepository.GetPetAndAppointmentByIdAsync(microchipItem.PetId, cancellationToken);
                if (pet == null)
                {
                    return new BaseResponse<MicrochipItemResponse>
                    {
                        Code = 200,
                        Message = "Microchip chưa được gắn cho thú cưng!",
                        Data = _mapper.Map<MicrochipItemResponse>(microchipItem)
                    };
                }

                //  Get AppointmentDetail information (if any)
                var appointmentDetail = await _appointmentDetailRepository.GetAppointmentDetailandServiceTypeByPetIdAsync(pet.PetId, cancellationToken);

                //  Use AutoMapper for AppointmentDetail
                AppointmentDetailResponseDTO? appointmentDetailDto = null;
                if (appointmentDetail != null)
                {
                    appointmentDetailDto = _mapper.Map<AppointmentDetailResponseDTO>(appointmentDetail);
                }

                //  Manually create DTO for Customer
                var customerDto = pet.Customer == null ? null : new CustomerResponseDTO
                {
                    CustomerId = pet.Customer.CustomerId,
                    AccountId = pet.Customer.AccountId,
                    MembershipId = pet.Customer.MembershipId,
                    CustomerCode = pet.Customer.CustomerCode,
                    FullName = pet.Customer.FullName,
                    UserName = pet.Customer.UserName,
                    Image = pet.Customer.Image,
                    PhoneNumber = pet.Customer.PhoneNumber,
                    DateOfBirth = pet.Customer.DateOfBirth,
                    Gender = pet.Customer.Gender,
                    Address = pet.Customer.Address,
                    CurrentPoints = pet.Customer.CurrentPoints,
                    CreatedAt = pet.Customer.CreatedAt,
                    CreatedBy = pet.Customer.CreatedBy,
                    ModifiedAt = pet.Customer.ModifiedAt,
                    ModifiedBy = pet.Customer.ModifiedBy
                };

                // Manually create DTO for Pet
                var petDto = new PetMicrochipItemResponse
                {
                    PetId = pet.PetId,
                    CustomerId = pet.CustomerId,
                    PetCode = pet.PetCode,
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
                    isSterilized = pet.isSterilized,
                    AppointmentDetail = appointmentDetailDto,
                    Customer = customerDto
                };

                //  Manually create DTO for MicrochipItem
                var microchipItemResponse = new MicrochipItemResponse
                {
                    MicrochipId = microchipItem.MicrochipId,
                    Name = microchipItem.Name,
                    Description = microchipItem.Description,
                    InstallationDate = microchipItem.InstallationDate,
                    Status = microchipItem.Status,
                    Pet = petDto
                };

                // Return result
                return new BaseResponse<MicrochipItemResponse>
                {
                    Code = 200,
                    Message = "Lấy thông tin Microchip thành công!",
                    Data = microchipItemResponse
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<MicrochipItemResponse>
                {
                    Code = 500,
                    Message = "Lỗi khi lấy thông tin Microchip: " + ex.Message,
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<BaseMicrochipItemResponse>> CreateMicrochipItemAsync(CreateMicrochipItemRequest request, CancellationToken cancellationToken)
        {
            try
            {
                //  Validate Microchip exists
                var microchip = await _microchipRepository.GetMicrochipByIdAsync(request.MicrochipId, cancellationToken);
                if (microchip == null || microchip.Status.Equals("Unavailable"))
                {
                    return new BaseResponse<BaseMicrochipItemResponse>
                    {
                        Code = 200,
                        Message = "Microchip không tồn tại hoặc microchip chưa kích hoạt!",
                        Data = null
                    };
                }


                //  Check if MicrochipItem already exists
                var checkMicroipItem = await _microchipItemRepository.GetMicrochipItemByMicrochipIdAsync(request.MicrochipId, cancellationToken);

                if (microchip.MicrochipId == checkMicroipItem?.MicrochipId)
                {
                    return new BaseResponse<BaseMicrochipItemResponse>
                    {
                        Code = 200,
                        Message = "Microchip item đã được tạo!",
                        Data = null
                    };
                }

                //  Check if Microchip is already installed
                if (checkMicroipItem.PetId != null && checkMicroipItem.PetId > 0)
                {
                    return new BaseResponse<BaseMicrochipItemResponse>
                    {
                        Code = 200,
                        Message = "Microchip đã được cấy vào cho thú cưng " + checkMicroipItem.PetId,
                        Data = null
                    };
                }

                //  Check if Pet already has a MicrochipItem installed
                var checkInstalled = await _microchipItemRepository.GetMicrochipItemByPetIdAsync(request.PetId, cancellationToken);
                if (checkInstalled != null)
                {
                    return new BaseResponse<BaseMicrochipItemResponse>
                    {
                        Code = 200,
                        Message = "Thú cưng này đã được cấy microchip",
                        Data = null
                    };
                }

                //  Validate Pet exists
                var pet = await _petRepository.GetPetAndAppointmentByIdAsync(request.PetId, cancellationToken);
                if (pet == null)
                {
                    return new BaseResponse<BaseMicrochipItemResponse>
                    {
                        Code = 200,
                        Message = "Thú cưng không tồn tại!",
                        Data = null
                    };
                }

                //  Map request to MicrochipItem entity
                MicrochipItem microchipItem = new MicrochipItem();

                microchipItem.MicrochipId = request.MicrochipId;
                microchipItem.PetId = request.PetId;
                microchipItem.Name = request.Name;
                microchipItem.Description = request.Description;
                microchipItem.InstallationDate = request.InstallationDate;
                microchipItem.CreatedAt = DateTime.UtcNow;
                microchipItem.CreatedBy = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "system";

                if (request.PetId == null || request.PetId <= 0)
                {
                    microchipItem.Status = "InActive";
                }
                else
                {
                    microchipItem.Status = "Active";
                }
                //  Save to repository
                var result = await _microchipItemRepository.CreateMicrochipItemAsync(microchipItem, cancellationToken);
                var microchipItemCheck = await _microchipItemRepository.GetMicrochipItemByIdAsync(microchipItem.MicrochipItemId, cancellationToken);
                return new BaseResponse<BaseMicrochipItemResponse>
                {
                    Code = 201,
                    Message = "Tạo Microchip thành công!",
                    Data = _mapper.Map<BaseMicrochipItemResponse>(microchipItemCheck)
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<BaseMicrochipItemResponse>
                {
                    Code = 500,
                    Message = "Lỗi khi tạo Microchip: " + ex.Message,
                    Data = null
                };
            }

        }

        public async Task<BaseResponse<BaseMicrochipItemResponse>> UpdateMicrochipItemAsync(int microchipItemId, UpdateMicrochipItemRequest request, CancellationToken cancellationToken)
        {
            try
            {
                //  Validate MicrochipItem exists
                var microchipItem = await _microchipItemRepository.GetMicrochipItemByIdAsync(microchipItemId, cancellationToken);
                if (microchipItem == null)
                {
                    return new BaseResponse<BaseMicrochipItemResponse>
                    {
                        Code = 200,
                        Message = "Microchip Item không tồn tại!",
                        Data = null
                    };
                }
                //  Validate Microchip exists
                var microchip = await _microchipRepository.GetMicrochipByIdAsync(request.MicrochipId, cancellationToken);
                if (microchip == null || microchip.Status.Equals("Unavailable"))
                {
                    return new BaseResponse<BaseMicrochipItemResponse>
                    {
                        Code = 200,
                        Message = "Microchip không tồn tại hoặc microchip chưa kích hoạt!",
                        Data = null
                    };
                }


                //  Check if Pet already has a MicrochipItem installed
                var checkInstalled = await _microchipItemRepository.GetMicrochipItemByPetIdAsync(request.PetId, cancellationToken);
                if (checkInstalled != null)
                {
                    return new BaseResponse<BaseMicrochipItemResponse>
                    {
                        Code = 200,
                        Message = "Thú cưng này đã được cấy microchip",
                        Data = null
                    };
                }


                var checkMicroipItem = await _microchipItemRepository.GetMicrochipItemByMicrochipIdAsync(request.MicrochipId, cancellationToken);

                if (microchip.MicrochipId == checkMicroipItem?.MicrochipId)
                {
                    return new BaseResponse<BaseMicrochipItemResponse>
                    {
                        Code = 200,
                        Message = "Microchip đã được tạo!",
                        Data = null
                    };
                }

                if (checkMicroipItem.PetId != null && checkMicroipItem.PetId > 0)
                {
                    return new BaseResponse<BaseMicrochipItemResponse>
                    {
                        Code = 200,
                        Message = "Microchip đã được cấy vào cho thú cưng " + checkMicroipItem.PetId,
                        Data = null
                    };
                }

                var pet = await _petRepository.GetPetAndAppointmentByIdAsync(request.PetId, cancellationToken);
                if (pet == null)
                {
                    return new BaseResponse<BaseMicrochipItemResponse>
                    {
                        Code = 200,
                        Message = "Thú cưng không tồn tại!",
                        Data = null
                    };
                }
                //  Map request to MicrochipItem entity
                _mapper.Map(request, microchipItem);
                microchipItem.ModifiedAt = DateTime.UtcNow;
                microchipItem.ModifiedBy = _httpContextAccessor.HttpContext?.User?.Identity?.Name ?? "system";
                //  Save to repository
                var result = await _microchipItemRepository.UpdateMicrochipItemAsync(microchipItem, cancellationToken);

                return new BaseResponse<BaseMicrochipItemResponse>
                {
                    Code = 200,
                    Message = "Cập nhật Microchip thành công!",
                    Data = _mapper.Map<BaseMicrochipItemResponse>(microchipItem)
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<BaseMicrochipItemResponse>
                {
                    Code = 500,
                    Message = "Lỗi khi cập nhật Microchip: " + ex.Message,
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<BaseMicrochipItemResponse>> GetMicrochipItemByIdAsync(int id, CancellationToken cancellationToken)
        {
            try
            {
                //  Validate MicrochipItem exists
                var microchipItem = await _microchipItemRepository.GetMicrochipItemByIdAsync(id, cancellationToken);
                if (microchipItem == null)
                {
                    return new BaseResponse<BaseMicrochipItemResponse>
                    {
                        Code = 200,
                        Message = "Microchip Item không tồn tại!",
                        Data = null
                    };
                }

                return new BaseResponse<BaseMicrochipItemResponse>
                {
                    Code = 200,
                    Message = "Lấy Microchip Item thành công!",
                    Data = _mapper.Map<BaseMicrochipItemResponse>(microchipItem)
                };

            }
            catch (Exception ex)
            {
                return new BaseResponse<BaseMicrochipItemResponse>
                {
                    Code = 500,
                    Message = "Lỗi khi lấy Microchip: " + ex.Message,
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<MegaData<BaseMicrochipItemResponse>>> GetAllMicrochipItemsPagingAsync(GetAllItemsDTO getAllItemsDTO, CancellationToken cancellationToken)
        {
            try
            {
                var microchipItems = await _microchipItemRepository.GetAllMicrochipItemsAsync(cancellationToken);

                // Filter by keyword if provided (search by Name or Description)
                if (!string.IsNullOrWhiteSpace(getAllItemsDTO.KeyWord))
                {
                    var lowerKeyword = getAllItemsDTO.KeyWord.Trim().ToLower();
                    microchipItems = microchipItems
                        .Where(m => (m.Name != null && m.Name.ToLower().Contains(lowerKeyword)) ||
                                    (m.Description != null && m.Description.ToLower().Contains(lowerKeyword)))
                        .ToList();
                }

                // Pagination
                int pageNumber = getAllItemsDTO?.PageNumber > 0 ? getAllItemsDTO.PageNumber : 1;
                int pageSize = getAllItemsDTO?.PageSize > 0 ? getAllItemsDTO.PageSize : 10;
                int skip = (pageNumber - 1) * pageSize;
                int totalItem = microchipItems.Count;
                int totalPage = (int)Math.Ceiling((double)totalItem / pageSize);

                var pagedItems = microchipItems
                    .Skip(skip)
                    .Take(pageSize)
                    .ToList();

                var responseData = new MegaData<BaseMicrochipItemResponse>
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
                        keyWord = getAllItemsDTO?.KeyWord,
                        status = getAllItemsDTO?.Status,
                    },
                    PageData = _mapper.Map<List<BaseMicrochipItemResponse>>(pagedItems)
                };

                if (!pagedItems.Any())
                {
                    return new BaseResponse<MegaData<BaseMicrochipItemResponse>>
                    {
                        Code = 200,
                        Success = true,
                        Message = "Không tìm thấy Microchip Item nào phù hợp với tiêu chí tìm kiếm",
                        Data = null
                    };
                }

                return new BaseResponse<MegaData<BaseMicrochipItemResponse>>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy danh sách Microchip Item thành công",
                    Data = responseData
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<MegaData<BaseMicrochipItemResponse>>
                {
                    Code = 500,
                    Success = false,
                    Message = "Lỗi khi lấy danh sách Microchip Item: " + (ex.InnerException?.Message ?? ex.Message),
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<BaseMicrochipItemResponse>> DeleteMicrochipItemAsync(int microchipItemId, CancellationToken cancellationToken)
        {
            try
            {
                //  Validate MicrochipItem exists
                var microchipItem = await _microchipItemRepository.GetMicrochipItemByIdAsync(microchipItemId, cancellationToken);
                if (microchipItem == null)
                {
                    return new BaseResponse<BaseMicrochipItemResponse>
                    {
                        Code = 200,
                        Message = "Microchip Item không tồn tại!",
                        Data = null
                    };
                }
                //  Delete MicrochipItem
                microchipItem.isDeleted = true;
                var result = await _microchipItemRepository.UpdateMicrochipItemAsync(microchipItem, cancellationToken);
                if (result <= 0)
                {
                    return new BaseResponse<BaseMicrochipItemResponse>
                    {
                        Code = 500,
                        Message = "Lỗi khi xóa Microchip Item!",
                        Data = null
                    };
                }
                return new BaseResponse<BaseMicrochipItemResponse>
                {
                    Code = 200,
                    Message = "Xóa Microchip Item thành công!",
                    Data = null
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<BaseMicrochipItemResponse>
                {
                    Code = 500,
                    Message = "Lỗi khi xóa Microchip Item: " + ex.Message,
                    Data = null
                };
            }
        }
    }
}
