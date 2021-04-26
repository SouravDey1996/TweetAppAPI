using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TweetAppAPI.Models
{
    [BsonIgnoreExtraElements]
    public class Reply
    {
        [BsonElement("replyId")]
        public string ReplyId { get; set; }
        [BsonElement("replyBody")]
        public string ReplyBody { get; set; }
        [BsonElement("replyTimestamp")]
        public string ReplyTimestamp { get; set; }
        [BsonElement("repliedBy")]
        public string RepliedBy { get; set; }
        [BsonElement("replyLoginId")]
        public string ReplyLoginId { get; set; }
        [BsonElement("tweetId")]
        public string TweetId { get; set; }
    }
}
