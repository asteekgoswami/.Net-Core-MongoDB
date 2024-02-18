using mongodb.Models;

namespace mongodb.Repository.Interface
{
    public interface IRole
    {
        Task<IEnumerable<RoleModel>> GetAllRoles();
        Task<int> AddRole(RoleModel role);

        Task<RoleModel> GetUserById(int id);

        Task<RoleModel> DeleteRoleById(int id);
    }
}
