using mongodb.Models;

namespace mongodb.Repository.Interface
{
    public interface ILogin
    {
        Task<string> LoginStudent(Login login);
    }
}
