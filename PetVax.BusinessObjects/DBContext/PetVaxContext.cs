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

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    if (!optionsBuilder.IsConfigured)
        //    {
        //        var configuration = new ConfigurationBuilder()
        //            .SetBasePath(Directory.GetCurrentDirectory())
        //            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
        //            .AddEnvironmentVariables()
        //            .Build();

        //        string connectionString = configuration.GetConnectionString("PostgreSQL");

        //        if (string.IsNullOrEmpty(connectionString))
        //        {
        //            throw new InvalidOperationException("Không thể lấy ConnectionString.");
        //        }

        //        optionsBuilder.UseNpgsql(connectionString, options =>
        //        {
        //            options.EnableRetryOnFailure();
        //        });

        //    }
        //}

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
        public DbSet<VaccinationCertificate> VaccinationCertificates { get; set; }
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
            //modelBuilder.HasDefaultSchema("public");
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
                .HasMany(p => p.VaccinationCertificates)
                .WithOne(pp => pp.Pet)
                .HasForeignKey(pp => pp.PetId)
                .OnDelete(DeleteBehavior.Restrict);

            // HealthCondition - Pet (N-1)
            modelBuilder.Entity<HealthCondition>()
                .HasOne(hc => hc.Pet)
                .WithMany(p => p.HealthConditions)
                .HasForeignKey(hc => hc.PetId)
                .OnDelete(DeleteBehavior.Restrict);

            // HealthCondition - Vet (N-1, optional)
            modelBuilder.Entity<HealthCondition>()
                .HasOne(hc => hc.Vet)
                .WithMany(v => v.HealthConditions)
                .HasForeignKey(hc => hc.VetId)
                .OnDelete(DeleteBehavior.Restrict);

            // HealthCondition - MicrochipItem (N-1, optional)
            modelBuilder.Entity<HealthCondition>()
                .HasOne(hc => hc.MicrochipItem)
                .WithMany()
                .HasForeignKey(hc => hc.MicrochipItemId)
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
                .HasOne(ad => ad.VaccinationCertificate)
                .WithOne()
                .HasForeignKey<AppointmentDetail>(ad => ad.VaccinationCertificateId)
                .OnDelete(DeleteBehavior.Restrict);

            // AppointmentDetail - HealthCondition (1-1)
            modelBuilder.Entity<AppointmentDetail>()
                .HasOne(ad => ad.HealthCondition)
                .WithOne()
                .HasForeignKey<AppointmentDetail>(ad => ad.HealthConditionId)
                .OnDelete(DeleteBehavior.Restrict);

            // AppointmentDetail - VaccineBatch (N-1)
            modelBuilder.Entity<AppointmentDetail>()
                .HasOne(ad => ad.VaccineBatch)
                .WithMany(vb => vb.AppointmentDetails)
                .HasForeignKey(ad => ad.VaccineBatchId)
                .OnDelete(DeleteBehavior.Restrict);

            //AppointmentDetail - Disease (1-N, optional)
            modelBuilder.Entity<AppointmentDetail>()
                .HasOne(ad => ad.Disease)
                .WithMany()
                .HasForeignKey(ad => ad.DiseaseId)
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

            // Payment - VaccineBatch (N-1)
            modelBuilder.Entity<Payment>()
                .HasOne(p => p.VaccineBatch)
                .WithMany()
                .HasForeignKey(p => p.VaccineBatchId)
                .OnDelete(DeleteBehavior.Restrict);

            // Payment - Microchip (N-1)
            modelBuilder.Entity<Payment>()
                .HasOne(p => p.Microchip)
                .WithMany()
                .HasForeignKey(p => p.MicrochipId)
                .OnDelete(DeleteBehavior.Restrict);

            // Payment - VaccinationCertificate (N-1)
            modelBuilder.Entity<Payment>()
                .HasOne(p => p.VaccinationCertificate)
                .WithMany()
                .HasForeignKey(p => p.VaccinationCertificateId)
                .OnDelete(DeleteBehavior.Restrict);

            // Payment - HealthCondition (N-1)
            modelBuilder.Entity<Payment>()
                .HasOne(p => p.HealthCondition)
                .WithMany()
                .HasForeignKey(p => p.HealthConditionId)
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

            modelBuilder.Entity<VaccinationSchedule>()
                .HasOne(vs => vs.Disease)
                .WithMany(d => d.VaccinationSchedules)
                .HasForeignKey(vs => vs.DiseaseId)
                .OnDelete(DeleteBehavior.Restrict);

            // VaccinationSchedule - VaccineProfile (1-N)
            modelBuilder.Entity<VaccinationSchedule>()
                .HasMany(vs => vs.VaccineProfiles)
                .WithOne(vp => vp.VaccinationSchedule)
                .HasForeignKey(vp => vp.VaccinationScheduleId)
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

            // VaccineProfile - Pet (1-N)
            modelBuilder.Entity<VaccineProfile>()
                .HasOne(vp => vp.Pet)
                .WithMany(p => p.VaccineProfiles) // Thêm ICollection<VaccineProfile> VaccineProfiles vào class Pet nếu chưa có
                .HasForeignKey(vp => vp.PetId)
                .OnDelete(DeleteBehavior.Restrict);
            //modelBuilder.Entity<VaccineProfile>()
            //    .HasIndex(vp => new { vp.PetId, vp.DiseaseId })
            //    .IsUnique();

            modelBuilder.Entity<VaccineProfile>()
                .HasOne(vp => vp.AppointmentDetail)
                .WithMany(a => a.VaccineProfiles)
                .HasForeignKey(vp => vp.AppointmentDetailId)
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

            // VaccinationCertificate - MicrochipItem (N-1, optional)
            modelBuilder.Entity<VaccinationCertificate>()
                .HasOne(pp => pp.MicrochipItem)
                .WithMany()
                .HasForeignKey("MicrochipItemId")
                .OnDelete(DeleteBehavior.Restrict);

            // VaccinationCertificate - Vet (N-1, optional)
            modelBuilder.Entity<VaccinationCertificate>()
                .HasOne(pp => pp.Vet)
                .WithMany(v => v.VaccinationCertificates)
                .HasForeignKey(pp => pp.VetId)
                .OnDelete(DeleteBehavior.Restrict);

            // VaccinationCertificate - Disease (N-1, optional)
            modelBuilder.Entity<VaccinationCertificate>()
                .HasOne(pp => pp.Disease)
                .WithMany(d => d.VaccinationCertificates)
                .HasForeignKey(pp => pp.DiseaseId)
                .OnDelete(DeleteBehavior.Restrict);

            //HealthConditionVaccinationCertificate - HealthCondition (N-1)
            modelBuilder.Entity<HealthConditionVaccinationCertificate>()
                .HasOne(hcvc => hcvc.HealthCondition)
                .WithMany(hc => hc.HealthConditionVaccinationCertificates)
                .HasForeignKey(hcvc => hcvc.HealthConditionId)
                .OnDelete(DeleteBehavior.Restrict);

            //HealthConditionVaccinationCertificate - VaccinationCertificate (N-1)
            modelBuilder.Entity<HealthConditionVaccinationCertificate>()
                .HasOne(hcvc => hcvc.VaccinationCertificate)
                .WithMany(vc => vc.HealthConditionVaccinationCertificates)
                .HasForeignKey(hcvc => hcvc.VaccinationCertificateId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Microchip>()
                .Property(m => m.Price)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Payment>()
                .Property(p => p.Amount)
                .HasColumnType("decimal(18,2)");
            modelBuilder.Entity<VaccinationCertificate>()
                .Property(p => p.Price)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Vaccine>()
                .Property(v => v.Price)
                .HasColumnType("decimal(18,2)");
            modelBuilder.Entity<HealthCondition>()
                .Property(hc => hc.Price)
                .HasColumnType("decimal(18,2)");

            SeedData.Seed(modelBuilder);
        }
    }
}
