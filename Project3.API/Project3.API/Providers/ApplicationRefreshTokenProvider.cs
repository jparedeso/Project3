using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using Microsoft.Owin.Security.Infrastructure;

namespace Project3.API.Providers
{
    public class ApplicationRefreshTokenProvider : AuthenticationTokenProvider
    {
        private const string IsRefreshTokenExpireName = "IsRefreshTokenExpired";

        public override void Create(AuthenticationTokenCreateContext context)
        {
            const int expireSeconds = 14 * 86400; // 14 days
            context.Ticket.Properties.IssuedUtc = DateTime.Now;
            context.Ticket.Properties.ExpiresUtc = new DateTimeOffset(DateTime.Now.AddSeconds(expireSeconds));
            context.SetToken(context.SerializeTicket());
        }

        public override void Receive(AuthenticationTokenReceiveContext context)
        {
            context.DeserializeTicket(context.Token);
        }
    }
}