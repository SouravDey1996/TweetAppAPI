using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TweetAppAPI.Models;

namespace TweetAppAPI.Repository
{
    public interface IUserRepo
    {
        public List<User> GetAllUsers();
        public User GetUserByLoginId(string loginId);
        public User GetUserDetails(string loginId);
        public User GetUserByEmailId(string email);
        public int LoginUser(string loginId, string password);
        public int RegisterUser(User user);
        public string SendOTP(string loginId);
        public int ResetPassword(string loginId, string password);
    }
}
