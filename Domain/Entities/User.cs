using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class User
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public DateTime RegistrationDate { get; set; }
    public string? Code { get; set; }
    public DateTimeOffset CodeTime { get; set; }
    public List<UserRole> UserRoles { get; set; }
    public List<Booking> Bookings { get; set; }
    public List<Payment> Payments { get; set; }
}