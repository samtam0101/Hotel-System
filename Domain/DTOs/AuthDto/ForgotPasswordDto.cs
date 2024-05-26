using System.ComponentModel.DataAnnotations;

namespace Domain.DTOs.AuthDto;

public class ForgotPasswordDto
{
    [DataType(DataType.EmailAddress)]
    public required string Email { get; set; }

}
