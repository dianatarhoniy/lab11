namespace Clinic.Domain.Entities;

public class Patient
{
    public int IdPatient { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName  { get; set; } = null!;
    public DateTime BirthDate { get; set; }

    public ICollection<Prescription> Prescriptions { get; set; } = [];
}
