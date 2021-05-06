namespace TweetAppAPI.Services
{
    public interface IEmail
    {
          public void SendEmail(string email, string firstName, string otp);
    }
}