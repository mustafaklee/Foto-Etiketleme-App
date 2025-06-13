using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using UI.Models;
using UI.Models.Dtos;
using WebAPI.Models.Dtos;
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
        var client = _httpClientFactory.CreateClient();
        var response = await client.GetAsync("https://localhost:7252/api/fotografetiketle");

        if (!response.IsSuccessStatusCode)
        {
            return View("Error");
        }

        var result = await response.Content.ReadFromJsonAsync<FotoResponse>();

        return View(result); // Model olarak Tüm FotoResponse gönderiyoruz
    }



    public class FotoResponse
    {
        public List<FotografDto> Fotograflar { get; set; }
        public List<EtiketDto> Etiketler { get; set; }
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
