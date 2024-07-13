﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MilkStore.Service.Models.ViewModels.BrandViewModels
{
	public class CreateBrandDTO
	{
		public string Name { get; set; }
		public string BrandOrigin { get; set; }
		public string? Description { get; set; }
		public int? ImageId { get; set; }
	}
}