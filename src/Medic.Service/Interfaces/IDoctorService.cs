using Medic.Service.DTOs.Doctors;

namespace Medic.Service.Interfaces;

public interface IDoctorService
{
    Task<bool> DeleteAsync(long id);
    Task<DoctorResultDto> GetByIdAsync(long id);
    Task<IEnumerable<DoctorResultDto>> GetAllAsync();
    Task<IEnumerable<DoctorResultDto>> GetBySpecializationAsync(string specialization);
    Task<IEnumerable<DoctorResultDto>> GetByHospitalIdAsync(long hospitalId);
    Task<IEnumerable<DoctorResultDto>> GetByNameAsync(string name);
    Task<DoctorResultDto> CreateAsync(DoctorCreationDto DoctorResultDto);
    Task<DoctorResultDto> UpdateAsync(DoctorUpdateDto DoctorResultDto);
}