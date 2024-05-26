using Domain.DTOs.RoomDto;
using Domain.Filters;
using Domain.Responses;

namespace Infrastructure.Services.RoomService;

public interface IRoomService
{
    Task<Response<string>> AddRoomAsync(AddRoomDto addRoomDto);
    Task<Response<string>> UpdateRoomAsync(UpdateRoomDto updateRoomDto);
    Task<Response<bool>> DeleteRoomAsync(int id );
    Task<Response<GetRoomDto>> GetRoomByIdAsync(int id);
    Task<PagedResponse<List<GetRoomDto>>> GetRoomsAsync(RoomFilter filter);

}
