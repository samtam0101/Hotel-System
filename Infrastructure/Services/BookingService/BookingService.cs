using System.Net;
using AutoMapper;
using Domain.DTOs.BookingDto;
using Domain.Entities;
using Domain.Filters;
using Domain.Responses;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services.BookingService;

public class BookingService(DataContext context, ILogger<BookingService> logger, IMapper mapper):IBookingService
{
    public async Task<Response<string>> AddBookingAsync(AddBookingDto addBookingDto)
    {
        try
        {
            logger.LogInformation("AddBooking method started at {DateTime}", DateTime.Now);
            var existing = await context.Bookings.AnyAsync(e => e.Id == addBookingDto.UserId);
            if (existing) return new Response<string>(HttpStatusCode.BadRequest, "User already exists!");
            var mapped = mapper.Map<Booking>(addBookingDto);
            await context.Bookings.AddAsync(mapped);
            await context.SaveChangesAsync();
            logger.LogInformation("AddBooking method finished at {DateTime}", DateTime.Now);
            return new Response<string>("Added successfully!");
        }
        catch (Exception ex)
        {
            logger.LogError("Error in code, {DateTime}", DateTime.Now);
            return new Response<string>(HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    public async Task<Response<bool>> DeleteBookingAsync(int id)
    {
        try
        {
            logger.LogInformation("AddBooking method started at {DateTime}", DateTime.Now);
            var existing = await context.Bookings.Where(e => e.Id == id).ExecuteDeleteAsync();
            if (existing == 0)
            {
                logger.LogWarning("Booking not found {Id} at {DateTime}", id, DateTime.Now);
                return new Response<bool>(HttpStatusCode.BadRequest, "Booking not found!");
            }
            logger.LogInformation("AddBooking method finished at {DateTime}", DateTime.Now);
            return new Response<bool>(true);
        }
        catch (Exception ex)
        {
            logger.LogError("Error in code, {DateTime}", DateTime.Now);
            return new Response<bool>(HttpStatusCode.InternalServerError, ex.Message);
        }
    }

    public async Task<PagedResponse<List<GetBookingDto>>> GetBookingsAsync(BookingFilter filter)
    {
        try
        {
            logger.LogInformation("AddBooking method started at {DateTime}", DateTime.Now);

            var bookings = context.Bookings.AsQueryable();
            if (!string.IsNullOrEmpty(filter.Status))
                bookings = bookings.Where(x => x.Status.ToLower().Contains(filter.Status.ToLower()));
            var result = await bookings.Skip((filter.PageNumber - 1) * filter.PageSize).Take(filter.PageSize).ToListAsync();
            var total = await bookings.CountAsync();

            var response = mapper.Map<List<GetBookingDto>>(result);
            logger.LogInformation("AddBooking method finished at {DateTime}", DateTime.Now);
            return new PagedResponse<List<GetBookingDto>>(response, total, filter.PageNumber, filter.PageSize);
        }
        catch (Exception e)
        {
            logger.LogError("Error in code, {DateTime}", DateTime.Now);
            return new PagedResponse<List<GetBookingDto>>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Response<GetBookingDto>> GetBookingByIdAsync(int id)
    {
        try
        {
            logger.LogInformation("AddBooking method started at {DateTime}", DateTime.Now);

            var existing = await context.Bookings.FirstOrDefaultAsync(x => x.Id == id);
            if (existing == null)
            {
                logger.LogWarning("Booking not found {Id} at {DateTime}", id, DateTime.Now);
                return new Response<GetBookingDto>(HttpStatusCode.BadRequest, "Booking not found");
            }
            var Bookings = mapper.Map<GetBookingDto>(existing);
            logger.LogInformation("AddBooking method finished at {DateTime}", DateTime.Now);
            return new Response<GetBookingDto>(Bookings);
        }
        catch (Exception e)
        {
            logger.LogError("Error in code, {DateTime}", DateTime.Now);
            return new Response<GetBookingDto>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Response<string>> UpdateBookingAsync(UpdateBookingDto updateBookingDto)
    {
        try
        {
            logger.LogInformation("AddBooking method started at {DateTime}", DateTime.Now);

            var existing = await context.Bookings.AnyAsync(e => e.Id == updateBookingDto.Id);
            if (!existing) return new Response<string>(HttpStatusCode.BadRequest, "Booking not found!");
            var mapped = mapper.Map<Booking>(updateBookingDto);
            context.Bookings.Update(mapped);
            await context.SaveChangesAsync();
            logger.LogInformation("AddBooking method finished at {DateTime}", DateTime.Now);
            return new Response<string>("Updated successfully");
        }
        catch (Exception ex)
        {
            logger.LogError("Error in code, {DateTime}", DateTime.Now);
            return new Response<string>(HttpStatusCode.InternalServerError, ex.Message);
        }
    }
}
