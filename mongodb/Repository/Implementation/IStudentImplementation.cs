using Microsoft.Extensions.Options;
using mongodb.Models;
using mongodb.Repository.Interface;
using MongoDB.Bson;
using MongoDB.Driver;

namespace mongodb.Repository.Implementation
{
    public class IStudentImplementation : IStudent
    {
        private readonly IMongoCollection<Student> _students;

        public IStudentImplementation(IOptions<SchoolDatabaseSettings> schooldbsetting , IMongoClient client)
        {
            var database = client.GetDatabase(schooldbsetting.Value.DatabaseName);
            _students = database.GetCollection<Student>(schooldbsetting.Value.StudentsCollectionName);
            
        }

        public async Task<int> AddStudent(Student student)
        {
            int res = 0;
            if (student == null)
            {
                return 0;
            }
            else
            {
                student.Id = ObjectId.GenerateNewId().ToString();
                student.Password=BCrypt.Net.BCrypt.EnhancedHashPassword(student.Password);
                await _students.InsertOneAsync(student);
                res = 1;
                return res;
            }
        }

        public async Task<Student> DeleteStudentById(int id)
        {
            Student user = await _students.Find(x=> x.RollNo == id).FirstOrDefaultAsync();
            if (user == null) 
            {
                return null;
            }
            else
            {
                await _students.DeleteOneAsync(x=>x.RollNo == id);  
                return (Student)user;
            }
        }

        public async Task<IEnumerable<Student>> GetAllStudents()
        {
            var lis = await _students.Find(_ => true).ToListAsync();
            return lis;
        }

        public async  Task<Student> GetStudentById(int id)
        {
            if (id == null)
            {
                return null;
            }
            else
            {
                Student user = await _students.Find(x=>x.RollNo== id).FirstOrDefaultAsync();
                return (Student)user;
            }
        }
    }
}
