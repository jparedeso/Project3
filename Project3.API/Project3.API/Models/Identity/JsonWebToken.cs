using System;

namespace Project3.API.Models.Identity
{
    public class JsonWebToken
    {
        public string AccessToken { get; set; }
        public string TokenType { get; set; }
        public int ExpiresIn { get; set; }
        public string RefreshToken { get; set; }
        public string UserName { get; set; }
        public DateTimeOffset Issued { get; set; }
        public DateTimeOffset Expires { get; set; }
    }
}
