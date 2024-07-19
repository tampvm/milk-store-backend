using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MilkStore.Service.Interfaces;
using MilkStore.Service.Models.ViewModels.ImageViewModels;

namespace MilkStore.API.Controllers
{
	public class ImageController : BaseController
	{
		private readonly IImageService _imageService;

		public ImageController(IImageService imageService)
		{
			_imageService = imageService;
		}

		[HttpPost]
		public async Task<IActionResult> UploadImageAsync(UploadImageDTO model)
		{
			var response = await _imageService.UploadImageAsync(model);
			if (response.Success)
			{
				return Ok(response);
			}
			return BadRequest(response);
		}
	}
}
