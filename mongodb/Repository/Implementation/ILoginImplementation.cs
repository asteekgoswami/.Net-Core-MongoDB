using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using mongodb.Models;
using mongodb.Repository.Interface;
using MongoDB.Driver;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace mongodb.Repository.Implementation
{
    public class ILoginImplementation : ILogin
    {
        private readonly IMongoCollection<Student> _students;
        private readonly IConfiguration configuration;
        private readonly IMongoCollection<RoleModel> roles;
        public ILoginImplementation(IOptions<SchoolDatabaseSettings> schooldbsetting, IMongoClient client, IConfiguration configuration)
        {
            var database = client.GetDatabase(schooldbsetting.Value.DatabaseName);
            _students = database.GetCollection<Student>(schooldbsetting.Value.StudentsCollectionName);
            database = client.GetDatabase(schooldbsetting.Value.DatabaseName);
            roles = database.GetCollection<RoleModel>(schooldbsetting.Value.RolesCollectionName);
            this.configuration = configuration;
        }
        public async  Task<string> LoginStudent(Login login)
        {
            if(login.Password != null)
            {
                Student user = await _students.Find(x=>x.RollNo == login.RollNo).FirstOrDefaultAsync();
                if(user != null)
                {
                    var res = BCrypt.Net.BCrypt.EnhancedVerify(login.Password, user.Password);
                    if (res == true)
                    {
                         return await CreateToken(login);
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        private async Task<string> CreateToken(Login login)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            RoleModel user = await roles.Find(x => x.RollNo == login.RollNo).FirstOrDefaultAsync();

            var claims = new[] {
                 new Claim(JwtRegisteredClaimNames.Sub, login.RollNo.ToString()),
                 new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                 new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
                 new Claim(ClaimTypes.Role,user.Roles)
                };

            var token = new JwtSecurityToken(
                configuration["Jwt:Issuer"],
                configuration["Jwt:Issuer"],
                claims,
                expires: DateTime.UtcNow.AddMinutes(1),
                signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
