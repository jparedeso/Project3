using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Project3.API.Models;

namespace Project3.API.Controllers
{
    [Route("[controller]")]
    public class OauthController : Controller
    {
        private readonly IConfiguration _config;
        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;

        public OauthController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IConfiguration config)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _config = config;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("Token")]
        public async Task<ActionResult> CreateToken([FromForm]LoginModel login)
        {
            if (!ModelState.IsValid) return BadRequest(new { message = "No se pudo crear el token." });

            ActionResult response = Unauthorized();

            switch (login.Grant_Type)
            {
                case "refresh_token":
                {
                    // Lookup which user is tied to model.RefreshToken
                    var refreshToken = Encoding.Unicode.GetString(Convert.FromBase64String(login.Refresh_Token));
                    var user = _userManager.FindByRefreshToken(refreshToken);

                    // Validate Refresh Token
                    if (user == null)
                    {
                        return BadRequest(new {message = "No se encontró un usuario con este refresh token."});
                    }

                    if (user.RefreshTokenExpiration.HasValue && DateTimeOffset.Compare(DateTimeOffset.Now, user.RefreshTokenExpiration.Value) > -1)
                    {
                        return Unauthorized();
                    }

                    // Generate access token from the username (no password check required)
                    var jwt = await BuildToken(user, refreshToken);

                    var tokenObject = new
                    {
                        access_token = jwt.AccessToken,
                        token_type = jwt.TokenType,
                        expires_in = jwt.ExpiresIn,
                        refresh_token = jwt.RefreshToken, 
                        userName = jwt.UserName,
                        issued = jwt.Issued.ToString("R"),
                        expires = jwt.Expires.ToString("R")
                    };

                    response = Ok(tokenObject);
                    break;
                }
                case "password":
                {
                    // Get User
                    var user = await _userManager.FindByEmailAsync(login.Username);

                    if (user == null)
                    {
                        return BadRequest(new { message = "El usuario no existe." });
                    }

                    var result = await _signInManager.CheckPasswordSignInAsync(user, login.Password, false);

                    if (!result.Succeeded) return BadRequest(new { message = "La contraseña no es válida." });

                    // Generate access token
                    // Generate refresh token (random GUID + model.username)
                    var jwt = await BuildToken(user, null);

                    var tokenObject = new
                    {
                        access_token = jwt.AccessToken,
                        token_type = jwt.TokenType,
                        expires_in = jwt.ExpiresIn,
                        refresh_token = jwt.RefreshToken,
                        userName = jwt.UserName,
                        issued = jwt.Issued.ToString("R"),
                        expires = jwt.Expires.ToString("R")
                    };

                    // Persist refresh token
                    user.RefreshToken = jwt.RefreshToken;
                    user.RefreshTokenExpiration = DateTime.Now.AddSeconds(14 * 86400);
                    await _userManager.UpdateAsync(user);

                    response = Ok(tokenObject);

                    break;
                }
            }

            // Return the complete token (access + refresh + expiration)
            return response;
        }

        private async Task<JsonWebToken> BuildToken(ApplicationUser user, string refreshToken)
        {
            // Generate Refresh Token
            if (string.IsNullOrEmpty(refreshToken) ||
                user.RefreshTokenExpiration.HasValue && (user.RefreshTokenExpiration.Value - DateTimeOffset.Now).Hours <= 1)
            {
                refreshToken = Guid.NewGuid() + user.UserName;
            }

            // Build Claims
            var userClaims = await _userManager.GetClaimsAsync(user);
            userClaims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.UserName));
            userClaims.Add(new Claim(JwtRegisteredClaimNames.Email, user.Email));
            userClaims.Add(new Claim(JwtRegisteredClaimNames.Jti, refreshToken));
            userClaims.Add(new Claim("FullyRegistered", user.FullyRegistered.ToString()));
            userClaims.Add(new Claim("DateOfBirth", user.DateOfBirth.ToString()));
            userClaims.Add(new Claim("Gender", user.Gender ?? ""));

            // Generate Access Token
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                userClaims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds);
            var accessToken = new JwtSecurityTokenHandler().WriteToken(token);

            return new JsonWebToken
            {
                AccessToken = accessToken,
                TokenType = "bearer",
                ExpiresIn = (int) new TimeSpan(1, 0, 0).TotalSeconds - 1,
                RefreshToken = refreshToken,
                UserName = user.UserName,
                Issued = DateTimeOffset.Now.ToUniversalTime(),
                Expires = token.ValidTo
            };
        }
    }
}