namespace Medic.Service.DTOs.Feedbacks;

public class FeedbackCreationDto
{
    public int Rating { get; set; }
    public string Text { get; set; }
    public long UserId { get; set; }
    public long DoctorId { get; set; }
}