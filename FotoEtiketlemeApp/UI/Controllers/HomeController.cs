using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using UI.Models;

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
    public async Task<IActionResult> labelImages(int id=1)
    {
        var client = _httpClientFactory.CreateClient();
        var response = await client.GetAsync($"https://localhost:7252/api/fotografetiketle/{id}");
        if (!response.IsSuccessStatusCode)
        {
            return View("an error occurred");
        }

        var result = await response.Content.ReadFromJsonAsync<FotoResponse>();
        return View(model: result?.Url);
    }

    public class FotoResponse
    {
        public string Url { get; set; }
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
