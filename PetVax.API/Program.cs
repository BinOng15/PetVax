using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using CloudinaryDotNet;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Options;
using PetVax.Repositories.IRepository;
using PetVax.Repositories.Repository;
using PetVax.Services.Service;
using PetVax.Services.IService;
using PetVax.Infrastructure;

namespace PediVax;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container
        builder.Services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.Never;
            });

        builder.Services.AddEndpointsApiExplorer();

        // Swagger configuration
        builder.Services.AddSwaggerGen(option =>
        {
            option.DescribeAllParametersInCamelCase();
            option.ResolveConflictingActions(conf => conf.First());
            option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please enter a valid token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "Bearer"
            });
            option.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type=ReferenceType.SecurityScheme,
                            Id="Bearer"
                        }
                    },
                    new string[]{}
                }
            });
        });

        // Authentication & Authorization
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
                };
            })
            .AddGoogle(options =>
             {
                 options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
                 options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
                 options.CallbackPath = "/signin-google";
             });
        builder.Services.AddAuthorization();

        // Session Configuration
        builder.Services.AddDistributedMemoryCache();
        builder.Services.AddSession(options =>
        {
            options.IdleTimeout = TimeSpan.FromMinutes(30);
            options.Cookie.IsEssential = true;
        });

        // CORS
        const string AllowAllOrigins = nameof(AllowAllOrigins);
        builder.Services.AddCors(options =>
        {
            options.AddPolicy(AllowAllOrigins, policy =>
            {
                policy.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });

        builder.Services.Register();
        builder.Services.AddHttpContextAccessor();
        builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
        builder.Services.AddScoped<IVetRepository, VetRepository>();
        builder.Services.AddScoped<IVetService, VetService>();
        builder.Services.AddScoped<IVetScheduleRepository, VetScheduleRepository>();
        builder.Services.AddScoped<IVetScheduleService, VetScheduleService>();

        builder.Services.AddHostedService<VetScheduleBackgroundService>();
        builder.Services.AddHostedService<AppointmentBackgroundService>();
        builder.Services.AddHostedService<AppointmentReminderBackgroundService>();

        var app = builder.Build();

        // Enable Swagger only in Development
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "PetVax API V1");
            c.RoutePrefix = "swagger";
        });

        app.UseHttpsRedirection();
        app.UseCors(AllowAllOrigins);
        app.UseSession();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();
        app.Run();
    }
}
