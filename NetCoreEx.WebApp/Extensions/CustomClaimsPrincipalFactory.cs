using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using NetCoreEx.Model.Entities;
using NetCoreEx.Utilities.Constants;

namespace NetCoreEx.WebApp.Extensions
{
    public class CustomClaimsPrincipalFactory : UserClaimsPrincipalFactory<AppUser, AppRole>
    {
        private readonly UserManager<AppUser> _userManger;
        public CustomClaimsPrincipalFactory(UserManager<AppUser> userManager,
            RoleManager<AppRole> roleManager, IOptions<IdentityOptions> options)
            : base(userManager, roleManager, options)
        {
            _userManger = userManager;
        }
        public override async Task<ClaimsPrincipal> CreateAsync(AppUser employee)
        {
            var principal = await base.CreateAsync(employee);
            var roles = await _userManger.GetRolesAsync(employee);
            ((ClaimsIdentity)principal.Identity).AddClaims(new[]
            {
                new Claim(ClaimTypes.NameIdentifier,employee.Id),
                new Claim(ClaimTypes.Email,employee.Email),
                new Claim(ClaimTypes.Name,employee.UserName),
                new Claim(ClaimTypes.Role,string.Join(";",roles))
            });
            return principal;
        }
    }
}