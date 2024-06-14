using Microsoft.EntityFrameworkCore;
using MilkStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore.Repository.FluentAPIs
{
	public class AccountConfiguration : IEntityTypeConfiguration<Account>
	{
		public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Account> builder)
		{
			builder.ToTable("Account");
			builder.Property(x => x.FirstName).HasMaxLength(50).IsRequired();
			builder.Property(x => x.LastName).HasMaxLength(50).IsRequired();
			builder.HasOne(x => x.Avatar)
				.WithMany(x => x.AccountAvatars)
				.HasForeignKey(x => x.AvatarId)
				.OnDelete(DeleteBehavior.Restrict);
			builder.HasOne(x => x.Background)
				.WithMany(x => x.AccountBackgrounds)
				.HasForeignKey(x => x.BackgroundId)
				.OnDelete(DeleteBehavior.Restrict);
		}	
	}
}
