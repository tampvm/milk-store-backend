using MilkStore.Service.Models.ResponseModels;
using MilkStore.Service.Models.ViewModels.FollowBrandViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore.Service.Interfaces
{
	public interface IFollowBrandService
	{
		Task<ResponseModel> GetFollowBrandByBrandIdAsync(int brandId, int pageIndex, int pageSize);
		Task<ResponseModel> GetFollowBrandByAccountIdAsync(string accountId, int pageIndex, int pageSize);
		Task<ResponseModel> UserFollowsBrand(UserFollowsBrandDTO model);
	}
}
