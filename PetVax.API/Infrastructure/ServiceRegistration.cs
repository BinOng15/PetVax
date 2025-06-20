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
            services.AddScoped<IPetPassportRepository, PetPassportRepository>();
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
            services.AddScoped<IVaccineProfileDiseaseRepository, VaccineProfileDiseaseRepository>();


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
            services.AddHostedService<VetScheduleBackgroundService>();

            #endregion

            return services;
        }
    }
}
