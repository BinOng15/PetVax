using PetVax.BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetVax.Repositories.IRepository
{
    public interface IVaccinationScheduleRepository
    {
        Task<int> CreateVaccinationScheduleAsync(VaccinationSchedule vaccinationSchedule, CancellationToken cancellationToken);
        Task<VaccinationSchedule> GetVaccinationScheduleByIdAsync(int vaccinationScheduleId, CancellationToken cancellationToken);
        Task<List<VaccinationSchedule>> GetAllVaccinationSchedulesAsync(CancellationToken cancellationToken);
        Task<int> UpdateVaccinationScheduleAsync(VaccinationSchedule vaccinationSchedule, CancellationToken cancellationToken);
        Task<bool> DeleteVaccinationScheduleAsync(int vaccinationScheduleId, CancellationToken cancellationToken);
        Task<List<VaccinationSchedule>> GetVaccinationScheduleByDiseaseIdAsync(int diseaseId, CancellationToken cancellationToken);
        Task<VaccinationSchedule> GetVaccinationScheduleByDiseaseIdAsyncs(int diseaseId, CancellationToken cancellationToken);
        Task<List<VaccinationSchedule>> GetVaccinationScheduleBySpeciesAsync(string species, CancellationToken cancellationToken);
    }
}
