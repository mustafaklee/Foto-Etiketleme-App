using Microsoft.AspNetCore.Identity;
using WebAPI.Models.Domain;

namespace WebAPI.Repositories
{
    public interface ITokenRepository
    {
        string CreateJWTToken(Doctor doctor, IList<string> roles);
    }
}
