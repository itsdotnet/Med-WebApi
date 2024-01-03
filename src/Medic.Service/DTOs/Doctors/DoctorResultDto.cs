using Medic.Domain.Enums;
using Medic.Service.DTOs.Attachments;

namespace Medic.Service.DTOs.Doctors;

public class DoctorResultDto
{
    public long Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public Gender Gender { get; set; }
    public string Specialization { get; set; }
    public string Address { get; set; }
    public string ContactNumber { get; set; }
    public string Email { get; set; }
    
    public AttachmentResultDto  AttachmentResultDto { get; set; }  
}