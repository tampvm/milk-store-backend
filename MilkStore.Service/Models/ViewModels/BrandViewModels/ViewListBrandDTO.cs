using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore.Service.Models.ViewModels.BrandViewModels
{
	public class ViewListBrandDTO
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string BrandOrigin { get; set; }
		public string? Description { get; set; }
		public bool Active { get; set; }
		public int? ImageId { get; set; }

		public string? ImageUrl { get; set; }

		public DateTime CreatedAt { get; set; }
		public string? CreatedBy { get; set; }
		public DateTime? UpdatedAt { get; set; }
		public string? UpdatedBy { get; set; }
		public DateTime? DeletedAt { get; set; }
		public string? DeletedBy { get; set; }
		public bool IsDeleted { get; set; }
	}
}
