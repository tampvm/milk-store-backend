using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using MilkStore.API;
using MilkStore.Domain.Entities;
using MilkStore.Repository.Data;
using MilkStore.Service.Common;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration.Get<AppConfiguration>();
builder.Services.AddInfrastructuresService(configuration.DatabaseConnection);
builder.Services.AddWebAPIService(configuration.JWT);
builder.Services.AddSingleton(configuration);

// Conection string for database
//builder.Services.AddDbContext<AppDbContext>(options =>
//	options.UseSqlServer(builder.Configuration.GetConnectionString("DatabaseConnection")));

// Add JWT authentication
//var jwtKey = builder.Configuration.GetSection("JWT:JWTSecretKey").Get<string>();
//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
// .AddJwtBearer(options =>
// {
//     options.TokenValidationParameters = new TokenValidationParameters
//     {
//         ValidateIssuer = false,
//         ValidateAudience = false,
//         ValidateLifetime = true,
//         ValidateIssuerSigningKey = true,
//         //ValidIssuer = jwtIssuer,
//         //ValidAudience = jwtIssuer,
//         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
//     };
// });
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = GoogleDefaults.AuthenticationScheme;
})
.AddCookie()
.AddGoogle(options =>
{
    var googleAuthSection = builder.Configuration.GetSection("Authentication:Google");
    options.ClientId = googleAuthSection["ClientId"];
    options.ClientSecret = googleAuthSection["ClientSecret"];
    options.CallbackPath = "/api/auth/google-callback";
    options.ClaimActions.MapJsonKey(ClaimTypes.Email, "email");
    options.ClaimActions.MapJsonKey(ClaimTypes.GivenName, "given_name");
    options.ClaimActions.MapJsonKey(ClaimTypes.Surname, "family_name");
    options.ClaimActions.MapJsonKey("image", "picture"); // Map image to 'picture' claim
});

var app = builder.Build();

// Call this method to seed data
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var seedData = services.GetRequiredService<SeedData>();
    await seedData.Initialize(services, services.GetRequiredService<RoleManager<Role>>(), services.GetRequiredService<UserManager<Account>>());
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
