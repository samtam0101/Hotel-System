namespace Domain.Entities;

public class Payment
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int BookingId { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public string Status { get; set; }
    public User User { get; set; }
    public Booking Booking { get; set; }
}
