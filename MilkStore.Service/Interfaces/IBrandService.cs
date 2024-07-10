using MilkStore.Service.Models.ResponseModels;
using MilkStore.Service.Models.ViewModels.BrandViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore.Service.Interfaces
{
	public interface IBrandService
	{
		Task<ResponseModel> GetBrandsAsync(int pageIndex, int pageSize);
		Task<ResponseModel> CreateBrandAsync(CreateBrandDTO model);
		Task<ResponseModel> UpdateBrandAsync(UpdateBrandDTO model);
		Task<ResponseModel> DeleteBrandAsync(int id);
	}
}
