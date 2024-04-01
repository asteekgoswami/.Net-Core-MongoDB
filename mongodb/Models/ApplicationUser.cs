using AspNetCore.Identity.MongoDbCore.Models;
using Microsoft.AspNetCore.Identity;

namespace mongodb.Models
{
    public class ApplicationUser : MongoIdentityUser<Guid>
    {
    }
}
