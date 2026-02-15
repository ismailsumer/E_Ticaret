using AdminPanel.ETicaretMVC.Filters;

var builder = WebApplication.CreateBuilder(args);

// MVC + Global Admin Authorization Filter
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add<AdminAuthorizationFilter>();
});

// 1. API ile iletişim için HttpClient Kaydı
builder.Services.AddHttpClient("ETicaretAPI", client =>
{
    client.BaseAddress = new Uri("http://localhost:5292/api/"); // API Portunu kontrol et
});

// 2. Token bilgilerine her yerden erişmek için
builder.Services.AddHttpContextAccessor();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
