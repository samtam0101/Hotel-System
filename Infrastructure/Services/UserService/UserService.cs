using System.Net;
using Domain.DTOs.UserDto;
using Domain.Filters;
using Domain.Responses;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services.UserService;

public class UserService(DataContext context, ILogger<UserService> logger) : IUserService
{
    public async Task<Response<GetUserDto>> GetUserByIdAsync(int id)
    {
        try
        {
            logger.LogInformation("Starting method {GetUserByIdAsync} in time:{DateTime} ", "GetUserByIdAsync",
                DateTimeOffset.UtcNow);
            var user = await context.Users.Select(x => new GetUserDto()
            {
                Email = x.Email,
                Name = x.Name,
                RegistrationDate = x.RegistrationDate,
                Id = x.Id,
            }).FirstOrDefaultAsync(x => x.Id == id);

            logger.LogInformation("Finished method {GetUserByIdAsync} in time:{DateTime} ", "GetUserByIdAsync",
                DateTimeOffset.UtcNow);
            return user == null
                ? new Response<GetUserDto>(HttpStatusCode.BadRequest, $"User not found by ID:{id}")
                : new Response<GetUserDto>(user);
        }
        catch (Exception e)
        {
            logger.LogError("Exception {Exception}, time={DateTimeNow}", e.Message, DateTimeOffset.UtcNow);
            return new Response<GetUserDto>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<PagedResponse<List<GetUserDto>>> GetUsersAsync(UserFilter filter)
    {
        try
        {
            logger.LogInformation("Starting method {GetUsersAsync} in time:{DateTime} ", "GetUsersAsync",
                DateTimeOffset.UtcNow);

            var users = context.Users.AsQueryable();

            if (!string.IsNullOrEmpty(filter.Name))
                users = users.Where(x => x.Name.ToLower().Contains(filter.Name.ToLower()));

            var response = await users.Select(x => new GetUserDto()
            {
                Email = x.Email,
                Id = x.Id,
                Name = x.Name,
                RegistrationDate = x.RegistrationDate

            }).Skip((filter.PageNumber - 1) * filter.PageSize).Take(filter.PageSize).ToListAsync();

            var totalRecord = await users.CountAsync();

            logger.LogInformation("Finished method {GetUsersAsync} in time:{DateTime} ", "GetUsersAsync",
                DateTimeOffset.UtcNow);
            return new PagedResponse<List<GetUserDto>>(response, filter.PageNumber, filter.PageSize, totalRecord);
        }
        catch (Exception e)
        {
            logger.LogError("Exception {Exception}, time={DateTimeNow}", e.Message, DateTimeOffset.UtcNow);
            return new PagedResponse<List<GetUserDto>>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Response<string>> UpdateUserAsync(UpdateUserDto updateUserDto)

    {
        try
        {
            logger.LogInformation("Starting method {UpdateUserAsync} in time:{DateTime} ", "UpdateUserAsync",
                DateTimeOffset.UtcNow);
            var existing = await context.Users.FirstOrDefaultAsync(x => x.Id == updateUserDto.Id);
            if (existing is null)
            {
                logger.LogWarning("User not found by id:{Id},time:{DateTimeNow} ", updateUserDto.Id,
                    DateTimeOffset.UtcNow);
                return new Response<string>(HttpStatusCode.BadRequest, "User not found");
            }
            existing.Id = updateUserDto.Id;
            existing.Name = updateUserDto.Name;
            existing.Email = updateUserDto.Email;

            await context.SaveChangesAsync();
            logger.LogInformation("Finished method {UpdateUserAsync} in time:{DateTime} ", "UpdateUserAsync",
                DateTimeOffset.UtcNow);
            return new Response<string>($"Successfully updated User by id:{existing.Id}");
        }
        catch (Exception e)
        {
            logger.LogError("Exception {Exception}, time={DateTimeNow}", e.Message, DateTimeOffset.UtcNow);
            return new Response<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }
}