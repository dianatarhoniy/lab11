using Clinic.Application.DTOs;

namespace Clinic.Application.Interfaces;

public interface IPrescriptionService
{
    Task<int> AddPrescriptionAsync(PrescriptionRequestDto dto, CancellationToken ct = default);
}

public interface IPatientService
{
    Task<PatientDetailsDto> GetPatientAsync(int id, CancellationToken ct = default);
}
