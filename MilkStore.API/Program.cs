using Microsoft.AspNetCore.Identity;
using MilkStore.API;
using MilkStore.Domain.Entities;
using MilkStore.Repository.Data;
using MilkStore.Service.Common;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration.Get<AppConfiguration>();
builder.Services.AddInfrastructuresService(configuration.DatabaseConnection);
builder.Services.AddWebAPIService(configuration.JWT);
builder.Services.AddSingleton(configuration);

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});
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

app.UseCors();
app.UseAuthorization();

app.MapControllers();

app.Run();
