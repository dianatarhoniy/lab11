namespace Clinic.Application.DTOs;

public record PrescriptionRequestDto(
    PatientDto Patient,
    int        DoctorId,
    ICollection<PrescriptionMedicamentDto> Medicaments,
    DateTime   Date,
    DateTime   DueDate);

public record PatientDto(
    int?   IdPatient,
    string FirstName,
    string LastName,
    DateTime BirthDate);

public record PrescriptionMedicamentDto(
    int    MedicamentId,
    int    Dose,
    string? Details);

public record PrescriptionResponseDto(
    int      IdPrescription,
    DateTime Date,
    DateTime DueDate,
    DoctorDto Doctor,
    IEnumerable<PrescriptionMedicamentFlatDto> Medicaments);

public record DoctorDto(int IdDoctor, string FirstName, string LastName, string Email);

public record PrescriptionMedicamentFlatDto(
    int IdMedicament,
    string Name,
    int Dose,
    string? Details);

public record PatientDetailsDto(
    int IdPatient,
    string FirstName,
    string LastName,
    DateTime BirthDate,
    IEnumerable<PrescriptionResponseDto> Prescriptions);
