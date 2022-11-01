using HttpContext.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace HttpContext.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpContextAccessor _contextAccessor;

        public HomeController(ILogger<HomeController> logger, IHttpContextAccessor context)
        {
            _logger = logger;
            _contextAccessor = context;
        }

        public IActionResult Index()
        {
            var context = _contextAccessor.HttpContext;
            var session = context.Session;
            session.SetString("U", "THISISAUSERID");
            var sessionUKey = session.GetString("U");
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