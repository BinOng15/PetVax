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
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IVetRepository, VetRepository>();
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IVaccineRepository, VaccineRepository>();
            services.AddScoped<IDiseaseRepository, DiseaseRepository>();
            services.AddScoped<IVaccineBatchRepository, VaccineBatchRepository>();
            services.AddScoped<IVaccineDiseaseRepository, VaccineDiseaseRepository>();
            services.AddScoped<IPetPassportRepository, PetPassportRepository>();
            services.AddScoped<IPetRepository, PetRepository>();
            services.AddScoped<IVaccineProfileRepository, VaccineProfileRepository>();
            services.AddScoped<IAppointmentRepository, AppointmentRepository>();

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
            

            #endregion

            return services;
        }
    }
}
