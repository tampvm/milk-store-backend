using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore.Service.Models.ViewModels.FollowBrandViewModels
{
	public class UserFollowsBrandDTO
	{
		public bool IsFollow { get; set; }
		public DateTime FollowedAt { get; set; }
		public DateTime? UnfollowedAt { get; set; }
		public string AccountId { get; set; }
		public int BrandId { get; set; }
	}
}
