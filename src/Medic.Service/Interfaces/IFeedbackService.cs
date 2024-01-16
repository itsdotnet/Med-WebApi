using Medic.Service.DTOs.Feedbacks;

namespace Medic.Service.Interfaces;

public interface IFeedbackService
{
    Task<bool> DeleteAsync(int id);
    Task<FeedbackResultDto> GetByIdAsync(int id);
    Task<IEnumerable<FeedbackResultDto>> GetAllAsync();
    Task<IEnumerable<FeedbackResultDto>> GetByUserAsync(long userId);
    Task<IEnumerable<FeedbackResultDto>> GetByDoctorAsync(long doctorId);
    Task<FeedbackResultDto> CreateAsync(FeedbackCreationDto feedbackCreationDto);
}