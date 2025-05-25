using Clinic.Application.DTOs;
using Clinic.Application.Interfaces;
using Clinic.Application.Services;
using Clinic.Domain.Entities;
using Clinic.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Shouldly;
using Xunit;

namespace Clinic.Tests;

public class PrescriptionServiceTests
{
    private static AppDbContext BuildContext()
    {
        var opts = new DbContextOptionsBuilder<AppDbContext>()
                   .UseInMemoryDatabase(Guid.NewGuid().ToString())
                   .Options;
        var ctx = new AppDbContext(opts);

        ctx.Doctors.Add(new Doctor { IdDoctor = 1, FirstName = "Alan", LastName = "Smith", Email = "a@b.com" });
        ctx.Medicaments.AddRange(
            new Medicament { IdMedicament = 1, Name = "Aspirin", Description = "", Type = "TAB" },
            new Medicament { IdMedicament = 2, Name = "Ibuprofen", Description = "", Type = "TAB" });
        ctx.SaveChanges();
        return ctx;
    }

    [Fact]
    public async Task Adds_new_patient_if_needed_and_returns_id()
    {
        await using var ctx = BuildContext();
        IPrescriptionService svc = new PrescriptionService(ctx);

        var dto = new PrescriptionRequestDto(
            new PatientDto(null, "John", "Doe", new DateTime(1980, 1, 1)),
            DoctorId: 1,
            Medicaments: new[]
            {
                new PrescriptionMedicamentDto(1, Dose: 1, Details: "once a day")
            },
            Date: DateTime.Today,
            DueDate: DateTime.Today.AddDays(7));

        var id = await svc.AddPrescriptionAsync(dto);

        id.ShouldBeGreaterThan(0);
        ctx.Patients.Count().ShouldBe(1);
        ctx.Prescriptions.Include(p => p.PrescriptionMedicaments).Single()
            .PrescriptionMedicaments.Count.ShouldBe(1);
    }

    [Fact]
    public async Task Fails_when_medicament_missing()
    {
        await using var ctx = BuildContext();
        IPrescriptionService svc = new PrescriptionService(ctx);

        var dto = new PrescriptionRequestDto(
            new PatientDto(null, "Ann", "M", new DateTime(1990, 1, 1)),
            DoctorId: 1,
            Medicaments: new[] { new PrescriptionMedicamentDto(42, 1, null) },
            Date: DateTime.Today,
            DueDate: DateTime.Today.AddDays(1));

        await Should.ThrowAsync<ArgumentException>(() => svc.AddPrescriptionAsync(dto));
    }
}
