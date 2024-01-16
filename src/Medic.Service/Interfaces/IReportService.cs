using Medic.Service.DTOs.Reports;

namespace Medic.Service.Interfaces;

public interface IReportService
{
    Task<bool> DeleteAsync(long id);
    Task<ReportResultDto> GetByIdAsync(long id);
    Task<IEnumerable<ReportResultDto>> GetAllAsync();
    Task<IEnumerable<ReportResultDto>> GetByDateAsync(TimeOnly date);
    Task<IEnumerable<ReportResultDto>> GetByDoctorAsync(long doctorId);
    Task<IEnumerable<ReportResultDto>> GetByHospitalAsync(long hospitalId);
    Task<ReportResultDto> CreateAsync(ReportCreationDto reportCreationDto);
    Task<ReportResultDto> UpdateAsync(ReportUpdateDto reportUpdateDto);
}