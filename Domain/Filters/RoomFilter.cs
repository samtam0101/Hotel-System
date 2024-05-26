namespace Domain.Filters;

public class RoomFilter:PaginationFilter
{
    public string? Description { get; set; }
    public string? Status { get; set; }
    public string? Type { get; set; }
}
