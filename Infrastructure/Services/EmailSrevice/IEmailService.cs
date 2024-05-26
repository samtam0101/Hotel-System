using Domain.DTOs.EmailDto;
using MimeKit.Text;

namespace Infrastructure.Services.EmailSrevice;

public interface IEmailService
{
    Task SendEmail(EmailMessageDto emailMessageDto, TextFormat textFormat);
}
