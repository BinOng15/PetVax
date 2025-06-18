using PetVax.BusinessObjects.DTO;
using PetVax.BusinessObjects.DTO.AppointmentDetailDTO;
using PetVax.BusinessObjects.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PetVax.BusinessObjects.DTO.ResponseModel;

namespace PetVax.Services.IService
{
    public interface IAppointmentDetailService
    {
        Task<DynamicResponse<AppointmentDetailResponseDTO>> GetAllAppointmentDetail (GetAllItemsDTO getAllItemsDTO, CancellationToken cancellationToken);
        Task<BaseResponse<AppointmentDetailResponseDTO>> GetAppointmentDetailById(int appointmentDetailId, CancellationToken cancellationToken);
        //Task<BaseResponse<AppointmentDetailResponseDTO>> CreateAppointmentDetail(CreateAppointmentDetailDTO createAppointmentDetailDTO, CancellationToken cancellationToken);
        Task<BaseResponse<AppointmentDetailResponseDTO>> UpdateAppointmentDetail(int appointmentDetailId, UpdateAppointmentDetailDTO updateAppointmentDetailDTO, CancellationToken cancellationToken);
        Task<BaseResponse<bool>> DeleteAppointmentDetail(int appointmentDetailId, CancellationToken cancellationToken);
        Task<BaseResponse<AppointmentDetailResponseDTO>> GetAppointmentDetailByAppointmentId(int appointmentId, CancellationToken cancellationToken);
        Task<BaseResponse<AppointmentDetailResponseDTO>> GetAppointmentDetailByPetId(int petId, CancellationToken cancellationToken);
        Task<BaseResponse<AppointmentDetailResponseDTO>> GetAppointmentDetailByVetId(int vetId, CancellationToken cancellationToken);
        Task<BaseResponse<AppointmentDetailResponseDTO>> GetAppointmentDetailByServiceType(EnumList.ServiceType serviceType, CancellationToken cancellationToken);
        Task<BaseResponse<AppointmentDetailResponseDTO>> GetAppointmentDetailByStatus(EnumList.AppointmentStatus status, CancellationToken cancellationToken);
        Task<BaseResponse<AppointmentVaccinationDetailResponseDTO>> UpdateAppointmentVaccination(int appointmentDetailId, UpdateAppointmentVaccinationDTO updateAppointmentVaccinationDTO, CancellationToken cancellationToken);
    }
}
