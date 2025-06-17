using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;
using System.Security.Claims;
using System.Text;
using UI.Models.Dtos;
using System.IdentityModel.Tokens.Jwt;
namespace UI.Controllers
{
    public class LoginController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public LoginController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }


        [HttpGet]
        public IActionResult Login()
        {
            // Zaten giriş yapmışsa home'a yönlendir
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("JwtToken")))
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestDto model)
        {
            ViewBag.AvailableRoles = new List<string> { "Admin", "Doctor" };

            if (!ModelState.IsValid)
            {
                ViewBag.Message = "Lütfen tüm alanları doldurun.";
                return View(model);
            }

            using (var client = _httpClientFactory.CreateClient())
            {
                string apiUrl = "https://localhost:7252/api/Auth/Login";
                var jsonContent = JsonConvert.SerializeObject(model);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(apiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var loginResponse = JsonConvert.DeserializeObject<LoginResponseDto>(responseContent);

                    HttpContext.Session.SetString("JwtToken", loginResponse.JwtToken);
                    Response.Cookies.Append("JwtToken", loginResponse.JwtToken, new CookieOptions
                    {
                        HttpOnly = false,
                        Secure = false,
                        SameSite = SameSiteMode.Strict,
                        Expires = DateTimeOffset.UtcNow.AddMinutes(15)
                    });

                    var handler = new JwtSecurityTokenHandler();
                    var jwtToken = handler.ReadJwtToken(loginResponse.JwtToken);
                    var claims = jwtToken.Claims.ToList();

                    var email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
                    var userId = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                    var roles = claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList();

                    var authClaims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Email, email),
                        new Claim(ClaimTypes.NameIdentifier, userId)
                    };

                    foreach (var role in roles)
                    {
                        authClaims.Add(new Claim(ClaimTypes.Role, role));
                    }

                    var identity = new ClaimsIdentity(authClaims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var principal = new ClaimsPrincipal(identity);

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                    if (roles.Contains("Admin"))
                    {
                        return RedirectToAction("AdminIndex", "Home");
                    }

                    return RedirectToAction("Index", "Home");
                }

                ViewBag.Message = "Geçersiz kullanıcı adı veya şifre.";
                return View(model);
            }
        }

        public IActionResult Register()
        {
            ViewBag.AvailableRoles = new List<string> { "Admin","Doctor" };
            return View();
        }



        [HttpPost]
        public async Task<IActionResult> Register(RegisterRequestDto model)
        {
            ViewBag.AvailableRoles = new List<string> { "Admin", "Doctor" };
            if (!ModelState.IsValid)
            {
                ViewBag.Message = "Lütfen tüm alanları doldurun.";
                return View(model);
            }

            using (var client = _httpClientFactory.CreateClient("AuthorizedClient"))
            {
                string apiUrl = "https://localhost:7252/api/Auth/Register";
                var jsonContent = JsonConvert.SerializeObject(model);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await client.PostAsync(apiUrl, content);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    ViewBag.Message = responseContent;
                    return View();
                }

                ViewBag.Message = responseContent;
                return View();
            }
        }


        [HttpGet]
        public IActionResult Logout()
        {
            // Oturumu temizle
            HttpContext.Session.Remove("JwtToken");
            HttpContext.Session.Clear(); // Tüm session'ı temizlemek istersen
            Response.Cookies.Delete("JwtToken");
            // Giriş sayfasına yönlendir
            return RedirectToAction("Login", "Login");
        }

    }
}
