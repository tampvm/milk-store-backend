using Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MilkStore.Domain.Entities;
using MilkStore.Repository.Data;
using MilkStore.WebApp.Models;
using System.Net.Http.Headers;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DatabaseConnection")));

builder.Services.AddIdentity<Account, Role>(options =>
        options.SignIn.RequireConfirmedAccount = false)
        .AddEntityFrameworkStores<AppDbContext>();

// Add services to the container.
builder.Services.AddHttpClient("ApiClient", client =>
{
    client.BaseAddress = new Uri("https://localhost:44329");
    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
});

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
});
builder.Services.AddRazorPages();
//builder.Services.AddRazorPages(options =>
//{
//    options.Conventions.AuthorizeFolder("/Secure");
//    options.Conventions.AllowAnonymousToPage("/Login");
//});

builder.Services.Configure<FirebaseSettings>(builder.Configuration.GetSection("Firebase"));

// Cấu hình dịch vụ xác thực
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
})
.AddCookie()
.AddGoogle(options =>
{
    var googleAuthSection = builder.Configuration.GetSection("Authentication:Google");
    options.ClientId = googleAuthSection["ClientId"];
    options.ClientSecret = googleAuthSection["ClientSecret"];
    options.ClaimActions.MapJsonKey("urn:google:picture", "picture", "url");
    options.SaveTokens = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseSession();
app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();
