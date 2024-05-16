using AutoMapper;
using Medic.DataAccess.UnitOfWorks;
using Medic.Domain.Entities;
using Medic.Service.DTOs.Bookings;
using Medic.Service.Exceptions;
using Medic.Service.Interfaces;

namespace Medic.Service.Services;

public class BookingService : IBookingService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public BookingService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<bool> DeleteAsync(long id)
    {
        var booking = await _unitOfWork.BookRepository.SelectAsync(q => q.Id == id);

        if (booking is null)
        {
            throw new NotFoundException("Booking not found");
        }

        await _unitOfWork.BookRepository.DeleteAsync(x => x == booking);
        return await _unitOfWork.SaveAsync();
    }

    public async Task<BookResultDto> GetByIdAsync(long id)
    {
        var booking = await _unitOfWork.BookRepository.SelectAsync(q => q.Id == id, includes: new string[] {"Doctor", "User"}) ;

        if (booking is null)
        {
            throw new NotFoundException("Booking not found");
        }

        return _mapper.Map<BookResultDto>(booking);
    }

    public async Task<IEnumerable<BookResultDto>> GetAllAsync()
    {
        var bookings = _unitOfWork.BookRepository.SelectAll(includes: new string[] {"Doctor", "User"});

        return _mapper.Map<IEnumerable<BookResultDto>>(bookings);
    }

    public async Task<IEnumerable<BookResultDto>> GetByUserAsync(long userId)
    {
        var bookings = _unitOfWork.BookRepository.SelectAll(q => q.UserId == userId);

        return _mapper.Map<IEnumerable<BookResultDto>>(bookings);
    }

    public async Task<IEnumerable<BookResultDto>> GetByDoctorAsync(long doctorId)
    {
        var bookings = _unitOfWork.BookRepository.SelectAll(q => q.DoctorId == doctorId);

        return _mapper.Map<IEnumerable<BookResultDto>>(bookings);
    }

    public async Task<IEnumerable<BookResultDto>> GetByDateRangeAsync(DateTime from, DateTime to)
    {
        var bookings = _unitOfWork.BookRepository.SelectAll(q => q.From >= from && q.To <= to);

        return _mapper.Map<IEnumerable<BookResultDto>>(bookings);
    }

    public async Task<BookResultDto> CreateAsync(BookCreationDto bookingCreationDto)
    {
        var existBooking = await _unitOfWork.BookRepository.SelectAsync(x => x.DoctorId == bookingCreationDto.DoctorId);
        
        if (existBooking is not null)
        {
            if (existBooking.From > bookingCreationDto.From && existBooking.To < bookingCreationDto.From)
            {
                throw new AlreadyExistException("This time is already booked");
            }

            if (existBooking.From > bookingCreationDto.To && existBooking.To < bookingCreationDto.To)
            {
                throw new AlreadyExistException("This time is already booked");
            }
        }   
        
        var newBooking = _mapper.Map<Book>(bookingCreationDto);

        await _unitOfWork.BookRepository.AddAsync(newBooking);
        await _unitOfWork.SaveAsync();
        var createdBookingDto = _mapper.Map<BookResultDto>(newBooking);

        return createdBookingDto;
    }

    public async Task<BookResultDto> UpdateAsync(BookUpdateDto bookingUpdateDto)
    {
        var existingBooking = await _unitOfWork.BookRepository.SelectAsync(q => q.Id == bookingUpdateDto.Id);

        if (existingBooking is null)
        {
            throw new NotFoundException("Booking not found");
        }

        _mapper.Map(bookingUpdateDto, existingBooking);

        await _unitOfWork.BookRepository.UpdateAsync(existingBooking);
        await _unitOfWork.SaveAsync();
        return _mapper.Map<BookResultDto>(existingBooking);

    }
}
