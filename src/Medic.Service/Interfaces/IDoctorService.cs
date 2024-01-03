using Medic.Service.DTOs.Doctors;

namespace Medic.Service.Interfaces;

public interface IDoctorService
{
    Task<bool> DeleteAsync(int id);
    Task<DoctorResultDto> GetByIdAsync(int id);
    Task<IEnumerable<DoctorResultDto>> GetAllAsync();
    Task<DoctorResultDto> GetByLicenseNumberAsync(string licenseNumber);
    Task<IEnumerable<DoctorResultDto>> GetBySpecializationAsync(string specialization);
    Task<IEnumerable<DoctorResultDto>> GetByHospitalAsync(string hospital);
    Task<IEnumerable<DoctorResultDto>> GetByNameAsync(string name);
    Task<DoctorResultDto> CreateAsync(DoctorCreationDto DoctorResultDto);
    Task<DoctorResultDto> UpdateAsync(DoctorUpdateDto DoctorResultDto);
}