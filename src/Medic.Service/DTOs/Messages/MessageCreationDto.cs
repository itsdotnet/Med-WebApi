namespace Medic.Service.DTOs.Messages;

public class MessageCreationDto
{
    public long UserId { get; set; }
    public long DoctorId { get; set; }
    
    public string Message { get; set; }
}