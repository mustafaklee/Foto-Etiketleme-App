using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using UI.Models;
using UI.Models.Dtos;
using UI.Repositories;
using Newtonsoft.Json;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Authorization;
namespace UI.Controllers;

public class HomeController : Controller
{

    private readonly IApiRepository _apiRepository;

    public HomeController(IApiRepository apiRepository)
    {
        _apiRepository = apiRepository;
    }

    public IActionResult Index()
    {
        return View();
    }
    public IActionResult labeledImages()
    {
        return View();
    }
    [HttpGet]
    public async Task<IActionResult> LabelImages()
    {
        try
        {
            var result = await _apiRepository.GetProtectedDataAsync("fotografetiketle");
            ViewBag.message = result.Message;
            return View(result.Data);
        }
        catch (Exception ex)
        {
            ViewBag.message = $"Sunucuda hata oluştu: {ex.Message}";
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
