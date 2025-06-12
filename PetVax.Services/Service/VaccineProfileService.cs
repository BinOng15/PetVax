using PetVax.BusinessObjects.Models;
using PetVax.Repositories.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static PetVax.BusinessObjects.DTO.ResponseModel;

namespace PetVax.Services.Service
{
    public class VaccineProfileService
    {
        private readonly IVaccineProfileRepository _vaccineProfileRepository;
        public VaccineProfileService(IVaccineProfileRepository vaccineProfileRepository)
        {
            _vaccineProfileRepository = vaccineProfileRepository;
        }

        public async Task<List<BaseResponse<>> GetAllVaccineProfilesAsync(CancellationToken cancellationToken)
        {
            var vaccineProfiles = await _vaccineProfileRepository.GetAllVaccineProfilesAsync(cancellationToken);

        }
    }
}
