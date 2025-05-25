using Clinic.Application.DTOs;
using Clinic.Application.Interfaces;
using Clinic.Domain.Entities;
using Clinic.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Clinic.Application.Services;

public class PrescriptionService(AppDbContext db) : IPrescriptionService
{
    public async Task<int> AddPrescriptionAsync(PrescriptionRequestDto dto, CancellationToken ct = default)
    {
        if (dto.Medicaments.Count == 0 || dto.Medicaments.Count > 10)
            throw new ArgumentException("A prescription must contain from 1 to 10 medicaments.");

        if (dto.DueDate < dto.Date)
            throw new ArgumentException("DueDate must be later than or equal to Date.");

        // patient create or fetch
        Patient patient;
        if (dto.Patient.IdPatient is { } pid &&
            await db.Patients.FindAsync([pid], ct) is { } existing)
        {
            patient = existing;
        }
        else
        {
            patient = new Patient
            {
                FirstName = dto.Patient.FirstName,
                LastName  = dto.Patient.LastName,
                BirthDate = dto.Patient.BirthDate
            };
            db.Patients.Add(patient);
        }

        // doctor must exist
        var doctor = await db.Doctors.FindAsync([dto.DoctorId], ct)
                     ?? throw new ArgumentException("Doctor not found.");

        // medicaments must exist
        var meds = await db.Medicaments
            .Where(m => dto.Medicaments.Select(x => x.MedicamentId).Contains(m.IdMedicament))
            .ToListAsync(ct);

        if (meds.Count != dto.Medicaments.Count)
            throw new ArgumentException("One or more medicaments do not exist.");

        var prescription = new Prescription
        {
            Date     = dto.Date,
            DueDate  = dto.DueDate,
            Patient  = patient,
            Doctor   = doctor,
            PrescriptionMedicaments =
                dto.Medicaments.Select(pm => new PrescriptionMedicament
                {
                    Medicament = meds.Single(m => m.IdMedicament == pm.MedicamentId),
                    Dose       = pm.Dose,
                    Details    = pm.Details
                }).ToList()
        };

        db.Prescriptions.Add(prescription);
        await db.SaveChangesAsync(ct);

        return prescription.IdPrescription;
    }
}
