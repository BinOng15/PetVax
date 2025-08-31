using Microsoft.EntityFrameworkCore;
using PetVax.BusinessObjects.Models;
using PetVax.Repositories.IRepository;
using PetVax.Repositories.Repository.BaseResponse;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetVax.Repositories.Repository
{
    public class HealthConditionVaccinationCertificateRepository : GenericRepository<HealthConditionVaccinationCertificate>, IHealthConditionVaccinationCertificateRepository
    {

        public HealthConditionVaccinationCertificateRepository() : base()
        {
        }
        public async Task<HealthConditionVaccinationCertificate> AddHealthConditionVaccinationCertificateAsync(HealthConditionVaccinationCertificate healthConditionVaccinationCertificate, CancellationToken cancellationToken)
        {
            _context.Add(healthConditionVaccinationCertificate);
            await _context.SaveChangesAsync(cancellationToken);
            return healthConditionVaccinationCertificate;
        }

        public async Task<bool> DeleteHealthConditionVaccinationCertificateAsync(int id, CancellationToken cancellationToken)
        {
            return await DeleteAsync(id, cancellationToken);
        }

        public async Task<List<HealthConditionVaccinationCertificate>> GetAllHealthConditionVaccinationCertificatesAsync(CancellationToken cancellationToken)
        {
            return await _context.HealthConditionVaccinationCertificates
                .Include(hcvc => hcvc.HealthCondition)
                .Include(hcvc => hcvc.VaccinationCertificate)
                .OrderByDescending(v => v.CreatedAt)
                .ToListAsync(cancellationToken);
        }

        public Task<HealthConditionVaccinationCertificate> GetHealthConditionVaccinationCertificateByIdAsync(int id, CancellationToken cancellationToken)
        {
            return _context.HealthConditionVaccinationCertificates
                .Include(hcvc => hcvc.HealthCondition)
                .Include(hcvc => hcvc.VaccinationCertificate)
                .FirstOrDefaultAsync(hcvc => hcvc.HealthConditionVaccinationCertificateId == id, cancellationToken);
        }

        public Task<List<HealthConditionVaccinationCertificate>> GetHealthConditionVaccinationCertificatesByHealthConditionIdAsync(int healthConditionId, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<HealthConditionVaccinationCertificate> UpdateHealthConditionVaccinationCertificateAsync(HealthConditionVaccinationCertificate healthConditionVaccinationCertificate, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
