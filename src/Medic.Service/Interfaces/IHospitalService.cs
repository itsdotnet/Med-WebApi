using Medic.Service.DTOs.Hospitals;

namespace Medic.Service.Interfaces;

public interface IHospitalService
{
    Task<bool> DeleteAsync(long id);
    Task<HospitalResultDto> GetByIdAsync(long id);
    Task<IEnumerable<HospitalResultDto>> GetAllAsync();
    Task<HospitalResultDto> GetByNameAsync(string name);
    Task<HospitalResultDto> CreateAsync(HospitalCreationDto hospitalCreationDto);
    Task<HospitalResultDto> UpdateAsync(HospitalUpdateDto hospitalUpdateDto);
}