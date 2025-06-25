using PetVax.BusinessObjects.DTO;
using PetVax.BusinessObjects.DTO.AppointmentDetailDTO;
using PetVax.BusinessObjects.DTO.AppointmentDTO;
using PetVax.BusinessObjects.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PetVax.BusinessObjects.DTO.ResponseModel;
using static PetVax.BusinessObjects.Enum.EnumList;

namespace PetVax.Services.IService
{
    public interface IAppointmentService
    {
        Task<DynamicResponse<AppointmentResponseDTO>> GetAllAppointmentAsync (GetAllItemsDTO getAllItemsDTO, CancellationToken cancellationToken);
        Task<BaseResponse<AppointmentResponseDTO>> GetAppointmentByIdAsync(int appointmentId, CancellationToken cancellationToken);
        Task<BaseResponse<List<AppointmentResponseDTO>>> GetAppointmentByPetIdAsync(int petId, CancellationToken cancellationToken);
        Task<BaseResponse<List<AppointmentResponseDTO>>> GetAppointmentByPetAndStatusAsync(int petId, EnumList.AppointmentStatus status, CancellationToken cancellationToken);
        Task<BaseResponse<List<AppointmentResponseDTO>>> GetAppointmentByCustomerIdAsync(int customerId, CancellationToken cancellationToken);
        Task<BaseResponse<AppointmentWithDetailResponseDTO>> CreateFullAppointmentAsync(CreateFullAppointmentDTO createFullAppointmentDTO, CancellationToken cancellationToken);
        Task<BaseResponse<AppointmentResponseDTO>> UpdateAppointmentAsync(int appointmentId, UpdateAppointmentDTO updateAppointmentDTO, CancellationToken cancellationToken);
        Task<BaseResponse<bool>> DeleteAppointmentAsync(int appointmentId, CancellationToken cancellationToken);
        Task<BaseResponse<AppointmentWithVaccinationResponseDTO>> CreateAppointmentVaccinationAsync(CreateAppointmentVaccinationDTO createAppointmentVaccinationDTO, CancellationToken cancellationToken);
        Task<BaseResponse<AppointmentForVaccinationResponseDTO>> UpdateAppointmentVaccinationAsync(int appointmentId, UpdateAppointmentForVaccinationDTO updateAppointmentForVaccinationDTO, CancellationToken cancellationToken);
        Task<BaseResponse<AppointmentVaccinationDetailResponseDTO>> UpdateAppointmentVaccination(int appointmentId, UpdateAppointmentVaccinationDTO updateAppointmentVaccinationDTO, CancellationToken cancellationToken);
        Task<BaseResponse<List<AppointmentResponseDTO>>> GetAppointmentStatusAsync(AppointmentStatus status, CancellationToken cancellationToken);
        Task<BaseResponse<List<AppointmentResponseDTO>>> GetAppointmentByCustomerIdAndStatusAsync(int customerId, AppointmentStatus status, CancellationToken cancellationToken);
        Task<BaseResponse<AppointmentForVaccinationResponseDTO>> GetAppointmentVaccinationByIdAsync(int appointmentId, CancellationToken cancellationToken);
        Task<BaseResponse<AppointmentWithMicorchipResponseDTO>> CreateAppointmentMicrochipAsync(CreateAppointmentMicrochipDTO createAppointmentMicrochipDTO, CancellationToken cancellationToken);
        Task<DynamicResponse<AppointmentResponseDTO>> GetPastAppointmentsByCustomerIdAsync(int customerId, GetAllItemsDTO getAllItemsDTO, CancellationToken cancellationToken);
        Task<DynamicResponse<AppointmentResponseDTO>> GetTodayAppointmentsByCustomerIdAsync(int customerId, GetAllItemsDTO getAllItemsDTO, CancellationToken cancellationToken);
        Task<DynamicResponse<AppointmentResponseDTO>> GetFutureAppointmentsByCustomerIdAsync(int customerId, GetAllItemsDTO getAllItemsDTO, CancellationToken cancellationToken);
        Task<BaseResponse<AppointmentMicrochipResponseDTO>> UpdateAppointmentMicrochip( UpdateAppointmentMicrochipDTO updateAppointmentMicrochipDTO, CancellationToken cancellationToken);

        Task<BaseResponse<AppointmentWithMicorchipResponseDTO>> UpdateAppointmentMicrochipAsync(int appointmentId, CreateAppointmentMicrochipDTO createAppointmentMicrochipDTO, CancellationToken cancellationToken);
    }
}
