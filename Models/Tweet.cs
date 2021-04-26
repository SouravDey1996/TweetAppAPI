using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TweetAppAPI.Models
{
    [BsonIgnoreExtraElements]
    public class Tweet
    {
        [BsonId]
        public string Id { get; set; }
        [BsonElement("createdOn")]
        public string CreatedOn { get; set; }
        [BsonElement("body")]
        public string Body { get; set; }
        [BsonElement("postedBy")]
        public string PostedBy { get; set; }
        [BsonElement("loginId")]
        public string LoginId { get; set; }
        [BsonElement("replies")]
        public List<Reply> Replies { get; set; }
    }
}
