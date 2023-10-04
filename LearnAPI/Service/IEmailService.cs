using LearnAPI.Helper;

namespace LearnAPI.Service
{
    public interface IEmailService
    {
        Task SendEmailAsync(MailRequest mailRequest);
    }
}
