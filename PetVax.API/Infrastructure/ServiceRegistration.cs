using Microsoft.Extensions.DependencyInjection;
using PediVax.BusinessObjects.DBContext;
using PetVax.Repositories.IRepository;
using PetVax.Repositories.Repository;
using PetVax.Services.Configurations.Mapper;
using PetVax.Services.ExternalService;
using PetVax.Services.IService;
using PetVax.Services.Service;

namespace PetVax.Infrastructure
{
    public static class ServiceRegistration
    {
        public static IServiceCollection Register(this IServiceCollection services)
        {
            // Configure AutoMapper
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddAutoMapper(typeof(MapperEntities).Assembly);

            #region DependencyInjection
            //Register repositories
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IAppointmentRepository, AppointmentRepository>();
            services.AddScoped<IAppointmentDetailRepository, AppointmentDetailRepository>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IDiseaseRepository, DiseaseRepository>();
            services.AddScoped<IHealthConditionRepository, HealthConditionRepository>();
            services.AddScoped<IMembershipRepository, MembershipRepository>();
            services.AddScoped<IMicrochipItemRepository, MicrochipItemRepository>();
            services.AddScoped<IMicrochipRepository, MicrochipRepository>();
            services.AddScoped<IVaccinationCertificateRepository, VaccinationCertificateRepository>();
            services.AddScoped<IPetRepository, PetRepository>();
            services.AddScoped<IVetRepository, VetRepository>();
            services.AddScoped<IVaccineRepository, VaccineRepository>();
            services.AddScoped<IVaccineBatchRepository, VaccineBatchRepository>();
            services.AddScoped<IVaccineDiseaseRepository, VaccineDiseaseRepository>();
            services.AddScoped<IVaccineProfileRepository, VaccineProfileRepository>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IVetRepository, VetRepository>();          
            services.AddScoped<IVetScheduleRepository, VetScheduleRepository>();
            services.AddScoped<IMicrochipRepository, MicrochipRepository>();
            services.AddScoped<IMicrochipItemRepository, MicrochipItemRepository>();
            services.AddScoped<IVaccinationScheduleRepository, VaccinationScheduleRepository>();
            services.AddScoped<IPaymentRepository, PaymentRepository>();
            services.AddScoped<IVaccinationCertificateRepository, VaccinationCertificateRepository>();
            services.AddScoped<IHealthConditionRepository, HealthConditionRepository>();
            services.AddScoped<IVaccinationScheduleRepository, VaccinationScheduleRepository>();
            services.AddScoped<IVaccineReceiptRepository, VaccineReceiptRepository>();
            services.AddScoped<IVaccineReceiptDetailRepository, VaccineReceiptDetailRepository>();
            services.AddScoped<IVaccineExportRepository, VaccineExportRepository>();
            services.AddScoped<IVaccineExportDetailRepository, VaccineExportDetailRepository>();
            services.AddScoped<IColdChainLogRepository, ColdChainLogRepository>();

            //Register services
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IVaccineService, VaccineService>();
            services.AddScoped<ICloudinariService, CloudinaryService>();
            services.AddScoped<IPetService, PetService>();
            services.AddScoped<IVetService, VetService>();
            services.AddScoped<IVetScheduleService, VetScheduleService>();
            services.AddScoped<IDiseaseService, DiseaseService>();
            services.AddScoped<IVaccineDiseaseService, VaccineDiseaseService>();
            services.AddScoped<IVaccineProfileService, VaccineProfileService>();
            services.AddScoped<IAppointmentService, AppointmentService>();
            services.AddScoped<IAppointmentDetailService, AppointmentDetailService>();
            services.AddScoped<IVetScheduleService, VetScheduleService>();
            services.AddScoped<IVetService, VetService>();
            services.AddScoped<IMicrochipService, MicrochipService>();
            services.AddScoped<IMicrochipItemService, MicrochipItemService>();
            services.AddScoped<IVaccineBatchService, VaccineBatchService>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IHealthConditionService, HealthConditionService>();
            services.AddScoped<IVaccinationScheduleService, VaccinationScheduleService>();
            services.AddScoped<IVaccineReceiptService, VaccineReceiptService>();
            services.AddScoped<IVaccineReceiptDetailService, VaccineReceiptDetailService>();

            //Register external services
            services.AddScoped<PayOsService>();
            services.AddScoped<PetVaxContext>();

            //Register background services
            services.AddHostedService<VetScheduleBackgroundService>();
            services.AddHostedService<AppointmentBackgroundService>();
            services.AddHostedService<AppointmentReminderBackgroundService>();
            

            #endregion

            return services;
        }
    }
}
