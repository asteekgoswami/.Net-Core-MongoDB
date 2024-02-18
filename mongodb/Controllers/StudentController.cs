using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using mongodb.Models;
using MongoDB.Bson;
using mongodb.Repository.Interface;

namespace mongodb.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        /*private readonly IMongoCollection<Student> _studentCollection;*/
        /*public StudentController(IOptions<SchoolDatabaseSettings> schoolDatabaseSettings, IMongoClient client)
        {
            var database = client.GetDatabase(schoolDatabaseSettings.Value.DatabaseName);
            _studentCollection = database.GetCollection<Student>(schoolDatabaseSettings.Value.StudentsCollectionName);
        }*/
        private readonly IStudent istudent;
        public StudentController(IStudent istudent)
        {
            this.istudent = istudent;
        }


        [HttpPost]
        public async Task<IActionResult> AddStudent(Student std)
        {
            var res = 0 ;
            res = await istudent.AddStudent(std);
            if (res == null)
            {
                return Problem("Unable to Add Student");
            }
            else
            {
                return Ok(std);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllStudents()
        {
            var lis = await istudent.GetAllStudents();
            if(lis== null)
            {
                return NotFound();
            }
            else
            {
                return Ok(lis);
            }

        }
        [HttpGet("GetStudentById")]
        public async Task<IActionResult> GetStudentById(int id)
        {
            Student user = await istudent.GetStudentById(id);
            if (user == null)
            {
                return Problem("No Student Exist");
            }
            else
            {
                return Ok(user);
            }
        }

        [HttpDelete("DeleteStudentById")]
        public async Task<IActionResult> DeleteStudentById(int RollNo)
        {
            if(RollNo==0)
            {
                return Problem("Roll No cant be null");
            }
            else
            {
                Student user = await istudent.DeleteStudentById(RollNo);
                if(user == null)
                {
                    return Problem("User cant exist");
                }
                else
                {
                    return Ok(user);
                }
            }
        }

    }
}
