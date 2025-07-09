using PetVax.BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetVax.Repositories.IRepository
{
    public interface IHealthConditionVaccinationCertificateRepository
    {
        Task<List<HealthConditionVaccinationCertificate>> GetAllHealthConditionVaccinationCertificatesAsync(CancellationToken cancellationToken);
        Task<HealthConditionVaccinationCertificate> GetHealthConditionVaccinationCertificateByIdAsync(int id, CancellationToken cancellationToken);
        Task<HealthConditionVaccinationCertificate> AddHealthConditionVaccinationCertificateAsync(HealthConditionVaccinationCertificate healthConditionVaccinationCertificate, CancellationToken cancellationToken);
        Task<HealthConditionVaccinationCertificate> UpdateHealthConditionVaccinationCertificateAsync(HealthConditionVaccinationCertificate healthConditionVaccinationCertificate, CancellationToken cancellationToken);
        Task<bool> DeleteHealthConditionVaccinationCertificateAsync(int id, CancellationToken cancellationToken);
        Task<List<HealthConditionVaccinationCertificate>> GetHealthConditionVaccinationCertificatesByHealthConditionIdAsync(int healthConditionId, CancellationToken cancellationToken);
    }
}
