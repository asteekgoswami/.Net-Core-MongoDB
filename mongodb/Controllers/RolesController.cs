using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using mongodb.Models;
using mongodb.Repository.Interface;
using MongoDB.Driver;

namespace mongodb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RolesController : ControllerBase
    {
        private readonly IRole irole;
        public RolesController(IRole irole)
        {
            this.irole = irole;
        }

        [HttpGet("GetAllRoles")]
        public async Task<IActionResult> GetAllRoles()
        {
            var lis = await irole.GetAllRoles();
            if(lis == null)
            {
                return NotFound("Not able to fetch Role");
            }
            else
            {
                return Ok(lis);
            }
        }

        [HttpPost("AddRole")]
        public async Task<IActionResult> AddRole(RoleModel role)
        {
            var res = await irole.AddRole(role);
            if (res == null)
            {
                return Problem("fail to add role");
            }
            else
            {
                return Ok(role);
            }
        }
        [HttpGet("GetRoleByRollNo")]
        [Authorize(Roles ="monitor")]
        public async Task<IActionResult> GetRole(int id)
        {
            RoleModel  user = await irole.GetUserById(id);
            if (user == null)
            {
                return Problem("No Student Exist");
            }
            else
            {
                return Ok(user);
            }
        }

        [HttpDelete("DeleteRoleById")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            var user = await irole.DeleteRoleById(id);
            if(user == null)
            {
                return Problem("not able to delete");
            }
            else
            {
                return Ok(user);
            }
        }


    }
}
