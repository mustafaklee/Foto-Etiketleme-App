using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Reflection;
using System.Text;
using UI.Models.Dtos;
using UI.Repositories;

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
            ViewBag.AvailableRoles = new List<string> { "Admin", "Doctor" }; // HER ZAMAN LAZIM
            if (!ModelState.IsValid)
            {
                ViewBag.Message = "Lütfen tüm alanları doldurun.";
                return View(model);
            }

            // Login için normal client kullan (token henüz yok)
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
                    ViewBag.Message = responseContent;

                    Response.Cookies.Append("JwtToken", loginResponse.JwtToken, new CookieOptions
                    {
                        HttpOnly = false, // JavaScript erişebilsin
                        Secure = false,
                        SameSite = SameSiteMode.Strict,
                        Expires = DateTimeOffset.UtcNow.AddMinutes(15)
                    });
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
