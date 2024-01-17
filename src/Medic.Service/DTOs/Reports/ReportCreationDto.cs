namespace Medic.Service.DTOs.Reports;

public class ReportCreationDto
{
    public TimeOnly Today { get; set; }
    
    public string? Reason { get; set; }
        
    public bool IsAbsent { get; set; }
    
    public long HospitalId { get; set; }
    
    public long DoctorId { get; set; }
    
    public DateTime ArrivedAt { get; set; }
    public DateTime LeaveAt { get; set; }
}