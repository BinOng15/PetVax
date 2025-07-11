using Npgsql.Internal;
using PetVax.BusinessObjects.DTO;
using PetVax.BusinessObjects.DTO.VaccinationSchedule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PetVax.BusinessObjects.DTO.ResponseModel;


namespace PetVax.Services.IService
{
    public interface IVaccinationScheduleService
    {
        Task<DynamicResponse<VaccinationScheduleResponseDTO>> GetAllVaccinationScheduleAsync(GetAllItemsDTO getAllItemsDTO, CancellationToken cancellationToken);
        Task<BaseResponse<VaccinationScheduleResponseDTO>> GetVaccinationScheduleByIdAsync(int vaccinationScheduleId, CancellationToken cancellationToken);
        Task<BaseResponse<VaccinationScheduleByDiseaseResponseDTO>> GetVaccinationScheduleByDiseaseIdAsync(int diseaseId, CancellationToken cancellationToken);
        Task<BaseResponse<VaccinationScheduleResponseDTO>> CreateVaccinationScheduleAsync(CreateVaccinationScheduleDTO createVaccinationScheduleDTO, CancellationToken cancellationToken);
        Task<BaseResponse<VaccinationScheduleResponseDTO>> UpdateVaccinationScheduleAsync(int vaccinationScheduleId, UpdateVaccinationScheduleDTO updateVaccinationScheduleDTO, CancellationToken cancellationToken);
        Task<BaseResponse<bool>> DeleteVaccinationScheduleAsync(int vaccinationScheduleId, CancellationToken cancellationToken);
    }
}
