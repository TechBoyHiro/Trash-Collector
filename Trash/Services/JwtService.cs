using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Trash.Models.ContextModels;

namespace Trash.Services
{
    public class JwtService
    {
        private readonly IConfiguration configuration;

        public JwtService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public string Generate(User user)
        {
            var secretkey = Encoding.Default.GetBytes(configuration.GetSection("JwtSettings").GetSection("jwtsecretkey").Value);
            //var secretkey = Base64UrlEncoder.DecodeBytes(configuration.GetSection("JwtSettings").GetSection("jwtsecretkey").Value);
            var signingcredintials = new SigningCredentials(new SymmetricSecurityKey(secretkey),SecurityAlgorithms.HmacSha256Signature);
            var claims = GetClaims(user);
            var securityTokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = "TrashServer",
                Audience = "TrashServer",
                IssuedAt = DateTime.Now,
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = signingcredintials,
                Subject = new ClaimsIdentity(claims)
            };

            var jwthandler = new JwtSecurityTokenHandler();
            var securitytoken = jwthandler.CreateToken(securityTokenDescriptor);
            var JWT = jwthandler.WriteToken(securitytoken);
            return JWT;
        }

        public IEnumerable<Claim> GetClaims(User user)
        {
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Role, "user"));
            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
            claims.Add(new Claim(ClaimTypes.Name, user.UserName));
            claims.Add(new Claim("Score", user.Score.ToString()));
            claims.Add(new Claim(ClaimTypes.MobilePhone,user.Phone));
            claims.Add(new Claim(ClaimTypes.Email,user.Email));
            return claims;
        }
    }
}
