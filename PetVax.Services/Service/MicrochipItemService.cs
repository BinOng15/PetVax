using AutoMapper;
using PetVax.BusinessObjects.DTO.AppointmentDetailDTO;
using PetVax.BusinessObjects.DTO.CustomerDTO;
using PetVax.BusinessObjects.DTO.MicrochipItemDTO;
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

        public MicrochipItemService(
            IMicrochipItemRepository microchipItemRepository,
            IMapper mapper,
            IPetRepository petRepository,
            IMicrochipRepository microchipRepository,
            IAppointmentDetailRepository appointmentDetailRepository)
        {
            _microchipItemRepository = microchipItemRepository;
            _mapper = mapper;
            _petRepository = petRepository;
            _microchipRepository = microchipRepository;
            _appointmentDetailRepository = appointmentDetailRepository;
        }
        public async Task<BaseResponse<MicrochipItemResponse>> GetMicrochipItemByMicrochipCodeAsync(string code, CancellationToken cancellationToken = default)
        {
            try
            {
                // 1. Get MicrochipItem by code
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

                // 2. Get corresponding Pet
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

                // 3. Get AppointmentDetail information (if any)
                var appointmentDetail = await _appointmentDetailRepository.GetAppointmentDetailandServiceTypeByPetIdAsync(pet.PetId, cancellationToken);

                // 4. Use AutoMapper for AppointmentDetail
                AppointmentDetailResponseDTO? appointmentDetailDto = null;
                if (appointmentDetail != null)
                {
                    appointmentDetailDto = _mapper.Map<AppointmentDetailResponseDTO>(appointmentDetail);
                }

                // 5. Manually create DTO for Customer
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

                // 6. Manually create DTO for Pet
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

                // 7. Manually create DTO for MicrochipItem
                var microchipItemResponse = new MicrochipItemResponse
                {
                    MicrochipId = microchipItem.MicrochipId,
                    Name = microchipItem.Name,
                    Description = microchipItem.Description,
                    InstallationDate = microchipItem.InstallationDate,
                    Status = microchipItem.Status,
                    Pet = petDto
                };

                // 8. Return result
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

    }
}
