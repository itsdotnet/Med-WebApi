using Medic.Domain.Commons;

namespace Medic.Domain.Entities;

public class Report : Auditable
{
    public TimeOnly Today { get; set; }
    
    public string? Reason { get; set; }
        
    public bool IsAbsent { get; set; }
    
    public long HospitalId { get; set; }
    public Hospital Hospital { get; set; }
    
    public long DoctorId { get; set; }
    public Doctor Doctor { get; set; }
    
    public DateTime ArrivedAt { get; set; }
    public DateTime LeaveAt { get; set; }
}
