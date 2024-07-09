using MilkStore.Service.Models.ResponseModels;
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
	}
}
