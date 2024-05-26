using System.Net;
using Domain.DTOs.RoomDto;
using Domain.Entities;
using Domain.Filters;
using Domain.Responses;
using Infrastructure.Data;
using Infrastructure.Services.FileService;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Infrastructure.Services.RoomService;

public class RoomService(IFileService fileService, ILogger<RoomService> logger, DataContext context) : IRoomService
{
    public async Task<Response<string>> AddRoomAsync(AddRoomDto addRoomDto)
    {
        try
        {
            logger.LogInformation("Method AddRoom started at {DateTime}", DateTime.UtcNow);
            var room = new Room()
            {
                RoomNumber = addRoomDto.RoomNumber,
                Description = addRoomDto.Description,
                Type = addRoomDto.Type,
                PricePerNight = addRoomDto.PricePerNight,
                Status = addRoomDto.Status,
                Photo = addRoomDto.Photo == null ? "null" : await fileService.CreateFile(addRoomDto.Photo)
            };
            await context.Rooms.AddAsync(room);
            await context.SaveChangesAsync();
            logger.LogInformation("Finished method AddRoom, Time:{DateTime}", DateTime.UtcNow);
            return new Response<string>($"Successfully craeted new room by id:{room.Id}");
        }
        catch (Exception e)
        {
            logger.LogError("Exception {Exception}, Time :{DateTime}", e.Message, DateTime.UtcNow);
            return new Response<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Response<bool>> DeleteRoomAsync(int id)
    {
        try
        {
            logger.LogInformation("Starting method {DeleteRoomAsync} in time:{DateTime} ", "DeleteRoomAsync",
                DateTimeOffset.UtcNow);
            var existing = await context.Rooms.FirstOrDefaultAsync(x => x.Id == id);
            if (existing is null)
            {
                logger.LogWarning("Could not found room, Time: {DateTime}", DateTime.UtcNow);
                return new Response<bool>(HttpStatusCode.BadRequest, $"Room was not found by this Id:{id}");
            }
            if (existing.Photo != null)
            {
                fileService.DeleteFile(existing.Photo);
            }
            context.Rooms.Remove(existing);
            await context.SaveChangesAsync();
            logger.LogInformation("Finished method {DeleteRoomAsync} in time:{DateTime} ", "DeleteRoomAsync",
                DateTimeOffset.UtcNow);
            return new Response<bool>(true);
        }
        catch (Exception e)
        {
            logger.LogError("Exception {Exception}, time={DateTimeNow}", e.Message, DateTimeOffset.UtcNow);
            return new PagedResponse<bool>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Response<GetRoomDto>> GetRoomByIdAsync(int id)
    {
        try
        {
            logger.LogInformation("Starting method {GetRoomByIdAsync} in time:{DateTime} ", "GetRoomByIdAsync",
                DateTimeOffset.UtcNow);
            var existing = await context.Rooms.Where(x => x.Id == id).Select(x => new GetRoomDto()
            {
                Description = x.Description,
                Id = x.Id,
                Photo = x.Photo,
                PricePerNight = x.PricePerNight,
                Type = x.Type,
                Status = x.Status,
                RoomNumber = x.RoomNumber
            }).FirstOrDefaultAsync();
            if (existing is null)
            {
                logger.LogWarning("Not found Room with id={Id},time={DateTimeNow}", id, DateTime.UtcNow);
                return new Response<GetRoomDto>(HttpStatusCode.BadRequest, "Room not found");
            }

            logger.LogInformation("Finished method {GetRoomByIdAsync} in time:{DateTime} ", "GetRoomByIdAsync",
                DateTimeOffset.UtcNow);
            return new Response<GetRoomDto>(existing);
        }
        catch (Exception e)
        {
            logger.LogError("Exception {Exception}, time={DateTimeNow}", e.Message, DateTimeOffset.UtcNow);
            return new Response<GetRoomDto>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<PagedResponse<List<GetRoomDto>>> GetRoomsAsync(RoomFilter filter)
    {
        try
        {
            logger.LogInformation("Starting method {GetRoomsAsync} in time:{DateTime} ", "GetRoomsAsync",
                DateTimeOffset.UtcNow);

            var rooms = context.Rooms.AsQueryable();
            if (!string.IsNullOrEmpty(filter.Description))
                rooms = rooms.Where(x => x.Description.ToLower().Contains(filter.Description.ToLower()));
            if (!string.IsNullOrEmpty(filter.Type))
                rooms = rooms.Where(x => x.Type.ToLower().Contains(filter.Type.ToLower()));
            if (!string.IsNullOrEmpty(filter.Status))
                rooms = rooms.Where(x => x.Status.ToLower().Contains(filter.Status.ToLower()));

            var response = await rooms.Select(x => new GetRoomDto()
            {
                Id = x.Id,
                Description = x.Description,
                RoomNumber = x.RoomNumber,
                Status = x.Status,
                Type = x.Type,
                PricePerNight = x.PricePerNight,
                Photo = x.Photo,
            }).Skip((filter.PageNumber - 1) * filter.PageSize).Take(filter.PageSize).ToListAsync();

            var totalRecord = await rooms.CountAsync();

            logger.LogInformation("Finished method {GetRoomsAsync} in time:{DateTime} ", "GetRoomsAsync",
                DateTimeOffset.UtcNow);

            return new PagedResponse<List<GetRoomDto>>(response, filter.PageNumber, filter.PageSize, totalRecord);
        }
        catch (Exception e)
        {
            logger.LogError("Exception {Exception}, time={DateTimeNow}", e.Message, DateTimeOffset.UtcNow);
            return new PagedResponse<List<GetRoomDto>>(HttpStatusCode.InternalServerError, e.Message);
        }
    }

    public async Task<Response<string>> UpdateRoomAsync(UpdateRoomDto updateRoomDto)
    {
        try
        {
            logger.LogInformation("Starting method {UpdateRoomAsync} in time:{DateTime} ", "UpdateRoomAsync",
                DateTimeOffset.UtcNow);
            var existing = await context.Rooms.FirstOrDefaultAsync(x => x.Id == updateRoomDto.Id);
            if (existing is null)
            {
                logger.LogWarning("Room not found by id:{Id},time:{DateTimeNow} ", updateRoomDto.Id,
                    DateTimeOffset.UtcNow);
                return new Response<string>(HttpStatusCode.BadRequest, "Room not found");
            }

            if (updateRoomDto.Photo != null)
            {
                if (existing.Photo != null) fileService.DeleteFile(existing.Photo);
                existing.Photo = await fileService.CreateFile(updateRoomDto.Photo);
            }
            existing.Id = updateRoomDto.Id;
            existing.RoomNumber = updateRoomDto.RoomNumber;
            existing.Description = updateRoomDto.Description;
            existing.Type = updateRoomDto.Type;
            existing.Status = updateRoomDto.Status;
            existing.PricePerNight = updateRoomDto.PricePerNight;

            await context.SaveChangesAsync();
            logger.LogInformation("Finished method {UpdateRoomAsync} in time:{DateTime} ", "UpdateRoomAsync",
                DateTimeOffset.UtcNow);
            return new Response<string>($"Successfully updated Room by id:{existing.Id}");
        }
        catch (Exception e)
        {
            logger.LogError("Exception {Exception}, time={DateTimeNow}", e.Message, DateTimeOffset.UtcNow);
            return new Response<string>(HttpStatusCode.InternalServerError, e.Message);
        }
    }
}
