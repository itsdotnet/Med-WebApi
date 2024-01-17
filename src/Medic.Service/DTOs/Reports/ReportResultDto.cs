using Medic.Service.DTOs.Doctors;
using Medic.Service.DTOs.Hospitals;

namespace Medic.Service.DTOs.Reports;

public class ReportResultDto
{
    public long Id { get; set; }

    public TimeOnly Today { get; set; }
    
    public string? Reason { get; set; }
        
    public bool IsAbsent { get; set; }
    
    public HospitalResultDto Hospital { get; set; }
    
    public DoctorResultDto Doctor { get; set; }
    
    public DateTime ArrivedAt { get; set; }
    public DateTime LeaveAt { get; set; }
}