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
        Task<BaseResponse<AppointmentVaccinationDetailResponseDTO>> UpdateAppointmentVaccination(int appointmentId, UpdateAppointmentVaccinationDTO updateAppointmentVaccinationDTO, CancellationToken cancellationToken);
    }
}
