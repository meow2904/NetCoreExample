using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NetCoreEx.Model;
using NetCoreEx.Model.Entities;
using NetCoreEx.Model.Infrastructure;
using NetCoreEx.Service;
using NetCoreEx.WebApp.Extensions;
using System.Security.Claims;

namespace NetCoreEx.WebApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContextPool<NetCoreExDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"),
                    o => o.MigrationsAssembly("NetCoreEx.Model")));
            services.AddIdentity<AppUser, AppRole>()
                .AddEntityFrameworkStores<NetCoreExDbContext>()
                .AddDefaultTokenProviders();
            services.AddMemoryCache();
            // Configure Identity
            services.Configure<IdentityOptions>(options =>
            {
                // Password settings
                options.Password.RequireDigit = true;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireLowercase = true;
                // Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 10;

                // User settings
                options.User.RequireUniqueEmail = true;
            });
            services.ConfigureApplicationCookie(options =>
                {
                    options.LoginPath = "/login-redirect";
                    options.AccessDeniedPath = "/login-redirect";
                    options.LogoutPath = "/logout";
                });
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromHours(2);
                options.Cookie.HttpOnly = true;
            });
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Founders", policy => policy.RequireClaim(ClaimTypes.NameIdentifier, "PhuDX"));
            });

            services.AddControllersWithViews(options =>
            {
                options.CacheProfiles.Add("Default",
                  new CacheProfile()
                  {
                      Duration = 60
                  });
                options.CacheProfiles.Add("Never",
                  new CacheProfile()
                  {
                      Location = ResponseCacheLocation.None,
                      NoStore = true
                  });
            }).AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix,
            opts =>
            {
                opts.ResourcesPath = "Resources";
            }
            ).AddDataAnnotationsLocalization()
             .AddJsonOptions(options =>
              {
                  options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                  options.JsonSerializerOptions.PropertyNamingPolicy = null;
              });
            services.AddLocalization(opts => { opts.ResourcesPath = "Resources"; });
            services.AddCors(options => options.AddPolicy(name: "DefaultPolicy",
                builder =>
                {
                    builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                }));
            services.AddTransient(typeof(IUnitOfWork), typeof(UnitOfWork));
            services.AddTransient(typeof(IRepository<,>), typeof(RepositoryBase<,>));
            // Add application services.
            services.AddTransient<UserManager<AppUser>, UserManager<AppUser>>();
            services.AddTransient<RoleManager<AppRole>, RoleManager<AppRole>>();
            services.AddTransient<ILogHistoryService, LogHistoryService>();
            services.AddTransient<IFormDemoService, FormDemoService>();
            services.AddTransient<IFormDemoDetailService, FormDemoDetailService>();
            services.AddTransient<IUnitOfWork, UnitOfWork>();
            services.AddTransient<IUserClaimsPrincipalFactory<AppUser>, CustomClaimsPrincipalFactory>();

            //Form
            services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                    options.JsonSerializerOptions.PropertyNamingPolicy = null;
                });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory, IServiceProvider serviceProvider)
        {
            if (env.EnvironmentName == Environments.Development)
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseCors("DefaultPolicy");
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSession();
            var options = app.ApplicationServices.GetService<IOptions<RequestLocalizationOptions>>();
            app.UseRequestLocalization(options.Value);

            app.UseEndpoints(routes =>
            {
                routes.MapControllerRoute(
                    "default",
                    "{controller=Home}/{action=Index}/{id?}");

                routes.MapControllerRoute(
                    "areaRoute",
                    "{area:exists}/{controller=Login}/{action=Index}/{id?}");
            });
            CreateRoles(serviceProvider);
        }

        private void CreateRoles(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<AppRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();
            Task<IdentityResult> roleResult;
            string email = "netcoreex@gmail.com";
            string[] roleNames = { "Admin", "User" };

            foreach (var roleName in roleNames)
            {
                Task<bool> hasAdminRole = roleManager.RoleExistsAsync(roleName);
                hasAdminRole.Wait();

                if (!hasAdminRole.Result)
                {
                    roleResult = roleManager.CreateAsync(new AppRole(roleName, roleName));
                    roleResult.Wait();
                }
            }
            //Check if the admin user exists and create it if not
            //Add to the Administrator role

            Task<AppUser> testUser = userManager.FindByEmailAsync(email);
            testUser.Wait();

            if (testUser.Result == null)
            {
                AppUser administrator = new AppUser();
                administrator.Email = email;
                administrator.UserName = "admin";
                administrator.FullName = "Quản trị viên";
                administrator.Avatar = "/";
                administrator.Status = 1;
                Task<IdentityResult> newUser = userManager.CreateAsync(administrator, "Abc@2022");
                newUser.Wait();

                if (newUser.Result.Succeeded)
                {
                    Task<IdentityResult> newUserRole = userManager.AddToRoleAsync(administrator, "Admin");
                    newUserRole.Wait();
                }
            }
        }
    }
}