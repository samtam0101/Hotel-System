using Domain.DTOs.AuthDto;
using Domain.Responses;
using Infrastructure.Services.AuthService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;
[ApiController]
[Route("[controller]")]
public class AuthController(IAuthService authService):ControllerBase
{
    [HttpPost("Login")]
    public async Task<Response<string>> Login([FromBody] LoginDto loginDto)
    {
        return await authService.Login(loginDto);
    }
    [HttpPost("Register")]
    public async Task<Response<string>> Register([FromBody] RegisterDto registerDto)
    {
        return await authService.Register(registerDto);
    }
    [Authorize(Roles = "Guest")]
    [HttpPut("Change-Password")]
    public async Task<Response<string>> ChangePassword([FromBody] ChangePasswordDto changePasswordDto)
    {
        var userId = Convert.ToInt32(User.Claims.FirstOrDefault(c => c.Type == "sid")?.Value);
        var result = await authService.ChangePassword(changePasswordDto, userId!);
        return result;
    }
    [HttpDelete("Forgot-Password")]
    public async Task<Response<string>> ForgotPassword([FromBody] ForgotPasswordDto forgotPasswordDto)
    {
        return await authService.ForgotPassword(forgotPasswordDto);
    }
    [HttpDelete("Reset-Password")]
    public async Task<Response<string>> ResetPassword([FromBody] ResetPasswordDto resetPasswordDto)
    {
        return await authService.ResetPassword(resetPasswordDto);
    }
    [HttpDelete("Delete-Account")]
    public async Task<Response<string>> DeleteAccount()
    {
        var userId = Convert.ToInt32(User.Claims.FirstOrDefault(c => c.Type == "sid")?.Value);
        var result = await authService.DeleteAccount(userId!);
        return result;
    }
}
