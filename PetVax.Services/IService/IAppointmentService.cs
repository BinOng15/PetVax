using PetVax.BusinessObjects.DTO;
using PetVax.BusinessObjects.DTO.AppointmentDetailDTO;
using PetVax.BusinessObjects.DTO.AppointmentDTO;
using PetVax.BusinessObjects.Enum;
using PetVax.BusinessObjects.Models;
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
        //Task<BaseResponse<AppointmentWithDetailResponseDTO>> CreateFullAppointmentAsync(CreateFullAppointmentDTO createFullAppointmentDTO, CancellationToken cancellationToken);
        //Task<BaseResponse<AppointmentResponseDTO>> UpdateAppointmentAsync(int appointmentId, UpdateAppointmentDTO updateAppointmentDTO, CancellationToken cancellationToken);
        Task<BaseResponse<bool>> DeleteAppointmentAsync(int appointmentId, CancellationToken cancellationToken);
        Task<BaseResponse<List<AppointmentResponseDTO>>> GetAppointmentStatusAsync(AppointmentStatus status, CancellationToken cancellationToken);
        Task<BaseResponse<List<AppointmentResponseDTO>>> GetAppointmentByCustomerIdAndStatusAsync(int customerId, AppointmentStatus status, CancellationToken cancellationToken);
        Task<DynamicResponse<AppointmentResponseDTO>> GetPastAppointmentsByCustomerIdAsync(int customerId, GetAllItemsDTO getAllItemsDTO, CancellationToken cancellationToken);
        Task<DynamicResponse<AppointmentResponseDTO>> GetTodayAppointmentsByCustomerIdAsync(int customerId, GetAllItemsDTO getAllItemsDTO, CancellationToken cancellationToken);
        Task<DynamicResponse<AppointmentResponseDTO>> GetFutureAppointmentsByCustomerIdAsync(int customerId, GetAllItemsDTO getAllItemsDTO, CancellationToken cancellationToken);
        
        //Vaccination
        Task<BaseResponse<AppointmentWithVaccinationResponseDTO>> CreateAppointmentVaccinationAsync(CreateAppointmentVaccinationDTO createAppointmentVaccinationDTO, CancellationToken cancellationToken);
        Task<BaseResponse<AppointmentForVaccinationResponseDTO>> UpdateAppointmentVaccinationAsync(int appointmentId, UpdateAppointmentForVaccinationDTO updateAppointmentForVaccinationDTO, CancellationToken cancellationToken);
        Task<BaseResponse<AppointmentVaccinationDetailResponseDTO>> UpdateAppointmentVaccination(int appointmentId, UpdateAppointmentVaccinationDTO updateAppointmentVaccinationDTO, CancellationToken cancellationToken);
        Task<BaseResponse<AppointmentForVaccinationResponseDTO>> GetAppointmentVaccinationByIdAsync(int appointmentId, CancellationToken cancellationToken);
        Task<DynamicResponse<AppointmentForVaccinationResponseDTO>> GetAllAppointmentVaccinationAsync(GetAllItemsDTO getAllItemsDTO, int? vetId, CancellationToken cancellationToken);

        //Microphip
        Task<BaseResponse<AppointmentWithMicorchipResponseDTO>> CreateAppointmentMicrochipAsync(CreateAppointmentMicrochipDTO createAppointmentMicrochipDTO, CancellationToken cancellationToken);
        Task<BaseResponse<AppointmentMicrochipResponseDTO>> UpdateAppointmentMicrochip( UpdateAppointmentMicrochipDTO updateAppointmentMicrochipDTO, CancellationToken cancellationToken);
        Task<BaseResponse<AppointmentWithMicorchipResponseDTO>> UpdateAppointmentMicrochipAsync(int appointmentId, UpdateAppointmentDTO updateAppointment, CancellationToken cancellationToken);
        Task<BaseResponse<AppointmentResponseDTO>> GetAppointmentMicrochipByAppointmentId(int appointmentId, CancellationToken cancellationToken);

        //Vaccination Certificate
        Task<BaseResponse<AppointmentWithVaccinationCertificateResponseDTO>> CreateAppointmentVaccinationCertificate(CreateAppointmentVaccinationCertificateDTO createAppointmentVaccinationCertificateDTO, CancellationToken cancellationToken);
        Task<BaseResponse<AppointmentResponseDTO>> UpdateAppointmentVaccinationCertificate(int appointmentId, UpdateAppointmentDTO updateAppointmentDTO, CancellationToken cancellationToken);
        Task<BaseResponse<AppointmentVaccinationCertificateResponseDTO>> UpdateAppointmentDetailVaccinationCertificate(int appointmentId, UpdateAppointmentVaccinationCertificateDTO updateAppointmentVaccinationCertificateDTO, CancellationToken cancellationToken);
        Task<BaseResponse<AppointmentWithVaccinationCertificateResponseDTO>> GetAppointmentVaccinationCertificateById(int appointmentId, CancellationToken cancellationToken);
        Task<DynamicResponse<AppointmentWithVaccinationCertificateResponseDTO>> GetAllAppointmentVaccinationCertificateAsync(GetAllItemsDTO getAllItemsDTO, CancellationToken cancellationToken);

        Task NotifyCustomerIfCancelledOrRejectedAsync(Appointment appointment, CancellationToken cancellationToken);

        // Health Condition
        Task<BaseResponse<AppointmenWithHealthConditionResponseDTO>> CreateAppointmentHealConditionAsync(CreateAppointmentHealthConditionDTO createAppointmentHealConditionDTO, CancellationToken cancellationToken);
        Task<BaseResponse<AppointmentHealthConditionResponseDTO>> UpdateAppointmentHealthConditionAsync(int AppointmentId, UpdateAppointmentHealthConditionDTO updateDTO, CancellationToken cancellationToken);
        Task<BaseResponse<AppointmentResponseDTO>> UpdateAppointmentHealConditionAsync(int appointmentId, UpdateAppointmentDTO updateAppointmentHealConditionDTO, CancellationToken cancellationToken);
    }
}
