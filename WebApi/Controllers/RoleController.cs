using Domain.DTOs.RoleDto;
using Domain.Filters;
using Domain.Responses;
using Infrastructure.Services.RoleService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;
[ApiController]
[Route("[controller]")]
[Authorize(Roles = "Admin")]
public class RoleController(IRoleService roleService):ControllerBase
{
    [HttpGet("Get-Roles")]
    public async Task<PagedResponse<List<GetRoleDto>>> GetRolesAsync([FromQuery]RoleFilter filter)
    {
        return await roleService.GetRolesAsync(filter);
    }
    [HttpPost("Add-Role")]
    public async Task<Response<string>> AddRoleAsync(AddRoleDto addRoleDto)
    {
        return await roleService.AddRoleAsync(addRoleDto);
    }
    [HttpPut("Update-Role")]
    public async Task<Response<string>> UpdateRoleAsync(UpdateRoleDto updateRoleDto)
    {
        return await roleService.UpdateRoleAsync(updateRoleDto);
    }
    [HttpGet("Get-RoleById")]
    public async Task<Response<GetRoleDto>>GetRoleByIdAsync(int id)
    {
        return await roleService.GetRoleByIdAsync(id);
    }
    [HttpDelete("Delete-Role")]
    public async Task<Response<bool>> DeleteRoleAsync(int id)
    {
        return await roleService.DeleteRoleAsync(id);
    }
}
