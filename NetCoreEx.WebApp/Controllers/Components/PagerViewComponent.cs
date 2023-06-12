using NetCoreEx.Utilities;
using Microsoft.AspNetCore.Mvc;

namespace NetCoreEx.WebApp.Controllers.Components
{
    public class PagerViewComponent : ViewComponent
    {
        public Task<IViewComponentResult> InvokeAsync(PagedResultBase result)
        {
            return Task.FromResult((IViewComponentResult)View("Default", result));
        }
    }

}