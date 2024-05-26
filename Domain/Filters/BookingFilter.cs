namespace Domain.Filters;

public class BookingFilter:PaginationFilter
{
    public string? Status { get; set; }
}
