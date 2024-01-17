using AutoMapper;
using Medic.DataAccess.UnitOfWorks;
using Medic.Domain.Entities;
using Medic.Service.DTOs.Hospitals;
using Medic.Service.Exceptions;
using Medic.Service.Interfaces;

namespace Medic.Service.Services;

public class HospitalService : IHospitalService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public HospitalService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<bool> DeleteAsync(long id)
    {
        var hospital = await _unitOfWork.HospitalRepository.SelectAsync(q => q.Id == id);

        if (hospital is null)
        {
            throw new NotFoundException("Hospital not found");
        }

        await _unitOfWork.HospitalRepository.DeleteAsync(x => x == hospital);
        return await _unitOfWork.SaveAsync();
    }

    public async Task<HospitalResultDto> GetByIdAsync(long id)
    {
        var hospital = await _unitOfWork.HospitalRepository.SelectAsync(q => q.Id == id);

        if (hospital is null)
            throw new NotFoundException("Hospital not found");

        return _mapper.Map<HospitalResultDto>(hospital);
    }

    public async Task<IEnumerable<HospitalResultDto>> GetAllAsync()
    {
        var hospitals = _unitOfWork.HospitalRepository.SelectAll();

        return _mapper.Map<IEnumerable<HospitalResultDto>>(hospitals);
    }

    public async Task<HospitalResultDto> GetByNameAsync(string name)
    {
        var hospital = await _unitOfWork.HospitalRepository.SelectAsync(q => q.Name == name);

        if (hospital is null)
            throw new NotFoundException("Hospital not found");

        return _mapper.Map<HospitalResultDto>(hospital);
    }

    public async Task<HospitalResultDto> CreateAsync(HospitalCreationDto hospitalCreationDto)
    {
        hospitalCreationDto.Name = hospitalCreationDto.Name.Trim().ToLower();

        var existingHospital = await _unitOfWork.HospitalRepository.SelectAsync(q => q.Name == hospitalCreationDto.Name);

        if (existingHospital is not null)
        {
            throw new AlreadyExistException("Hospital already exists with this name");
        }

        var newHospital = _mapper.Map<Hospital>(hospitalCreationDto);
        await _unitOfWork.HospitalRepository.AddAsync(newHospital);
        await _unitOfWork.SaveAsync();

        return _mapper.Map<HospitalResultDto>(newHospital);
    }

    public async Task<HospitalResultDto> UpdateAsync(HospitalUpdateDto hospitalUpdateDto)
    {
        var existingHospital = await _unitOfWork.HospitalRepository.SelectAsync(q => q.Id == hospitalUpdateDto.Id);
        if (existingHospital is null)
            throw new NotFoundException("Hospital not found");

        if (!string.Equals(existingHospital.Name, hospitalUpdateDto.Name, StringComparison.OrdinalIgnoreCase))
        {
            var nameAlreadyUsed = await _unitOfWork.HospitalRepository.SelectAsync(q => q.Name == hospitalUpdateDto.Name);

            if (nameAlreadyUsed is not null)
                throw new AlreadyExistException("Hospital already exists with this name");
        }

        _mapper.Map(hospitalUpdateDto, existingHospital);
        await _unitOfWork.HospitalRepository.UpdateAsync(existingHospital);
        await _unitOfWork.SaveAsync();

        return _mapper.Map<HospitalResultDto>(existingHospital);
    }
}
