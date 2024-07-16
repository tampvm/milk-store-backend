using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore.Service.Models.ViewModels.FollowBrandViewModels
{
	public class UserFollowsBrandDTO
	{
		public DateTime FollowedAt { get; set; }
		public string AccountId { get; set; }
		public int BrandId { get; set; }
	}
}
