using Medic.Service.DTOs.Feedbacks;

namespace Medic.Service.Interfaces;

public interface IFeedbackService
{
    Task<bool> DeleteAsync(long id);
    Task<FeedbackResultDto> GetByIdAsync(long id);
    Task<IEnumerable<FeedbackResultDto>> GetAllAsync();
    Task<IEnumerable<FeedbackResultDto>> GetByUserAsync(long userId);
    Task<IEnumerable<FeedbackResultDto>> GetByDoctorAsync(long doctorId);
    Task<FeedbackResultDto> CreateAsync(FeedbackCreationDto feedbackCreationDto);
}