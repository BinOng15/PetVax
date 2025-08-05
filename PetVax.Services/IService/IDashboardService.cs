using PetVax.BusinessObjects.DTO.DashboardDTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PetVax.BusinessObjects.DTO.ResponseModel;

namespace PetVax.Services.IService
{
    public interface IDashboardService
    {
        Task<BaseResponse<AdminDashboardResponseDTO>> GetDashboardDataForAdminAsync(CancellationToken cancellationToken);
        Task<BaseResponse<VetDashboardResponseDTO>> GetDashboardDataForVetAsync(CancellationToken cancellationToken);
    }
}
