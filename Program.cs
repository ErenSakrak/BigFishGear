using Microsoft.AspNetCore.Builder;  // ASP.NET Core için middleware yapılandırma
using Microsoft.Extensions.DependencyInjection;  // Servis bağımlılıkları için
using bigSales.Services;  // Kendi servislerinizi eklediğiniz kısımlar
using bigSales.Data;  // Veritabanı sınıflarınız
using Microsoft.EntityFrameworkCore;  // Entity Framework kullanımı
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Configuration;
using System;
using Microsoft.Extensions.Hosting;  // Ki

var builder = WebApplication.CreateBuilder(args);

// **Veritabanı Bağlantı Ayarları**
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// DbContext'i ekle
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// **Kimlik Doğrulama ve Çerez Ayarları**
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login"; // Giriş sayfası yolu
        options.LogoutPath = "/Account/Logout"; // Çıkış sayfası yolu
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30); // Oturum süresi
        options.AccessDeniedPath = "/Account/AccessDenied"; // Yetkisiz erişim
    });

// **Session Ayarları**
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Oturum süresi
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true; // GDPR uyumluluğu
});

// **Servis Bağımlılıkları**
builder.Services.AddControllersWithViews(); // MVC yapılandırması

// Kendi servislerinizi ekleyin

// builder.Services.AddScoped<ICategoryService, CategoryManager>();
// builder.Services.AddScoped<ICategoryDal, EfCategoryDal>();

builder.Services.AddScoped<CategoryService>();
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<UserService>(); // Eğer `UserService` kullanılacaksa

var app = builder.Build();

// **Ortam Kontrolleri**
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error"); // Hata sayfası
    app.UseHsts(); // HTTPS sıkı taşıma güvenliği
}

// **Middleware Yapılandırması**
app.UseHttpsRedirection(); // HTTP'den HTTPS'e yönlendirme
app.UseStaticFiles(); // Statik dosyaları kullanıma aç

app.UseRouting(); // Yönlendirme middleware

// Oturum ve kimlik doğrulama
app.UseSession();
app.UseAuthentication(); // Kimlik doğrulama
app.UseAuthorization(); // Yetkilendirme

// **Varsayılan Rota Ayarları**
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
