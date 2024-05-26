using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Entities;

public class UserRole
{
    [Key]
    public int Id { get; set; }
    [ForeignKey("UserId")]
    public int UserId { get; set; }
    [ForeignKey("RoleId")]
    public int RoleId { get; set; }
    public Role Role { get; set; }
    public User User { get; set; }
}
