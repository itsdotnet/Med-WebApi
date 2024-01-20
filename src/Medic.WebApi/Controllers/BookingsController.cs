using Medic.Service.DTOs.Bookings;
using Medic.Service.Interfaces;
using Medic.WebApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace Medic.WebApi.Controllers;

public class BookingsController : BaseController
{
    private readonly IBookingService _bookingService;

    public BookingsController(IBookingService bookingService)
    {
        _bookingService = bookingService;
    }

    [HttpGet("get-all")]
    public async Task<IActionResult> GetAllBookings()
    {
        var bookings = await _bookingService.GetAllAsync();
        return Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = bookings
        });
    }

    [HttpGet("get/{id}")]
    public async Task<IActionResult> GetBookingById(long id)
    {
        var booking = await _bookingService.GetByIdAsync(id);
        return Ok(new Response
        {
            StatusCode = 200,
            Message = "Success",
            Data = booking
        });
    }

    [HttpPost("create")]
    public async Task<IActionResult> CreateBooking([FromBody] BookCreationDto bookingCreationDto)
    {
        var createdBooking = await _bookingService.CreateAsync(bookingCreationDto);
        return Ok(new Response
        {
            StatusCode = 201,
            Message = "Booking created successfully",
            Data = createdBooking
        });
    }

    [HttpPut("update")]
    public async Task<IActionResult> UpdateBooking([FromBody] BookUpdateDto bookingUpdateDto)
    {
        var updatedBooking = await _bookingService.UpdateAsync(bookingUpdateDto);
        return Ok(new Response
        {
            StatusCode = 200,
            Message = "Booking updated successfully",
            Data = updatedBooking
        });
    }

    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> DeleteBooking(long id)
    {
        var result = await _bookingService.DeleteAsync(id);
        if (result)
        {
            return Ok(new Response
            {
                StatusCode = 200,
                Message = "Booking deleted successfully"
            });
        }
        return NotFound(new Response
        {
            StatusCode = 404,
            Message = "Booking not found"
        });
    }
}