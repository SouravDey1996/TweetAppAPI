
using System;
using System.Collections.Generic;
using System.Linq;

using System.Threading.Tasks;
using TweetAppAPI.Models;
using TweetAppAPI.Services;
using MongoDB.Driver;

namespace TweetAppAPI.Repository
{
    public class UserRepo : IUserRepo
    {
        private IMongoCollection<User> _users;
        
        private readonly Email _email;
        public UserRepo(IMongoClient client, Email email)
        {
            var database = client.GetDatabase("TweetDB");
            _users = database.GetCollection<User>("User_Details");
            _email = email;
        }
        

        public List<User> GetAllUsers()
        {
            return _users.Find(user => true).ToList();
        }

        public User GetUserByLoginId(string loginId)
        {
            return _users.Find(user => user.LoginId == loginId).FirstOrDefault();
        }
        public User GetUserByEmailId(string email)
        {
            return _users.Find(user => user.Email == email).FirstOrDefault();
        }
        public User GetUserDetails(string loginId)
        {
            var result = _users.AsQueryable().Where(s => s.LoginId == loginId).Select(s => new { s.FirstName, s.LastName, s.Email }).FirstOrDefault();
            if (result != null)
            {
                return new User
                {
                    FirstName = result.FirstName,
                    LastName = result.LastName,
                    Email = result.Email
                };
            }
            else
                return null;
        }
        public int LoginUser(string loginId, string password)
        {
            User isExists = GetUserByLoginId(loginId);
            if (isExists != null)
            {
                if (isExists.Password == password)
                    return 0;
                else
                    return 2;
            }
            else
                return 1;
        }

        public int RegisterUser(User user)
        {
            User isExists = GetUserByLoginId(user.LoginId);
            if (isExists != null)
            {
                return 1;
            }
            isExists = GetUserByEmailId(user.Email);
            if (isExists != null)
            {
                return 2;
            }
            else
            {
                user.Id = Guid.NewGuid().ToString();
                _users.InsertOne(user);
                return 0;
            }

        }
        public string SendOTP(string loginId)
        {
            var user = GetUserByLoginId(loginId);
            if (user != null)
            {
                string otp = new Random().Next(1000, 9999).ToString();
                _email.SendEmail(user.Email, user.FirstName, otp);
                return otp;
            }
            else return null;

        }

        
        public int ResetPassword(string loginId, string password)
        {

            var result = _users.Find(user => user.LoginId == loginId).FirstOrDefault();
            if (result != null)
            {

                var filter = Builders<User>.Filter.Eq(e => e.LoginId, loginId);

                var update = Builders<User>.Update.Set(e => e.Password, password);

                _users.FindOneAndUpdate(filter, update);

                return 0;
            }
            else
                return 1;
        }
    }
}
