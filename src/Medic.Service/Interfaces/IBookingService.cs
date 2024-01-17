using Medic.Service.DTOs.Bookings;

namespace Medic.Service.Interfaces;

public interface IBookingService
{
    Task<bool> DeleteAsync(long id);
    Task<BookResultDto> GetByIdAsync(long id);
    Task<IEnumerable<BookResultDto>> GetAllAsync();
    Task<IEnumerable<BookResultDto>> GetByUserAsync(long userId);
    Task<IEnumerable<BookResultDto>> GetByDoctorAsync(long doctorId);
    Task<IEnumerable<BookResultDto>> GetByDateRangeAsync(DateTime from, DateTime to);
    Task<BookResultDto> CreateAsync(BookCreationDto bookingCreationDto);
    Task<BookResultDto> UpdateAsync(BookUpdateDto bookingUpdateDto);
}