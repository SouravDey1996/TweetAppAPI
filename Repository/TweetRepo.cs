using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using TweetAppAPI.Models;

namespace TweetAppAPI.Repository
{
    public class TweetRepo : ITweetRepo
    {
        private IUserRepo _userRepo;
        private IMongoCollection<Tweet> _tweets;

        public TweetRepo(IMongoClient client, IUserRepo userRepo)
        {
            var database = client.GetDatabase("TweetDB");
            _tweets = database.GetCollection<Tweet>("Tweet");
            _userRepo = userRepo;
        }
        public List<Tweet> GetTweets()
        {
            return _tweets.Find(tweet => true).SortByDescending(s => s.CreatedOn).ToList();
        }
        public int PostTweet(Tweet tweet)
        {
            tweet.Id = Guid.NewGuid().ToString();
            var result = _userRepo.GetUserByLoginId(tweet.LoginId);
            if (result != null)
            {
                tweet.PostedBy = string.Format(result.FirstName + " " + result.LastName);
                _tweets.InsertOne(tweet);
                return 0;
            }
            else
                return 1;
        }

        public int PostReply(Reply reply)
        {
            reply.ReplyId = Guid.NewGuid().ToString();
            var result = _userRepo.GetUserByLoginId(reply.ReplyLoginId);
            if (result != null)
            {
                reply.RepliedBy = string.Format(result.FirstName + " " + result.LastName);

                var filter = Builders<Tweet>.Filter.Eq(e => e.Id, reply.TweetId);

                var update = Builders<Tweet>.Update.Push<Reply>(e => e.Replies, reply);

                _tweets.FindOneAndUpdate(filter, update);

                return 0;
            }
            else
                return 1;
        }
    }
}
