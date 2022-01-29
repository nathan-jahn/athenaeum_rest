using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Athenaeum_REST_API.Models
{
    //data class which stores all values for a book
    public class Book
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [StringLength(24, MinimumLength = 24)]
        public string Id { get; set; }

        [BsonElement("userID")]
        public string UserID { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("bookshelf_id")]
        public string BookshelfId { get; set; }

        [BsonElement("title")]
        public string Title { get; set; }

        [BsonElement("subtitle")]
        public string Subtitle { get; set; }

        [BsonElement("authors")]
        public List<string> Authors { get; set; }

        [BsonElement("publisher")]
        public string Publisher { get; set; }

        [BsonElement("published_date")]
        public DateTime PublishedDate { get; set; }

        [BsonElement("description")]
        public string Description { get; set; }

        [BsonElement("industry_identifiers")]
        public List<Dictionary<string, string>> IndustryIdentifiers { get; set; }

        [BsonElement("categories")]
        public List<string> Categories { get; set; }

        [BsonElement("images")]
        public List<Dictionary<string, string>> Images { get; set; }


        public override string ToString()
        {
            return Title + " " + Id;
        }
    }
}
