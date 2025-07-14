using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace WebAPI.Repositories
{
    public class TokenRepository : ITokenRepository
    {
        public readonly IConfiguration configuration;

        public TokenRepository(IConfiguration configuration)
        {
            this.configuration = configuration;
        }



        public string CreateJWTToken(Models.Domain.Doctor doctor, IList<string> roles)
        {
            var claims = new List<Claim>
{
                new Claim(ClaimTypes.Email, doctor.Email),
                new Claim(ClaimTypes.NameIdentifier, doctor.Id.ToString()),
                new Claim("doktorId", doctor.Id.ToString()) // ✅ Özel claim eklendi
            };

            // Roller claim olarak ekle
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                configuration["Jwt:Issuer"],
                configuration["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
