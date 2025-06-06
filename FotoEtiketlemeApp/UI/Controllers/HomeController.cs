using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using UI.Models;

namespace UI.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }
    public IActionResult labeledImages()
    {
        return View();
    }
    public IActionResult labelImages()
    {
        return View();
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
