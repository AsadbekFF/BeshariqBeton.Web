using BeshariqBeton.BLL.Services;
using BeshariqBeton.Web.Infrastructure;
using BeshariqBeton.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BeshariqBeton.Web.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        private readonly StatisticsService _statisticsService;

        public HomeController(Common.Logging.ILogger logger, StatisticsService statisticsService) : base(logger)
        {
            _statisticsService = statisticsService;
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

        [HttpGet("[action]")]
        public async Task<IActionResult> GetYearStatistics()
        {
            var result = await _statisticsService.GetYearStatistics();
            return JsonResult(result);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetMonthStatistics(DateTime? date)
        {
            if (date == null)
            {
                return JsonNotFoundResult();
            }

            var result = await _statisticsService.GetDailyStatistics(date.Value.Month, date.Value.Year);
            return JsonResult(result);
        }
    }
}
