using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using OrganizationsWaterSupplyL4.Data;
using OrganizationsWaterSupplyL4.Middleware;
using OrganizationsWaterSupplyL4.Models;
using OrganizationsWaterSupplyL4.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
string conStr = builder.Configuration.GetConnectionString("SqlServerConnection");
builder.Services.AddDbContext<OrganizationsWaterSupplyContext>(options => options.UseSqlServer(conStr));
builder.Services.AddControllersWithViews();
builder.Services.AddMemoryCache();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddScoped<ICachedService<CounterModel>, CachedModelsService>();
builder.Services.AddScoped<ICachedService<Counter>, CachedCountersService>();
builder.Services.AddScoped<ICachedService<Organization>, CachedOrganizationsService>();
builder.Services.AddScoped<ICachedService<Rate>, CachedRatesService>();
builder.Services.AddScoped<ICachedService<CountersCheck>, CachedCountersChecksService>();
builder.Services.AddScoped<ICachedService<CountersDatum>, CachedCountersDataService>();
builder.Services.AddScoped<ICachedService<RateOrgNote>, CachedRateOrgNotesService>();
var usersConnectionString = builder.Configuration.GetConnectionString("UsersConnection");

builder.Services.AddDbContext<UsersDbContext>(options => options.UseSqlServer(usersConnectionString));
builder.Services.AddIdentity<User, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
        .AddEntityFrameworkStores<UsersDbContext>()
        .AddDefaultUI()
        .AddDefaultTokenProviders();


builder.Services.AddControllersWithViews();
var app = builder.Build();
var supportedCultures = new[]
        {
            new CultureInfo("ru-RU"),
        };

app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("ru-RU"),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures,
});
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseDbInitializer();
app.UseRouting();
app.UseAuthentication();;

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();
/*app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "counters",
    pattern: "{controller=Counters}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "countermodels",
    pattern: "{controller=CounterModels}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "organizations",
    pattern: "{controller=Organizations}/{action=Index}/{id?}");
*/
app.Run();
