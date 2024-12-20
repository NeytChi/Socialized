using Domain;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace WebAPI.Middleware
{
    public interface IJwtTokenManager
    {
        string Authenticate(Admin admin);
    }
    public class JwtTokenManager : IJwtTokenManager
    {
        private readonly IConfiguration _configuration;

        public JwtTokenManager(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string Authenticate(Admin admin)
        {
            var key = _configuration.GetValue<string>("JwtConfig:Key");
            var keyBytes = Encoding.UTF8.GetBytes(key);
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                 {
                    new Claim(ClaimTypes.NameIdentifier, admin.Id.ToString()),
                    new Claim(ClaimTypes.NameIdentifier, admin.Email),
                    new Claim(ClaimTypes.Role, admin.Role)
                 }),
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = new SigningCredentials(
                                      new SymmetricSecurityKey(keyBytes),
                                      SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}