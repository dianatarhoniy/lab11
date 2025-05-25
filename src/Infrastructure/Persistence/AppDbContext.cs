using Clinic.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Clinic.Infrastructure.Persistence;

public class AppDbContext(DbContextOptions<AppDbContext> opts) : DbContext(opts)
{
    public DbSet<Patient>     Patients      => Set<Patient>();
    public DbSet<Doctor>      Doctors       => Set<Doctor>();
    public DbSet<Medicament>  Medicaments   => Set<Medicament>();
    public DbSet<Prescription> Prescriptions => Set<Prescription>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        b.Entity<Patient>().HasKey(p => p.IdPatient);
        b.Entity<Doctor>().HasKey(d => d.IdDoctor);
        b.Entity<Medicament>().HasKey(m => m.IdMedicament);

        b.Entity<Prescription>()
            .HasKey(p => p.IdPrescription);

        b.Entity<Prescription>()
            .Property(p => p.RowVersion)
            .IsRowVersion();

        b.Entity<PrescriptionMedicament>()
            .HasKey(pm => new { pm.IdPrescription, pm.IdMedicament });

        b.Entity<PrescriptionMedicament>()
            .HasOne(pm => pm.Prescription)
            .WithMany(p => p.PrescriptionMedicaments)
            .HasForeignKey(pm => pm.IdPrescription);

        b.Entity<PrescriptionMedicament>()
            .HasOne(pm => pm.Medicament)
            .WithMany(m => m.PrescriptionMedicaments)
            .HasForeignKey(pm => pm.IdMedicament);
    }
}
