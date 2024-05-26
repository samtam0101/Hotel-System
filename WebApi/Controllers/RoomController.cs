using Domain.DTOs.RoomDto;
using Domain.Filters;
using Domain.Responses;
using Infrastructure.Services.RoomService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;
[ApiController]
[Route("[controller]")]
public class RoomController(IRoomService roomService) : ControllerBase
{
    [Authorize(Roles ="Guest")]
    [HttpGet("Get-Rooms")]
    public async Task<PagedResponse<List<GetRoomDto>>> GetRoomsAsync([FromQuery] RoomFilter filter)
    {
        return await roomService.GetRoomsAsync(filter);
    }
    [HttpGet("Get-RoomById")]
    public async Task<Response<GetRoomDto>> GetRoomByIdAsync(int id)
    {
        return await roomService.GetRoomByIdAsync(id);
    }
    [Authorize(Roles = "Admin")]
    [HttpPost("Add-Room")]
    public async Task<Response<string>> AddRoomAsync(AddRoomDto addRoomDto)
    {
        return await roomService.AddRoomAsync(addRoomDto);
    }
    [HttpPut("Update-Room")]
    public async Task<Response<string>> UpdateRoomAsync(UpdateRoomDto updateRoomDto)
    {
        return await roomService.UpdateRoomAsync(updateRoomDto);
    }

    [HttpDelete("Delete-Room")]
    public async Task<Response<bool>> DeleteRoomAsync(int id)
    {
        return await roomService.DeleteRoomAsync(id);
    }
}
