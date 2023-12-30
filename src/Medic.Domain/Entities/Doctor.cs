namespace Medic.Domain.Entities;

public class Doctor
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string Gender { get; set; }
    public string Specialization { get; set; }
    public string Address { get; set; }
    public string ContactNumber { get; set; }
    public string Email { get; set; }
    public string Hospital { get; set; }
}