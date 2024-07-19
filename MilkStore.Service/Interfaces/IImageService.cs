using MilkStore.Service.Models.ResponseModels;
using MilkStore.Service.Models.ViewModels.ImageViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore.Service.Interfaces
{
	public interface IImageService
	{
		Task<ResponseModel> UploadImageAsync(UploadImageDTO model);
	}
}
