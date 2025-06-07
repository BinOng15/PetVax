using PetVax.Repositories.IRepository;
using PetVax.Repositories.Repository;
using PetVax.Services.Configurations.Mapper;
using PetVax.Services.IService;
using PetVax.Services.Service;

namespace PediVax.Infrastructure
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


            //Register services
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IAccountService, AccountService>();
            #endregion

            return services;
        }
    }
}
