using Microsoft.AspNetCore.Http;

namespace Domain.DTOs.RoomDto;

public class AddRoomDto
{
    public string RoomNumber { get; set; }
    public string? Description { get; set; }
    public string Type { get; set; }
    public decimal PricePerNight { get; set; }
    public string Status { get; set; }
    public IFormFile? Photo { get; set; }
}
