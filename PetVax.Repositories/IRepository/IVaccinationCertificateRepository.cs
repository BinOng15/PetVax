using PetVax.BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetVax.Repositories.IRepository
{
    public interface IVaccinationCertificateRepository
    {
        Task<List<VaccinationCertificate>> GetAllVaccinationCertificatesAsync(CancellationToken cancellationToken);
        Task<VaccinationCertificate?> GetVaccinationCertificateByIdAsync(int id, CancellationToken cancellationToken);
        Task<VaccinationCertificate> AddVaccinationCertificateAsync(VaccinationCertificate vaccinationCertificate, CancellationToken cancellationToken);
        Task<VaccinationCertificate> UpdateVaccinationCertificateAsync(VaccinationCertificate vaccinationCertificate, CancellationToken cancellationToken);
        Task<bool> DeleteVaccinationCertificateAsync(int id, CancellationToken cancellationToken);
        Task<List<VaccinationCertificate>> GetVaccinationCertificatesByPetIdAsync(int petId, CancellationToken cancellationToken);
        Task<List<VaccinationCertificate>> GetVaccinationCertificateByMicrochipItemIdAsync(int microchipItemId, CancellationToken cancellationToken);
        Task<VaccinationCertificate> GetVaccinationCertificateByCertificateCodeAsync(string certificateCode, CancellationToken cancellationToken);
    }
}
