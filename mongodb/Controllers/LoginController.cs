using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using mongodb.Models;
using mongodb.Repository.Interface;

namespace mongodb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILogin ilogin;
        private readonly IEmail iemail;

        public LoginController(ILogin ilogin, IEmail iemail)
        {
            this.ilogin = ilogin;
            this.iemail = iemail;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> LoginStudent(Login login)
        {
            if (login == null)
            {
                return BadRequest("Enter the credentials");
            }
            else
            {
                var res = await ilogin.LoginStudent(login);
                if(res == null)
                {
                    return Problem("Invalid credentials");
                }
                else
                {
                    return Ok(res);
                }
            }
        }
        [HttpPost("Send Email")]
        public async Task<IActionResult> SendEmail(MailRequest mailrequest)
        {
            var res = await iemail.SendEmailAsync(mailrequest);
            if (res == 0)
            {
                return Problem("Fail to send mail");
            }
            else
            {
                return Ok("Email sent successfully");
            }

        }
    }
}
