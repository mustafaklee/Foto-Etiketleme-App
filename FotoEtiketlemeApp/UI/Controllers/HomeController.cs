using System.Diagnostics;
using System.Net.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using UI.Models;
using UI.Models.Dtos;
using UI.Repositories;
namespace UI.Controllers;
//[Authorize]
public class HomeController : Controller
{

    private readonly IApiRepository _apiRepository;

    public HomeController(IApiRepository apiRepository)
    {
        _apiRepository = apiRepository;
    }

    [HttpGet]
    public IActionResult Index()
    {
        return View();
    }


    [HttpGet]
    public IActionResult AdminIndex()
    {
        return View();
    }


    [HttpGet]
    public IActionResult labeledImages()
    {
        return View(new FotoEtiketDto());
    }

    [HttpGet]
    public async Task<IActionResult> LabelImages(int count)
    {

        return View(new FotoEtiketDto() { });
        var result = await _apiRepository.GetProtectedDataAsync($"fotografetiketle/GetFoto?count={count}");
        try
        {
            ViewBag.message = result.Message;
            return View(result.Data);
        }
        catch
        {
            ViewBag.message = $"Sunucuda hata oluştu:";
            return View(new FotoEtiketDto());
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
