using Clinic.Application.DTOs;
using Clinic.Application.Interfaces;
using Clinic.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Clinic.Application.Services;

public class PatientService(AppDbContext db) : IPatientService
{
    public async Task<PatientDetailsDto> GetPatientAsync(int id, CancellationToken ct = default)
    {
        var data = await db.Patients
                     .Include(p => p.Prescriptions)
                     .ThenInclude(pr => pr.Doctor)
                     .Include(p => p.Prescriptions)
                     .ThenInclude(pr => pr.PrescriptionMedicaments)
                     .ThenInclude(pm => pm.Medicament)
                     .FirstOrDefaultAsync(p => p.IdPatient == id, ct)
                   ?? throw new KeyNotFoundException("Patient not found.");

        var response = new PatientDetailsDto(
            data.IdPatient,
            data.FirstName,
            data.LastName,
            data.BirthDate,
            data.Prescriptions
                .OrderBy(pr => pr.DueDate)
                .Select(pr => new PrescriptionResponseDto(
                    pr.IdPrescription,
                    pr.Date,
                    pr.DueDate,
                    new DoctorDto(pr.Doctor.IdDoctor, pr.Doctor.FirstName, pr.Doctor.LastName, pr.Doctor.Email),
                    pr.PrescriptionMedicaments.Select(pm => new PrescriptionMedicamentFlatDto(
                        pm.Medicament.IdMedicament,
                        pm.Medicament.Name,
                        pm.Dose,
                        pm.Details)))))
            ;

        return response;
    }
}
