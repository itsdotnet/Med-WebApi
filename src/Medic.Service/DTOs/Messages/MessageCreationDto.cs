namespace Medic.Service.DTOs.Messages;

public class MessageCreationDto
{
    public long UserId { get; set; }
    public long DoctorId { get; set; }
    
    public string Message { get; set; }
}

public class MessageResultDto
{
    public long User { get; set; }
    public long Doctor { get; set; }
    
    public string Message { get; set; }
}