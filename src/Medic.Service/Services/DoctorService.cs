using AutoMapper;
using Medic.Domain.Entities;
using Medic.Service.Exceptions;
using Medic.Service.Interfaces;
using Medic.Service.DTOs.Doctors;
using Medic.DataAccess.UnitOfWorks;

namespace Medic.Service.Services;

public class DoctorService : IDoctorService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public DoctorService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<bool> DeleteAsync(long id)
    {
        var doctor = await _unitOfWork.DoctorRepository.SelectAsync(q => q.Id == id);

        if (doctor is null)
            throw new NotFoundException("Doctor not found");

        await _unitOfWork.DoctorRepository.DeleteAsync(x => x == doctor);
        return await _unitOfWork.SaveAsync();
    }

    public async Task<DoctorResultDto> GetByIdAsync(long id)
    {
        var doctor = await _unitOfWork.DoctorRepository
            .SelectAsync(q => q.Id == id, includes: new string[] { "Hospital" });

        if (doctor is null)
            throw new NotFoundException("Doctor not found");

        return _mapper.Map<DoctorResultDto>(doctor);
    }

    public async Task<IEnumerable<DoctorResultDto>> GetAllAsync()
    {
        var doctors = _unitOfWork.DoctorRepository.SelectAll(includes: new string[] { "Hospital" });

        return _mapper.Map<IEnumerable<DoctorResultDto>>(doctors);
    }
    
    public async Task<IEnumerable<DoctorResultDto>> GetBySpecializationAsync(string specialization)
    {
        var doctors = _unitOfWork.DoctorRepository
            .SelectAll(d => d.Specialization.Contains(specialization), includes: new string[] { "Hospital" });

        return _mapper.Map<IEnumerable<DoctorResultDto>>(doctors);
    }

    public async Task<IEnumerable<DoctorResultDto>> GetByHospitalIdAsync(long hospitalId)
    {
        var doctors = _unitOfWork.DoctorRepository
            .SelectAll(d => d.HospitalId == hospitalId, includes: new string[] { "Hospital" });

        return _mapper.Map<IEnumerable<DoctorResultDto>>(doctors);
    }

    public async Task<IEnumerable<DoctorResultDto>> GetByNameAsync(string name)
    {
        var doctors = _unitOfWork.DoctorRepository.SelectAll(d =>
            d.FirstName.Contains(name) || d.LastName.Contains(name), includes: new string[] { "Hospital" });

        return _mapper.Map<IEnumerable<DoctorResultDto>>(doctors);
    }

    public async Task<DoctorResultDto> CreateAsync(DoctorCreationDto doctorCreationDto)
    {
        doctorCreationDto.Email = doctorCreationDto.Email.Trim().ToLower();
        var existingDoctor = await _unitOfWork.DoctorRepository.SelectAsync(q => q.Email == doctorCreationDto.Email, includes: new string[] { "Hospital" });
        if (existingDoctor is not null)
            throw new AlreadyExistException("Doctor already exists with this Email");

        var existDoctor = await _unitOfWork.DoctorRepository.SelectAsync(q => q.ContactNumber == doctorCreationDto.ContactNumber);
        if (existDoctor is not null)
            throw new AlreadyExistException("Doctor already exists with this Number");

        var hospital = await _unitOfWork.HospitalRepository.SelectAsync(x => doctorCreationDto.HospitalId == x.Id);
        if (hospital is null)
            throw new NotFoundException("Hospital not found with this ID");
        
        
        var newDoctor = _mapper.Map<Doctor>(doctorCreationDto);
        await _unitOfWork.DoctorRepository.AddAsync(newDoctor);
        await _unitOfWork.SaveAsync();

        return _mapper.Map<DoctorResultDto>(newDoctor);
    }


    public async Task<DoctorResultDto> UpdateAsync(DoctorUpdateDto doctorUpdateDto)
    {
        var existingDoctor = await _unitOfWork.DoctorRepository.SelectAsync(q => q.Id == doctorUpdateDto.Id);

        if (existingDoctor is null)
            throw new NotFoundException("Doctor not found");

        if (!string.Equals(existingDoctor.Email, doctorUpdateDto.Email, StringComparison.OrdinalIgnoreCase))
        {
            var emailAlreadyUsed = await _unitOfWork.DoctorRepository.SelectAsync(q => q.Email == doctorUpdateDto.Email);

            if (emailAlreadyUsed is not null)
                throw new AlreadyExistException("Email is already used by another doctor");
        }

        if (!string.Equals(existingDoctor.ContactNumber, doctorUpdateDto.ContactNumber, StringComparison.OrdinalIgnoreCase))
        {
            var contactNumberAlreadyUsed = await _unitOfWork.DoctorRepository.SelectAsync(q => q.ContactNumber == doctorUpdateDto.ContactNumber);

            if (contactNumberAlreadyUsed is not null)
                throw new AlreadyExistException("Contact number is already used by another doctor");
        }

        _mapper.Map(doctorUpdateDto, existingDoctor);
        await _unitOfWork.DoctorRepository.UpdateAsync(existingDoctor);
        await _unitOfWork.SaveAsync();

        return _mapper.Map<DoctorResultDto>(existingDoctor);
    }
} 