using MilkStore.Service.Models.ResponseModels;
using MilkStore.Service.Models.ViewModels.VoucherViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore.Service.Interfaces
{
	public interface IVoucherService
	{
		Task<ResponseModel> GetVouchersAsync(int pageIndex, int pageSize);
		Task<ResponseModel> GetVoucherByCodeAsync(string code);
		Task<ResponseModel> GetVoucherByIdAsync(int id);
		Task<ResponseModel> CreateVoucherAsync(CreateVoucherDTO model);
		Task<ResponseModel> UpdateVoucherAsync(UpdateVoucherDTO model);
		Task<ResponseModel> DeleteVoucherAsync(int id);
	}
}
