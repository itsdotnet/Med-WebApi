using Medic.Domain.Commons;
using Medic.Domain.Enums;

namespace Medic.Domain.Entities;

public class Doctor : Auditable
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public Gender Gender { get; set; }
    public string Specialization { get; set; }
    public string Address { get; set; }
    public string ContactNumber { get; set; }
    public string Email { get; set; }
    public long HospitalId { get; set; }
    public Hospital Hospital { get; set; }
    
    public long AttachmentId { get; set; }
    public Attachment Attachment { get; set; } 
}