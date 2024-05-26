using Domain.DTOs.BookingDto;
using Domain.Filters;
using Domain.Responses;

namespace Infrastructure.Services.BookingService;

public interface IBookingService
{
    Task<Response<string>> AddBookingAsync(AddBookingDto addBookingDto);
    Task<Response<string>> UpdateBookingAsync(UpdateBookingDto updateBookingDto);
    Task<Response<bool>> DeleteBookingAsync(int id );
    Task<Response<GetBookingDto>> GetBookingByIdAsync(int id);
    Task<PagedResponse<List<GetBookingDto>>> GetBookingsAsync(BookingFilter filter);
}
