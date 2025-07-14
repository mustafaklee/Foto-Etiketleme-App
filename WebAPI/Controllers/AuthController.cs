using Microsoft.AspNetCore.Mvc;
using WebAPI.Data;
using WebAPI.Models.Dtos;
using WebAPI.Repositories;
using Microsoft.AspNetCore.Identity;
namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public readonly ITokenRepository tokenRepository;
        public readonly AppDbContext appDbContext;
        public readonly UserManager<Models.Domain.Doctor> userManager;
        public readonly RoleManager<IdentityRole<int>> roleManager;
        public AuthController(ITokenRepository tokenRepository, AppDbContext appDbContext, UserManager<Models.Domain.Doctor> userManager, RoleManager<IdentityRole<int>> roleManager)
        {
            this.tokenRepository = tokenRepository;
            this.appDbContext = appDbContext;
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequestDto)
        {
            // Email kontrolü
            var existingUser = await userManager.FindByEmailAsync(registerRequestDto.UserName);
            if (existingUser != null)
                return BadRequest("Bu email ile zaten bir kullanıcı var.");

            var doctor = new Models.Domain.Doctor
            {
                UserName = registerRequestDto.UserName,
                Email = registerRequestDto.UserName,
                EmailConfirmed = true
            };
            var result = await userManager.CreateAsync(doctor, registerRequestDto.Password);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            // Rol atama
            if (registerRequestDto.Roles != null && registerRequestDto.Roles.Length > 0)
            {
                foreach (var role in registerRequestDto.Roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                        await roleManager.CreateAsync(new IdentityRole<int>(role));
                    await userManager.AddToRoleAsync(doctor, role);
                }
            }
            else
            {
                // Varsayılan rol ata
                await userManager.AddToRoleAsync(doctor, "doktor");
            }
            return Ok("Kullanıcı başarıyla kaydedildi. Lütfen giriş yapın.");
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
        {
            var doctor = await userManager.FindByEmailAsync(loginRequestDto.Username);
            if (doctor == null)
                return BadRequest("Kullanıcı bulunamadı.");

            var isPasswordValid = await userManager.CheckPasswordAsync(doctor, loginRequestDto.Password);
            if (!isPasswordValid)
                return BadRequest("Şifre yanlış.");

            // Kullanıcının rollerini al
            var roles = await userManager.GetRolesAsync(doctor);
            var jwtToken = tokenRepository.CreateJWTToken(doctor, roles);
            Response.Cookies.Append("JwtToken", jwtToken, new CookieOptions
            {
                HttpOnly = false,
                Secure = false,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddMinutes(15)
            });
            var response = new LoginResponseDto
            {
                JwtToken = jwtToken
            };
            return Ok(response);
        }
    }
}
