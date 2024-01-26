using Medic.Domain.Enums;

namespace Medic.Service.DTOs.Doctors;

public class DoctorUpdateDto
{
    public long Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public Gender Gender { get; set; }
    public string Specialization { get; set; }
    public string Address { get; set; }
    public long HospitalId { get; set; }
    public string ContactNumber { get; set; }
    public string Email { get; set; }
}