using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Webshop_opp.Models;
using Webshop_opp.ViewModels;

namespace Webshop_opp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        Account account = new Account();

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index(AccountViewModel Account)
        {
            return RedirectToAction("Index", "Shop");
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
    }
}