using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MilkStore.Domain.Entities;
using MilkStore.Repository.Data;

namespace MilkStore.API
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddWebAPIService(this IServiceCollection services)
        {
            // Add Identity
            services.AddIdentity<Account, Role>(/* options => options.SignIn.RequireConfirmedAccount = true */)
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders()
                .AddSignInManager()
                .AddRoles<Role>();

            // Add services to the container.
            services.AddControllers();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            return services;
        }

        public static IServiceCollection AddInfrastructuresService(this IServiceCollection services, string databaseConnection)
        {
            services.AddDbContext<AppDbContext>(option => option.UseSqlServer(databaseConnection));

            return services;
        }
    }
}
