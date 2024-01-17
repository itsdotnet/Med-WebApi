namespace Medic.Service.DTOs.Bookings;

public class BookResultDto
{
    public long Id { get; set; }
    
    public long UserId { get; set; }
    
    public long DoctorId { get; set; }
    
    public DateTime From { get; set; }
    public DateTime To { get; set; }
    
    public bool IsActive { get; set; }
}