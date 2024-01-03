using Medic.Domain.Commons;

namespace Medic.Domain.Entities;

public class Feedback : Auditable
{
    public int Rating { get; set; }
    public string Text { get; set; }
    
    public long UserId { get; set; }
    public User User { get; set; }
    
    public long DoctorId { get; set; }
    public Doctor Doctor { get; set; }
}