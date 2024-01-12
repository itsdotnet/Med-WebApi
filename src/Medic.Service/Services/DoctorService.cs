using Medic.Service.DTOs.Doctors;
using Medic.Service.Interfaces;

namespace Medic.Service.Services;

public class DoctorService : IDoctorService
{
    public Task<bool> DeleteAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<DoctorResultDto> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<DoctorResultDto>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<DoctorResultDto> GetByLicenseNumberAsync(string licenseNumber)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<DoctorResultDto>> GetBySpecializationAsync(string specialization)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<DoctorResultDto>> GetByHospitalAsync(string hospital)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<DoctorResultDto>> GetByNameAsync(string name)
    {
        throw new NotImplementedException();
    }

    public Task<DoctorResultDto> CreateAsync(DoctorCreationDto DoctorResultDto)
    {
        throw new NotImplementedException();
    }

    public Task<DoctorResultDto> UpdateAsync(DoctorUpdateDto DoctorResultDto)
    {
        throw new NotImplementedException();
    }
}