using BeshariqBeton.BLL.Services;
using BeshariqBeton.Common.Models;
using BeshariqBeton.DAL.Infrastructure;
using BeshariqBeton.Web.Infrastructure.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.EntityFrameworkCore;
using ILogger = BeshariqBeton.Common.Logging.ILogger;
using BeshariqBeton.Common.Logging;
using Microsoft.AspNetCore.Authorization;
using System.Security.Principal;
using BeshariqBeton.Common.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using BeshariqBeton.Web.Infrastructure;
using BeshariqBeton.Web.Utilities;
using BeshariqBeton.Web.ModelBinders;

var builder = WebApplication.CreateBuilder(args);

var settings = new Settings();
builder.Configuration.Bind(settings);

builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.MinimumSameSitePolicy = SameSiteMode.Unspecified;
    options.OnAppendCookie = cookieContext =>
        cookieContext.CookieOptions.SameSite = SameSiteMode.None;
    options.OnDeleteCookie = cookieContext =>
        cookieContext.CookieOptions.SameSite = SameSiteMode.None;
    options.Secure = CookieSecurePolicy.Always;
});

builder.Services.AddAntiforgery(antiforgeryOptions =>
{
    antiforgeryOptions.SuppressXFrameOptionsHeader = true;
});

builder.Services.AddDbContext<MasterContext>(options => options.UseSqlServer(settings.ConnectionStrings.DefaultConnection));

builder.Services.AddDataProtection()
                .SetApplicationName("BeshariqBeton")
                .PersistKeysToDbContext<MasterContext>();
builder.Services.Configure<RouteOptions>(options =>
{
    options.LowercaseUrls = true;
});
// AutoMapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddRazorPages().AddRazorRuntimeCompilation();

builder.Services
    .AddControllers(options =>
    {
        options.ModelBinderProviders.Insert(0, new CustomDateTimeModelBinderProvider());
        options.ModelBinderProviders.Insert(0, new CustomDecimalModelBinderProvider());
    })
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/user/log-in";
                    options.LogoutPath = "/user/log-out";
                    options.AccessDeniedPath = "/error/403";
                    options.SlidingExpiration = false; // expiration set explicitly
                    options.Events.OnValidatePrincipal = PrincipalCookieValidator.ValidatePrincipal;
                })
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                {
                    options.Authority = settings.Url;
                    // name of the API resource
                    options.RequireHttpsMetadata = false;
                    options.Audience = "default";
                });


builder.Services.AddAuthorization(options =>
{
    // Standard permission policies
    foreach (var standardPermission in StandardPermissionsProvider.GetAllPermissions())
    {
        var policy = new AuthorizationPolicyBuilder()
            .AddRequirements(new StandardPermissionRequirement(standardPermission))
            .Build();

        options.AddPolicy(standardPermission.SystemName, policy);
    }
});


builder.Services.AddSingleton<IAuthorizationHandler, StandardPermissionHandler>();
builder.Services.AddScoped<IPrincipal>(implementationFactory: sp => sp.GetService<IHttpContextAccessor>()?.HttpContext?.User);
#region DI
builder.Services.AddSingleton(settings);
builder.Services.AddSingleton<ILogger, NLogLogger>();
builder.Services.AddScoped<IMapper, AutoMapperMapper>();

builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<DefaultParametersService>();
builder.Services.AddScoped<PermissionService>();
builder.Services.AddScoped<ClientService>();
builder.Services.AddScoped<SaleService>();
builder.Services.AddScoped<ListHelper>();
builder.Services.AddScoped<StorageService>();
builder.Services.AddScoped<StatisticsService>();
#endregion

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

var scope = app.Services.CreateScope();
var context = scope.ServiceProvider.GetRequiredService<MasterContext>();
var logger = app.Services.GetRequiredService<ILogger>();

// Migrate database
context.Database.Migrate();
// Must-have data
context.Initialize();

app.Run();
