namespace Medic.Service.DTOs.Reports;

public class ReportUpdateDto
{
    public long Id { get; set; }
    
    public TimeOnly Today { get; set; }
    
    public string? Reason { get; set; }
        
    public bool IsAbsent { get; set; }
    
    public DateTime ArrivedAt { get; set; }
    public DateTime LeaveAt { get; set; }
}