using System.ComponentModel.DataAnnotations;

namespace Domain.DTOs.AuthDto;

public class LoginDto
{
    public required string UserName { get; set; }   
    [DataType(DataType.Password)]
    public required string Password { get; set; }
    
}
