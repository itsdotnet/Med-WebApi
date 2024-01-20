using AutoMapper;
using Medic.Domain.Entities;
using Medic.Service.Exceptions;
using Medic.Service.Interfaces;
using Medic.Service.DTOs.Reports;
using Medic.DataAccess.UnitOfWorks;

namespace Medic.Service.Services
{
    public class ReportService : IReportService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ReportService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<bool> DeleteAsync(long id)
        {
            var report = await _unitOfWork.ReportRepository.SelectAsync(q => q.Id == id);

            if (report is null)
                throw new NotFoundException("Report not found");

            await _unitOfWork.ReportRepository.DeleteAsync(x => x == report);
            return await _unitOfWork.SaveAsync();
        }

        public async Task<ReportResultDto> GetByIdAsync(long id)
        {
            var report = await _unitOfWork.ReportRepository.SelectAsync(q => q.Id == id);

            if (report is null)
                throw new NotFoundException("Report not found");

            return _mapper.Map<ReportResultDto>(report);
        }

        public async Task<IEnumerable<ReportResultDto>> GetAllAsync()
        {
            var reports = _unitOfWork.ReportRepository.SelectAll();

            return _mapper.Map<IEnumerable<ReportResultDto>>(reports);
        }

        public async Task<IEnumerable<ReportResultDto>> GetByDateAsync(TimeOnly date)
        {
            var reports = _unitOfWork.ReportRepository.SelectAll(q => q.Today == date);

            return _mapper.Map<IEnumerable<ReportResultDto>>(reports);
        }

        public async Task<IEnumerable<ReportResultDto>> GetByDoctorAsync(long doctorId)
        {
            var existDoctor = await _unitOfWork.DoctorRepository
                .SelectAsync(x => doctorId == x.Id);
        
            if (existDoctor is null)
                throw new NotFoundException("Doctor not found with this ID");

            var reports = _unitOfWork.ReportRepository.SelectAll(q => q.DoctorId == doctorId);

            return _mapper.Map<IEnumerable<ReportResultDto>>(reports);
        }

        public async Task<IEnumerable<ReportResultDto>> GetByHospitalAsync(long hospitalId)
        {
            var existHospital = await _unitOfWork.HospitalRepository
                .SelectAsync(x => hospitalId == x.Id);
        
            if (existHospital is null)
                throw new NotFoundException("Hospital not found with this ID");

            var reports = _unitOfWork.ReportRepository.SelectAll(q => q.HospitalId == hospitalId);

            return _mapper.Map<IEnumerable<ReportResultDto>>(reports);
        }

        public async Task<ReportResultDto> CreateAsync(ReportCreationDto reportCreationDto)
        {
            var existHospital = await _unitOfWork.HospitalRepository
                .SelectAsync(x => reportCreationDto.HospitalId == x.Id);
        
            var existDoctor = await _unitOfWork.DoctorRepository
                .SelectAsync(x => reportCreationDto.DoctorId == x.Id);

            if (existHospital is null)
                throw new NotFoundException("Hospital not found with this ID");

            if (existDoctor is null)
                throw new NotFoundException("Doctor not found with this ID");

            var newReport = _mapper.Map<Report>(reportCreationDto);

            await _unitOfWork.ReportRepository.AddAsync(newReport);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<ReportResultDto>(newReport);
        }

        public async Task<ReportResultDto> UpdateAsync(ReportUpdateDto reportUpdateDto)
        {
            var existingReport = await _unitOfWork.ReportRepository.SelectAsync(q => q.Id == reportUpdateDto.Id);
            if (existingReport is null)
                throw new NotFoundException("Report not found");

            _mapper.Map(reportUpdateDto, existingReport);
            await _unitOfWork.ReportRepository.UpdateAsync(existingReport);
            await _unitOfWork.SaveAsync();

            return _mapper.Map<ReportResultDto>(existingReport);
        }
    }
}
