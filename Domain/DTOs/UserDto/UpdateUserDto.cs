namespace Domain.DTOs.UserDto;

public class UpdateUserDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public int RoleId { get; set; }
}
