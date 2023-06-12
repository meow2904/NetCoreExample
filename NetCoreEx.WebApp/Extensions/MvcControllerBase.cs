using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace NetCoreEx.WebApp.Extensions
{
    [Authorize]
    public class MvcControllerBase : Controller
    {
    }
}