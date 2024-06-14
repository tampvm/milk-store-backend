using Microsoft.EntityFrameworkCore;
using MilkStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore.Repository.FluentAPIs
{
	public class OrderConfiguration : IEntityTypeConfiguration<Order>
	{
		public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Order> builder)
		{
			builder.ToTable("Order");
			builder.HasOne(x => x.Account)
				.WithMany(x => x.Orders)
				.HasForeignKey(x => x.AccountId)
				.OnDelete(DeleteBehavior.Restrict);
			builder.HasOne(x => x.AccountVoucher)
				.WithOne(x => x.Order)
				.HasForeignKey<Order>(x => x.AccountVoucherId)
				.OnDelete(DeleteBehavior.Restrict);
		}
	}
}
