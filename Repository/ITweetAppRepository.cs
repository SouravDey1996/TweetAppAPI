using System.Collections.Generic;
using TweetAppAPI.Models;

namespace TweetAppAPI.Repository
{
    public interface ITweetAppRepository
    {
        public List<User> GetAllUsers();
        public User GetUserByLoginId(string loginId);
        public User GetUserDetails(string loginId);
        public User GetUserByEmailId(string email);
        public int LoginUser(string loginId, string password);
        public int RegisterUser(User user);
        public List<Tweet> GetTweets();
        public int PostTweet(Tweet tweet);
        public int PostReply(Reply reply);
        public string SendOTP(string loginId);
        public int ResetPassword(string loginId, string password);
    }
}
