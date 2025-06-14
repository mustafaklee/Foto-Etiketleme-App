using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using UI.Models;
using UI.Models.Dtos;
using UI.Repositories;

using Newtonsoft.Json;
using System.Linq.Expressions;
namespace UI.Controllers;

public class HomeController : Controller
{

    private readonly IHttpClientFactory _httpClientFactory;

    public HomeController(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public IActionResult Index()
    {
        return View();
    }
    public IActionResult labeledImages()
    {
        return View();
    }
    public async Task<IActionResult> labelImages()
    {
        try
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync("https://localhost:7252/api/fotografetiketle");

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.message = "Sunucuda hata oluştu.";
                return View(new FotoEtiketDto
                {
                    Fotograflar = new List<FotoDto>(),
                    Etiketler = new List<EtiketDto>()
                });
            }
            else
            {

                var json = await response.Content.ReadAsStringAsync();
                var result = JsonConvert.DeserializeObject<OperationResultDto<FotoEtiketDto>>(json);

                ViewBag.message = result.Message;
                return View(result.Data);
            }
        }
        catch(Exception ex)
        {
            ViewBag.message = $"Sunucuda hata oluştu. {ex}";
            return View();
        }
    }

    public IActionResult updateInfo()
    {
        return View();
    }


    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
