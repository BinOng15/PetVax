using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PetVax.BusinessObjects.DBContext;
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

                optionsBuilder.UseSqlServer(connectionString, options =>
                {
                    options.EnableRetryOnFailure();
                });

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

            // Customer - Account (1-1)
            modelBuilder.Entity<Customer>()
                .HasOne(c => c.Account)
                .WithOne()
                .HasForeignKey<Customer>(c => c.AccountId)
                .OnDelete(DeleteBehavior.Cascade);

            // Customer - Membership (N-1)
            modelBuilder.Entity<Customer>()
                .HasOne(c => c.Membership)
                .WithMany()
                .HasForeignKey(c => c.MembershipId)
                .OnDelete(DeleteBehavior.Restrict);

            // Customer - Pet (1-N)
            modelBuilder.Entity<Customer>()
                .HasMany(c => c.Pets)
                .WithOne(p => p.Customer)
                .HasForeignKey(p => p.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            // Membership - Customer (1-1)
            modelBuilder.Entity<Customer>()
                .HasOne(c => c.Membership)
                .WithOne(m => m.Customer)
                .HasForeignKey<Customer>(c => c.MembershipId)
                .OnDelete(DeleteBehavior.Restrict);

            // Pet - MicrochipItem (1-N)
            modelBuilder.Entity<Pet>()
                .HasMany(p => p.MicrochipItems)
                .WithOne()
                .HasForeignKey(mi => mi.PetId)
                .OnDelete(DeleteBehavior.Restrict);

            // Pet - PetPassport (1-N)
            modelBuilder.Entity<Pet>()
                .HasMany(p => p.PetPassports)
                .WithOne(pp => pp.Pet)
                .HasForeignKey(pp => pp.PetId)
                .OnDelete(DeleteBehavior.Restrict);

            // HealthCondition - Pet (N-1)
            modelBuilder.Entity<HealthCondition>()
                .HasOne(hc => hc.Pet)
                .WithMany(p => p.HealthConditions) // Thêm ICollection<HealthCondition> HealthConditions vào Pet nếu cần
                .HasForeignKey(hc => hc.PetId)
                .OnDelete(DeleteBehavior.Restrict);

            // Appointment - Customer (N-1)
            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Customer)
                .WithMany()
                .HasForeignKey(a => a.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            // Appointment - Pet (N-1)
            modelBuilder.Entity<Appointment>()
                .HasOne(a => a.Pet)
                .WithMany(p => p.Appointments)
                .HasForeignKey(a => a.PetId)
                .OnDelete(DeleteBehavior.Restrict);

            // Appointment - AppointmentDetail (1-1)
            modelBuilder.Entity<Appointment>()
                .HasOne<AppointmentDetail>()
                .WithOne(ad => ad.Appointment)
                .HasForeignKey<AppointmentDetail>(ad => ad.AppointmentId)
                .OnDelete(DeleteBehavior.Restrict);

            // AppointmentDetail - Vet (N-1)
            modelBuilder.Entity<AppointmentDetail>()
                .HasOne(ad => ad.Vet)
                .WithMany(v => v.AppointmentDetails)
                .HasForeignKey(ad => ad.VetId)
                .OnDelete(DeleteBehavior.Restrict);

            // AppointmentDetail - MicrochipItem (1-1)
            modelBuilder.Entity<AppointmentDetail>()
                .HasOne(ad => ad.MicrochipItem)
                .WithOne()
                .HasForeignKey<AppointmentDetail>(ad => ad.MicrochipItemId)
                .OnDelete(DeleteBehavior.Restrict);

            // AppointmentDetail - PetPassport (1-1)
            modelBuilder.Entity<AppointmentDetail>()
                .HasOne(ad => ad.PetPassport)
                .WithOne()
                .HasForeignKey<AppointmentDetail>(ad => ad.PassportId)
                .OnDelete(DeleteBehavior.Restrict);

            // AppointmentDetail - HealthCondition (1-1)
            modelBuilder.Entity<AppointmentDetail>()
                .HasOne(ad => ad.HealthCondition)
                .WithOne()
                .HasForeignKey<AppointmentDetail>(ad => ad.HealthConditionId)
                .OnDelete(DeleteBehavior.Restrict);

            // AppointmentDetail - VaccineBatch (1-1)
            modelBuilder.Entity<AppointmentDetail>()
                .HasOne(ad => ad.VaccineBatch)
                .WithOne()
                .HasForeignKey<AppointmentDetail>(ad => ad.VaccineBatchId)
                .OnDelete(DeleteBehavior.Restrict);

            // ServiceHistory - AppointmentDetail (1-N)
            modelBuilder.Entity<ServiceHistory>()
                .HasMany(sh => sh.AppointmentDetails)
                .WithOne()
                .HasForeignKey("ServiceHistoryId")
                .OnDelete(DeleteBehavior.Restrict);

            // Payment - AppointmentDetail (1-1)
            modelBuilder.Entity<Payment>()
                .HasOne(p => p.AppointmentDetail)
                .WithOne()
                .HasForeignKey<Payment>(p => p.AppointmentDetailId)
                .OnDelete(DeleteBehavior.Restrict);

            // Payment - Customer (N-1)
            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Customer)
                .WithMany()
                .HasForeignKey(p => p.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            // Payment - Vaccine (N-1)
            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Vaccine)
                .WithMany(v => v.Payments) // Nếu Vaccine có ICollection<Payment>
                .HasForeignKey(p => p.VaccineId)
                .OnDelete(DeleteBehavior.Restrict);

            // PointTransaction - Customer (N-1)
            modelBuilder.Entity<PointTransaction>()
                .HasOne(pt => pt.Customer)
                .WithMany()
                .HasForeignKey(pt => pt.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            // MicrochipItem - Microchip (1-1)
            modelBuilder.Entity<MicrochipItem>()
                .HasOne(mi => mi.Microchip)
                .WithOne()
                .HasForeignKey<MicrochipItem>(mi => mi.MicrochipId)
                .OnDelete(DeleteBehavior.Restrict);

            // VaccineBatch - Vaccine (N-1)
            modelBuilder.Entity<VaccineBatch>()
                .HasOne(vb => vb.Vaccine)
                .WithMany(v => v.VaccineBatches)
                .HasForeignKey(vb => vb.VaccineId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<VaccineDisease>()
                .HasOne(vd => vd.Vaccine)
                .WithMany(v => v.VaccineDiseases)
                .HasForeignKey(vd => vd.VaccineId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<VaccineDisease>()
                .HasOne(vd => vd.Disease)
                .WithMany(d => d.VaccineDiseases)
                .HasForeignKey(vd => vd.DiseaseId)
                .OnDelete(DeleteBehavior.Restrict);

            // VaccineProfile - Disease (N-1)
            modelBuilder.Entity<VaccineProfile>()
                .HasOne(vp => vp.Disease)
                .WithMany(d => d.VaccineProfiles)
                .HasForeignKey(vp => vp.DiseaseId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<VaccinationSchedule>()
                .HasOne(vs => vs.Disease)
                .WithMany(d => d.VaccinationSchedules)
                .HasForeignKey(vs => vs.DiseaseId)
                .OnDelete(DeleteBehavior.Restrict);

            // VaccinationSchedule - VaccineProfile (1-N)
            modelBuilder.Entity<VaccinationSchedule>()
                .HasMany(vs => vs.VaccineProfiles)
                .WithOne()
                .HasForeignKey("VaccinationScheduleId")
                .OnDelete(DeleteBehavior.Restrict);

            // VaccineExportDetail - VaccineBatch (N-1, optional)
            modelBuilder.Entity<VaccineExportDetail>()
                .HasOne(ved => ved.VaccineBatch)
                .WithMany()
                .HasForeignKey("VaccineBatchId")
                .OnDelete(DeleteBehavior.Restrict);

            // VaccineExportDetail - VaccineExport (N-1)
            modelBuilder.Entity<VaccineExportDetail>()
                .HasOne(ved => ved.VaccineExport)
                .WithMany(ve => ve.VaccineExportDetails)
                .HasForeignKey(ved => ved.VaccineExportId)
                .OnDelete(DeleteBehavior.Restrict);

            // VaccineExportDetail - AppointmentDetail (1-1)
            modelBuilder.Entity<VaccineExportDetail>()
                .HasOne(ved => ved.AppointmentDetail)
                .WithOne()
                .HasForeignKey<VaccineExportDetail>(ved => ved.AppointmentDetailId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<VaccineReceiptDetail>()
                .HasOne(vrd => vrd.VaccineReceipt)
                .WithMany(vr => vr.VaccineReceiptDetails)
                .HasForeignKey(vrd => vrd.VaccineReceiptId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<VaccineReceiptDetail>()
                .HasOne(vrd => vrd.VaccineBatch)
                .WithMany()
                .HasForeignKey(vrd => vrd.VaccineBatchId)
                .OnDelete(DeleteBehavior.Restrict);

            // VaccineProfile - Pet (1-1)
            modelBuilder.Entity<VaccineProfile>()
                .HasOne(vp => vp.Pet)
                .WithOne()
                .HasForeignKey<VaccineProfile>(vp => vp.PetId)
                .OnDelete(DeleteBehavior.Restrict);

            // VaccineProfile - AppointmentDetail (N-1, optional)
            modelBuilder.Entity<AppointmentDetail>()
                .HasOne(a => a.VaccineProfile)
                .WithMany(vp => vp.AppointmentDetails)
                .HasForeignKey(a => a.VaccineProfileId)
                .OnDelete(DeleteBehavior.Restrict);

            // Vet - Account (1-1)
            modelBuilder.Entity<Vet>()
                .HasOne(v => v.Account)
                .WithOne()
                .HasForeignKey<Vet>(v => v.AccountId)
                .OnDelete(DeleteBehavior.Restrict);

            // Vet - VetSchedule (1-N)
            modelBuilder.Entity<Vet>()
                .HasMany(v => v.VetSchedules)
                .WithOne(vs => vs.Vet)
                .HasForeignKey(vs => vs.VetId)
                .OnDelete(DeleteBehavior.Restrict);

            // PetPassport - MicrochipItem (N-1, optional)
            modelBuilder.Entity<PetPassport>()
                .HasOne(pp => pp.MicrochipItem)
                .WithMany()
                .HasForeignKey("MicrochipItemId")
                .OnDelete(DeleteBehavior.Restrict);

            // PetPassport - HealthCondition (N-1, optional)
            modelBuilder.Entity<PetPassport>()
                .HasOne(pp => pp.HealthCondition)
                .WithMany()
                .HasForeignKey("HealthConditionId")
                .OnDelete(DeleteBehavior.Restrict);
            modelBuilder.Entity<Microchip>()
                .Property(m => m.Price)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Payment>()
                .Property(p => p.Amount)
                .HasColumnType("decimal(18,2)");
            modelBuilder.Entity<PetPassport>()
                .Property(p => p.Price)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Vaccine>()
                .Property(v => v.Price)
                .HasColumnType("decimal(18,2)");

            SeedData.Seed(modelBuilder);
        }
    }
}
