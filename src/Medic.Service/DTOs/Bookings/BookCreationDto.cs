namespace Medic.Service.DTOs.Bookings;

public class BookCreationDto
{
    public long UserId { get; set; }
    
    public long DoctorId { get; set; }
    
    public DateTime From { get; set; }
    public DateTime To { get; set; }
}