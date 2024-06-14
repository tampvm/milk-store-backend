using Microsoft.AspNetCore.Identity;
using MilkStore.API;
using MilkStore.Domain.Entities;
using MilkStore.Repository.Data;
using MilkStore.Service.Common;

var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddDbContext<AppDbContext>(options =>
//	options.UseSqlServer(builder.Configuration.GetConnectionString("DatabaseConnection")));

var configuration = builder.Configuration.Get<AppConfiguration>();
builder.Services.AddInfrastructuresService(configuration.DatabaseConnection);
builder.Services.AddWebAPIService();
builder.Services.AddSingleton(configuration);

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
