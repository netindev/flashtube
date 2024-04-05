using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using flashtube.Models;

namespace flashtube.Controllers;

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

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    public IActionResult FireAction(string youtubeLink)
    {
        _logger.LogInformation("Received a download link: " + youtubeLink);

        return Content(youtubeLink);
    }
}
