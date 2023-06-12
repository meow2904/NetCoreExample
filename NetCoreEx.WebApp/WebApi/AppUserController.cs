using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NetCoreEx.Library.Helper;
using NetCoreEx.Model.Entities;
using NetCoreEx.Service;
using NetCoreEx.Utilities.ViewModels;
using NetCoreEx.WebApp.Controllers;
using NetCoreEx.WebApp.Extensions;
using Newtonsoft.Json;

namespace NetCoreEx.WebApp.WebApi
{
    public class AppUserController : ApiControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly ILogger<HomeController> _logger;

        public AppUserController(ILogHistoryService logHistoryService, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, ILogger<HomeController> logger) : base(logHistoryService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            try
            {
                if (!ModelState.IsValid) return BadRequest(string.Join("; ", ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage)));
                var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded) return Ok(model);
                if (result.IsLockedOut) return BadRequest("Tài khoản của bạn bị khóa. Vui lòng kiểm tra lại");
                return BadRequest("Đăng nhập thất bại. Vui lòng kiểm tra lại");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Param: " + JsonConvert.SerializeObject(model));
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("Logout")]
        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await _signInManager.SignOutAsync();
                return Ok("");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error");
                return BadRequest(ex.Message);
            }
        }
    }
}