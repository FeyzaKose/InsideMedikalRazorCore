using AdaKurumsal.DataLayer;
using AdaKurumsal.Middlewares;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddLocalization(option => option.ResourcesPath = "Resources");

builder.Services.AddDbContext<EFContext>();

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddAuthentication("MyCookieAuth").AddCookie("MyCookieAuth", options =>
{

    options.Cookie.Name = "MyCookieAuth";
    options.LoginPath = "/giris";
    options.ExpireTimeSpan = TimeSpan.FromDays(18000);
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
var cultures = new List<CultureInfo> {
    new CultureInfo("en"),
    new CultureInfo("tr")
};
app.UseRequestLocalization();
//app.UseRequestLocalization(options =>
//{
//    options.DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture("tr-TR");
//    options.SupportedCultures = cultures;
//    options.SupportedUICultures = cultures;
//});
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseMiddleware<LanguageRedirectMiddleware>();
app.MapRazorPages();

app.Run();
