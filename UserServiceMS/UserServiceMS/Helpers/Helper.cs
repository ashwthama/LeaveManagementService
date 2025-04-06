using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using UserServiceMS.Models;


namespace UserServiceMS.Helpers
{
    public class Helper : IHelper
    {
        private readonly IConfiguration _config;
        public Helper(IConfiguration config)
        {
            _config = config;
        }

        public string GenerateToken(User user)
        {
            var claims = new[]
            {
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.Role, user.Role)
        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _config["JWT:Issuer"],
                _config["JWT:Issuer"],
                claims,
                expires: DateTime.Now.AddHours(2),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);

            //return "token";
        }

    }
}
