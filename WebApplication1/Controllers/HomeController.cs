using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using eventEasefour.Models;
using System.Diagnostics;

namespace eventEasefour.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment _env;

        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment env)
        {
            _logger = logger;
            _env = env;
        }

        // ? Error Handler (Cleaned)
        [Route("Home/Error")]
        public IActionResult Error()
        {
            var exceptionDetails = HttpContext.Features.Get<IExceptionHandlerFeature>();

            var model = new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier,
                ErrorMessage = exceptionDetails?.Error?.Message,
                StackTrace = exceptionDetails?.Error?.StackTrace,
                ShowDetails = _env.IsDevelopment()
            };

            // ? Log the full error for debugging
            _logger.LogError(exceptionDetails?.Error, "An unexpected error occurred.");

            return View(model);
        }
    }
}
