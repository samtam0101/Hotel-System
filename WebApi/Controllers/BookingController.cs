using Domain.DTOs.BookingDto;
using Domain.Filters;
using Domain.Responses;
using Infrastructure.Services.BookingService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;
[ApiController]
[Route("[controller]")]
[Authorize(Roles = "Admin")]
public class BookingController(IBookingService bookingService):ControllerBase
{
    [HttpGet("Get-Bookings")]
    public async Task<PagedResponse<List<GetBookingDto>>> GetBookingsAsync([FromQuery]BookingFilter filter)
    {
        return await bookingService.GetBookingsAsync(filter);
    }
    [HttpPost("Add-Booking")]
    public async Task<Response<string>> AddBookingAsync(AddBookingDto addBookingDto)
    {
        return await bookingService.AddBookingAsync(addBookingDto);
    }
    [HttpPut("Update-Booking")]
    public async Task<Response<string>> UpdateBookingAsync(UpdateBookingDto updateBookingDto)
    {
        return await bookingService.UpdateBookingAsync(updateBookingDto);
    }
    [HttpGet("Get-BookingById")]
    public async Task<Response<GetBookingDto>>GetBookingByIdAsync(int id)
    {
        return await bookingService.GetBookingByIdAsync(id);
    }
    [HttpDelete("Delete-Booking")]
    public async Task<Response<bool>> DeleteBookingAsync(int id)
    {
        return await bookingService.DeleteBookingAsync(id);
    }
}
