using Microsoft.EntityFrameworkCore;
using MilkStore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore.Repository.FluentAPIs
{
	public class RoleConfiguration : IEntityTypeConfiguration<Role>
	{
		public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<Role> builder)
		{
			builder.ToTable("Role");
			
		}
	}
}
