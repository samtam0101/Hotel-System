namespace Domain.Filters;

public class PaymentFilter:PaginationFilter
{
    public string? Status { get; set; }
}
