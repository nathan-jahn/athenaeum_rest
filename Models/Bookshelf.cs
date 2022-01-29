using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Athenaeum_REST_API.Models
{
    //data class which stores all values for a bookshelf
    public class Bookshelf
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("user_id")]
        public string UserID { get; set; }

        [BsonElement("name")]
        public string Name { get; set; }

        [BsonElement("books")]
        [BsonRepresentation(BsonType.ObjectId)]
        List<string> Books { get; set; }

    }
}
