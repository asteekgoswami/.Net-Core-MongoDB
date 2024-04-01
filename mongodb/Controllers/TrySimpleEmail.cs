using AspNetCore.SimpleEmail;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace mongodb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrySimpleEmail : ControllerBase
    {

       

        [HttpPost("SendEmail")]
        public IActionResult SendEmail()
        {

            var email = new Mail();
            email.Sender = "asteekgoswami650@gmail.com";
            email.To = "aasteekgoswami@gmail.com";
            /*email.CC.Add("aasteekgoswami@gmail.com");*/
            email.CC.Add("asteek.goswami@tmotions.com");
            email.Subject = "Demo Email Sent via Asteek Goswami's ASP.NET Core Package";
            email.Body = "Hello," +
                "This email serves as a quick demonstration using Asteek Goswami's ASP.NET Core package for easy Email Sending feature" ;

            //sender email or app password for the authentication
            email.Credentials("asteekgoswami650@gmail.com", "hrmm mecm dxuy oech");

            //sending the email
            email.SendAsync();

            //just for getting the response 200 with message
            return Ok("Email Sent Successfully");
        }


    }
}
