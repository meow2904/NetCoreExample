using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NetCoreEx.Utilities.ViewModels;
using NetCoreEx.WebApp.Models;
using System.Diagnostics;

namespace NetCoreEx.WebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [Route("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string returnUrl = "")
        {
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [Route("login-redirect")]
        [AllowAnonymous]
        public IActionResult LoginRedirect(string returnUrl = "")
        {
            ViewData["ReturnUrl"] = returnUrl;
            return PartialView();
        }
        
        [HttpGet]
        [AllowAnonymous]
        public IActionResult Lockout()
        {
            return View();
        }

        [Authorize]
        public IActionResult Index()
        {
            return Redirect("/FormDemo");
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

    [ViewComponent(Name = "GetMenu")]
    public class GetMenuViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }

    [ViewComponent(Name = "GetSidebar")]
    [Authorize]
    public class GetSidebarViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }

    [ViewComponent(Name = "EmployeeUpdateMenu")]
    public class EmployeeUpdateMenuViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(int employeeId)
        {
            ViewData["EmployeeId"] = employeeId;
            return View();
        }
    }

    [ViewComponent(Name = "EmployeeInfoEdit")]
    public class EmployeeInfoEditMenuViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(AppUserViewModel employeeViewModel)
        {
            return View(employeeViewModel);
        }
    }
}