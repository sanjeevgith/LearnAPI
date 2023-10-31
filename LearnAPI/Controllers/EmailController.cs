using LearnAPI.Helper;
using LearnAPI.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LearnAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        private readonly IEmailService emailService;

        public EmailController(IEmailService emailService)
        {
            this.emailService = emailService;
        }

        //[HttpGet("SendMail")]
        [HttpPost("SendMail")]
        public async Task<IActionResult> Sendmail(MailRequest mail)
        {
            try
            {
                MailRequest mailRequest = new MailRequest();
            /*  mailRequest.ToEmail = "prajapatisanjiv8@gmail.com";
                mailRequest.Subject = "test"; */
                mailRequest.ToEmail = mail.ToEmail;
                mailRequest.Subject = mail.Subject;
                mailRequest.Body = GetHtmlContent(mail.Body);
                await emailService.SendEmailAsync(mailRequest);
                return Ok(mailRequest);
            }
            catch (Exception ex)
            {
                throw;
            }
           
        }

        private string GetHtmlContent(string data)
        {
            string response = "<!DOCTYPE html>\r\n<html lang=\"en\">\r\n<head>\r\n    <meta charset=\"UTF-8\">\r\n    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">\r\n    <title>Document</title>\r\n    <link href=\"https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css\" rel=\"stylesheet\" integrity=\"sha384-9ndCyUaIbzAi2FUVXJi0CjmCapSmO7SnpJef0486qhLnuZ2cdeRhO02iuK6FUUVM\" crossorigin=\"anonymous\">\r\n</head>\r\n<body>\r\n    <script src=\"https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js\" integrity=\"sha384-geWF76RCwLtnZ8qwWowPQNguL3RmwHVBC9FhGdlKrxdiJJigb/j/68SIy3Te4Bkz\" crossorigin=\"anonymous\"></script>\r\n\r\n\r\n    <div class=\"container\">\r\n        <div class=\"card\" style=\"width: auto;border: none;background-color: rgb(160, 252, 252);\">\r\n            <div class=\"card-header\">\r\n                <h2 style=\"text-align: center;\">Welcome to the Blog!</h2><br/>\r\n                <h2 style=\"color: rgb(16, 64, 16);\">Hi</h2>\r\n            </div>\r\n            <div class=\"cart-body\" style=\"margin: 10px;\">\r\n              <h3>this is dummy mail formate.</h3><br>\r\n\r\n              <h4>Contact Us : prajapatisanjiv8@gmail.com</h4>\r\n\r\n            </div>\r\n\r\n             \r\n        </div>\r\n    </div>\r\n\r\n\r\n</body>\r\n</html>";
            response += data;
            return response;
        }
    }
}
