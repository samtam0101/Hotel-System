using Domain.DTOs.UserDto;
using Domain.Filters;
using Domain.Responses;

namespace Infrastructure.Services.UserService;

public interface IUserService
{
    Task<Response<string>> UpdateUserAsync(UpdateUserDto updateUserDto);
    Task<PagedResponse<List<GetUserDto>>> GetUsersAsync(UserFilter userFilter);
    Task<Response<GetUserDto>> GetUserByIdAsync(int id);
}
