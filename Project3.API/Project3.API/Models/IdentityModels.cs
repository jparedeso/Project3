using System;
using System.Linq;
using Project3.API.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Project3.API.Models
{
    public class CustomUserRole : IdentityUserRole<int> { }
    public class CustomUserClaim : IdentityUserClaim<int> { }
    public class CustomUserLogin : IdentityUserLogin<int> { }
    public class CustomRoleClaim : IdentityRoleClaim<int> { }
    public class CustomUserToken : IdentityUserToken<int> { }

    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser<int>
    {
        // Personal Fields
        public string FirstName { get; set; }
        public string LastName { get; set; }

        // Registration
        public DateTimeOffset Joined { get; set; }
        public bool FullyRegistered { get; set; }

        // Refresh Token
        public string RefreshToken { get; set; }
        public DateTimeOffset? RefreshTokenExpiration { get; set; }

        //Additional Information
        public byte[] Picture { get; set; }
        public string Gender { get; set; }
        public DateTimeOffset DateOfBirth { get; set; }
    }

    public class CustomRole : IdentityRole<int>
    {
        public CustomRole() { }
        public CustomRole(string name) { Name = name; }
    }

    public class CustomUserStore : UserStore<ApplicationUser, CustomRole, ApplicationDbContext, int>
    {
        public CustomUserStore(ApplicationDbContext context)
            : base(context)
        {
        }
    }

    public class CustomRoleStore : RoleStore<CustomRole, ApplicationDbContext, int>
    {
        public CustomRoleStore(ApplicationDbContext context)
            : base(context)
        {
        }
    }

    public static class UserManagerExtensions
    {
        public static ApplicationUser FindByRefreshToken(this UserManager<ApplicationUser> um, string refreshToken)
        {
            return um?.Users?.SingleOrDefault(x => x.RefreshToken == refreshToken);
        }
    }
}
