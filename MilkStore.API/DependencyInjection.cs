using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using MilkStore.Domain.Entities;
using MilkStore.Repository.Data;
using MilkStore.Repository.Interfaces;
using MilkStore.Repository.Repositories;
using MilkStore.Service.Common;
using MilkStore.Service.Interfaces;
using MilkStore.Service.Mappers;
using MilkStore.Service.Services;
using MilkStore.Service.Utils;
using System.Text;
using Net.payOS;

namespace MilkStore.API
{

	public static class DependencyInjection
	{
		public static IServiceCollection AddWebAPIService(this IServiceCollection services, JWTSettings jwt)
		{
			// Configure the token lifespan
			services.Configure<DataProtectionTokenProviderOptions>(options =>
			   options.TokenLifespan = TimeSpan.FromMinutes(10));

			// Add Identity
			services.AddIdentity<Account, Role>(/* options => options.SignIn.RequireConfirmedAccount = true */)
				.AddEntityFrameworkStores<AppDbContext>()
				.AddDefaultTokenProviders()
				.AddSignInManager()
				.AddRoles<Role>();

			// Add JWT authentication
			services.AddAuthentication(options =>
			{
				options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
			}).AddJwtBearer(options =>
			{
				options.SaveToken = true;
				options.RequireHttpsMetadata = false;
				options.TokenValidationParameters = new TokenValidationParameters
				{
					ValidateIssuer = true,
					ValidateAudience = true,
					ValidateLifetime = true,
					ValidateIssuerSigningKey = true,
					ValidIssuer = jwt.Issuer,
					ValidAudience = jwt.Audience,
					ClockSkew = TimeSpan.Zero,
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.JWTSecretKey))
				};
			});

			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			// Swagger Configuration
			services.AddEndpointsApiExplorer();
			services.AddSwaggerGen(option =>
			{
				option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
				{
					In = ParameterLocation.Header,
					Description = "Please enter a valid token",
					Name = "Authorization",
					Type = SecuritySchemeType.Http,
					BearerFormat = "JWT",
					Scheme = "Bearer"
				});
				option.AddSecurityRequirement(new OpenApiSecurityRequirement
				{
					{
						new OpenApiSecurityScheme
						{
							Reference = new OpenApiReference
							{
								Type = ReferenceType.SecurityScheme,
								Id = "Bearer"
							}
						},
						new string[] { }
					}
				});
			});

			services.AddCors(options =>
			{
				options.AddDefaultPolicy(builder =>
				{
					builder.WithOrigins("http://localhost:5173")
						.AllowAnyHeader()
						.AllowAnyMethod();
				});
			});

			// Add services to the container.
			services.AddTransient<SeedData>();
			services.AddControllers();
			services.AddMemoryCache();
			services.AddHttpClient();

			return services;
		}

        public static IServiceCollection AddInfrastructuresService(this IServiceCollection services, string databaseConnection)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ICurrentTime, CurrentTime>();
            services.AddScoped<IClaimsService, ClaimsService>();
            services.AddScoped<ISmsSender, TwilioSmsSender>();
            services.AddScoped<IZaloService, ZaloService>();
            services.AddScoped<IEmailSender, EmailSender>();
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IFirebaseService, FirebaseService>();
            services.AddScoped<IGoogleSerive, GoogleService>();
            services.AddScoped<IFacebookService, FacebookService>();
            
            //services.AddScoped<PayOS>();

			services.AddScoped<IAcccountRepository, AccountRepository>();
			services.AddScoped<IAuthService, AuthService>();
			services.AddScoped<IAccountService, AccountService>();

			services.AddScoped<IRoleRepository, RoleRepository>();
			services.AddScoped<IRoleService, RoleService>();

            services.AddScoped<IImageRepository, ImageRepository>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IOrderRepository, OrderRepository>();

			services.AddScoped<IAddressRepository, AddressRepository>();

            services.AddScoped<IAddressService, AddressService>();

            services.AddScoped<IBlogRepostiory, BlogRepository>();
            services.AddScoped<IBlogService, BlogService>();

            // Brand
            services.AddScoped<IBrandRepository, BrandRepository>();
            services.AddScoped<IBrandService, BrandService>();

            // FollowBrand
            services.AddScoped<IFollowBrandRepository, FollowBrandRepository>();
            services.AddScoped<IFollowBrandService, FollowBrandService>();

            // Voucher
            services.AddScoped<IVoucherRepository, VoucherRepository>();
            services.AddScoped<IVoucherService, VoucherService>();

            // Point
            services.AddScoped<IPointRepository, PointRepository>();
            services.AddScoped<IPointService, PointService>();

			//Category
			services.AddScoped<ICategoryRepository, CategoryRepository>();
			services.AddScoped<ICategoryService, CategoryService>();

			//BlogCategory
			services.AddScoped<IBlogCategoryRepository, BlogCategoryRepository>();
			services.AddScoped<IBlogCategoryService, BlogCategoryService>();

			//BlogImage
			services.AddScoped<IBlogImageRepository, BlogImageRepository>();

			//Like
			services.AddScoped<ILikeRepository, LikeRepository>();
			services.AddScoped<ILikeBlogService, LikeBlogService>();
			
			//CommentBlog
			services.AddScoped<ICommentBlogRepository, CommentBlogRepository>();
			services.AddScoped<ICommentBlogService, CommentBlogService>();

			//Cart
			services.AddScoped<ICartRepository, CartRepository>();
			services.AddScoped<ICartService, CartService>();
      
			//Product
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IProductService, ProductService>();

            //AgeRange
            services.AddScoped<IAgeRangeRepository, AgeRangeRepository>();
            services.AddScoped<IAgeRangeService, AgeRangeService>();

            //ProductType
            services.AddScoped<IProductTypeRepository, ProductTypeRepository>();
            services.AddScoped<IProductTypeService, ProductTypeService>();
          
            
            //OrderDetail
            services.AddScoped<IOrderDetailRepository, OrderDetailRepository>();

            //ProductImage
            services.AddScoped<IProductImageRepository, ProductImageRepository>();
            services.AddScoped<IProductImageService, ProductImageService>();

            services.AddDbContext<AppDbContext>(option => option.UseSqlServer(databaseConnection));

            services.AddAutoMapper(typeof(MapperConfigurationsProfile).Assembly);

            return services;
        }
    }
}
