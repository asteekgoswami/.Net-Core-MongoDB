using mongodb.Models;

namespace mongodb.Repository.Interface
{
    public interface IStudent
    {
        Task<IEnumerable<Student>> GetAllStudents();
        Task<int> AddStudent(Student student);

        Task<Student> GetStudentById(int id);

        Task<Student> DeleteStudentById(int id);
    }
}
