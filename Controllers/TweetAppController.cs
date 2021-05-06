using Confluent.Kafka;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using TweetAppAPI.Models;
using TweetAppAPI.Repository;

namespace TweetAppAPI.Controllers
{
    [ApiController]

    public class TweetAppController : ControllerBase
    {
          private readonly ProducerConfig config = new ProducerConfig {BootstrapServers= "localhost:9092"};
        private readonly string topic = "Tweet-Topic";

    
        private Object SendToKafka(string topic, string message)
        {
            using (var producer =
                 new ProducerBuilder<Null, string>(config).Build())
            {
                try
                {
                    return producer.ProduceAsync(topic, new Message<Null, string> { Value = message })
                        .GetAwaiter()
                        .GetResult();
                }
                catch (Exception e)
                {
                    Console.WriteLine($"Oops, something went wrong: {e}");
                }
            }
            return null;
        }
        private IUserRepo _userRepo;
        private ITweetRepo _tweetRepo;
        public TweetAppController(IUserRepo userRepo, ITweetRepo tweetRepo)
        {
            _userRepo = userRepo;
            _tweetRepo = tweetRepo;
        }

        [HttpGet]
        [Route("[controller]")]
        public IActionResult GetAllUsers()
        {
            return Ok(_userRepo.GetAllUsers());
        }

        [HttpGet]
        [Route("[controller]/{loginId}")]
        public IActionResult GetUserDetails(string loginId)
        {
            User response = _userRepo.GetUserDetails(loginId);
            return Ok(response);
        }

        [HttpPost]
        [Route("[controller]/sendOTP/{user}")]
        public IActionResult SendOTP(User user)
        {
            string response = _userRepo.SendOTP(user.LoginId);
            if(response != null)
            {
                return Ok(response);
            }
            else
                return Unauthorized("Login Id does not exists..!!");
        }

        [HttpPost]
        [Route("[controller]/resetPassword/{user}")]
        public IActionResult ResetPassword(User user)
        {
            int result = _userRepo.ResetPassword(user.LoginId, user.Password);
            if (result == 0)
            {
                return Ok();
            }
            else
                return BadRequest("Failed to Reset Password..!!");
        }

        [HttpGet]
        [Route("[controller]/tweets/")]
        public IActionResult GetTweets()
        {
            return Ok(_tweetRepo.GetTweets());
        }

        [HttpPost]
        [Route("[controller]/tweets/{tweet}")]
        public IActionResult PostTweet(Tweet tweet)
        {
            int result = _tweetRepo.PostTweet(tweet);
            if (result == 1)
            {
                return BadRequest("Failed to Post Tweet..!!");
            }
            else
            {
                // SendToKafka(topic,tweet.Body);
                return Ok();
            }
        }

        [HttpPost]
        [Route("[controller]/replies/{reply}")]
        public IActionResult PostReply(Reply reply)
        {
            int result = _tweetRepo.PostReply(reply);
            if (result == 1)
            {
                return BadRequest("Failed to Reply to Tweet..!!");
            }
            else
            {
                return Ok();
            }
        }

        [HttpPost]
        [Route("[controller]/login/{user}")]
        public IActionResult LoginUser(User user)
        {
            int result = _userRepo.LoginUser(user.LoginId, user.Password);
            ArrayList l =new ArrayList();
            l.Add(user.LoginId);
            if(result == 1)
            {
                return Unauthorized("Login Id does not exists..!!");
            }
            else if(result == 2)
            {
                return Unauthorized("Password Incorrect..!!");
            }
            else
            {
                return Ok(l);
            }
        }

        [HttpPost]
        [Route("[controller]/register/{user}")]
        public IActionResult RegisterUser(User user)
        {
            int result = _userRepo.RegisterUser(user);
            
            if(result == 1)
            {
                return BadRequest("Login Id already exists..!!");
            }
            else if (result == 2)
            {
                return BadRequest("Email address already exists..!!");
            }
            else
            {
                return Ok();
            }
        }

        
    }
}
