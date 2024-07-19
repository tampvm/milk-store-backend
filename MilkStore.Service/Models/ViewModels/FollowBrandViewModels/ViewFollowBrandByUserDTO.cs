using MilkStore.Service.Models.ViewModels.BrandViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore.Service.Models.ViewModels.FollowBrandViewModels
{
	public class ViewFollowBrandByUserDTO
	{
		public int Id { get; set; }
		public bool IsFollow { get; set; }
		public DateTime FollowedAt { get; set; }
		public ViewBrandDetailDTO Brand { get; set; }
	}
}
