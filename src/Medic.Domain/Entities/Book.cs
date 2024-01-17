using Medic.Domain.Commons;

namespace Medic.Domain.Entities;

public class Book : Auditable
{
    public long UserId { get; set; }
    public User User { get; set; }
    
    public long DoctorId { get; set; }
    public Doctor Doctor { get; set; }
    
    public DateTime From { get; set; }
    public DateTime To { get; set; }
    
    public bool IsActive { get; set; }
}