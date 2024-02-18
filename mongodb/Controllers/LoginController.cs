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

        public LoginController(ILogin ilogin)
        {
            this.ilogin = ilogin;
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
    }
}
