using Medic.Service.DTOs.Doctors;
using Medic.Service.DTOs.Users;

namespace Medic.Service.DTOs.Feedbacks;

public class FeedbackResultDto
{
    public long Id { get; set; }
    public int Rating { get; set; }
    public string Text { get; set; }
    public UserResultDto User { get; set; }
    public DoctorResultDto Doctor { get; set; }
}