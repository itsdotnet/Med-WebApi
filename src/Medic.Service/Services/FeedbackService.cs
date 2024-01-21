using AutoMapper;
using Medic.Domain.Entities;
using Medic.Service.Exceptions;
using Medic.Service.Interfaces;
using Medic.DataAccess.UnitOfWorks;
using Medic.Service.DTOs.Feedbacks;

namespace Medic.Service.Services;

public class FeedbackService : IFeedbackService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public FeedbackService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<bool> DeleteAsync(long id)
    {
        var feedback = await _unitOfWork.FeedbackRepository.SelectAsync(q => q.Id == id);

        if (feedback is null)
            throw new NotFoundException("Feedback not found");

        await _unitOfWork.FeedbackRepository.DeleteAsync(x => x == feedback);
        return await _unitOfWork.SaveAsync();
    }

    public async Task<FeedbackResultDto> GetByIdAsync(long id)
    {
        var feedback = await _unitOfWork.FeedbackRepository
            .SelectAsync(q => q.Id == id, includes: new string[] { "User", "Doctor"});

        if (feedback is null)
            throw new NotFoundException("Feedback not found");

        return _mapper.Map<FeedbackResultDto>(feedback);
    }

    public async Task<IEnumerable<FeedbackResultDto>> GetAllAsync()
    {
        var feedbacks = _unitOfWork.FeedbackRepository
            .SelectAll(includes: new string[] { "User", "Doctor"});

        return _mapper.Map<IEnumerable<FeedbackResultDto>>(feedbacks);
    }

    public async Task<IEnumerable<FeedbackResultDto>> GetByUserAsync(long userId)
    {
        var existUser = await _unitOfWork.DoctorRepository
            .SelectAsync(x => userId == x.Id);
        
        if (existUser is null)
            throw new NotFoundException("User not found with this ID");

        var feedbacks = _unitOfWork.FeedbackRepository
            .SelectAll(q => q.UserId == userId, new string[] { "User", "Doctor"});

        return _mapper.Map<IEnumerable<FeedbackResultDto>>(feedbacks);
    }

    public async Task<IEnumerable<FeedbackResultDto>> GetByDoctorAsync(long doctorId)
    {
        var existDoctor = await _unitOfWork.DoctorRepository
            .SelectAsync(x => doctorId == x.Id);
        
        if (existDoctor is null)
            throw new NotFoundException("Doctor not found with this ID");

        var feedbacks = _unitOfWork.FeedbackRepository
            .SelectAll(q => q.DoctorId == doctorId, includes: new string[] { "User", "Doctor"});
        return _mapper.Map<IEnumerable<FeedbackResultDto>>(feedbacks);
    }

    public async Task<FeedbackResultDto> CreateAsync(FeedbackCreationDto feedbackCreationDto)
    {
        var existDoctor = await _unitOfWork.DoctorRepository
            .SelectAsync(x => feedbackCreationDto.DoctorId == x.Id);
        var existUser = await _unitOfWork.UserRepository
            .SelectAsync(u => feedbackCreationDto.UserId == u.Id);

        if (existDoctor is null)
            throw new NotFoundException("Doctor not found with this ID");
        
        if (existUser is null)
            throw new NotFoundException("User not found with this ID");
        
        var newFeedback = _mapper.Map<Feedback>(feedbackCreationDto);
        await _unitOfWork.FeedbackRepository.AddAsync(newFeedback);
        await _unitOfWork.SaveAsync();
        return _mapper.Map<FeedbackResultDto>(newFeedback);
    }
}