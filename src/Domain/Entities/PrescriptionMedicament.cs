namespace Clinic.Domain.Entities;

public class PrescriptionMedicament
{
    public int IdMedicament   { get; set; }
    public Medicament Medicament { get; set; } = null!;

    public int IdPrescription { get; set; }
    public Prescription Prescription { get; set; } = null!;

    public int Dose         { get; set; }
    public string? Details  { get; set; }
}
