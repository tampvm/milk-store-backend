using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MilkStore.Domain.Entities;
using MilkStore.Repository.FluentAPIs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore.Repository.Data
{
	public class AppDbContext : IdentityDbContext<Account, Role, string>
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
		{
		}

		public DbSet<Account> Accounts { get; set; }
		public DbSet<Role> Roles { get; set; }
		public DbSet<AccountVoucher> AccountVouchers { get; set; }
		public DbSet<Address> Addresses { get; set; }
		public DbSet<AgeRange> AgeRanges { get; set; }
		public DbSet<Brand> Brands { get; set; }
		public DbSet<Category> Categories { get; set; }
		public DbSet<CommentBrand> CommentBrands { get; set; }
		public DbSet<CommentPost> CommentPosts { get; set; }
		public DbSet<Feedback> Feedbacks { get; set; }
		public DbSet<FollowBrand> FollowBrands { get; set; }
		public DbSet<Image> Images { get; set; }
		public DbSet<LikePost> LikePosts { get; set; }
		public DbSet<Order> Orders { get; set; }
		public DbSet<OrderDetail> OrderDetails { get; set; }
		public DbSet<Point> Points { get; set; }
		public DbSet<Post> Posts { get; set; }
		public DbSet<PostCategory> PostCategories { get; set; }
		public DbSet<PostImage> PostImages { get; set; }
		public DbSet<Product> Products { get; set; }
		public DbSet<ProductImage> ProductImages { get; set; }
		public DbSet<ProductType> Types { get; set; }
		public DbSet<Voucher> Vouchers { get; set; }

		protected override void OnModelCreating(ModelBuilder builder)
		{
			base.OnModelCreating(builder);

			builder.ApplyConfiguration(new AccountConfiguration());
			builder.ApplyConfiguration(new RoleConfiguration());
			builder.ApplyConfiguration(new OrderConfiguration());

			builder.Entity<IdentityUserRole<string>>(entity => entity.ToTable("AccountRole"));
			builder.Entity<IdentityUserClaim<string>>(entity => entity.ToTable("AccountClaim"));
			builder.Entity<IdentityUserLogin<string>>(entity => entity.ToTable("AccountLogin"));
			builder.Entity<IdentityUserToken<string>>(entity => entity.ToTable("AccountToken"));
			builder.Entity<IdentityRoleClaim<string>>(entity => entity.ToTable("RoleClaim"));
		}
	}
}
