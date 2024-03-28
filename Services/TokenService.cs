using API.Entities;
using API.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API.Services
{
    public class TokenService : ITokenService // The TokenService class will implement all methods defined in the ITokenService interface
    {
        private readonly SymmetricSecurityKey _key;
        public TokenService(IConfiguration config) //config is used to read and get value from configuration
        {
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"])); // create a security key 
        }
        public string CreateToken(AppUser user)
        {
            var claims = new List<Claim> // claims is informations to prove the authenticity and authority of users in the system
            {
                new Claim(JwtRegisteredClaimNames.NameId, user.UserName)
            };

            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature); // create a SigningCredentials with security key and security algorthim

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor); // create token with informations in tokenDescriptor

            return tokenHandler.WriteToken(token);
        }
    }
}
