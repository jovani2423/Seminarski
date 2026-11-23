using Microsoft.AspNetCore.Mvc;
using RentACar.BLL.Services;
using RentACar.Presentation.Models;
using System.Diagnostics;

namespace RentACar.Presentation.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly BranchService _branchService;

        public HomeController(
            ILogger<HomeController> logger,
            BranchService branchService)
        {
            _logger = logger;
            _branchService = branchService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }

        public async Task<IActionResult> Contact()
        {
            var branches = await _branchService.GetActiveAsync();

            return View(branches);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }
}
