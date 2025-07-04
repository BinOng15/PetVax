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
    public class VaccinationCertificateRepository : GenericRepository<VaccinationCertificate>, IVaccinationCertificateRepository
    {
        public VaccinationCertificateRepository() : base()
        {
        }

        public async Task<VaccinationCertificate> AddVaccinationCertificateAsync(VaccinationCertificate vaccinationCertificate, CancellationToken cancellationToken)
        {
            _context.Add(vaccinationCertificate);
            await _context.SaveChangesAsync(cancellationToken);
            return vaccinationCertificate;
        }

        public async Task<bool> DeleteVaccinationCertificateAsync(int id, CancellationToken cancellationToken)
        {
            return await DeleteAsync(id, cancellationToken);
        }

        public async Task<List<VaccinationCertificate>> GetAllVaccinationCertificatesAsync(CancellationToken cancellationToken)
        {
            return await _context.VaccinationCertificates
                .Include(pp => pp.Pet)
                .Include(pp => pp.MicrochipItem)
                .Include(pp => pp.Vet)
                .Include(pp => pp.Disease)
                .ToListAsync(cancellationToken);
        }

        public async Task<VaccinationCertificate?> GetVaccinationCertificateByIdAsync(int id, CancellationToken cancellationToken)
        {
            return await _context.VaccinationCertificates
                .Include(pp => pp.Pet)
                .Include(pp => pp.MicrochipItem)
                .Include(pp => pp.Vet)
                .Include(pp => pp.Disease)
                .FirstOrDefaultAsync(pp => pp.CertificateId == id, cancellationToken);
        }

        public async Task<List<VaccinationCertificate>> GetVaccinationCertificateByMicrochipItemIdAsync(int mmicrochipItemlId, CancellationToken cancellationToken)
        {
            return await _context.VaccinationCertificates
                .Where(pp => pp.MicrochipItemId == mmicrochipItemlId)
                .Include(pp => pp.Pet)
                .Include(pp => pp.MicrochipItem)
                .Include(pp => pp.Vet)
                .Include(pp => pp.Disease)
                .ToListAsync(cancellationToken);
        }

        public async Task<VaccinationCertificate> GetVaccinationCertificateByCertificateCodeAsync(string certificateCode, CancellationToken cancellationToken)
        {
            return await _context.VaccinationCertificates
                .Include(pp => pp.Pet)
                .Include(pp => pp.MicrochipItem)
                .Include(pp => pp.Vet)
                .Include(pp => pp.Disease)
                .FirstOrDefaultAsync(pp => pp.CertificateCode == certificateCode, cancellationToken);
        }

        public async Task<List<VaccinationCertificate>> GetVaccinationCertificatesByPetIdAsync(int petId, CancellationToken cancellationToken)
        {
            return await _context.VaccinationCertificates
                .Where(pp => pp.PetId == petId)
                .Include(pp => pp.Pet)
                .Include(pp => pp.MicrochipItem)
                .Include(pp => pp.Vet)
                .Include(pp => pp.Disease)
                .ToListAsync(cancellationToken);
        }

        public async Task<VaccinationCertificate> UpdateVaccinationCertificateAsync(VaccinationCertificate vaccinationCertificate, CancellationToken cancellationToken)
        {
            _context.Update(vaccinationCertificate);
            await _context.SaveChangesAsync(cancellationToken);
            return vaccinationCertificate;
        }
    }
}
