using AdaKurumsal.DataLayer;
using AdaKurumsal.Middlewares;
using Microsoft.AspNetCore.Mvc.Razor;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddLocalization(option => option.ResourcesPath = "Resources");
builder.Services.AddMemoryCache();
builder.Services.AddDbContext<EFContext>();
builder.Services.AddScoped<ILayoutDataService, LayoutDataService>();
// Add services to the container.
builder.Services.AddRazorPages(options =>
        {
            options.Conventions.AddPageRoute("/site/hakkimizda", "{dil=tr}/hakkimizda");  // TR route
            options.Conventions.AddPageRoute("/site/hakkimizda", "{dil=en}/about-us");   // EN route
        }).AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
          .AddDataAnnotationsLocalization();





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

app.UseRequestLocalization(options =>
{
    options.DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture("tr-TR");
    options.SupportedCultures = cultures;
    options.SupportedUICultures = cultures;
});
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseMiddleware<LanguageRedirectMiddleware>();

app.MapRazorPages();

app.Run();
