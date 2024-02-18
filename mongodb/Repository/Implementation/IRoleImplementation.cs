using Microsoft.Extensions.Options;
using mongodb.Models;
using mongodb.Repository.Interface;
using MongoDB.Bson;
using MongoDB.Driver;

namespace mongodb.Repository.Implementation
{
    public class IRoleImplementation : IRole
    {
        private readonly IMongoCollection<RoleModel> _roles;
        private readonly IMongoDatabase database;
        public IRoleImplementation(IOptions<SchoolDatabaseSettings> schooldbsetting , IMongoClient client)
        {
             database = client.GetDatabase(schooldbsetting.Value.DatabaseName);
            _roles = database.GetCollection<RoleModel>(schooldbsetting.Value.RolesCollectionName);
            
        }

        public async Task<int> AddRole(RoleModel role)
        {
            try
            {
                if (role == null)
                {
                    return 0;
                }
                else
                {
                    role.Id = ObjectId.GenerateNewId().ToString();
                    await _roles.InsertOneAsync(role);
                    return 1;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public async Task<RoleModel> DeleteRoleById(int id)
        {
            if (id == null)
            {
                return null;
            }
            else
            {
                RoleModel user = await _roles.Find(x=>x.RollNo==id).FirstOrDefaultAsync();
                if(user == null)
                {
                    return null;
                }
                else
                {
                    await _roles.DeleteOneAsync(x=>x.RollNo==id);
                    return user;
                }
            }
        }

        public async Task<IEnumerable<RoleModel>> GetAllRoles()
        {
            var lis = await _roles.Find(_=> true).ToListAsync();
            return lis;

        }

        public async Task<RoleModel> GetUserById(int id)
        {
            if (id == 0)
            {
                return null;
            }
            else
            {
                var user = await _roles.Find(x=> x.RollNo== id).FirstOrDefaultAsync();
                return (RoleModel)user;
            }
        }
    }
}
