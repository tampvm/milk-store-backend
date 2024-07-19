using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore.Service.Models.ViewModels.ImageViewModels
{
	public class UploadImageDTO
	{
		public string ImageUrl { get; set; }
		public string ThumbnailUrl { get; set; }
		public string Type { get; set; }
		public string UserId { get; set; }
	}
}
