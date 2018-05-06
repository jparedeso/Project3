namespace Project3.API.Models
{
    public class LoginModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Grant_Type { get; set; }

        public string Refresh_Token { get; set; }
    }
}
