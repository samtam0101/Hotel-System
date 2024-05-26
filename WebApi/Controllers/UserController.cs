using Domain.DTOs.UserDto;
using Domain.Filters;
using Domain.Responses;
using Infrastructure.Services.UserService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;
[ApiController]
[Route("[controller]")]
[Authorize(Roles = "Admin")]
public class UserController(IUserService userService):ControllerBase
{
    [HttpGet("Get-Users")]
    public async Task<PagedResponse<List<GetUserDto>>> GetUsersAsync([FromQuery]UserFilter filter)
    {
        return await userService.GetUsersAsync(filter);
    }
    [HttpPut("Update-User")]
    public async Task<Response<string>> UpdateUserAsync(UpdateUserDto updateUserDto)
    {
        return await userService.UpdateUserAsync(updateUserDto);
    }
    [HttpGet("Get-UserById")]
    public async Task<Response<GetUserDto>>GetUserByIdAsync(int id)
    {
        return await userService.GetUserByIdAsync(id);
    }
}
