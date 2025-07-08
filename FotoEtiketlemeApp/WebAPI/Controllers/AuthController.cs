using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Data;
using WebAPI.Models.Dtos;
using WebAPI.Repositories;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public readonly UserManager<IdentityUser> userManager;
        public readonly ITokenRepository tokenRepository;
        public readonly AppDbContext appDbContext;
        public AuthController(UserManager<IdentityUser> userManager, ITokenRepository tokenRepository,AppDbContext appDbContext)
        {
            this.userManager = userManager;
            this.tokenRepository = tokenRepository;
            this.appDbContext = appDbContext;
        }


        [HttpPost]
        [Route("Register")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequestDto)
        {
            var identityUser = new IdentityUser
            {
                UserName = registerRequestDto.UserName,
                Email = registerRequestDto.UserName,
            };

            var identityResult = await userManager.CreateAsync(identityUser, registerRequestDto.Password);

            if (identityResult.Succeeded)
            {
                //add roles to this user
                if (registerRequestDto.Roles != null && registerRequestDto.Roles.Any())
                {
                    var doktor = new Models.Domain.Doctor
                    {
                        Id = Guid.Parse(identityUser.Id),
                        Email = identityUser.Email,
                    };

                    await appDbContext.Doctor.AddAsync(doktor);
                    await appDbContext.SaveChangesAsync(); 

                    identityResult = await userManager.AddToRolesAsync(identityUser, registerRequestDto.Roles);
                    if (identityResult.Succeeded)
                    {
                        return Ok("User was registered! please login.");
                    }
                }
            }
            return BadRequest("Something went wrong");

        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            var user = await userManager.FindByEmailAsync(loginRequestDto.Username);
            if (user != null)
            {
                var userCheckPassword = await userManager.CheckPasswordAsync(user, loginRequestDto.Password);


                if (userCheckPassword)
                {
                    //get roles for this user
                    var roles = await userManager.GetRolesAsync(user);

                    if (roles != null)
                    {
                        //create a token
                        var jwtToken = tokenRepository.CreateJWTToken(user, roles.ToList());

                        Response.Cookies.Append("JwtToken", jwtToken, new CookieOptions
                        {
                            HttpOnly = false,          // JavaScript erişebilsin diye
                            Secure = false,             // HTTPS zorunlu
                            SameSite = SameSiteMode.Strict,
                            Expires = DateTimeOffset.UtcNow.AddMinutes(15)
                        });
                        var response = new LoginResponseDto
                        {
                            JwtToken = jwtToken
                        };
                        return Ok(response);
                    }

                    return Ok();

                }



            }
            return BadRequest("Username or password incorrect");
        }
    }
}
