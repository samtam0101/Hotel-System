using Domain.DTOs.UserRoleDto;
using Domain.Filters;
using Domain.Responses;
using Infrastructure.Services.UserRoleService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;
[ApiController]
[Route("[controller]")]
[Authorize(Roles = "Admin")]
public class UserRoleController(IUserRoleService userroleService):ControllerBase
{
    [HttpGet("Get-UserRoles")]
    public async Task<PagedResponse<List<GetUserRoleDto>>> GetUserRolesAsync([FromQuery]PaginationFilter filter)
    {
        return await userroleService.GetUserRolesAsync(filter);
    }
    [HttpPost("Add-UserRole")]
    public async Task<Response<string>> AddUserRoleAsync(AddUserRoleDto addUserRoleDto)
    {
        return await userroleService.AddUserRoleAsync(addUserRoleDto);
    }
    [HttpPut("Update-UserRole")]
    public async Task<Response<string>> UpdateUserRoleAsync(UpdateUserRoleDto updateUserRoleDto)
    {
        return await userroleService.UpdateUserRoleAsync(updateUserRoleDto);
    }
    [HttpDelete("Delete-UserRole")]
    public async Task<Response<bool>> DeleteUserRoleAsync(int id)
    {
        return await userroleService.DeleteUserRoleAsync(id);
    }
    [HttpGet("Get-UserRoleById")]
    public async Task<Response<GetUserRoleDto>> GetUserRoleByIdAsync(int id)
    {
        return await userroleService.GetUserRoleByIdAsync(id);
    }
}
