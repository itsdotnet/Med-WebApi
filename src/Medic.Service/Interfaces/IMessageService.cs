using Medic.Service.DTOs.Messages;

namespace Medic.Service.Interfaces
{
    public interface IMessageService
    {
        Task<bool> DeleteAsync(long id);
        Task<MessageResultDto> GetByIdAsync(long id);
        Task<IEnumerable<MessageResultDto>> GetAllAsync();
        Task<IEnumerable<MessageResultDto>> GetByUserAsync(long userId);
        Task<IEnumerable<MessageResultDto>> GetByDoctorAsync(long doctorId);
        Task<MessageResultDto> CreateAsync(MessageCreationDto messageCreationDto);
    }
}