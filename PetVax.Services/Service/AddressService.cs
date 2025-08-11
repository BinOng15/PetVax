using AutoMapper;
using PetVax.BusinessObjects.DTO;
using PetVax.BusinessObjects.DTO.AddressDTO;
using PetVax.BusinessObjects.DTO.CustomerDTO;
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
    public class AddressService : IAddressService
    {
        private readonly IAddressRepository _addressRepository;
        private readonly IMapper _mapper;

        public AddressService(IAddressRepository addressRepository, IMapper mapper)
        {
            _addressRepository = addressRepository;
            _mapper = mapper;
        }

        public async Task<BaseResponse<AddressResponseDTO>> CreateAddressAsync(CreateAddressDTO createAddressDTO, CancellationToken cancellationToken)
        {
            if (createAddressDTO.Location == null)
            {
                return new BaseResponse<AddressResponseDTO>
                {
                    Code = 400,
                    Success = false,
                    Message = "Địa chỉ không được để trống",
                    Data = null
                };
            }
            try
            {
                // Kiểm tra nếu đã có dữ liệu trong database thì không cho tạo nữa
                var addresses = await _addressRepository.GetAllAddressesAsync(cancellationToken);
                if (addresses != null && addresses.Count > 0)
                {
                    return new BaseResponse<AddressResponseDTO>
                    {
                        Code = 409,
                        Success = false,
                        Message = "Đã có dữ liệu địa chỉ trong hệ thống, không thể tạo thêm.",
                        Data = null
                    };
                }

                var newAddress = new Address { Location = createAddressDTO.Location };
                var newId = await _addressRepository.CreateAddressAsync(newAddress, cancellationToken);
                newAddress.AddressId = newId;
                var responseDTO = _mapper.Map<AddressResponseDTO>(newAddress);
                return new BaseResponse<AddressResponseDTO>
                {
                    Code = 201,
                    Success = true,
                    Message = "Tạo địa chỉ thành công",
                    Data = responseDTO
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<AddressResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = $"Lỗi khi tạo địa chỉ: {ex.Message}",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<bool>> DeleteAddressAsync(int addressId, CancellationToken cancellationToken)
        {
            if (addressId <= 0)
            {
                return new BaseResponse<bool>
                {
                    Code = 400,
                    Success = false,
                    Message = "ID địa chỉ không hợp lệ",
                    Data = false
                };
            }
            try
            {
                var isDeleted = await _addressRepository.DeleteAddressAsync(addressId, cancellationToken);
                if (isDeleted)
                {
                    return new BaseResponse<bool>
                    {
                        Code = 200,
                        Success = true,
                        Message = "Xóa địa chỉ thành công",
                        Data = true
                    };
                }
                else
                {
                    return new BaseResponse<bool>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Địa chỉ không tồn tại",
                        Data = false
                    };
                }
            }
            catch (Exception ex)
            {
                return new BaseResponse<bool>
                {
                    Code = 500,
                    Success = false,
                    Message = $"Lỗi khi xóa địa chỉ: {ex.Message}",
                    Data = false
                };
            }
        }

        public async Task<BaseResponse<AddressResponseDTO>> GetAddressByIdAsync(int addressId, CancellationToken cancellationToken)
        {
            if (addressId <= 0)
            {
                return new BaseResponse<AddressResponseDTO>
                {
                    Code = 400,
                    Success = false,
                    Message = "ID địa chỉ không hợp lệ",
                    Data = null
                };
            }
            try
            {
                var address = await _addressRepository.GetAddressByIdAsync(addressId, cancellationToken);
                if (address == null)
                {
                    return new BaseResponse<AddressResponseDTO>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Địa chỉ không tồn tại",
                        Data = null
                    };
                }
                var responseDTO = _mapper.Map<AddressResponseDTO>(address);
                return new BaseResponse<AddressResponseDTO>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy địa chỉ thành công",
                    Data = responseDTO
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<AddressResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = $"Lỗi khi lấy địa chỉ: {ex.Message}",
                    Data = null
                };
            }
        }

        public async Task<DynamicResponse<AddressResponseDTO>> GetAllAddressAsync(GetAllItemsDTO getAllItemsDTO, CancellationToken cancellationToken)
        {
            try
            {
                var addresses = await _addressRepository.GetAllAddressesAsync(cancellationToken);
                if (!string.IsNullOrWhiteSpace(getAllItemsDTO.KeyWord))
                {
                    addresses = addresses.Where(a => a.Location.Contains(getAllItemsDTO.KeyWord, StringComparison.OrdinalIgnoreCase)).ToList();
                }

                int pageNumber = getAllItemsDTO?.PageNumber > 0 ? getAllItemsDTO.PageNumber : 1;
                int pageSize = getAllItemsDTO?.PageSize > 0 ? getAllItemsDTO.PageSize : 10;
                int skip = (pageNumber - 1) * pageSize;
                int totalItem = addresses.Count;
                int totalPage = (int)Math.Ceiling((double)totalItem / pageSize);

                var pagedAddresses = addresses
                    .Skip(skip)
                    .Take(pageSize)
                    .Select(a => _mapper.Map<AddressResponseDTO>(a))
                    .ToList();

                var resposneData = new MegaData<AddressResponseDTO>
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
                        keyWord = getAllItemsDTO.KeyWord,
                        status = getAllItemsDTO.Status
                    },
                    PageData = _mapper.Map<List<AddressResponseDTO>>(pagedAddresses)
                };
                if (!pagedAddresses.Any())
                {
                    return new DynamicResponse<AddressResponseDTO>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Không tìm thấy địa chỉ nào",
                        Data = resposneData
                    };
                }
                return new DynamicResponse<AddressResponseDTO>
                {
                    Code = 200,
                    Success = true,
                    Message = "Lấy danh sách địa chỉ thành công",
                    Data = resposneData
                };
            }
            catch (Exception ex)
            {
                return new DynamicResponse<AddressResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = $"Lỗi khi lấy danh sách địa chỉ: {ex.Message}",
                    Data = null
                };
            }
        }

        public async Task<BaseResponse<AddressResponseDTO>> UpdateAddressAsync(int addressId, UpdateAddressDTO updateAddressDTO, CancellationToken cancellationToken)
        {
            if (addressId <= 0)
            {
                return new BaseResponse<AddressResponseDTO>
                {
                    Code = 400,
                    Success = false,
                    Message = "ID địa chỉ không hợp lệ",
                    Data = null
                };
            }
            if (updateAddressDTO.Location == null)
            {
                return new BaseResponse<AddressResponseDTO>
                {
                    Code = 400,
                    Success = false,
                    Message = "Địa chỉ không được để trống",
                    Data = null
                };
            }
            try
            {
                var address = await _addressRepository.GetAddressByIdAsync(addressId, cancellationToken);
                if (address == null)
                {
                    return new BaseResponse<AddressResponseDTO>
                    {
                        Code = 404,
                        Success = false,
                        Message = "Địa chỉ không tồn tại",
                        Data = null
                    };
                }
                address.Location = updateAddressDTO.Location ?? address.Location;
                await _addressRepository.UpdateAddressAsync(address, cancellationToken);
                var responseDTO = _mapper.Map<AddressResponseDTO>(address);
                return new BaseResponse<AddressResponseDTO>
                {
                    Code = 200,
                    Success = true,
                    Message = "Cập nhật địa chỉ thành công",
                    Data = responseDTO
                };
            }
            catch (Exception ex)
            {
                return new BaseResponse<AddressResponseDTO>
                {
                    Code = 500,
                    Success = false,
                    Message = $"Lỗi khi cập nhật địa chỉ: {ex.Message}",
                    Data = null
                };
            }
        }
    }
}
