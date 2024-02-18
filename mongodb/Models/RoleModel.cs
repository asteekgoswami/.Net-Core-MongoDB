using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace mongodb.Models
{
    public class RoleModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [Key]
        public int RollNo { get; set; }
        public string Name { get; set; }

        public string Roles { get; set; } 
    }
}
