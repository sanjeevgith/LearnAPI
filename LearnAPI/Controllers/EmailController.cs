using LearnAPI.Helper;
using LearnAPI.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LearnAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService emailService;

        public EmailController(IEmailService emailService)
        {
            this.emailService = emailService;
        }


        [HttpGet("SendMail")]

        public async Task<IActionResult> Sendmail()
        {
            try
            {
                MailRequest mailRequest = new MailRequest();
                mailRequest.ToEmail = "prajapatisanjiv8@gmail.com";
                mailRequest.Subject = "test";
                mailRequest.Body = "thank you";
                await emailService.SendEmailAsync(mailRequest);
                return Ok(mailRequest);
            }
            catch (Exception ex)
            {
                throw;
            }
           
        }
    }
}
