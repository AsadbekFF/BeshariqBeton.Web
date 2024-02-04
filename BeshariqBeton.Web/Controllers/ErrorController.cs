using BeshariqBeton.Web.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace BeshariqBeton.Web.Controllers
{
    public class ErrorController : BaseController
    {
        protected ErrorController(Common.Logging.ILogger logger) : base(logger)
        {
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("403")]
        public IActionResult Forbidden()
        {
            return View();
        }

        [HttpGet]
        [Route("404")]
        public new IActionResult NotFound()
        {
            return View();
        }
    }
}
