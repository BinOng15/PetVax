using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PetVax.BusinessObjects.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PediVax.BusinessObjects.DBContext
{
    public class PetVaxContext : DbContext
    {
        public PetVaxContext() { }
        public PetVaxContext(DbContextOptions<PetVaxContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var configuration = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                    .AddEnvironmentVariables()
                    .Build();

                string connectionString = configuration.GetConnectionString("DefaultConnection");

                if (string.IsNullOrEmpty(connectionString))
                {
                    throw new InvalidOperationException("Không thể lấy ConnectionString.");
                }

                optionsBuilder.UseSqlServer(connectionString);
            }
        }

        // DbSet properties
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<AppointmentDetail> AppointmentDetails { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Disease> Diseases { get; set; }
        public DbSet<HealthCondition> HealthConditions { get; set; }
        public DbSet<MicrochipItem> MicrochipItems { get; set; }
        public DbSet<Membership> Memberships { get; set; }
        public DbSet<Microchip> Microchips { get; set; }
        public DbSet<Pet> Pets { get; set; }
        public DbSet<PetPassport> PetPassports { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<PointTransaction> PointTransactions { get; set; }
        public DbSet<ServiceHistory> ServiceHistories { get; set; }
        public DbSet<VaccinationSchedule> VaccinationSchedules { get; set; }
        public DbSet<VaccineBatch> VaccineBatches { get; set; }
        public DbSet<Vaccine> Vaccines { get; set; }
        public DbSet<VaccineDisease> VaccineDiseases { get; set; }
        public DbSet<VaccineExport> VaccineExports { get; set; }
        public DbSet<VaccineExportDetail> VaccineExportDetails { get; set; }
        public DbSet<VaccineProfile> VaccineProfiles { get; set; }
        public DbSet<VaccineReceipt> VaccineReceipts { get; set; }
        public DbSet<VaccineReceiptDetail> VaccineReceiptDetails { get; set; }
        public DbSet<Vet> Vets { get; set; }
        public DbSet<VetSchedule> VetSchedules { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure relationships using Fluent API
            

            SeedData.Seed(modelBuilder);
        }
    }
}
